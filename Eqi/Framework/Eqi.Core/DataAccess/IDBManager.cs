namespace Eqi.Core.DataAccess
{
    /// <summary>
    /// DB manager.
    /// </summary>
    public interface IDBManager
    {
        /// <summary>
        /// Create db command by configured command name.
        /// </summary>
        /// <param name="commandName">Configured command name.</param>
        /// <returns>Db command.</returns>
        IDataCommand CreateDBCommand(string commandName);

        /// <summary>
        /// Create db command by configured command name.
        /// </summary>
        /// <param name="databaseName">Configured database name.</param>
        /// <param name="commandName">Configured command name.</param>
        /// <returns>Db command.</returns>
        IDataCommand CreateDBCommand(string databaseName, string commandName);

        /// <summary>
        /// Create custom db command.
        /// </summary>
        /// <param name="databaseName">Configured database name.</param>
        /// <param name="commandText">Command text.</param>
        /// <returns>Db command.</returns>
        IDataCommand CreateCustomDBCommand(string databaseName, string commandText);
    }
}
