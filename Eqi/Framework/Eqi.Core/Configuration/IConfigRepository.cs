using System.Collections.Generic;

namespace Eqi.Core.Configuration
{
    public interface IConfigRepository
    {
        /// <summary>
        /// Get config files.
        /// </summary>
        /// <returns>All config files.</returns>
        IList<IConfigFileDefinition> GetConfigFiles();

        /// <summary>
        /// Get config file by key.
        /// </summary>
        /// <param name="configName">Config name.</param>
        /// <returns>Config file.</returns>
        IConfigFileDefinition GetConfigFileByName(string configName);
    }
}
