using System;
using System.Collections.Generic;
using System.Linq;
using Eqi.Core.Configuration;
using Eqi.Core.DataAccess.Config;

namespace Eqi.Core.DataAccess.Impl
{
    /// <summary>
    /// Default db configuration respostory.
    /// </summary>
    [DependencyInjection(typeof(IDBConfigRepository))]
    public class DefaultDBConfigRepository : IDBConfigRepository
    {
        /// <summary>
        /// Configuration manager.
        /// </summary>
        private readonly IConfigurationManager _configManager;

        public DefaultDBConfigRepository(IConfigurationManager configManager)
        {
            this._configManager = configManager;
        }

        /// <summary>
        /// Get data command.
        /// </summary>
        /// <param name="dataCommandName">Data command name.</param>
        /// <returns>Data command descriptor.</returns>
        public DataCommandUnit GetDataCommand(string dataCommandName)
        {
            DataCommandUnit result = null;
            var dataCommandList = this.GetDataCommandConfigList();
            if (!dataCommandList.IsNullOrEmpty())
            {
                foreach (var dataCommand in dataCommandList)
                {
                    var dataCommandConfig = this._configManager.GetConfiguration<DataCommandsConfig>("Data/" + dataCommand);
                    if (dataCommandConfig != null && !dataCommandConfig.DataCommandCollection.IsNullOrEmpty())
                    {
                        result = dataCommandConfig.DataCommandCollection.Find(command => command.Name.Equals(dataCommandName, StringComparison.OrdinalIgnoreCase));
                        if (result != null)
                        {
                            break;
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Get database by name.
        /// </summary>
        /// <param name="databaseName">Database name.</param>
        /// <returns>Database unit.</returns>
        public DataBaseUnit GetDataBase(string databaseName)
        {
            DataBaseUnit result = null;

            if (string.IsNullOrWhiteSpace(databaseName))
            {
                return null;
            }

            var dataBasesConfig = this._configManager.GetConfiguration<DataBasesConfig>();
            if (dataBasesConfig != null && !dataBasesConfig.DatabaseGroups.IsNullOrEmpty())
            {
                IList<DataBaseUnit> databaseUnits = dataBasesConfig.DatabaseGroups.SelectMany(group => group.DatabaseCollection).ToList();
                if (databaseUnits != null && databaseUnits.Any())
                {
                    result = databaseUnits.FirstOrDefault(database => database.Name.Equals(databaseName, StringComparison.OrdinalIgnoreCase));
                }
            }

            return result;
        }

        private List<string> GetDataCommandConfigList()
        {
            var dataCommandList = new List<string>();
            var dataAccessConfig = this._configManager.GetConfiguration<DataCommandFilesConfig>();
            if (dataAccessConfig != null && !dataAccessConfig.DataCommandFiles.IsNullOrEmpty())
            {
                dataCommandList = dataAccessConfig.DataCommandFiles.Select(n => n.Name).ToList();
            }
            else
            {
                dataCommandList.Add("DataCommands.config");
            }

            return dataCommandList;
        }
    }
}
