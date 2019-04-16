using System;
using System.Collections.Generic;

namespace Contec.Framework.Configuration
{
    public class ConfigurationRegistry
    {
        private static readonly object SyncObject = new object();
        private static ConfigurationRegistryImpl _registry;

        internal static ConfigurationRegistryImpl Registry
        {
            get
            {
                if (_registry != null)
                    return _registry;

                lock (SyncObject)
                {
                    if (_registry != null)
                        return _registry;

                    _registry = new ConfigurationRegistryImpl();
                }

                return _registry;
            }
        }

        public static void AddConfiguration(IConfiguration config)
        {
            Registry.AddConfiguration(config);
        }

        public static void Reload(Type configType)
        {
            Registry.Reload(configType);
        }

        public static void Reload(IConfiguration config)
        {
            Registry.Reload(config.GetType());
        } 
    }

    internal class ConfigurationRegistryImpl
    {
        private readonly Dictionary<Type, List<IConfiguration>> _configs = new Dictionary<Type, List<IConfiguration>>();

        public void AddConfiguration(IConfiguration config)
        {
            lock (this)
            {
                var configType = config.GetType();

                if (!_configs.ContainsKey(configType))
                    _configs.Add(configType, new List<IConfiguration>());

                _configs[configType].Add(config);
            }
        }

        public void Reload(Type configType)
        {
            if (!_configs.ContainsKey(configType)) return;

            lock (this)
            {
                _configs[configType].ForEach(cfg => cfg.Reload());
            }
        }
    }
}