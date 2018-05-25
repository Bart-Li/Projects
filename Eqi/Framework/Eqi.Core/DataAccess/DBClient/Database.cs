using System;
using System.Data;
using System.Data.Common;
using System.Globalization;
using Eqi.Core.DataAccess.DBClient;

namespace Eqi.Core.DataAccess
{
    public abstract class Database
    {
        /// <summary>
        /// Connection string.
        /// </summary>
        private readonly string connectionString;

        /// <summary>
        /// Database provider factory.
        /// </summary>
        private readonly DbProviderFactory dbproviderfactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="Database"/> class with a connection string,
        /// a DbProviderFactory and an IDataInstrumentationProvider.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="dbproviderfactory"></param>
        protected Database(string connectionString, DbProviderFactory dbproviderfactory)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException("The value can not be null or an empty string.", "connectionString");
            }

            this.connectionString = connectionString;
            this.dbproviderfactory = dbproviderfactory;
        }

        /// <summary>
        /// Connection string.
        /// </summary>
        public string ConnectionString => this.connectionString;

        /// <summary>
        /// <para>Executes the <paramref name="command"/> and returns the number of rows affected.</para>
        /// </summary>
        /// <param name="command">
        /// <para>The command that contains the query to execute.</para>
        /// </param>       
        /// <returns>Effect rows count.</returns>
        /// <seealso cref="IDbCommand.ExecuteScalar"/>
        public virtual int ExecuteNonQuery(DbCommand command)
        {
            using (var wrapper = this.GetOpenConnection())
            {
                PrepareCommand(command, wrapper.Connection);
                return this.DoExecuteNonQuery(command);
            }
        }

        /// <summary>
        /// <para>Executes the <paramref name="command"/> and returns an <see cref="IDataReader"></see> through which the result can be read.
        /// It is the responsibility of the caller to close the reader when finished.</para>
        /// </summary>
        /// <param name="command">
        /// <para>The command that contains the query to execute.</para>
        /// </param>
        /// <returns>
        /// <para>An <see cref="IDataReader"/> object.</para>
        /// </returns>        
        public virtual IDataReader ExecuteReader(DbCommand command)
        {
            using (DatabaseConnectionWrapper wrapper = this.GetOpenConnection())
            {
                PrepareCommand(command, wrapper.Connection);
                IDataReader realReader = this.DoExecuteReader(command, CommandBehavior.Default);
                return this.CreateWrappedReader(wrapper, realReader);
            }
        }

        /// <summary>
        /// <para>Executes the <paramref name="command"/> and returns the first column of the first row in the result set returned by the query. Extra columns or rows are ignored.</para>
        /// </summary>
        /// <param name="command">
        /// <para>The command that contains the query to execute.</para>
        /// </param>
        /// <returns>
        /// <para>The first column of the first row in the result set.</para>
        /// </returns>
        /// <seealso cref="IDbCommand.ExecuteScalar"/>
        public virtual object ExecuteScalar(DbCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            using (var wrapper = this.GetOpenConnection())
            {
                PrepareCommand(command, wrapper.Connection);
                return command.ExecuteScalar();
            }
        }

        /// <summary>
        /// <para>Executes the <paramref name="command"/> and returns the results in a new <see cref="DataSet"/>.</para>
        /// </summary>
        /// <param name="command"><para>The <see cref="DbCommand"/> to execute.</para></param>
        /// <returns>A <see cref="DataSet"/> with the results of the <paramref name="command"/>.</returns>        
        public virtual DataSet ExecuteDataSet(DbCommand command)
        {
            DataSet dataSet = new DataSet
            {
                Locale = CultureInfo.InvariantCulture
            };
            using (var wrapper = this.GetOpenConnection())
            {
                PrepareCommand(command, wrapper.Connection);
                this.DoLoadDataSet(command, dataSet, new[] { "Table" });
            }

            return dataSet;
        }

        /// <summary>
        /// <para>Creates a connection for this database.</para>
        /// </summary>
        /// <returns>
        /// <para>The <see cref="DbConnection"/> for this database.</para>
        /// </returns>
        /// <seealso cref="DbConnection"/>        
        public virtual DbConnection CreateConnection()
        {
            DbConnection newConnection = this.dbproviderfactory.CreateConnection();
            newConnection.ConnectionString = this.ConnectionString;

            return newConnection;
        }

        /// <summary>
        /// <para>Adds a new instance of a <see cref="DbParameter"/> object.</para>
        /// </summary>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="dbtype"><para>One of the <see cref="DbType"/> values.</para></param>
        /// <param name="size"><para>The maximum size of the data within the column.</para></param>
        /// <param name="direction"><para>One of the <see cref="ParameterDirection"/> values.</para></param>
        /// <param name="precision"><para>The maximum number of digits used to represent the <paramref name="value"/>.</para></param>
        /// <param name="scale"><para>The number of decimal places to which <paramref name="value"/> is resolved.</para></param>
        /// <param name="value"><para>The value of the parameter.</para></param>  
        /// <returns>A newly created <see cref="DbParameter"/> fully initialized with given parameters.</returns>
        internal DbParameter CreateParameter(string name, DbType dbtype, int size, ParameterDirection direction, byte precision, byte scale, object value)
        {
            return this.CreateParameter(name, dbtype, size, direction, true, precision, scale, string.Empty, DataRowVersion.Default, value);
        }

        /// <summary>
        /// Create db commond by command type.
        /// </summary>
        /// <param name="commandType">Db command type.</param>
        /// <param name="commandText">Db command text.</param>
        /// <returns>Db commond.</returns>
        internal DbCommand CreateCommand(CommandType commandType, string commandText)
        {
            DbCommand command = this.dbproviderfactory.CreateCommand();
            command.CommandType = commandType;
            command.CommandText = commandText;

            return command;
        }

        /// <summary>
        /// Get new open connection.
        /// </summary>
        /// <returns>Db connection.</returns>
        internal DbConnection GetNewOpenConnection()
        {
            DbConnection connection = null;
            try
            {
                try
                {
                    connection = this.CreateConnection();
                    connection.Open();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            catch
            {
                if (connection != null)
                {
                    connection.Close();
                }

                throw;
            }

            return connection;
        }

        /// <summary>
        ///		Gets a "wrapped" connection that will be not be disposed if a transaction is
        ///		active (created by creating a TransactionScope instance). The
        ///		connection will be disposed when no transaction is active.
        /// </summary>
        /// <returns>Database connection wrapper.</returns>
        protected DatabaseConnectionWrapper GetOpenConnection()
        {
            DatabaseConnectionWrapper connection = TransactionScopeConnections.GetConnection(this);
            return connection ?? this.GetWrappedConnection();
        }

        /// <summary>
        /// Gets a "wrapped" connection for use outside a transaction.
        /// </summary>
        /// <returns>The wrapped connection.</returns>
        protected virtual DatabaseConnectionWrapper GetWrappedConnection()
        {
            return new DatabaseConnectionWrapper(this.GetNewOpenConnection());
        }

        /// <summary>
        /// <para>Adds a new instance of a <see cref="DbParameter"/> object.</para>
        /// </summary>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="dbtype"><para>One of the <see cref="DbType"/> values.</para></param>
        /// <param name="size"><para>The maximum size of the data within the column.</para></param>
        /// <param name="direction"><para>One of the <see cref="ParameterDirection"/> values.</para></param>
        /// <param name="nullable"><para>Avalue indicating whether the parameter accepts <see langword="null"/> (<b>Nothing</b> in Visual Basic) values.</para></param>
        /// <param name="precision"><para>The maximum number of digits used to represent the <paramref name="value"/>.</para></param>
        /// <param name="scale"><para>The number of decimal places to which <paramref name="value"/> is resolved.</para></param>
        /// <param name="sourceColumn"><para>The name of the source column mapped to the DataSet and used for loading or returning the <paramref name="value"/>.</para></param>
        /// <param name="sourceVersion"><para>One of the <see cref="DataRowVersion"/> values.</para></param>
        /// <param name="value"><para>The value of the parameter.</para></param>  
        /// <returns>A newly created <see cref="DbParameter"/> fully initialized with given parameters.</returns>
        protected DbParameter CreateParameter(string name, DbType dbtype, int size, ParameterDirection direction, bool nullable, byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, object value)
        {
            DbParameter param = CreateParameter(name);
            this.ConfigureParameter(param, name, dbtype, size, direction, nullable, precision, scale, sourceColumn, sourceVersion, value);
            return param;
        }

        /// <summary>
        /// Configures a given <see cref="DbParameter"/>.
        /// </summary>
        /// <param name="param">The <see cref="DbParameter"/> to configure.</param>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="dbtype"><para>One of the <see cref="DbType"/> values.</para></param>
        /// <param name="size"><para>The maximum size of the data within the column.</para></param>
        /// <param name="direction"><para>One of the <see cref="ParameterDirection"/> values.</para></param>
        /// <param name="nullable"><para>Avalue indicating whether the parameter accepts <see langword="null"/> (<b>Nothing</b> in Visual Basic) values.</para></param>
        /// <param name="precision"><para>The maximum number of digits used to represent the <paramref name="value"/>.</para></param>
        /// <param name="scale"><para>The number of decimal places to which <paramref name="value"/> is resolved.</para></param>
        /// <param name="sourceColumn"><para>The name of the source column mapped to the DataSet and used for loading or returning the <paramref name="value"/>.</para></param>
        /// <param name="sourceVersion"><para>One of the <see cref="DataRowVersion"/> values.</para></param>
        /// <param name="value"><para>The value of the parameter.</para></param>  
        protected virtual void ConfigureParameter(DbParameter param, string name, DbType dbtype, int size, ParameterDirection direction, bool nullable, byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, object value)
        {
            param.DbType = dbtype;
            param.Size = size;
            param.Value = value ?? DBNull.Value;
            param.Direction = direction;
            param.IsNullable = nullable;
            param.SourceColumn = sourceColumn;
            param.SourceVersion = sourceVersion;
            (param as IDbDataParameter).Precision = precision;
            (param as IDbDataParameter).Scale = scale;
        }

        /// <summary>
        /// <para>Adds a new instance of a <see cref="DbParameter"/> object.</para>
        /// </summary>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <returns><para>An unconfigured parameter.</para></returns>
        protected DbParameter CreateParameter(string name)
        {
            DbParameter param = this.dbproviderfactory.CreateParameter();
            param.ParameterName = name;

            return param;
        }

        /// <summary>
        /// <para>Assigns a <paramref name="connection"/> to the <paramref name="command"/> and discovers parameters if needed.</para>
        /// </summary>
        /// <param name="command"><para>The command that contains the query to prepare.</para></param>
        /// <param name="connection">The connection to assign to the command.</param>
        protected static void PrepareCommand(DbCommand command, DbConnection connection)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }
            else if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            command.Connection = connection;
        }

        /// <summary>
        /// Executes the query for <paramref name="command"/>.
        /// </summary>
        /// <param name="command">The <see cref="DbCommand"/> representing the query to execute.</param>
        /// <returns>The quantity of rows affected.</returns>
        protected int DoExecuteNonQuery(DbCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            try
            {
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// All data readers get wrapped in objects so that they properly manage connections.
        /// Some derived Database classes will need to create a different wrapper, so this
        /// method is provided so that they can do this.
        /// </summary>
        /// <param name="connection">Connection + refcount.</param>
        /// <param name="innerReader">The reader to wrap.</param>
        /// <returns>The new reader.</returns>
        protected virtual IDataReader CreateWrappedReader(DatabaseConnectionWrapper connection, IDataReader innerReader)
        {
            return new RefCountingDataReader(connection, innerReader);
        }

        /// <summary>
        /// Do execute reader.
        /// </summary>
        /// <param name="command">Db command.</param>
        /// <param name="cmdBehavior">Db behavior.</param>
        /// <returns>Data reader.</returns>
        protected virtual IDataReader DoExecuteReader(DbCommand command, CommandBehavior cmdBehavior)
        {
            try
            {
                IDataReader reader = command.ExecuteReader(cmdBehavior);
                return reader;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Do load data set.
        /// </summary>
        /// <param name="command">Db command.</param>
        /// <param name="dataSet">Data set.</param>
        /// <param name="tableNames">Table names.</param>
        protected virtual void DoLoadDataSet(IDbCommand command, DataSet dataSet, string[] tableNames)
        {
            if (tableNames == null)
            {
                throw new ArgumentNullException("tableNames");
            }

            using (DbDataAdapter adapter = this.dbproviderfactory.CreateDataAdapter())
            {
                ((IDbDataAdapter)adapter).SelectCommand = command;

                try
                {
                    string systemCreatedTableNameRoot = "Table";
                    for (int i = 0; i < tableNames.Length; i++)
                    {
                        string systemCreatedTableName = (i == 0)
                                                            ? systemCreatedTableNameRoot
                                                            : systemCreatedTableNameRoot + i;

                        adapter.TableMappings.Add(systemCreatedTableName, tableNames[i]);
                    }

                    adapter.Fill(dataSet);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
    }
}
