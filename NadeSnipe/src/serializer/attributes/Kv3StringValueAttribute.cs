namespace NadeSnipe.Serializer.Attributes;

public class Kv3StringValueAttribute : Attribute {
    private readonly string _value;

    public Kv3StringValueAttribute(string value) {
        _value = value;
    }

    public string Value {
        get => _value;
    }
}