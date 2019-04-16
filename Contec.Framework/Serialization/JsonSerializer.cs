using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using Contec.Framework.Files;

namespace Contec.Framework.Serialization
{
    public class JsonSerializer : IJsonSerializer
    {
        private readonly ILocalFileSystem _fileSystem;

        private static readonly JsonSerializerSettings _settings = new JsonSerializerSettings()
        {
            DefaultValueHandling = DefaultValueHandling.Include,
            NullValueHandling = NullValueHandling.Include,
            Formatting = Formatting.Indented
        };

        static JsonSerializer()
        {
            _settings.Converters.Add(new StringEnumConverter());
        }

        public JsonSerializer(ILocalFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public SerializationType SerializationType { get { return SerializationType.Json; } }

        public void SerializeToFile<T>(T obj, string path)
        {
            _fileSystem.WriteToFile(path, Serialize(obj));
        }

        public void SerializeToFile<T>(T obj, string path, JsonSerializerSettings settings)
        {
            _fileSystem.WriteToFile(path, Serialize(obj, settings));
        }

        public string Serialize<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj, _settings);
        }

        public string Serialize<T>(T obj, JsonSerializerSettings settings)
        {
            if (settings != null)
                return JsonConvert.SerializeObject(obj, settings);

            return JsonConvert.SerializeObject(obj);
        }

        public T DeserializeFromFile<T>(string path)
        {
            var json = _fileSystem.GetFileContents(path);

            return Deserialize<T>(json);
        }

        public T DeserializeFromFile<T>(string path, JsonSerializerSettings settings)
        {
            var json = _fileSystem.GetFileContents(path);
            return Deserialize<T>(json, settings);
        }

        public T Deserialize<T>(string data)
        {
            return JsonConvert.DeserializeObject<T>(data, _settings);
        }

        public T Deserialize<T>(string data, JsonSerializerSettings settings)
        {
            if (settings != null)
            {
                return JsonConvert.DeserializeObject<T>(data, settings);
            }

            return JsonConvert.DeserializeObject<T>(data);
        }
    }
}
