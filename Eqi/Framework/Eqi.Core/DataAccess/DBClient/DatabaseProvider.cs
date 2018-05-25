using System;
using System.Collections.Concurrent;
using System.IO;
using System.Reflection;

namespace Eqi.Core.DataAccess.DBClient
{
    internal class DatabaseProvider
    {
        /// <summary>
        /// My databases.
        /// </summary>
        protected Lazy<ConcurrentDictionary<string, Database>> Databases = new Lazy<ConcurrentDictionary<string, Database>>();

        /// <summary>
        /// Databases fail time.
        /// </summary>
        protected Lazy<ConcurrentDictionary<string, DateTime>> FailTimes = new Lazy<ConcurrentDictionary<string, DateTime>>();

        /// <summary>
        /// Retry interval.
        /// </summary>
        private readonly TimeSpan _retryInterval = TimeSpan.FromMinutes(5);

        /// <summary>
        /// Create database instance by type,default is Sqlserver.
        /// </summary>
        /// <param name="connectionString">Connection string.</param>
        /// <param name="databaseType">Database type,default is sqlserver.</param>
        /// <returns>Database instance.</returns>
        public Database CreateDatabase(string connectionString, string databaseType = "sqlserver")
        {
            Database result = null;

            if (!string.IsNullOrWhiteSpace(connectionString))
            {
                string key = connectionString;

                if (!this.Databases.Value.TryGetValue(key, out result))
                {
                    result = this.GenerateDataBase(key, databaseType);

                    if (result != null)
                    {
                        this.Databases.Value.TryAdd(key, result);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Generate database.
        /// </summary>
        /// <param name="connectionString">Connection string.</param>
        /// <param name="databaseType">Database type.</param>
        /// <returns>Sql database instance.</returns>
        private Database GenerateDataBase(string connectionString, string databaseType)
        {
            Database result = null;

            if (!string.IsNullOrWhiteSpace(connectionString))
            {
                bool needGenerate = true;

                if (this.FailTimes.Value.ContainsKey(connectionString))
                {
                    DateTime failTime = this.FailTimes.Value[connectionString];

                    if (DateTime.Now - failTime < this._retryInterval)
                    {
                        needGenerate = false;
                    }
                }

                if (needGenerate)
                {
                    try
                    {
                        result = GenerateDataBaseInstance(connectionString, databaseType);
                    }
                    catch (Exception ex)
                    {
                        this.FailTimes.Value.TryAdd(connectionString, DateTime.Now);
                        throw ex;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Generate database instance.
        /// </summary>
        /// <param name="connectionString">Connection string.</param>
        /// <param name="databaseType">Database type.</param>
        /// <returns>Database provider.</returns>
        private Database GenerateDataBaseInstance(string connectionString, string databaseType)
        {
            if (string.IsNullOrWhiteSpace(databaseType))
            {
                databaseType = "sqlserver";
            }

            switch (databaseType.ToLower())
            {
                //case "mysql":
                    //return LoadDbProviderFactory("Newegg.EC.DataAccess.Extension.MySqlDatabase", connectionString);
                case "sqlserver":
                    return new SqlDatabase(connectionString);
                default:
                    return new SqlDatabase(connectionString);
            }
        }

        ///// <summary>
        ///// Load database provider.
        ///// </summary>
        ///// <param name="typeName">Type name.</param>
        ///// <param name="connectionString">Connection string.</param>
        ///// <returns>Database provider.</returns>
        //private Database LoadDbProviderFactory(string typeName, string connectionString)
        //{
        //    var assemblyName = "Newegg.EC.DataAccess.Extension.dll";
        //    var assemblyPath = AppDomain.CurrentDomain.BaseDirectory + assemblyName;

        //    if (File.Exists(assemblyPath))
        //    {
        //        Assembly assembly = Assembly.LoadFile(assemblyPath);
        //        Type type = assembly.GetType(typeName);
        //        object obj = Activator.CreateInstance(type, connectionString);
        //        return obj as Database;
        //    }
        //    else
        //    {
        //        throw new FileNotFoundException($"Not found {assemblyName} in {assemblyPath}");
        //    }
        //}
    }
}
