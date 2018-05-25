using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace Eqi.Core.DataAccess.DBClient
{
    internal class EntityBuilder
    {
        public static TEntity BuildEntity<TEntity>(IDataReader dataReader) where TEntity : class, new()
        {
            var dataCollection = FormateData(dataReader);
            PropertyInfo[] properties = typeof(TEntity).GetProperties();
            TEntity entity = new TEntity();
            foreach (var p in properties)
            {
                if (p.CanWrite)
                {
                    string columnName = p.Name;
                    if (dataCollection.ContainsKey(columnName.ToLower()))
                    {
                        SetValue(p, entity, dataCollection[columnName.ToLower()]);
                    }
                }
            }

            return entity;
        }

        private static IDictionary<string, object> FormateData(IDataReader dataReader)
        {
            Dictionary<string, object> formattedData = new Dictionary<string, object>();

            for (int index = 0; index < dataReader.FieldCount; index++)
            {
                string fieldName = dataReader.GetName(index).ToLower();

                if (!string.IsNullOrWhiteSpace(fieldName))
                {
                    if (formattedData.ContainsKey(fieldName))
                    {
                        formattedData[fieldName] = dataReader[fieldName];
                    }
                    else
                    {
                        formattedData.Add(fieldName, dataReader[fieldName]);
                    }
                }
            }

            return formattedData;
        }

        private static void SetValue(PropertyInfo p, object entity, object value)
        {
            if (value == null || value == DBNull.Value)
            {
                return;
            }

            if (p.PropertyType == typeof(int) || p.PropertyType == typeof(int?))
            {
                p.SetValue(entity, Convert.ToInt32(value));
            }
            else if (p.PropertyType == typeof(decimal) || p.PropertyType == typeof(decimal?))
            {
                p.SetValue(entity, Convert.ToDecimal(value));
            }
            else if (p.PropertyType == typeof(DateTime) || p.PropertyType == typeof(DateTime?))
            {
                p.SetValue(entity, Convert.ToDateTime(value));
            }
            else if (p.PropertyType == typeof(float) || p.PropertyType == typeof(float?))
            {
                p.SetValue(entity, Convert.ToSingle(value));
            }
            else if (p.PropertyType == typeof(long) || p.PropertyType == typeof(long?))
            {
                p.SetValue(entity, Convert.ToInt64(value));
            }
            else if (p.PropertyType == typeof(double) || p.PropertyType == typeof(double?))
            {
                p.SetValue(entity, Convert.ToDouble(value));
            }
            else if (p.PropertyType == typeof(bool) || p.PropertyType == typeof(bool?))
            {
                p.SetValue(entity, Convert.ToBoolean(value));
            }
            else if (p.PropertyType == typeof(char) || p.PropertyType == typeof(char?))
            {
                p.SetValue(entity, Convert.ToChar(value));
            }
            else
            {
                p.SetValue(entity, Convert.ToString(value).Trim());
            }
        }
    }
}
