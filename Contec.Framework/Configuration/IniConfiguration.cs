using System.Configuration;
using System.Web;
using System.Web.Configuration;

namespace Contec.Framework.Configuration
{
    public class IniConfiguration : IniConfiguration<object> { }

    public class IniConfiguration<T> : GenericConfiguration, IConfiguration<T>
    {
        static readonly string FullNamePrefix = typeof(T).FullName + ".";

        private IniConfiguration(IniConfigurationSectionElement element)
        {
            foreach (NameValueConfigurationElement item in element.Items)
            {
                Add(item.Name, item.Value);
            }
        }

        public IniConfiguration()
        {
            GetConfigSettings();

            //-------------------------------------------------------------------------------------
            // Add it to the configuration registry if we not being called from a subclass.
            if (GetType() == typeof(IniConfiguration))
                ConfigurationRegistry.AddConfiguration(this);
        }

        /// <summary>
        /// Get the requested configuration setting's value from the AppSettings block of the
        /// application's configuration file.
        /// </summary>
        /// <param name="settingName">The name of the setting to retrieve a value for.</param>
        /// <returns>The string value for the requested setting.</returns>
        public override string this[string settingName]
        {
            get
            {
                var val = base[FullNamePrefix + settingName];

                if (val == null) val = base[settingName];

                return val;
            }
        }

        protected override void OnReload()
        {
            GetConfigSettings();
        }

        private void GetConfigSettings()
        {
            var config = GetConfiguration();

            if (config.HasFile)
            {
                foreach (var section in config.Sections)
                {
                    var iniSection = section as IniConfigurationSection;

                    if (iniSection != null)
                    {
                        foreach (IniConfigurationSectionElement elem in iniSection.Sections)
                        {
                            foreach (NameValueConfigurationElement item in elem.Items)
                            {
                                Set(item.Name, item.Value);
                            }

                            Groups.Add(elem.Name, new IniConfiguration<T>(elem));
                        }
                    }
                }

            }
        }

        private System.Configuration.Configuration GetConfiguration()
        {
            if (HttpRuntime.AppDomainAppId != null)
                return WebConfigurationManager.OpenWebConfiguration(HttpRuntime.AppDomainAppVirtualPath);

            return ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        }
    }
}
