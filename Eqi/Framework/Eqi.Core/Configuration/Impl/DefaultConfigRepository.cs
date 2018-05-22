using System;
using System.Collections.Generic;
using System.IO;
using Eqi.Core.Reflection;
using Eqi.Core.Serialization;

namespace Eqi.Core.Configuration.Impl
{
    [DependencyInjection(typeof(IConfigRepository))]
    public class DefaultConfigRepository : IConfigRepository
    {
        private readonly ICurrentAppDomain _currentAppDomain;
        private readonly ISerializer _serializer;

        public DefaultConfigRepository(ICurrentAppDomain currentAppDomain, ISerializer serializer)
        {
            this._currentAppDomain = currentAppDomain;
            this._serializer = serializer;
        }

        /// <summary>
        /// Get all config file definition.
        /// </summary>
        /// <returns>All config file definition.</returns>
        public IList<IConfigFileDefinition> GetAllConfigFileDefinition()
        {
            var list = new List<IConfigFileDefinition>();
            var systemFileConfig = this.GetSystemFileConfig();
            if (systemFileConfig != null && systemFileConfig.ConfigurationFiles != null)
            {
                systemFileConfig.ConfigurationFiles.ForEach(file =>
                {
                    list.Add(new ConfigFileDefinition
                    {
                        Name = file.Name,
                        FilePath = file.FilePath,
                        Format = (FileFormat)Enum.Parse(typeof(FileFormat), file.Format)
                    });
                });
            }

            return list;
        }

        /// <summary>
        /// Get config file definition by key.
        /// </summary>
        /// <param name="configName">Config name.</param>
        /// <returns>Config file definition.</returns>
        public IConfigFileDefinition GetConfigFileDefinitionByName(string configName)
        {
            var allConfigFiles = this.GetAllConfigFileDefinition();
            if (!allConfigFiles.IsNullOrEmpty())
            {
                return allConfigFiles.Find(file => file.Name.Equals(configName, StringComparison.OrdinalIgnoreCase));
            }

            return null;
        }

        /// <summary>
        /// Get config file definition by attribute.
        /// </summary>
        /// <param name="attribute">Config file Attribute.</param>
        /// <returns>Config file definition.</returns>
        public IConfigFileDefinition GetConfigFileDefinitionByAttribute(ConfigFileAttribute attribute)
        {
            var configDefinition = new ConfigFileDefinition();
            configDefinition.Name = attribute.Name;
            configDefinition.FilePath = attribute.FilePath;
            configDefinition.Format = attribute.Format;

            return configDefinition;
        }

        private ConfigurationFileConfig GetSystemFileConfig()
        {
            var filePath = Path.Combine(this._currentAppDomain.BaseDirectory, "Configuration/ConfigFiles.config");
            return this._serializer.DeserializeXmlFromFile<ConfigurationFileConfig>(filePath);
        }
    }
}
