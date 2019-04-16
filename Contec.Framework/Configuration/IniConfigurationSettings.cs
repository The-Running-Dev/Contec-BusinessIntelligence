using System.Collections.Generic;

namespace Contec.Framework.Configuration
{
    public class IniConfigurationSettings : Dictionary<string, string>
    {
        private string _name;

        public IniConfigurationSettings() : this(string.Empty) { }

        public IniConfigurationSettings(string name)
            : base()
        {
            _name = name;
        }

        public string Name { get { return _name; } set { _name = value; } }

        public void CopyFrom(IniConfigurationSettings section)
        {
            Clear();

            _name = section._name;

            foreach (var fieldName in section.Keys)
            {
                Add(fieldName, section[fieldName]);
            }
        }
    }
}