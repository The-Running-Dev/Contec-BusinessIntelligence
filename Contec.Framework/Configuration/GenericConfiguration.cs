using System.Collections.Generic;

namespace Contec.Framework.Configuration
{
    /// <summary>
    /// An implementation of the IConfiguaration interface that maintains the collection of setting
    /// in memory and doesn't preload them from anywhere.
    /// </summary>
    public class GenericConfiguration : ConfigurationBase
    {
        private readonly Dictionary<string, IConfiguration> _groups = new Dictionary<string, IConfiguration>();

        /// <summary>
        /// Dictionary used to store the configuration settings that this object is managing.
        /// </summary>
        private readonly Dictionary<string, string> _settings = new Dictionary<string, string>();

        public GenericConfiguration()
        {
            //-------------------------------------------------------------------------------------
            // Add it to the configuration registry if we not being called from a subclass.
            if (GetType() == typeof (GenericConfiguration))
            {
                ConfigurationRegistry.AddConfiguration(this);
            }
        }

        /// <summary>
        /// Retrieves the requested configuration setting from some configuration source.
        /// </summary>
        /// <param name="settingName">The name of the setting to retrieve a value for.</param>
        /// <returns>The string value for the requested setting.</returns>
        public override string this[string settingName]
        {
            get
            {
                if (!_settings.ContainsKey(settingName))
                {
                    return null;
                }

                return _settings[settingName];
            }
        }

        public override IDictionary<string, IConfiguration> Groups
        {
            get { return _groups; }
        }

        public override IEnumerable<string> Keys
        {
            get { return _settings.Keys; }
        }

        /// <summary>
        /// Add or sets a setting name/value pair in the collection of settings that is being
        /// managed by this object.
        /// </summary>
        /// <param name="settingName">The name of the setting to set.</param>
        /// <param name="settingValue">The value of the setting to set.</param>
        public void Set(string settingName, string settingValue)
        {
            _settings[settingName] = settingValue;
        }

        /// <summary>
        /// Add a setting name/value pair to the collection of settings that is being managed by
        /// this object.
        /// </summary>
        /// <param name="settingName">The name of the setting to add.</param>
        /// <param name="settingValue">The value of the setting to add.</param>
        public void Add(string settingName, string settingValue)
        {
            _settings.Add(settingName, settingValue);
        }

        protected override void OnReset()
        {
            _settings.Clear();
            _groups.Clear();
        }
    }
}