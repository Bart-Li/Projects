using System.Collections.Generic;
using Eqi.Core.IO;
using Eqi.Core.Serialization;

namespace Eqi.Core.Configuration.Impl
{
    [DependencyInjection(typeof(IConfigAccessor))]
    public class DefaultConfigAccessor : IConfigAccessor
    {
        private readonly IFileWatcher _fileWatcher;
        private readonly IConfigRepository _configRepository;
        private readonly ISerializer _serializer;
        private static readonly IDictionary<string, object> ConfigCollection = new Dictionary<string, object>();

        public DefaultConfigAccessor(IFileWatcher fileWatcher, IConfigRepository configRepository, ISerializer serializer)
        {
            this._fileWatcher = fileWatcher;
            this._configRepository = configRepository;
            this._serializer = serializer;

            this._fileWatcher.WatchDataChange(this._configRepository.DefaultConfigDirectory, ()=> ConfigCollection.Clear());
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

        private T GetConfigValueByPath<T>(IConfigFileDefinition definition)
        {
            if (definition == null || string.IsNullOrWhiteSpace(definition.FilePath))
            {
                return default(T);
            }

            switch (definition.Format)
            {
                case FileFormat.Json:
                    return this._serializer.DeserializeJsonFromFile<T>(definition.FilePath);
                case FileFormat.Xml:
                    return this._serializer.DeserializeXmlFromFile<T>(definition.FilePath);
                default:
                    return (T)((object)this._serializer.DeserializeTextFromFile(definition.FilePath));
            }
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
