using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eqi.Core.Serialization;

namespace Eqi.Core.Configuration.Impl
{
    [DependencyInjection(typeof(IConfigAccessor))]
    public class DefaultConfigAccessor : IConfigAccessor
    {
        private readonly ISerializer _serializer;
        private static readonly IDictionary<string, object> ConfigCollection = new Dictionary<string, object>();

        public DefaultConfigAccessor(ISerializer serializer)
        {
            this._serializer = serializer;
        }

        /// <summary>
        /// Get configuration by 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileConfig"></param>
        /// <returns></returns>
        public T GetConfigValue<T>(IConfigFileDefinition fileConfig) where T : class
        {
            if (fileConfig == null || string.IsNullOrWhiteSpace(fileConfig.FilePath))
            {
                return null;
            }

            var configCollectionKey = fileConfig.FilePath.ToLower();
            if (ConfigCollection.ContainsKey(configCollectionKey))
            {
                return ConfigCollection[configCollectionKey] as T;
            }

            T configValue = this.GetConfigValueByPath<T>(fileConfig);
            this.AddOrUpdateConfigCollection(configCollectionKey, configValue);
            return configValue;
        }

        private T GetConfigValueByPath<T>(IConfigFileDefinition configFileDefinition)
        {
            if (configFileDefinition == null || string.IsNullOrWhiteSpace(configFileDefinition.FilePath))
            {
                return default(T);
            }



            return default(T);
        }

        /// <summary>
        /// Add or update config collection.
        /// </summary>
        /// <param name="configCollectionKey">Config collection key.</param>
        /// <param name="configValue">Config value.</param>
        private void AddOrUpdateConfigCollection(string configCollectionKey, object configValue)
        {
            if (configValue == null)
            {
                return;
            }

            if (ConfigCollection.ContainsKey(configCollectionKey))
            {
                ConfigCollection[configCollectionKey] = configValue;
            }
            else
            {
                ConfigCollection.Add(configCollectionKey, configValue);
            }
        }
    }
}
