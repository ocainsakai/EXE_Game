
using System.Globalization;
using System.Text;
using UnityEditor.UIElements;

public class SerializableDictionaryConverter<KeyType, ValueType> : UxmlAttributeConverter<SerializableDictionary<KeyType, ValueType>>
{
    static string ValueToString(object InValue) => System.Convert.ToString(InValue, CultureInfo.InvariantCulture);

    public override string ToString(SerializableDictionary<KeyType, ValueType> InSource)
    {
        var DataBuilder = new StringBuilder();

        foreach (var KVP in InSource)
        {
            DataBuilder.Append($"{ValueToString(KVP.Key)}|{ValueToString(KVP.Value)},");
        }

        return DataBuilder.ToString();
    }

    public override SerializableDictionary<KeyType, ValueType> FromString(string InSource)
    {
        var OutputDictionary = new SerializableDictionary<KeyType, ValueType>();

        var KeyValuePairs = InSource.Split(',');
        foreach (var KVP in KeyValuePairs)
        {
            var Fields = KVP.Split("|");
            KeyType Key = (KeyType)System.Convert.ChangeType(Fields[0], typeof(KeyType));
            ValueType Value = (ValueType)System.Convert.ChangeType(Fields[1], typeof(ValueType));

            OutputDictionary.EditorOnly_Add(Key, Value);
        }

        OutputDictionary.SynchroniseToSerializedData();

        return OutputDictionary;
    }
}
