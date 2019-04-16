using System.IO;

using Contec.Framework.Files;

namespace Contec.Framework.Serialization
{
    public class XmlSerializer : IXmlSerializer
    {
        private readonly ILocalFileSystem _fileSystem;

        public XmlSerializer(ILocalFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public SerializationType SerializationType { get { return SerializationType.Xml;} }

        public void SerializeToFile<T>(T obj, string path)
        {
            _fileSystem.WriteToFile(path, Serialize(obj));
        }

        public string Serialize<T>(T obj)
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));

            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, obj);

                return writer.ToString();
            }
        }

        public T DeserializeFromFile<T>(string path)
        {
            return Deserialize<T>(_fileSystem.GetFileContents(path));
        }

        public T Deserialize<T>(string data)
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof (T));
            return (T)serializer.Deserialize(new StringReader(data));
        }
    }
}
