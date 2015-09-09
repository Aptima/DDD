using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Text;
using System.Reflection;

namespace DashboardDataAccess
{
    internal class TableDef
    {
        public string TableName;
        public string PKFieldName;
    }

    public static class DatabaseHelper
    {

        internal static Dictionary<Type, TableDef> _TableDefCache = new Dictionary<Type, TableDef>();

        public static void DeleteByPK<TSource, TPK>(TPK pk, DataContext dc)
                where TSource : class
        {
            Table<TSource> table = dc.GetTable<TSource>();
            TableDef tableDef = GetTableDef<TSource>();
            dc.ExecuteCommand("DELETE FROM [" + tableDef.TableName
                + "] WHERE [" + tableDef.PKFieldName + "] = {0}",
                pk);
        }

        public static void DeleteByPK<TSource, TPK>(TPK[] pkArray, DataContext data)
                where TSource : class
        {
            Table<TSource> table = data.GetTable<TSource>();
            TableDef tableDef = GetTableDef<TSource>();

            var buffer = new StringBuilder();
            buffer.Append("DELETE FROM [")
                .Append(tableDef.TableName)
                .Append("] WHERE [")
                .Append(tableDef.PKFieldName)
                .Append("] IN (");

            for (int i = 0; i < pkArray.Length; i++)
                buffer.Append('\'')
                    .Append(pkArray[i].ToString())
                    .Append('\'')
                    .Append(',');

            buffer.Length--;
            buffer.Append(')');

            data.ExecuteCommand(buffer.ToString());
        }

        internal static TableDef GetTableDef<TEntity>() where TEntity : class
        {
            Type entityType = typeof(TEntity);
            if (!_TableDefCache.ContainsKey(entityType))
            {
                lock (_TableDefCache)
                {
                    if (!_TableDefCache.ContainsKey(entityType))
                    {
                        object[] attributes = entityType.GetCustomAttributes(typeof(TableAttribute), true);
                        string tableName = (attributes[0] as TableAttribute).Name;
                        if (tableName.StartsWith("dbo."))
                            tableName = tableName.Substring("dbo.".Length);
                        string pkFieldName = "ID";

                        // Find the property which is the primary key so that we can find the 
                        // primary key field name in database
                        foreach (PropertyInfo prop in entityType.GetProperties())
                        {
                            object[] columnAttributes = prop.GetCustomAttributes(typeof(ColumnAttribute), true);
                            if (columnAttributes.Length > 0)
                            {
                                ColumnAttribute columnAtt = columnAttributes[0] as ColumnAttribute;
                                if (columnAtt.IsPrimaryKey)
                                    pkFieldName = columnAtt.Storage.TrimStart('_');
                            }
                        }

                        var tableDef = new TableDef { TableName = tableName, PKFieldName = pkFieldName };
                        _TableDefCache.Add(entityType, tableDef);
                        return tableDef;
                    }
                    else
                    {
                        return _TableDefCache[entityType];
                    }
                }
            }
            else
            {
                return _TableDefCache[entityType];
            }
        }

    }
}