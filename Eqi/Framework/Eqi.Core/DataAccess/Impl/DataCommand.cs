using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Eqi.Core.DataAccess.Config;
using Eqi.Core.DataAccess.DBClient;

namespace Eqi.Core.DataAccess.Impl
{
    public class DataCommand : IDataCommand
    {
        #region Private field

        private readonly DataBaseUnit databaseConfig;
        private readonly DataCommandUnit dataCommandConfig;

        /// <summary>
        /// Database provider.
        /// </summary>
        private readonly Lazy<DatabaseProvider> _databaseProvider = new Lazy<DatabaseProvider>();

        /// <summary>
        /// Field of my parameters.
        /// </summary>
        private readonly Lazy<List<DbParameter>> _myParameters = new Lazy<List<DbParameter>>();

        /// <summary>
        /// Field of my group parameters.
        /// </summary>
        private readonly Lazy<Dictionary<string, string>> _myGroupParameters = new Lazy<Dictionary<string, string>>();

        /// <summary>
        /// Field of format args.
        /// </summary>
        private readonly List<object> _formatArgs = new List<object>();

        #endregion

        #region Construct method

        /// <summary>
        /// Initializes a new instance of the DataCommand class.
        /// </summary>
        /// <param name="database">Database descriptor.</param>
        /// <param name="dataCommand">Data command descriptor.</param>
        public DataCommand(DataBaseUnit database, DataCommandUnit dataCommand)
        {
            this.databaseConfig = database;
            this.dataCommandConfig = dataCommand;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets db command name.
        /// </summary>
        public string Name => this.dataCommandConfig.Name;

        /// <summary>
        /// Gets database name.
        /// </summary>
        public string DatabaseName => this.databaseConfig.Name;

        /// <summary>
        /// Gets parameters.
        /// </summary>
        public IList<DbParameter> Parameters => this._myParameters.Value;

        #endregion

        #region Add parameters

        /// <summary>
        /// Adds a new in parameter object to this command.
        /// </summary>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="dbtype"><para>One of the <see cref="DbType"/> values.</para></param>                
        /// <param name="value"><para>The value of the parameter.</para></param>      
        public void AddInParameter(string name, DbType dbtype, object value)
        {
            this.AddInParameter(name, dbtype, 0, 0, 0, value);
        }

        /// <summary>
        /// Adds a new in parameter object to this command.
        /// </summary>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="dbType"><para>One of the <see cref="DbType"/> values.</para></param>   
        /// <param name="size"><para>The maximum size of the data within the column.</para></param>
        /// <param name="precision"><para>The maximum number of digits used to represent the <paramref name="value"/>.</para></param>
        /// <param name="scale"><para>The number of decimal places to which <paramref name="value"/> is resolved.</para></param>
        /// <param name="value"><para>The value of the parameter.</para></param>      
        public void AddInParameter(string name, DbType dbType, int size, byte precision, byte scale, object value)
        {
            Database db = this._databaseProvider.Value.CreateDatabase(this.databaseConfig.ConnectionString, this.databaseConfig.Type);

            if (db != null)
            {
                DbParameter parameter = db.CreateParameter(name, dbType, size, ParameterDirection.Input, precision, scale, value);
                this.AddParameter(parameter);
            }
        }

        /// <summary>
        /// Add parameter to command.
        /// </summary>
        /// <param name="parameter">Parameter instance.</param>
        public void AddParameter(DbParameter parameter)
        {
            this._myParameters.Value.Add(parameter);
        }

        /// <summary>
        /// Add parameter to command.
        /// </summary>
        /// <param name="parameterName">Parameter name.</param>
        /// <param name="parameterValue">Parameter value.</param>
        public void AddParameter(string parameterName, object parameterValue)
        {
            DataParameterUnit currentParameterDescriptor = this.GetParameterDescriptor(parameterName);
            if (currentParameterDescriptor == null)
            {
                throw new ArgumentNullException(string.Format(@"Not find parameter ""{0}"" from config file", parameterName));
            }

            if (currentParameterDescriptor.IsOutput)
            {
                throw new ArgumentException(string.Format(@"The parameter ""{0}"" is output.", parameterName));
            }

            this.AddInParameter(currentParameterDescriptor.Name, currentParameterDescriptor.DbType, currentParameterDescriptor.Size, currentParameterDescriptor.Precision, currentParameterDescriptor.Scale, parameterValue);
        }

        /// <summary>
        /// Adds a new out parameter object to this command.
        /// </summary>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="dbtype"><para>One of the <see cref="DbType"/> values.</para></param>        
        /// <param name="size"><para>The maximum size of the data within the parameter.</para></param>   
        /// <param name="precision"><para>The maximum number of digits used to represent the parameter.</para></param>
        /// <param name="scale"><para>The number of decimal places to which parameter is resolved.</para></param>
        public void AddOutParameter(string name, DbType dbtype, int size, byte precision, byte scale)
        {
            Database db = this._databaseProvider.Value.CreateDatabase(this.databaseConfig.ConnectionString, this.databaseConfig.Type);

            if (db != null)
            {
                DbParameter parameter = db.CreateParameter(name, dbtype, size, ParameterDirection.Output, precision, scale, DBNull.Value);
                this.AddParameter(parameter);
            }
        }

        /// <summary>
        /// Adds a new Out <see cref="DbParameter"/> object to this command.
        /// </summary>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="dbtype"><para>One of the <see cref="DbType"/> values.</para></param>            
        public void AddOutParameter(string name, DbType dbtype)
        {
            this.AddOutParameter(name, dbtype, 0, 0, 0);
        }

        /// <summary>
        /// Add parameter group to command.
        /// </summary>
        /// <param name="parameterGroupName">Parameter group name.</param>
        /// <param name="parameterValues">Parameter group value.</param>
        public void AddGroupParameter(string parameterGroupName, IEnumerable parameterValues)
        {
            DataParameterGroupUnit currentParameterGroupDescriptor = this.GetParameterGroupDescriptor(parameterGroupName);
            if (currentParameterGroupDescriptor == null)
            {
                throw new ArgumentNullException(string.Format(@"Not find parameter group ""{0}"" from configure file", parameterGroupName));
            }

            StringBuilder finalParameters = new StringBuilder();
            int index = 1;
            foreach (object value in parameterValues)
            {
                this.AddInParameter(string.Concat(currentParameterGroupDescriptor.Name, index), currentParameterGroupDescriptor.DbType, currentParameterGroupDescriptor.Size, currentParameterGroupDescriptor.Precision, currentParameterGroupDescriptor.Scale, value);

                if (finalParameters.Length > 0)
                {
                    finalParameters.Append(", ");
                }

                finalParameters.Append("@");
                finalParameters.Append(parameterGroupName.Trim('@'));
                finalParameters.Append(index);

                index++;
            }

            string groupName = string.Concat(parameterGroupName);

            if (this._myGroupParameters.Value.ContainsKey(groupName))
            {
                this._myGroupParameters.Value[groupName] = string.Format("({0})", finalParameters.ToString());
            }
            else
            {
                this._myGroupParameters.Value.Add(groupName, string.Format("({0})", finalParameters.ToString()));
            }
        }

        /// <summary>
        /// Add format argument.
        /// </summary>
        /// <param name="args">Argument value.</param>
        public void AddFormatArgs(object args)
        {
            this._formatArgs.Add(args);
        }

        #endregion

        #region Get parameters

        /// <summary>
        /// Get parameter value.
        /// </summary>
        /// <param name="name">Parameter name.</param>
        /// <returns>Parameter value.</returns>
        public object GetParameterValue(string name)
        {
            object result = null;

            if (!string.IsNullOrWhiteSpace(name))
            {
                if (this._myParameters.Value != null && this._myParameters.Value.Any())
                {
                    DbParameter parameter = this._myParameters.Value.FirstOrDefault(para => string.Equals(name.Trim('@'), para.ParameterName.Trim('@'), StringComparison.OrdinalIgnoreCase));

                    if (parameter != null)
                    {
                        result = parameter.Value;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Get parameter value.
        /// </summary>
        /// <typeparam name="T">Parameter value type.</typeparam>
        /// <param name="name">Parameter name.</param>
        /// <returns>Parameter value.</returns>
        public T GetParameterValue<T>(string name)
        {
            object value = GetParameterValue(name);
            return (T)value;
        }

        #endregion

        #region Execute

        /// <summary>
        /// Executes a SQL statement against a connection object.
        /// </summary>
        /// <returns>The number of rows affected.</returns>
        public int ExecuteNonQuery()
        {
            int result = default(int);
            Database db = this._databaseProvider.Value.CreateDatabase(this.databaseConfig.ConnectionString, this.databaseConfig.Type);

            if (db != null)
            {
                DbCommand command = this.GetRealCommand(db);

                if (command != null)
                {
                    try
                    {
                        result = db.ExecuteNonQuery(command);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        command.Parameters.Clear();
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Executes the query and returns the entity of the first row in the result set returned by the query. All other columns and rows are ignored.
        /// </summary>
        /// <typeparam name="TEntity">Entity type.</typeparam>
        /// <returns>Entity instance.</returns>
        public TEntity ExecuteEntity<TEntity>() where TEntity : class, new()
        {
            TEntity result = default(TEntity);

            Database db = this._databaseProvider.Value.CreateDatabase(this.databaseConfig.ConnectionString, this.databaseConfig.Type);
            if (db != null)
            {
                DbCommand command = this.GetRealCommand(db);
                if (command != null)
                {
                    try
                    {
                        using (IDataReader reader = db.ExecuteReader(command))
                        {
                            if (reader.Read())
                            {
                                result = EntityBuilder.BuildEntity<TEntity>(reader);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        command.Parameters.Clear();
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Executes the query and returns the entity collection of the result set returned by the query. 
        /// </summary>
        /// <typeparam name="TEntity">Entity type.</typeparam>
        /// <returns>Entity instance.</returns>
        public IEnumerable<TEntity> ExecuteEntityCollection<TEntity>() where TEntity : class, new()
        {
            IEnumerable<TEntity> result = Enumerable.Empty<TEntity>();
            Database db = this._databaseProvider.Value.CreateDatabase(this.databaseConfig.ConnectionString, this.databaseConfig.Type);

            if (db != null)
            {
                DbCommand command = this.GetRealCommand(db);

                if (command != null)
                {
                    try
                    {
                        List<TEntity> data = new List<TEntity>();
                        using (IDataReader reader = db.ExecuteReader(command))
                        {
                            while (reader.Read())
                            {
                                data.Add(EntityBuilder.BuildEntity<TEntity>(reader));
                            }

                            result = data.ToList();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        command.Parameters.Clear();
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Executes the query and returns the first column of the first row in the result set returned by the query. All other columns and rows are ignored.
        /// </summary>
        /// <returns>The first column of the first row in the result set.</returns>
        public object ExecuteScalar()
        {
            object result = null;
            Database db = this._databaseProvider.Value.CreateDatabase(this.databaseConfig.ConnectionString, this.databaseConfig.Type);
            if (db != null)
            {
                DbCommand command = this.GetRealCommand(db);

                if (command != null)
                {
                    try
                    {
                        result = db.ExecuteScalar(command);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        command.Parameters.Clear();
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Executes the query and returns the first column of the first row in the result set returned by the query. All other columns and rows are ignored.
        /// </summary>
        /// <returns>The first column of the first row in the result set.</returns>
        public T ExecuteScalar<T>()
        {
            object result = ExecuteScalar();
            return (T)result;
        }

        /// <summary>
        /// Executes the query and returns the data set.
        /// </summary>
        /// <returns>Dataset result.</returns>
        public DataSet ExecuteDataSet()
        {
            DataSet result = null;
            Database db = this._databaseProvider.Value.CreateDatabase(this.databaseConfig.ConnectionString, this.databaseConfig.Type);

            if (db != null)
            {
                DbCommand command = this.GetRealCommand(db);

                if (command != null)
                {
                    try
                    {
                        result = db.ExecuteDataSet(command);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        command.Parameters.Clear();
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Executes the query and returns the data table.
        /// </summary>
        /// <returns>Datatable result.</returns>
        public DataTable ExecuteDataTable()
        {
            DataTable result = null;
            DataSet ds = ExecuteDataSet();
            if(ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                result = ds.Tables[0];
            }

            return result;
        }

        #endregion

        #region Private method

        /// <summary>
        /// Get parameter descriptor.
        /// </summary>
        /// <param name="parameterName">Parameter name.</param>
        /// <returns>Data parameter descriptor.</returns>
        private DataParameterUnit GetParameterDescriptor(string parameterName)
        {
            DataParameterUnit result = null;

            if (!string.IsNullOrWhiteSpace(parameterName))
            {
                if (this.dataCommandConfig.ParameterCollection != null && this.dataCommandConfig.ParameterCollection.Any())
                {
                    result = this.dataCommandConfig.ParameterCollection.FirstOrDefault(para => string.Equals(para.Name.Trim('@'), parameterName.Trim('@'), StringComparison.OrdinalIgnoreCase));
                }
            }

            return result;
        }

        /// <summary>
        /// Get parameter descriptor.
        /// </summary>
        /// <param name="parameterName">Parameter name.</param>
        /// <returns>Data parameter descriptor.</returns>
        private DataParameterGroupUnit GetParameterGroupDescriptor(string parameterName)
        {
            DataParameterGroupUnit result = null;

            if (!string.IsNullOrWhiteSpace(parameterName))
            {
                if (this.dataCommandConfig.ParameterGroupCollection != null && this.dataCommandConfig.ParameterGroupCollection.Any())
                { 
                    result = this.dataCommandConfig.ParameterGroupCollection.FirstOrDefault(para => string.Equals(para.Name.Trim('@'), parameterName.Trim('@'), StringComparison.OrdinalIgnoreCase));
                }
            }

            return result;
        }

        /// <summary>
        /// Get real db command.
        /// </summary>
        /// <param name="db">Database instance.</param>
        /// <returns>Real db command.</returns>
        private DbCommand GetRealCommand(Database db)
        {
            DbCommand result = null;
            if (db != null)
            {
                CommandType cmdtype = (CommandType)Enum.Parse(typeof(CommandType), this.dataCommandConfig.CommandType.ToString());

                var formatedCommandText = this.dataCommandConfig.CommandText;
                if (this._formatArgs.Count > 0)
                {
                    formatedCommandText = string.Format(this.dataCommandConfig.CommandText, this._formatArgs.ToArray());
                }

                result = db.CreateCommand(cmdtype, formatedCommandText);
                result.CommandTimeout = this.dataCommandConfig.TimeOut > 0 ? this.dataCommandConfig.TimeOut : 300;

                if (this._myParameters.Value != null && this._myParameters.Value.Any())
                {
                    this._myParameters.Value.ForEach(para => result.Parameters.Add(para));
                }
            }

            if (result != null && !string.IsNullOrWhiteSpace(result.CommandText) && this._myGroupParameters.Value != null)
            {
                foreach (KeyValuePair<string, string> group in this._myGroupParameters.Value)
                {
                    string pattern = string.Format(@"\(\s*@{0}\s*\)", group.Key.TrimStart('@'));
                    result.CommandText = Regex.Replace(result.CommandText, pattern, group.Value, RegexOptions.IgnoreCase);
                }
            }

            return result;
        }


        #endregion
    }
}
