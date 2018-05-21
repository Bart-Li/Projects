using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Eqi.Core.Reflection;

namespace Eqi.Core.Configuration.Impl
{
    [DependencyInjection(typeof(IConfigRepository))]
    public class DefaultConfigRepository : IConfigRepository
    {
        private readonly ICurrentAppDomain _currentAppDomain;
        public DefaultConfigRepository(ICurrentAppDomain currentAppDomain)
        {
            this._currentAppDomain = currentAppDomain;
        }

        /// <summary>
        /// Get config files.
        /// </summary>
        /// <returns>All config files.</returns>
        public IList<IConfigFileDefinition> GetConfigFiles()
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
        /// Get config file by key.
        /// </summary>
        /// <param name="configName">Config name.</param>
        /// <returns>Config file.</returns>
        public IConfigFileDefinition GetConfigFileByName(string configName)
        {
            var allConfigFiles = this.GetConfigFiles();
            if (!allConfigFiles.IsNullOrEmpty())
            {
                return allConfigFiles.Find(file => file.Name.Equals(configName, StringComparison.OrdinalIgnoreCase));
            }

            return null;
        }

        private ConfigurationFileConfig GetSystemFileConfig()
        {
            var filePath = Path.Combine(this._currentAppDomain.BaseDirectory, "Configuration/ConfigFiles.config");

            FileStream stream = null;
            ConfigurationFileConfig configurationFileConfig = null;
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ConfigurationFileConfig));
                stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                configurationFileConfig = (ConfigurationFileConfig)serializer.Deserialize(stream);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }

            return configurationFileConfig;
        }
    }
}
