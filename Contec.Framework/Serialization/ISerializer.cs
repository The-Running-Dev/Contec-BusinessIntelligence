namespace Contec.Framework.Serialization
{
    public interface ISerializer
    {
        SerializationType SerializationType { get; }

        void SerializeToFile<T>(T obj, string path);
        string Serialize<T>(T obj);

        T DeserializeFromFile<T>(string path);
        T Deserialize<T>(string data);
    }
}