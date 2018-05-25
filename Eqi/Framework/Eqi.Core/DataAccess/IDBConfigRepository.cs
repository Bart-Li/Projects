using Eqi.Core.DataAccess.Config;

namespace Eqi.Core.DataAccess
{
    /// <summary>
    /// DB configuration repository.
    /// </summary>
    public interface IDBConfigRepository
    {
        /// <summary>
        /// Get data command.
        /// </summary>
        /// <param name="dataCommandName">Data command name.</param>
        /// <returns>Data command descriptor.</returns>
        DataCommandUnit GetDataCommand(string dataCommandName);

        /// <summary>
        /// Get database by name.
        /// </summary>
        /// <param name="databaseName">Database name.</param>
        /// <returns>Database unit.</returns>
        DataBaseUnit GetDataBase(string databaseName);
    }
}
