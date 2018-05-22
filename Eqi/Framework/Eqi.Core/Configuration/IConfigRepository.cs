using System.Collections.Generic;

namespace Eqi.Core.Configuration
{
    public interface IConfigRepository
    {
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
    }
}
