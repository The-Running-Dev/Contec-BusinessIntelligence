using Newtonsoft.Json;

namespace Contec.Framework.Serialization
{
    public interface IJsonSerializer : ISerializer
    {
        void SerializeToFile<T>(T obj, string path, JsonSerializerSettings settings);

        string Serialize<T>(T obj, JsonSerializerSettings settings);


        T DeserializeFromFile<T>(string path, JsonSerializerSettings settings);

        T Deserialize<T>(string data, JsonSerializerSettings settings);
    }
}