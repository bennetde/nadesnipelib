# NadeSnipe Lib

This is the backend library for parsing grenade lineups from Counter-Strike 2 replay files.
It is separated in further sub-projects. Files will be exported using Valve's KV3 format that can then be used as CS2 Annotations

The hosted frontend using WASM can be found here: https://bennet.me/nadesnipe/

## ./NadeSnipe

The actual code of the library. 

## ./NadeSnipe.Cli

Command-Line-Interface for quickly exporting lineups from demos.

## ./NadeSnipe.Tests

Test Suite for the libray using xUnit.

## ./NadeSnipe.Wasm

JavaScript-Interop of the library using WebAssembly.

## ./NadeSnipe.Benchmarks

Benchmarking functionality.
