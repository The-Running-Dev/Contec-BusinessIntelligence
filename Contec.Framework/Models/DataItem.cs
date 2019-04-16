namespace Contec.Framework.Models
{
    public class DataItem
    {
        public string Key { get; set; }

        public string Value { get; set; }

        public string Type { get; set; }

        public DataItem() { }

        public DataItem(string key, string value, string type)
        {
            Key = key;
            Value = value;
            Type = type;
        }
    }
}