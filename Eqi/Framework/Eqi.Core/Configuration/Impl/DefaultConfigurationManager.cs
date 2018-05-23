using System;
using System.Reflection;

namespace Eqi.Core.Configuration.Impl
{
    /// <summary>
    /// Configuration manager.
    /// </summary>
    [DependencyInjection(typeof(IConfigurationManager))]
    public class DefaultConfigurationManager : IConfigurationManager
    {
        private readonly IConfigRepository _configRepository;
        private readonly IConfigAccessor _configAccessor;

        public DefaultConfigurationManager(IConfigRepository configRepository, IConfigAccessor configAccessor)
        {
            this._configRepository = configRepository;
            this._configAccessor = configAccessor;
        }

        /// <summary>
        /// Get an instance of the given configuration with attribute or name.
        /// </summary>
        /// <typeparam name="TConfig">Type of configuration.</typeparam>
        /// <returns>The requested configuration.</returns>
        public TConfig GetConfiguration<TConfig>() where TConfig : class
        {
            Type type = typeof(TConfig);
            var attribute = type.GetCustomAttribute<ConfigFileAttribute>();
            if (attribute != null)
            {
                IConfigFileDefinition config = this._configRepository.GetConfigFileDefinitionByAttribute(attribute);
                if (config != null)
                {
                    return this._configAccessor.GetConfigValue<TConfig>(config);
                }
            }
            else
            {
                return GetConfiguration<TConfig>(string.Format("{0}.config", type.Name));
            }

            return default(TConfig);
        }

        /// <summary>
        /// Get an instance of the given configuration by file path.
        /// </summary>
        /// <typeparam name="TConfig">Type of configuration.</typeparam>
        /// <param name="filePath">Config file path. File path is support absolute and relative paths. Relastive path is base on appdomain + configuration folder.</param>
        /// <returns>The requested configuration.</returns>
        public TConfig GetConfiguration<TConfig>(string filePath) where TConfig : class
        {
            IConfigFileDefinition config = this._configRepository.GetConfigFileDefinitionByFilePath(filePath);
            if (config != null)
            {
                return this._configAccessor.GetConfigValue<TConfig>(config);
            }

            return default(TConfig);
        }

        /// <summary>
        /// Get an instance of the given configuration by key.
        /// </summary>
        /// <typeparam name="TConfig">Type of configuration.</typeparam>
        /// <param name="key">Requested key.</param>
        /// <returns>The requested configuration.</returns>
        public TConfig GetConfigurationByKey<TConfig>(string key) where TConfig : class
        {
            IConfigFileDefinition config = this._configRepository.GetConfigFileDefinitionByName(key);
            if (config != null)
            {
                return this._configAccessor.GetConfigValue<TConfig>(config);
            }

            return default(TConfig);
        }
    }
}
