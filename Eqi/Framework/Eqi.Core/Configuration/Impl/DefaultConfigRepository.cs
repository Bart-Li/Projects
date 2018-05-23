using System;
using System.Collections.Generic;
using System.IO;
using Eqi.Core.Reflection;
using Eqi.Core.Serialization;
using Eqi.Core;

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
        /// Get default system config directory.
        /// </summary>
        public string DefaultConfigDirectory
        {
            get {
                return Path.Combine(this._currentAppDomain.BaseDirectory, "Configuration");
            }
        }

        /// <summary>
        /// Get all config file definition.
        /// </summary>
        /// <returns>All config file definition.</returns>
        public IList<IConfigFileDefinition> GetAllConfigFileDefinition()
        {
            var list = new List<IConfigFileDefinition>();
            var systemFileConfig = this.GetSystemFileConfig();
            if (systemFileConfig != null && systemFileConfig.ConfigFiles != null)
            {
                systemFileConfig.ConfigFiles.ForEach(file =>
                {
                    list.Add(new ConfigFileDefinition
                    {
                        Name = file.Name,
                        FilePath = this.BuildConfigFilePath(file.FilePath),
                        Format = file.Format.ToEnum<FileFormat>()
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
            configDefinition.FilePath = this.BuildConfigFilePath(attribute.FilePath);
            configDefinition.Format = attribute.Format;

            return configDefinition;
        }

        /// <summary>
        /// Get config file definition by file path.
        /// </summary>
        /// <param name="filePath">File path.</param>
        /// <returns>Config file definition.</returns>
        public IConfigFileDefinition GetConfigFileDefinitionByFilePath(string filePath)
        {
            var configDefinition = new ConfigFileDefinition();
            configDefinition.FilePath = this.BuildConfigFilePath(filePath);

            return configDefinition;
        }

        /// <summary>
        /// Build config file path.
        /// </summary>
        /// <param name="filePath">File path.</param>
        /// <returns>Config file path.</returns>
        private string BuildConfigFilePath(string filePath)
        {
            string pathRoot = Path.GetPathRoot(filePath);
            if (pathRoot == null || pathRoot.Trim().Length <= 0)
            {
                return Path.Combine(DefaultConfigDirectory, filePath);
            }

            return filePath;
        }

        /// <summary>
        /// Get system config files.
        /// </summary>
        /// <returns></returns>
        private ConfigurationFileConfig GetSystemFileConfig()
        {
            var filePath = this.BuildConfigFilePath("ConfigFiles.config");
            return this._serializer.DeserializeXmlFromFile<ConfigurationFileConfig>(filePath);
        }
    }
}
