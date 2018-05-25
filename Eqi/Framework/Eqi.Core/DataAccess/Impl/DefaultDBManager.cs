using System;
using Eqi.Core.DataAccess.Config;

namespace Eqi.Core.DataAccess.Impl
{
    /// <summary>
    /// Default data access manager.
    /// </summary>
    [DependencyInjection(typeof(IDBManager))]
    public class DefaultDBManager : IDBManager
    {
        private readonly IDBConfigRepository _dBConfigRepository;

        public DefaultDBManager(IDBConfigRepository dBConfigRepository)
        {
            this._dBConfigRepository = dBConfigRepository;
        }

        /// <summary>
        /// Create db command by configured command name.
        /// </summary>
        /// <param name="commandName">Configured command name.</param>
        /// <returns>Db command.</returns>
        public IDataCommand CreateDBCommand(string commandName)
        {
            IDataCommand result = null;
            if (string.IsNullOrEmpty(commandName))
            {
                throw new ArgumentNullException("DB command name is null or empty.");
            }
            else
            {
                var commandUnit = this._dBConfigRepository.GetDataCommand(commandName);
                if (commandUnit != null)
                {
                    var databaseName = commandUnit.Database;
                    if (!string.IsNullOrWhiteSpace(databaseName))
                    {
                        var databaseUnit = this._dBConfigRepository.GetDataBase(databaseName);
                        if (databaseUnit != null)
                        {
                            result = new DataCommand(databaseUnit, commandUnit);
                        }
                        else
                        {
                            throw new ArgumentNullException($"Database \"{databaseName}\" not found.");
                        }
                    }
                    else
                    {
                        throw new ArgumentNullException($"The database name of DB command \"{commandName}\" is missing.");
                    }
                }
                else
                {
                    throw new ArgumentNullException($"DB command \"{commandName}\" not found.");
                }
            }

            return result;
        }

        /// <summary>
        /// Create db command by configured command name.
        /// </summary>
        /// <param name="databaseName">Configured database name.</param>
        /// <param name="commandName">Configured command name.</param>
        /// <returns>Db command.</returns>
        public IDataCommand CreateDBCommand(string databaseName, string commandName)
        {
            IDataCommand result = null;
            if (string.IsNullOrEmpty(commandName))
            {
                throw new ArgumentNullException("DB command name is null or empty.");
            }
            else
            {
                var commandUnit = this._dBConfigRepository.GetDataCommand(commandName);
                if (commandUnit != null)
                {
                    if (!string.IsNullOrWhiteSpace(databaseName))
                    {
                        var databaseUnit = this._dBConfigRepository.GetDataBase(databaseName);
                        if (databaseUnit != null)
                        {
                            result = new DataCommand(databaseUnit, commandUnit);
                        }
                        else
                        {
                            throw new ArgumentNullException($"Database \"{databaseName}\" not found.");
                        }
                    }
                    else
                    {
                        throw new ArgumentNullException($"The database name of DB command \"{commandName}\" is missing.");
                    }
                }
                else
                {
                    throw new ArgumentNullException($"DB command \"{commandName}\" not found.");
                }
            }

            return result;
        }

        /// <summary>
        /// Create custom db command.
        /// </summary>
        /// <param name="databaseName">Configured database name.</param>
        /// <param name="commandText">Command text.</param>
        /// <returns>Db command.</returns>
        public IDataCommand CreateCustomDBCommand(string databaseName, string commandText)
        {
            DataCommand result = null;

            if (string.IsNullOrWhiteSpace(commandText))
            {
                throw new ArgumentNullException("Custom command text is null or empty.");
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(databaseName))
                {
                    var databaseUnit = this._dBConfigRepository.GetDataBase(databaseName);
                    if (databaseUnit != null)
                    {
                        var commandUnit = new DataCommandUnit();
                        commandUnit.CommandType = "Text";
                        commandUnit.CommandText = commandText;
                        result = new DataCommand(databaseUnit, commandUnit);
                    }
                    else
                    {
                        throw new ArgumentNullException($"Database \"{databaseName}\" not found.");
                    }
                }
                else
                {
                    throw new ArgumentNullException("The database name is null or empty.");
                }
            }

            return result;
        }
    }
}
