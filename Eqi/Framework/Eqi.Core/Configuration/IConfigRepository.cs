using System.Collections.Generic;

namespace Eqi.Core.Configuration
{
    /// <summary>
    /// Config repository.
    /// </summary>
    public interface IConfigRepository
    {
        /// <summary>
        /// Get default system config directory.
        /// </summary>
        string DefaultConfigDirectory { get; }

        /// <summary>
        /// Get all config file definition.
        /// </summary>
        /// <returns>All config file definition.</returns>
        IList<IConfigFileDefinition> GetAllConfigFileDefinition();

        /// <summary>
        /// Get config file definition by key.
        /// </summary>
        /// <param name="configName">Config name.</param>
        /// <returns>Config file definition.</returns>
        IConfigFileDefinition GetConfigFileDefinitionByName(string configName);

        /// <summary>
        /// Get config file definition by attribute.
        /// </summary>
        /// <param name="attribute">Config file Attribute.</param>
        /// <returns>Config file definition.</returns>
        IConfigFileDefinition GetConfigFileDefinitionByAttribute(ConfigFileAttribute attribute);

        /// <summary>
        /// Get config file definition by file path.
        /// </summary>
        /// <param name="filePath">File path.</param>
        /// <returns>Config file definition.</returns>
        IConfigFileDefinition GetConfigFileDefinitionByFilePath(string filePath);
    }
}
