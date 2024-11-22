using System.Collections;
using System.Reflection;
using System.Text;
using NadeSnipe.Math;
using NadeSnipe.Serializer.Attributes;

namespace NadeSnipe.Serializer;

public class Kv3Serializer: IDisposable {
    public const string HEADER = "<!-- kv3 encoding:text:version{e21c7f3c-8a33-41c5-9977-a76d3a32aa0d} format:generic:version{7412167c-06e9-4698-aff2-e63eb59037e7} -->";

    private TextWriter Stream { get; set; }
    private int Indentation { get; set; }

    public Kv3Serializer(Stream stream) {
        Stream = new StreamWriter(stream, new UTF8Encoding());
        Stream.NewLine = "\n";
        Indentation = 0;

        Stream.WriteLine(HEADER);

    }

    public void Serialize<TData>(TData data) {
    
        if (data == null) {
            WriteValue("null");
            return;
        }

        Type type = data!.GetType();

        if(type.IsPrimitive) {
            WriteValue(data);
            return;
        } else if (type.IsEnum) {
            WriteEnum(data, type);
            return;
        } else if (type.Equals(typeof(Guid))) {
            WriteString(data);
            return;
        } else if (type.Equals(typeof(string))) {
            WriteString(data);
            return;
        } else if(type.IsArray) {
            WriteArray(data);
            return;
        } else if(type.GetInterface(nameof(IEnumerable)) != null) {
            
            WriteEnumerable(data);
            return;
        } else if(data is Vector3) {
            var d = data as Vector3;
            WriteArray(new float[]{d.X, d.Y, d.Z});
            return;
        }

        WriteObject(data, type);
    }

    private void WriteObject<TData>(TData data, Type type) {
        Stream.WriteLine('{');
        Indentation += 1;

        foreach(var prop in type.GetProperties()) {

            var unlistAttribute = prop.GetCustomAttribute(typeof(Kv3UnlistAttribute)) as Kv3UnlistAttribute;
            if(unlistAttribute != null) {
                WriteEnumerableAsProperties(prop.GetValue(data));
                continue;
            }

            var key = prop.Name;
            var keyAttribute = prop.GetCustomAttribute(typeof(Kv3KeyAttribute)) as Kv3KeyAttribute;
            if (keyAttribute != null) {
                key = keyAttribute.Value;
            }

            Indent();
            Stream.Write($"{key} = ");
            Serialize(prop.GetValue(data));
            Stream.WriteLine();

        }

        Indentation -= 1;
        Indent();
        Stream.Write('}');
    }

    private void WriteEnumerableAsProperties<TData>(TData data) {
        int index = 0;
        var enumerator = (data as IEnumerable)!.GetEnumerator();
        Type itemType = data.GetType().GetGenericArguments()[0];
        while(enumerator.MoveNext()) {
            var typeName = itemType.Name;
            var key = $"{typeName}{index}";
            Indent();
            Stream.Write($"{key} = ");
            Serialize(enumerator.Current);
            Stream.WriteLine();
            index++;
        }
    }

    private void WriteEnumerable<TData>(TData data) {
        Stream.WriteLine("[ ");
        Indentation += 1;

        var enumerator = (data as IEnumerable)!.GetEnumerator();
        if(enumerator != null && enumerator.MoveNext()) {
            Indent();
            Serialize(enumerator.Current);
            while(enumerator.MoveNext()) {
                Stream.WriteLine(",");
                Indent();
                Serialize(enumerator.Current);
            }
            Stream.WriteLine();
        }

        Indentation -= 1;
        Indent();
        Stream.WriteLine(" ]");
    }

    private void WriteArray<TData>(TData data) {
        Stream.Write("[ ");

        Array array = (data as Array)!;
        for(int i = 0; i < array.Length; i++) {
            Serialize(array.GetValue(i));
            if(i < array!.Length - 1) Stream.Write(", ");
        }

        Stream.Write(" ]");
    }

    private void WriteEnum<TData>(TData data, Type type) {
        var memInfo = type.GetMember(data!.ToString()!);
        var stringValueAttributes = memInfo[0].GetCustomAttributes(typeof(Kv3StringValueAttribute), false);

        if(stringValueAttributes.Length == 0) {
            WriteValue(data!);
            return;
        }

        WriteString((stringValueAttributes[0] as Kv3StringValueAttribute)!.Value);
    }

    private void Indent() {
        Stream.Write(new string('\t', Indentation));
    }

    private void WriteValue(object value) {
        Stream.Write(value);
    }

    private void WriteString(object value) {
        Stream.Write($"\"{value}\"");
    }

    public void Flush() {
        Stream.Flush();
    }

    public void Dispose()
    {
        Stream.Flush();
        Stream.Dispose();
    }
}