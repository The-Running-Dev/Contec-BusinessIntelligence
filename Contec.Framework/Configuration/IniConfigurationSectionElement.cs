using System.Configuration;
using System.IO;

namespace Contec.Framework.Configuration
{
    public sealed class IniConfigurationSectionElement : ConfigurationElement
    {
        private static readonly ConfigurationPropertyCollection _propertiesCollection;
        private static readonly ConfigurationProperty _nameProperty;
        private static readonly ConfigurationProperty _fileProperty;
        private static readonly ConfigurationProperty _itemsProperty;

        static IniConfigurationSectionElement()
        {
            _nameProperty = new ConfigurationProperty("name", typeof(string), string.Empty, ConfigurationPropertyOptions.IsRequired | ConfigurationPropertyOptions.IsKey);

            _fileProperty = new ConfigurationProperty("file", typeof(string), string.Empty, ConfigurationPropertyOptions.None);

            _itemsProperty = new ConfigurationProperty("", typeof(NameValueConfigurationCollection), null, ConfigurationPropertyOptions.IsDefaultCollection);

            _propertiesCollection = new ConfigurationPropertyCollection {_nameProperty, _fileProperty, _itemsProperty};
        }

        public IniConfigurationSectionElement() { }

        public IniConfigurationSectionElement(string name)
        {
            Name = name;
        }

        public IniConfigurationSectionElement(string name, string file)
        {
            Name = name;
            File = file;
        }

        protected override ConfigurationPropertyCollection Properties
        {
            get { return _propertiesCollection; }
        }

        [ConfigurationProperty("name", DefaultValue = "", Options = ConfigurationPropertyOptions.IsRequired | ConfigurationPropertyOptions.IsKey)]
        public string Name
        {
            get { return (string)base[_nameProperty]; }
            set { base[_nameProperty] = value; }
        }

        [ConfigurationProperty("file", DefaultValue = "", Options = ConfigurationPropertyOptions.None)]
        public string File
        {
            get
            {
                var str = (string)base[_fileProperty];

                if (str == null) return string.Empty;

                return str;
            }
            set
            {
                base[_fileProperty] = value;
            }

        }

        [ConfigurationProperty("", Options = ConfigurationPropertyOptions.IsDefaultCollection)]
        public NameValueConfigurationCollection Items
        {
            get { return (NameValueConfigurationCollection) base[_itemsProperty]; }
        }

        protected override void DeserializeElement(System.Xml.XmlReader reader, bool serializeCollectionKey)
        {
            base.DeserializeElement(reader, serializeCollectionKey);

            if (!string.IsNullOrWhiteSpace(File))
            {
                var source = ElementInformation.Source;
                var configFilePath = !string.IsNullOrEmpty(source)
                    ? Path.Combine(Path.GetDirectoryName(source), File)
                    : File;

                if (System.IO.File.Exists(configFilePath))
                {
                    var config = ConfigurationManager.OpenMappedExeConfiguration(
                        new ExeConfigurationFileMap() { ExeConfigFilename = configFilePath },
                        ConfigurationUserLevel.None);

                    if (config.HasFile)
                    {
                        foreach (var section in config.Sections)
                        {
                            var iniSection = section as IniConfigurationSection;

                            if (iniSection != null)
                            {
                                var element = iniSection.Sections[Name];

                                if (element != null)
                                {
                                    foreach (var item in element.Items)
                                    {
                                        Items.Add((NameValueConfigurationElement)item);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}