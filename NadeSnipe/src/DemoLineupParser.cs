using System.Reflection.Metadata;
using DemoFile;
using DemoFile.Game.Cs;
using NadeSnipe.Math;

namespace NadeSnipe;

public class DemoLineupParser {

    private Stream DemoFile { get; set; }
    public string MapName { get; private set; }
    public List<Lineup> Lineups { get; private set; }

    private Dictionary<CEntityIndex, Vector3> _jumpingPlayerOriginalPositions = new();

    private Dictionary<CEntityIndex, Lineup> _activeNades = new();

    private Dictionary<CEntityIndex, Lineup> _lastThrownLineups = new();

    private Dictionary<CEntityIndex, Vector3> _playerVelocities = new();

    public DemoLineupParser(Stream demo) {
        DemoFile = demo;
        Lineups = new();
        MapName = "notSet";
    }

    public async Task Parse() {

        var demo = new CsDemoParser();

        demo.Source1GameEvents.WeaponFire += e => {
            if(e.Weapon.Equals("weapon_smokegrenade")) {
                var ev = (Source1WeaponFireEvent)e;
                CCSPlayerPawn pawn = e.PlayerPawn!; 
                CCSPlayerController playerController = e.Player!;

                var pos = new Vector3(pawn.Origin.X, pawn.Origin.Y, pawn.Origin.Z);
                var angle = new Vector3(pawn.EyeAngles.Pitch, pawn.EyeAngles.Yaw, pawn.EyeAngles.Roll);
                var eyePos = new Vector3(pawn.ViewOffset.X, pawn.ViewOffset.Y, pawn.ViewOffset.Z);
                var jumpThrow = false;
                var playerName = playerController.PlayerName;
                var velocity = _playerVelocities[pawn.EntityIndex];
                var verticalVelocity = velocity.Z;

                var round = GetRound(demo);

                var isTerrorist = pawn.CSTeamNum == CSTeamNumber.Terrorist;
                var team = isTerrorist ? Team.Terrorist : Team.CounterTerrorist;

                velocity.Z = 0.0f;

                if(_jumpingPlayerOriginalPositions.ContainsKey(pawn.EntityIndex)) {
                    var jumpingOriginalPos = _jumpingPlayerOriginalPositions[pawn.EntityIndex];
                    if(verticalVelocity <= 0.0f) {
                        // Console.WriteLine("Plaer does not seem to be jumping anymore");
                    }
                    else if(pos.Distance(jumpingOriginalPos) > 200.0f) {
                        // Console.WriteLine($"Original Distance and Mid-Air Position unexpectedly high. Not overwriting position\nOriginal Position: {jumpingOriginalPos}\nNew Pos: {pos}");
                    } else {
                        pos = jumpingOriginalPos;
                        jumpThrow = true;
                    }
                    // Console.WriteLine("Player is jumping");
                    _jumpingPlayerOriginalPositions.Remove(pawn.EntityIndex);
                }

                // pawn.Speed
                // var velocity = new Vector3(pawn.Velocity.X, pawn.Velocity.Y, pawn.Velocity.Z);
                int throwTypeMask = 0;
                if(velocity.Magnitude() > 0.0f) {
                    throwTypeMask |= 0b1;
                }
                if(jumpThrow) {
                    throwTypeMask |= 0b10;
                }
                if(pawn.MovementServices!.DesiresDuck) {
                    // Console.WriteLine("Duck Jump");
                    throwTypeMask |= 0b100;
                }
                
                var lineup = new Lineup(pos, angle, eyePos, playerName, GrenadeType.Smoke, (ThrowType)throwTypeMask, round, team);
                if(_lastThrownLineups.ContainsKey(pawn.EntityIndex)) {
                    Console.WriteLine($"{playerController.PlayerName} previously threw a lineup that was not cleared by a smoke projectile.");
                }
                _lastThrownLineups[pawn.EntityIndex] = lineup;
                Lineups.Add(lineup);
            }
        };

        // demo.EntityEvents.CSmokeGrenade.AddChangeCallback(x => x.JumpThrow, (smoke, oldVal, newVal) => {
        //     if (smoke.JumpThrow) {
        //         Console.WriteLine("Jump throw");
        //     }
        // });

        // demo.EntityEvents.CSmokeGrenade.AddChangeCallback(x => x.ThrowTime, (smoke, old, newt) => {
        //     Console.WriteLine($"{smoke.FireSequenceStartTime} throw other");
        // });

        // This event is used to connect a grenade projectile with a lineup as the `Source1WeaponFireEvent` event does not 
        // include a projectile entity.
        demo.EntityEvents.CSmokeGrenadeProjectile.Create += e => {
            // Console.WriteLine("")
            CSmokeGrenadeProjectile g = e;
            var playerPawn = (CCSPlayerPawn)g.OwnerEntity!;
            var playerController = playerPawn.Controller!;
            // var smoke = g.Smok

            if(!_lastThrownLineups.ContainsKey(playerPawn.EntityIndex)) {
                // Ignore smoke grenades where the player died while priming a grenade.
                if(!playerPawn.IsAlive) {
                    return;
                }
                Console.WriteLine($"New smoke projectile without lineup thrown by {playerPawn?.Controller?.PlayerName} at {playerPawn?.Origin} in round {GetRound(demo)}");
                return;
            }

            _activeNades[g.EntityIndex] = _lastThrownLineups[playerPawn.EntityIndex];
            _lastThrownLineups.Remove(playerPawn.EntityIndex);
        };

        demo.Source1GameEvents.SmokegrenadeDetonate += e => {
            Source1SmokegrenadeDetonateEvent ev = e;
            var entityIndex = new CEntityIndex((uint)ev.Entityid);
            if(!_activeNades.ContainsKey(entityIndex)) {
                // Grenades that were thrown by dead players are ignored.
                if(!ev.PlayerPawn!.IsAlive) return;
                throw new Exception("Untracked Smoke Grenade detonated");
            }

            _activeNades[entityIndex].DetonationOrigin = new Vector3(ev.X, ev.Y, ev.Z);
            _activeNades.Remove(entityIndex);
        };

        // Primary way of detecting jumps. This event doesn't seem to fire at all times and is unreliable.
        // The player position will already be one frame in the air when this event is fired which causes some inacuraccy.
        demo.EntityEvents.CCSPlayerPawn.AddChangeCallback(pawn => pawn.MovementServices!.OldJumpPressed, (pawn, old, newT) => {
            // Console.WriteLine($"OldJump detected {pawn.Origin}");
            _jumpingPlayerOriginalPositions[pawn.EntityIndex] = new Vector3(pawn.Origin.X, pawn.Origin.Y, pawn.Origin.Z - 1.5f);
        });

        // Dont track any smokes after round end
        demo.Source1GameEvents.RoundEnd += e => {
            _jumpingPlayerOriginalPositions.Clear();
            _activeNades.Clear();
            _lastThrownLineups.Clear();
        };

        // Secondary technique of detecting jumps by looking at changes of player stamina. Inacurrate positions as it is a tick delayed.
        // Subtick probably makes it impossible to reconstruct the original position so we just shift the position down by 2 units.
        demo.EntityEvents.CCSPlayerPawn.AddChangeCallback(pawn => pawn.MovementServices?.Stamina, (pawn, old, newT) => {

                            
            if(newT > 23.0f) {
                _jumpingPlayerOriginalPositions[pawn.EntityIndex] = new Vector3(pawn.Origin.X, pawn.Origin.Y, pawn.Origin.Z - 2.0f);
            } else if(newT > 14.5f && newT < 15.0f && old == 0.0f) {
                if(!_jumpingPlayerOriginalPositions.ContainsKey(pawn.EntityIndex)) {
                    // throw new Exception($"Player detected as landed that was not tracked as jumped. Stamina: {newT}");
                    return;
                }
                _jumpingPlayerOriginalPositions.Remove(pawn.EntityIndex);
            }
        });

        demo.EntityEvents.CCSPlayerPawn.AddChangeCallback(pawn => pawn.Origin, (pawn, old, newT) => {
            // Vector3 view = new Vector3(newT.X, newT.Y, newT.Z) + new Vector3(pawn.ViewOffset.X, pawn.ViewOffset.Y, pawn.ViewOffset.Z);
            // Console.WriteLine(demo.);
            _playerVelocities[pawn.EntityIndex] = (new Vector3(newT) - new Vector3(old)) / (1.0f / 32.0f);
            // Console.WriteLine($"Pawn Origin: {newT} Controller Origin: {view}");
        });

        var reader = DemoFileReader.Create(demo, DemoFile);
        await reader.ReadAllAsync();
        MapName = demo.ServerInfo?.MapName!;

    }

    private int GetRound(CsDemoParser demo) {
        return demo.TeamCounterTerrorist.Score + demo.TeamTerrorist.Score + 1;
    }

    public string GetMap() {
        return MapName;
    }
}