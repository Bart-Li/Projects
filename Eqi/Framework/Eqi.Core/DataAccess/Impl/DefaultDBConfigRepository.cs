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
            //var dataCommandList = this.GetDataCommandConfigList();
            //if (!dataCommandList.IsNullOrEmpty())
            //{
            //    var systemName = this.GetSystemName();
            //    foreach (var dataCommand in dataCommandList)
            //    {
            //        var dataCommandConfig = this._configManager.GetConfigFromService<DataCommandsConfig>(dataCommand, systemName, NodeDataType.Xml);
            //        if (dataCommandConfig != null && !dataCommandConfig.DataCommandCollection.IsNullOrEmpty())
            //        {
            //            result = dataCommandConfig.DataCommandCollection.Find(command => command.Name.Equals(dataCommandName, StringComparison.OrdinalIgnoreCase));
            //            if (result != null)
            //            {
            //                break;
            //            }
            //        }
            //    }
            //}

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

            //var dataBasesConfig = this._configManager.GetConfigFromService<DataBasesConfig>(this.GetDatabaseConfigName(), this.GetSystemName(), NodeDataType.Xml);
            //if (dataBasesConfig != null && !dataBasesConfig.DatabaseGroups.IsNullOrEmpty())
            //{
            //    IList<DataBaseUnit> databaseUnits = dataBasesConfig.DatabaseGroups.SelectMany(group => group.DatabaseCollection).ToList();
            //    if (databaseUnits != null && databaseUnits.Any())
            //    {
            //        result = databaseUnits.FirstOrDefault(database => database.Name.Equals(databaseName, StringComparison.OrdinalIgnoreCase));
            //    }
            //}

            return result;
        }

        private string GetSystemName()
        {
            var systemName = string.Empty;
            //var dataAccessConfig = this._configManager.GetSection<DataAccessConfig>("DataAccessConfig");
            //if (dataAccessConfig != null && !string.IsNullOrWhiteSpace(dataAccessConfig.SystemName))
            //{
            //    systemName = dataAccessConfig.SystemName;
            //}

            return systemName;
        }

        private string GetDatabaseConfigName()
        {
            var databaseConfigName = "Databases";
            //var dataAccessConfig = this._configManager.GetSection<DataAccessConfig>("DataAccessConfig");
            //if (dataAccessConfig != null && !string.IsNullOrWhiteSpace(dataAccessConfig.DatabaseConfig))
            //{
            //    databaseConfigName = dataAccessConfig.DatabaseConfig;
            //}

            return databaseConfigName;
        }

        private List<string> GetDataCommandConfigList()
        {
            var dataCommandList = new List<string>();
            //var dataAccessConfig = this._configManager.GetSection<DataAccessConfig>("DataAccessConfig");
            //if (dataAccessConfig != null && !dataAccessConfig.DataCommandConfig.IsNullOrEmpty())
            //{
            //    dataCommandList = dataAccessConfig.DataCommandConfig;
            //}
            //else
            //{
            //    dataCommandList.Add("DataCommands");
            //}

            return dataCommandList;
        }
    }
}
