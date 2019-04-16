using System.Collections.Generic;

using Contec.Framework.Extensions;

namespace Contec.Framework.Configuration
{
    /// <summary>
    /// Base implementation of the IConfiguration interface that defines the the properties and
    /// methods that can be used to pass around an application's configuration settings.
    /// </summary>
    public abstract class ConfigurationBase : IConfiguration
    {
        //public event ConfigurationEventHandler OnResetComplete;
        public event ConfigurationEventHandler OnReloadComplete;

        public abstract string this[string settingName] { get; }

        public virtual IDictionary<string, IConfiguration> Groups
        {
            get { return new Dictionary<string, IConfiguration>(); }
        }

        public virtual IEnumerable<string> Keys
        {
            get { return new List<string>(); }
        }

        public virtual string GetSetting(string settingName, string defaultValue = "")
        {
            return GetSetting<string>(settingName, defaultValue);
        }

        public virtual string GetGroupSetting(string groupName, string settingName, string defaultValue = "")
        {
            return GetGroupSetting<string>(groupName, settingName, defaultValue);
        }

        public virtual T GetSetting<T>(string settingName, T defaultValue)
        {
            return this[settingName].To<T>(defaultValue);
        }

        public virtual T GetGroupSetting<T>(string groupName, string settingName, T defaultValue)
        {
            if (Groups.ContainsKey(groupName))
            {
                return Groups[groupName].GetSetting(settingName, defaultValue);
            }

            return defaultValue;
        }

        public void Reset()
        {
            OnBeforeReset();
            OnReset();
            OnAfterReset();
        }

        public void Reload()
        {
            OnBeforeReload();
            OnReload();
            OnAfterReload();
        }

        protected virtual void OnBeforeReset()
        {
        }

        protected virtual void OnReset()
        {

        }

        protected virtual void OnAfterReset()
        {
            if (OnReloadComplete != null) OnReloadComplete.Invoke();
        }

        protected virtual void OnBeforeReload()
        {
            Reset();
        }

        protected virtual void OnReload()
        {
        }

        protected virtual void OnAfterReload()
        {
            if (OnReloadComplete != null) OnReloadComplete.Invoke();
        }
    }

    public delegate void ConfigurationEventHandler();
}