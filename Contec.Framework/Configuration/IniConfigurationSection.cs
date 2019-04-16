using System.Configuration;

namespace Contec.Framework.Configuration
{
    public sealed class IniConfigurationSection : ConfigurationSection
    {
        private static readonly ConfigurationPropertyCollection _properties;
        private static readonly ConfigurationProperty _sectionsProperty;

        public IniConfigurationSection() { }

        static IniConfigurationSection()
        {
            _sectionsProperty = new ConfigurationProperty("", typeof(IniConfigurationSectionCollection), null, ConfigurationPropertyOptions.IsDefaultCollection);

            _properties = new ConfigurationPropertyCollection();
            _properties.Add(_sectionsProperty);
        }

        protected override ConfigurationPropertyCollection Properties
        {
            get { return _properties; }
        }

        [ConfigurationProperty("", Options = ConfigurationPropertyOptions.IsDefaultCollection)]
        public IniConfigurationSectionCollection Sections
        {
            get { return (IniConfigurationSectionCollection)base[_sectionsProperty]; }
        }
    }
}