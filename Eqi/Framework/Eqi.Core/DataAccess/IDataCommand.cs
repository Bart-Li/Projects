using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Eqi.Core.DataAccess
{
    public interface IDataCommand
    {
        #region Properties

        /// <summary>
        /// Gets db command name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets database name.
        /// </summary>
        string DatabaseName{ get; }

        /// <summary>
        /// Gets parameters.
        /// </summary>
        IList<DbParameter> Parameters{ get; }

        #endregion

        #region Add parameters

        /// <summary>
        /// Adds a new in parameter object to this command.
        /// </summary>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="dbtype"><para>One of the <see cref="DbType"/> values.</para></param>                
        /// <param name="value"><para>The value of the parameter.</para></param>      
        void AddInParameter(string name, DbType dbtype, object value);

        /// <summary>
        /// Adds a new in parameter object to this command.
        /// </summary>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="dbType"><para>One of the <see cref="DbType"/> values.</para></param>   
        /// <param name="size"><para>The maximum size of the data within the column.</para></param>
        /// <param name="precision"><para>The maximum number of digits used to represent the <paramref name="value"/>.</para></param>
        /// <param name="scale"><para>The number of decimal places to which <paramref name="value"/> is resolved.</para></param>
        /// <param name="value"><para>The value of the parameter.</para></param>      
        void AddInParameter(string name, DbType dbType, int size, byte precision, byte scale, object value);

        /// <summary>
        /// Add parameter to command.
        /// </summary>
        /// <param name="parameter">Parameter instance.</param>
        void AddParameter(DbParameter parameter);

        /// <summary>
        /// Add parameter to command.
        /// </summary>
        /// <param name="parameterName">Parameter name.</param>
        /// <param name="parameterValue">Parameter value.</param>
        void AddParameter(string parameterName, object parameterValue);

        /// <summary>
        /// Adds a new out parameter object to this command.
        /// </summary>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="dbtype"><para>One of the <see cref="DbType"/> values.</para></param>        
        /// <param name="size"><para>The maximum size of the data within the parameter.</para></param>   
        /// <param name="precision"><para>The maximum number of digits used to represent the parameter.</para></param>
        /// <param name="scale"><para>The number of decimal places to which parameter is resolved.</para></param>
        void AddOutParameter(string name, DbType dbtype, int size, byte precision, byte scale);

        /// <summary>
        /// Adds a new Out <see cref="DbParameter"/> object to this command.
        /// </summary>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="dbtype"><para>One of the <see cref="DbType"/> values.</para></param>            
        void AddOutParameter(string name, DbType dbtype);

        /// <summary>
        /// Add parameter group to command.
        /// </summary>
        /// <param name="parameterGroupName">Parameter group name.</param>
        /// <param name="parameterValues">Parameter group value.</param>
        void AddGroupParameter(string parameterGroupName, IEnumerable parameterValues);

        /// <summary>
        /// Add format argument.
        /// </summary>
        /// <param name="args">Argument value.</param>
        void AddFormatArgs(object args);

        #endregion

        #region Get parameters

        /// <summary>
        /// Get parameter value.
        /// </summary>
        /// <param name="name">Parameter name.</param>
        /// <returns>Parameter value.</returns>
        object GetParameterValue(string name);

        /// <summary>
        /// Get parameter value.
        /// </summary>
        /// <typeparam name="T">Parameter value type.</typeparam>
        /// <param name="name">Parameter name.</param>
        /// <returns>Parameter value.</returns>
        T GetParameterValue<T>(string name);

        #endregion

        #region Execute

        /// <summary>
        /// Executes a SQL statement against a connection object.
        /// </summary>
        /// <returns>The number of rows affected.</returns>
        int ExecuteNonQuery();

        /// <summary>
        /// Executes the query and returns the entity of the first row in the result set returned by the query. All other columns and rows are ignored.
        /// </summary>
        /// <typeparam name="TEntity">Entity type.</typeparam>
        /// <returns>Entity instance.</returns>
        TEntity ExecuteEntity<TEntity>() where TEntity : class, new();

        /// <summary>
        /// Executes the query and returns the entity collection of the result set returned by the query. 
        /// </summary>
        /// <typeparam name="TEntity">Entity type.</typeparam>
        /// <returns>Entity instance.</returns>
        IEnumerable<TEntity> ExecuteEntityCollection<TEntity>() where TEntity : class, new();

        /// <summary>
        /// Executes the query and returns the first column of the first row in the result set returned by the query. All other columns and rows are ignored.
        /// </summary>
        /// <returns>The first column of the first row in the result set.</returns>
        object ExecuteScalar();

        /// <summary>
        /// Executes the query and returns the first column of the first row in the result set returned by the query. All other columns and rows are ignored.
        /// </summary>
        /// <returns>The first column of the first row in the result set.</returns>
        T ExecuteScalar<T>();

        /// <summary>
        /// Executes the query and returns the data set.
        /// </summary>
        /// <returns>Dataset result.</returns>
        DataSet ExecuteDataSet();

        /// <summary>
        /// Executes the query and returns the data table.
        /// </summary>
        /// <returns>Datatable result.</returns>
        DataTable ExecuteDataTable();

        #endregion
    }
}
