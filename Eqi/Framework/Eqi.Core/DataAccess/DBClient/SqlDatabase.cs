using System.Data.SqlClient;

namespace Eqi.Core.DataAccess.DBClient
{
    public class SqlDatabase : Database
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlDatabase"/> class with a connection string.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public SqlDatabase(string connectionString)
            : base(connectionString, SqlClientFactory.Instance)
        {
        }
    }
}
