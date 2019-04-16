using System.Xml;

namespace Contec.Framework.Extensions
{
    public static class XmlExtensions
    {
        public static T GetAttribute<T>(this XmlNode node, string name, T defaultValue)
        {
            var attributes = node.Attributes;

            if (attributes == null)
            {
                return defaultValue;
            }

            return attributes.GetAttribute(name, defaultValue);
        }

        public static T GetAttribute<T>(this XmlAttributeCollection attributes, string name, T defaultValue)
        {
            var attribute = attributes[name];

            if (attribute == null)
                return defaultValue;

            var str = attribute.Value;

            if (string.IsNullOrWhiteSpace(str))
            {
                return defaultValue;
            }

            return str.To(defaultValue);
        }
    }
}
