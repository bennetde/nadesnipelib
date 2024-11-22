namespace NadeSnipe.Serializer.Attributes;

public class Kv3KeyAttribute : Attribute {
    private readonly string _value;

    public Kv3KeyAttribute(string value) {
        _value = value;
    }

    public string Value {
        get => _value;
    }
}