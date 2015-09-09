using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;
using System.IO;
using System.Collections;
using System.Drawing;
using AME.Controllers;
using System.Collections.Specialized;
using System.Reflection;

namespace AME.Model
{
    public class SqlServerExpressModel : Model
    {
        private String connectionString;
        private String database;

        public SqlServerExpressModel(String server, AME.AMEManager.AuthenticationMode mode, String username, String password, String database)
        {
            //if (server.Equals("localhost"))
                //server = @".\SQLEXPRESS";
            switch (mode)
            {
                case AME.AMEManager.AuthenticationMode.WindowsAuthentication:
                    connectionString = String.Format(@"Data Source={0}; Initial Catalog={1}; Integrated Security=SSPI; Connection Timeout=60", server, database, username, password);
                    break;
                case AME.AMEManager.AuthenticationMode.SQLServerAuthentication:
                    connectionString = String.Format(@"Data Source={0}; Initial Catalog={1}; User Id={2}; Password={3}; Connection Timeout=60", server, database, username, password);
                    break;
            }
            this.database = database;

            SqlConnection connection = new SqlConnection(connectionString);
            bool success = false;
            //SqlConnection.ClearAllPools();

            try
            {
                connection.Open();
                success = true;
            }
            catch (SqlException ex)
            {
                // error codes from http://dev.mysql.com/doc/refman/5.0/en/error-messages-server.html 

                switch (ex.Number)
                {
                    case 4060:  // unknown / bad DB, create the database for them
                        {
                            String previousConnectionString = connectionString;
                            // change connection string to use 'tempdb' as database
                            switch (mode)
                            {
                                case AME.AMEManager.AuthenticationMode.WindowsAuthentication:
                                    connectionString = String.Format(@"Data Source={0}; Initial Catalog=tempdb; Integrated Security=SSPI; Connection Timeout=30", server, database, username, password);
                                    break;
                                case AME.AMEManager.AuthenticationMode.SQLServerAuthentication:
                                    connectionString = String.Format(@"Data Source={0}; Initial Catalog=tempdb; User Id={2}; Password={3}; Connection Timeout=30", server, database, username, password);
                                    break;
                            }

                            InitializeDB(); // create tables

                            // change connection string back
                            connectionString = previousConnectionString;

                            break;
                        }
                    default:
                        {
                            connectionString = String.Empty;
                            database = String.Empty;
                            throw new Exception(ex.Message);
                        }
                }
            }
            finally
            {
                connection.Close();
                if (success)
                {
                    addBinaryValueColumnIfNotExists();
                }
            }
        }

        private void addBinaryValueColumnIfNotExists()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command;

            try
            {
                connection.Open();

                String checkBinaryValueColumn = "IF NOT EXISTS (SELECT * FROM syscolumns WHERE id=object_id('parameterTable') AND name='binaryvalue')"+
                                                    " ALTER table parameterTable add binaryvalue varbinary(max)";
                command = new SqlCommand(checkBinaryValueColumn, connection);
                command.ExecuteNonQuery();
            }
            catch (Exception)
            {
                MessageBox.Show("Error checking binaryvaluecolumn");
            }
            finally
            {
                connection.Close();
            }
        }

        override public void DropDatabase()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command;

            try
            {
                connection.Open();
                SqlConnection.ClearAllPools();
                String tempdb = "tempdb";
                connection.ChangeDatabase(tempdb);

                String dropDBQuery = String.Format("IF EXISTS (SELECT * from master.dbo.sysdatabases WHERE name='{0}') DROP DATABASE {0}", database);
                command = new SqlCommand(dropDBQuery, connection);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error initializing database: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        override public void InitializeDB()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command;
            SqlDataReader datareader;
            DataTable table;
            DataSet dataset = new DataSet("dataset");

            try
            {
                connection.Open();
                SqlConnection.ClearAllPools();
                String tempdb = "tempdb";
                connection.ChangeDatabase(tempdb);

                String dropDBQuery = String.Format("IF EXISTS (SELECT * from master.dbo.sysdatabases WHERE name='{0}') DROP DATABASE {0}", database);
                command = new SqlCommand(dropDBQuery, connection);
                command.ExecuteNonQuery();

                String createDBQuery = String.Format("CREATE DATABASE {0}", database);
                command = new SqlCommand(createDBQuery, connection);
                command.ExecuteNonQuery();

                connection.ChangeDatabase(database);

                // Create Tables and schema file.
                String createTableQuery = String.Empty;
                String tableName = String.Empty;
                String sql = String.Empty;

                // ComponentTable
                tableName = "componentTable";
                createTableQuery = String.Format("CREATE TABLE {0} (id INT IDENTITY, type VARCHAR(256), name VARCHAR(256), description VARCHAR(256), etype VARCHAR(80), PRIMARY KEY (id))", tableName);
                command = new SqlCommand(createTableQuery, connection);
                command.ExecuteNonQuery();
                // For schema file.
                sql = String.Format("SELECT * FROM {0}", tableName);
                command = new SqlCommand(sql, connection);
                datareader = command.ExecuteReader();
                table = new DataTable(tableName);
                table.Load(datareader);
                dataset.Tables.Add(table);
                datareader.Close();

                // ParameterTable
                tableName = "parameterTable"; // value needs to be a text field!!!!!!!!!!!!!!
                createTableQuery = String.Format("CREATE TABLE {0} (id INT IDENTITY, parentId INT NOT NULL, parentType VARCHAR(256), name VARCHAR(256), value TEXT, description VARCHAR(256), binaryvalue varbinary(max), PRIMARY KEY (id), UNIQUE (parentId, parentType, name))", tableName);
                command = new SqlCommand(createTableQuery, connection);
                command.ExecuteNonQuery();
                // For schema file.
                sql = String.Format("SELECT * FROM {0}", tableName);
                command = new SqlCommand(sql, connection);
                datareader = command.ExecuteReader();
                table = new DataTable(tableName);
                table.Load(datareader);
                dataset.Tables.Add(table);
                datareader.Close();

                // LinkTable
                tableName = "linkTable";
                createTableQuery = String.Format("CREATE TABLE {0} (id INT IDENTITY, fromComponentId INT, toComponentId INT, type VARCHAR(256), description VARCHAR(256), PRIMARY KEY (id), UNIQUE (fromComponentId, toComponentId, type))", tableName);
                command = new SqlCommand(createTableQuery, connection);
                command.ExecuteNonQuery();
                // For schema file.
                sql = String.Format("SELECT * FROM {0}", tableName);
                command = new SqlCommand(sql, connection);
                datareader = command.ExecuteReader();
                table = new DataTable(tableName);
                table.Load(datareader);
                dataset.Tables.Add(table);
                datareader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                connection.Close();
            }
        }
        override public void ImportSql(String filename)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            DataTable table = new DataTable();
            try
            {
                connection.Open();
                FileInfo sql = new FileInfo(filename);
                TextReader textreader = sql.OpenText();
                SqlCommand command = new SqlCommand(textreader.ReadToEnd(), connection);
                command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, String.Format("Failed to import {0}", filename));
            }
            finally
            {
                connection.Close();
            }
        }
        override public DataTable GetComponentTable()
        {
            SqlConnection connection = new SqlConnection(connectionString);

            DataTable table = new DataTable();
            try
            {
                connection.Open();
                String sql = "SELECT * FROM ComponentTable";
                SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
                adapter.Fill(table);
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Failed to get ComponentTable");
            }
            finally
            {
                connection.Close();
            }
            return table;
        }
        override public DataTable GetComponentTable(String columnName, String columnValue)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            DataTable table = new DataTable();
            try
            {
                connection.Open();
                String sql = String.Format("SELECT * FROM ComponentTable WHERE {0} = '{1}'", columnName, columnValue);
                SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
                adapter.Fill(table);
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, String.Format("Failed to get ComponentTable for {0} '{1}'", columnName, columnValue));
            }
            finally
            {
                connection.Close();
            }
            return table;
        }
        override public DataTable GetComponent(Int32 id)
        {
            DataTable table = new DataTable();

            if (id <= 0) // Why go through with it if index is invalid...
                return table;

            SqlConnection connection = new SqlConnection(connectionString);
            
            try
            {
                connection.Open();
                String sql = String.Format("SELECT * FROM ComponentTable WHERE id = '{0}'", id);
                SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
                adapter.Fill(table);
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, String.Format("Failed to get ComponentTable for id '{0}'", id));
            }
            finally
            {
                connection.Close();
            }
            return table;
        }
        override public DataTable GetLink(Int32 id)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            DataTable table = new DataTable();
            try
            {
                connection.Open();
                String sql = String.Format("SELECT * FROM LinkTable WHERE id = '{0}' ORDER BY id", id);
                SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
                adapter.Fill(table);
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, String.Format("Failed to get Link for id '{0}'", id));
            }
            finally
            {
                connection.Close();
            }
            return table;
        }
        public override DataTable GetLink(int fromID, int toID, string linkType)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            DataTable table = new DataTable();
            try
            {
                connection.Open();
                String sql = String.Format("SELECT * FROM LinkTable WHERE fromComponentId = '{0}' AND toComponentId = '{1}' AND type = '{2}' ", fromID, toID, linkType);
                SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
                adapter.Fill(table);
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, String.Format("Failed to get Link for fromID '{0}', toID '{1}', link type '{2}'", fromID, toID, linkType));
            }
            finally
            {
                connection.Close();
            }
            return table;
        }
        override public DataTable GetLinkTable()
        {
            SqlConnection connection = new SqlConnection(connectionString);

            DataTable table = new DataTable();
            try
            {
                connection.Open();
                String sql = "SELECT * FROM LinkTable ORDER BY id";
                SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
                adapter.Fill(table);
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Failed to get LinkTable");
            }
            finally
            {
                connection.Close();
            }
            return table;
        }
        override public DataTable GetLinkTable(String linkType)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            DataTable table = new DataTable();
            try
            {
                connection.Open();
                String sql = String.Format("SELECT * FROM LinkTable WHERE {0} = '{1}' ORDER BY id", "type", linkType);
                SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
                adapter.Fill(table);
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Failed to get LinkTable for linktype " + linkType);
            }
            finally
            {
                connection.Close();
            }
            return table;
        }
        override public List<String> GetDynamicLinkTypes(String linkType)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            DataTable table = new DataTable();
            List<String> dynamicLinkTypes = new List<String>();
            try
            {
                connection.Open();
                String sql = String.Format("SELECT type FROM LinkTable WHERE type LIKE '{0}%' GROUP BY type", linkType);
                SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
                adapter.Fill(table);

                foreach (DataRow row in table.Rows)
                {
                    dynamicLinkTypes.Add((String)row[0]);
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, String.Format("Failed to get Dynamic Link Types of {0}", linkType));
            }
            finally
            {
                connection.Close();
            }
            return dynamicLinkTypes;
        }
        override public DataTable GetParameterTable(Int32 parentId, String parentType)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            DataTable table = new DataTable();
            try
            {
                connection.Open();
                String sql = String.Format("SELECT * FROM ParameterTable WHERE parentId = '{0}' AND parentType = '{1}'", parentId, parentType);
                SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
                adapter.Fill(table);
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, String.Format("Failed to get ParameterTable for parentId '{0}' and parentType '{1}'", parentId, parentType));
            }
            finally
            {
                connection.Close();
            }
            return table;
        }
        override public DataTable GetParameterTable()
        {
            SqlConnection connection = new SqlConnection(connectionString);

            DataTable table = new DataTable();
            try
            {
                connection.Open();
                String sql = "SELECT * FROM ParameterTable";
                SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
                adapter.Fill(table);
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, String.Format("Failed to get ParameterTable"));
            }
            finally
            {
                connection.Close();
            }
            return table;
        }
        override public DataTable GetChildComponents(Int32 id)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            String sql = String.Empty;
            DataTable tableSource = new DataTable();
            DataTable tableTemp = new DataTable();
            DataTable table = new DataTable();
            try
            {
                connection.Open();
                sql = String.Format("SELECT l.id, l.toComponentId FROM linktable l WHERE l.fromComponentId = '{0}' ORDER BY l.id", id);
                //String sql = String.Format("SELECT * FROM componenttable c WHERE c.id IN (SELECT l.toComponentId FROM linktable l WHERE l.fromComponentId = '{0}' AND l.type = '{1}')", id, linkType);
                SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
                adapter.Fill(tableSource);

                foreach (DataRow row in tableSource.Rows)
                {
                    tableTemp = new DataTable();
                    sql = String.Format("SELECT * FROM componenttable c WHERE c.id = '{0}'", row["toComponentId"].ToString());
                    //String sql = String.Format("SELECT * FROM componenttable c WHERE c.id IN (SELECT l.toComponentId FROM linktable l WHERE l.fromComponentId = '{0}' AND l.type = '{1}')", id, linkType);
                    adapter = new SqlDataAdapter(sql, connection);
                    builder = new SqlCommandBuilder(adapter);
                    adapter.Fill(tableTemp);

                    tableTemp.Columns.Add(SchemaConstants.LinkID, typeof(int));  //add column "LinkID"

                    if (table.Rows.Count == 0)
                    {
                        table = tableTemp.Clone();
                    }
                    if (tableTemp.Rows.Count == 1)
                    {
                        tableTemp.Rows[0][SchemaConstants.LinkID] = row["id"]; // set LinkID column's value
                        table.ImportRow(tableTemp.Rows[0]);
                    }
                }
            }
            //DataTable table = new DataTable();
            //try
            //{
            //    connection.Open();
            //    String sql = String.Format("SELECT * FROM componenttable c WHERE c.id IN (SELECT l.toComponentId FROM linktable l WHERE l.fromComponentId = '{0}')", id);
            //    adapter = new SqlDataAdapter(sql, connection);
            //    builder = new SqlCommandBuilder(adapter);
            //    adapter.Fill(table);
            //}
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, String.Format("Failed to get child components for id '{0}'", id));
            }
            finally
            {
                connection.Close();
            }
            return table;
        }
        override public DataTable GetChildComponents(Int32 id, String linkType)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            String sql = String.Empty;
            DataTable tableSource = new DataTable();
            DataTable tableTemp = new DataTable();
            DataTable table = new DataTable();
            try
            {
                connection.Open();
                sql = String.Format("SELECT l.id, l.toComponentId FROM linktable l WHERE l.fromComponentId = '{0}' AND l.type = '{1}' ORDER BY l.id", id, linkType);
                //String sql = String.Format("SELECT * FROM componenttable c WHERE c.id IN (SELECT l.toComponentId FROM linktable l WHERE l.fromComponentId = '{0}' AND l.type = '{1}')", id, linkType);
                SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
                adapter.Fill(tableSource);

                foreach (DataRow row in tableSource.Rows)
                {
                    tableTemp = new DataTable();
                    sql = String.Format("SELECT * FROM componenttable c WHERE c.id = '{0}'", row["toComponentId"].ToString());
                    //String sql = String.Format("SELECT * FROM componenttable c WHERE c.id IN (SELECT l.toComponentId FROM linktable l WHERE l.fromComponentId = '{0}' AND l.type = '{1}')", id, linkType);
                    adapter = new SqlDataAdapter(sql, connection);
                    builder = new SqlCommandBuilder(adapter);
                    adapter.Fill(tableTemp);

                    tableTemp.Columns.Add(SchemaConstants.LinkID, typeof(int));  //add column "LinkID"

                    if (table.Rows.Count == 0)
                    {
                        table = tableTemp.Clone();
                    }
                    if (tableTemp.Rows.Count == 1)
                    {
                        tableTemp.Rows[0][SchemaConstants.LinkID] = row["id"]; // set LinkID column's value
                        table.ImportRow(tableTemp.Rows[0]);
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, String.Format("Failed to get child components for id '{0}'", id));
            }
            finally
            {
                connection.Close();
            }
            return table;
        }
        override public DataTable GetChildComponents(Int32 id, String columnName, String columnValue)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            String sql = String.Empty;
            DataTable tableSource = new DataTable();
            DataTable tableTemp = new DataTable();
            DataTable table = new DataTable();
            try
            {
                connection.Open();
                sql = String.Format("SELECT l.id, l.toComponentId FROM linktable l WHERE l.fromComponentId = '{0}' ORDER BY l.id", id);
                //String sql = String.Format("SELECT * FROM componenttable c WHERE c.id IN (SELECT l.toComponentId FROM linktable l WHERE l.fromComponentId = '{0}' AND l.type = '{1}')", id, linkType);
                SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
                adapter.Fill(tableSource);

                foreach (DataRow row in tableSource.Rows)
                {
                    tableTemp = new DataTable();
                    sql = String.Format("SELECT * FROM componenttable c WHERE c.id = '{0}' AND {1} = '{2}'", row["toComponentId"].ToString(), columnName, columnValue);
                    //String sql = String.Format("SELECT * FROM componenttable c WHERE c.id IN (SELECT l.toComponentId FROM linktable l WHERE l.fromComponentId = '{0}' AND l.type = '{1}')", id, linkType);
                    adapter = new SqlDataAdapter(sql, connection);
                    builder = new SqlCommandBuilder(adapter);
                    adapter.Fill(tableTemp);

                    tableTemp.Columns.Add(SchemaConstants.LinkID, typeof(int));  //add column "LinkID"

                    if (table.Rows.Count == 0)
                    {
                        table = tableTemp.Clone();
                    }
                    if (tableTemp.Rows.Count == 1)
                    {
                        tableTemp.Rows[0][SchemaConstants.LinkID] = row["id"]; // set LinkID column's value
                        table.ImportRow(tableTemp.Rows[0]);
                    }
                }
            }
            //DataTable table = new DataTable();
            //try
            //{
            //    connection.Open();
            //    String sql = String.Format("SELECT * FROM componenttable WHERE id IN (SELECT l.toComponentId FROM linktable l WHERE l.fromComponentId = '{0}') AND {1} = '{2}'", id, columnName, columnValue);
            //    adapter = new SqlDataAdapter(sql, connection);
            //    builder = new SqlCommandBuilder(adapter);
            //    adapter.Fill(table);
            //}
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, String.Format("Failed to get child components for id '{0}'", id));
            }
            finally
            {
                connection.Close();
            }
            return table;
        }
        override public DataTable GetChildComponents(Int32 id, String columnName, String columnValue, String linkType)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            String sql = String.Empty;
            DataTable tableSource = new DataTable();
            DataTable tableTemp = new DataTable();
            DataTable table = new DataTable();
            try
            {
                connection.Open();
                sql = String.Format("SELECT l.id, l.toComponentId FROM linktable l WHERE l.fromComponentId = '{0}' AND l.type = '{1}' ORDER BY l.id", id, linkType);
                //String sql = String.Format("SELECT * FROM componenttable c WHERE c.id IN (SELECT l.toComponentId FROM linktable l WHERE l.fromComponentId = '{0}' AND l.type = '{1}')", id, linkType);
                SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
                adapter.Fill(tableSource);

                foreach (DataRow row in tableSource.Rows)
                {
                    tableTemp = new DataTable();
                    sql = String.Format("SELECT * FROM componenttable c WHERE c.id = '{0}' AND {1} = '{2}'", row["toComponentId"].ToString(), columnName, columnValue);
                    //String sql = String.Format("SELECT * FROM componenttable c WHERE c.id IN (SELECT l.toComponentId FROM linktable l WHERE l.fromComponentId = '{0}' AND l.type = '{1}')", id, linkType);
                    adapter = new SqlDataAdapter(sql, connection);
                    builder = new SqlCommandBuilder(adapter);
                    adapter.Fill(tableTemp);

                    tableTemp.Columns.Add(SchemaConstants.LinkID, typeof(int));  //add column "LinkID"

                    if (table.Rows.Count == 0)
                    {
                        table = tableTemp.Clone();
                    }
                    if (tableTemp.Rows.Count == 1)
                    {
                        tableTemp.Rows[0][SchemaConstants.LinkID] = row["id"]; // set LinkID column's value
                        table.ImportRow(tableTemp.Rows[0]);
                    }
                }
            }
            //DataTable table = new DataTable();
            //try
            //{
            //    connection.Open();
            //    String sql = String.Format("SELECT * FROM componenttable WHERE id IN (SELECT l.toComponentId FROM linktable l WHERE l.fromComponentId = '{0}' AND l.type = '{1}') AND {2} = '{3}'", id, linkType, columnName, columnValue);
            //    adapter = new SqlDataAdapter(sql, connection);
            //    builder = new SqlCommandBuilder(adapter);
            //    adapter.Fill(table);
            //}
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, String.Format("Failed to get child components for id '{0}'", id));
            }
            finally
            {
                connection.Close();
            }
            return table;
        }
        override public NameValueCollection GetRootLinkTypes()
        {
            NameValueCollection collection = new NameValueCollection();

            SqlConnection connection = new SqlConnection(connectionString);

            String sql = String.Empty;
            DataTable tableTypes = new DataTable();
            DataTable table = new DataTable();
            try
            {
                connection.Open();
                sql = String.Format("SELECT l.type FROM linktable l GROUP BY l.type");
                SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
                adapter.Fill(tableTypes);

                // linktype indexes to fromID indexes to list of toIDs
                Dictionary<String, Dictionary<String, List<String>>> fromToMapping = new Dictionary<String, Dictionary<String, List<String>>>();
                Dictionary<String, Dictionary<String, List<String>>> fromParentMapping = new Dictionary<String, Dictionary<String, List<String>>>();
                String from, to, type;
                foreach (DataRow rowType in tableTypes.Rows)
                {
                    type = rowType["type"].ToString();
                    table = new DataTable();
                    sql = String.Format("SELECT l.fromComponentId, l.toComponentId FROM linktable l WHERE l.type = '{0}'", type);
                    adapter = new SqlDataAdapter(sql, connection);
                    builder = new SqlCommandBuilder(adapter);
                    adapter.Fill(table);

                    foreach (DataRow row in table.Rows)
                    {
                        if (!fromToMapping.ContainsKey(type))
                        {
                            fromToMapping.Add(type, new Dictionary<String, List<String>>());
                        }
                        if (!fromParentMapping.ContainsKey(type))
                        {
                            fromParentMapping.Add(type, new Dictionary<String, List<String>>());
                        }

                        from = row["fromComponentId"].ToString();
                        if (!fromToMapping[type].ContainsKey(from))
                        {
                            fromToMapping[type].Add(from, new List<String>());
                        }

                        to = row["toComponentId"].ToString();
                        fromToMapping[type][from].Add(to);

                        if (!fromParentMapping[type].ContainsKey(to))
                        {
                            fromParentMapping[type].Add(to, new List<String>());
                        }
     
                        fromParentMapping[type][to].Add(from);
                    }
                }

                foreach (String linkType in fromToMapping.Keys)
                {
                    foreach (String fromID in fromToMapping[linkType].Keys)
                    {
                        bool found = false;
                        foreach (String otherFromID in fromToMapping[linkType].Keys)
                        {
                            if (fromID != otherFromID && fromToMapping[linkType][otherFromID].Contains(fromID))
                            {
                                found = true;
                                break;
                            }
                        }

                        // this 'from' never appears in as a to, a child of another from for the same link type
                        if (!found)
                        {
                            collection.Add(fromID, linkType);
                        }
                        else
                        {
                            found = false;
                            // or, this from appears as a 'to', but that from has its parent as a child - a symmetric pair
                            foreach (String parent in fromParentMapping[linkType][fromID])
                            {
                                if (!found)
                                {
                                    foreach (String child in fromToMapping[linkType][fromID])
                                    {
                                        if (!found)
                                        {
                                            if (parent.Equals(child))
                                            {
                                                collection.Add(fromID, linkType);
                                                found = true;
                                            }
                                        }
                                    }
                                }
                            }   
                        }
                    }
                }  
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, String.Format("Failed to get root components"));
            }
            finally
            {
                connection.Close();
            }
            return collection;
        }
        override public DataTable GetParentComponents(Int32 id)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            String sql = String.Empty;
            DataTable tableSource = new DataTable();
            DataTable tableTemp = new DataTable();
            DataTable table = new DataTable();
            try
            {
                connection.Open();
                sql = String.Format("SELECT l.fromComponentId FROM linktable l WHERE l.toComponentId = '{0}' ORDER BY l.id", id);
                //    String sql = String.Format("SELECT * FROM componenttable c WHERE c.id IN (SELECT l.fromComponentId FROM linktable l WHERE l.toComponentId = '{0}')", id);
                SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
                adapter.Fill(tableSource);

                foreach (DataRow row in tableSource.Rows)
                {
                    tableTemp = new DataTable();
                    sql = String.Format("SELECT * FROM componenttable c WHERE c.id = '{0}'", row["fromComponentId"].ToString());
                    adapter = new SqlDataAdapter(sql, connection);
                    builder = new SqlCommandBuilder(adapter);
                    adapter.Fill(tableTemp);
                    if (table.Rows.Count == 0)
                    {
                        table = tableTemp.Clone();
                    }
                    if (tableTemp.Rows.Count == 1)
                    {
                        table.ImportRow(tableTemp.Rows[0]);
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, String.Format("Failed to get parent components for id '{0}'", id));
            }
            finally
            {
                connection.Close();
            }
            return table;
            //DataTable table = new DataTable();
            //try
            //{
            //    connection.Open();
            //    String sql = String.Format("SELECT * FROM componenttable c WHERE c.id IN (SELECT l.fromComponentId FROM linktable l WHERE l.toComponentId = '{0}')", id);
            //    adapter = new SqlDataAdapter(sql, connection);
            //    builder = new SqlCommandBuilder(adapter);
            //    adapter.Fill(table);
            //}
            //catch (SqlException ex)
            //{
            //    MessageBox.Show(ex.Message, String.Format("Failed to get parent components for id '{0}'", id));
            //}
            //finally
            //{
            //    connection.Close();
            //}
            //return table;
        }
        override public DataTable GetParentComponents(Int32 id, String linkType)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            String sql = String.Empty;
            DataTable tableSource = new DataTable();
            DataTable tableTemp = new DataTable();
            DataTable table = new DataTable();
            try
            {
                connection.Open();
                sql = String.Format("SELECT l.fromComponentId FROM linktable l WHERE l.toComponentId = '{0}' AND l.type = '{1}' ORDER BY l.id", id, linkType);
                //String sql = String.Format("SELECT * FROM componenttable c WHERE c.id IN (SELECT l.toComponentId FROM linktable l WHERE l.fromComponentId = '{0}' AND l.type = '{1}')", id, linkType);
                SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
                adapter.Fill(tableSource);

                foreach (DataRow row in tableSource.Rows)
                {
                    tableTemp = new DataTable();
                    sql = String.Format("SELECT * FROM componenttable c WHERE c.id = '{0}'", row["fromComponentId"].ToString());
                    //String sql = String.Format("SELECT * FROM componenttable c WHERE c.id IN (SELECT l.toComponentId FROM linktable l WHERE l.fromComponentId = '{0}' AND l.type = '{1}')", id, linkType);
                    adapter = new SqlDataAdapter(sql, connection);
                    builder = new SqlCommandBuilder(adapter);
                    adapter.Fill(tableTemp);
                    if (table.Rows.Count == 0)
                    {
                        table = tableTemp.Clone();
                    }
                    if (tableTemp.Rows.Count == 1)
                    {
                        table.ImportRow(tableTemp.Rows[0]);
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, String.Format("Failed to get parent components for id '{0}'", id));
            }
            finally
            {
                connection.Close();
            }
            return table;
        }
        override public DataTable GetChildComponentLinks(Int32 id)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            DataTable table = new DataTable();
            try
            {
                connection.Open();
                String sql = String.Format("SELECT * FROM LinkTable WHERE fromComponentId = '{0}' ORDER BY id", id);
                SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
                adapter.Fill(table);
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, String.Format("Failed to get child links of component '{0}'", id));
            }
            finally
            {
                connection.Close();
            }
            return table;
        }
        override public DataTable GetChildComponentLinks(Int32 id, String linkType)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            DataTable table = new DataTable();
            try
            {
                connection.Open();
                String sql = String.Format("SELECT * FROM LinkTable WHERE fromComponentId = '{0}' AND type = '{1}' ORDER BY id", id, linkType);
                SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
                adapter.Fill(table);
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, String.Format("Failed to get child links of component '{0}' with type '{1}' ", id, linkType));
            }
            finally
            {
                connection.Close();
            }
            return table;
        }
        override public IXPathNavigable GetOutputXml(String filename)
        {
            XmlDocument document = new XmlDocument();
            //String outputFilepath = Path.GetFullPath(configurationPath + @"\output");
            String simRunFile = Path.Combine(outputPath, filename + ".xml");
            if (filename.Length > 0)
            {
                document.Load(simRunFile);
            }
            return (IXPathNavigable)document.Clone();
        }
        override public List<String> GetConfigurationNames()
        {
            List<String> list = new List<String>();
            try
            {
                XmlNodeList configurationList = xmlDoc.SelectNodes(String.Format("/GME/Configurations/Configuration"));
                foreach (XmlNode configurationNode in configurationList)
                {
                    list.Add(configurationNode.Attributes["name"].Value);
                }
            }
            catch (Exception ex)
            {
                list.Clear();
                MessageBox.Show(ex.Message, String.Format("Failed to get configurations'"));
            }
            return list;
        }
        override public String GetRootComponent(String configuration)
        {
            String root = String.Empty;
            try
            {
                XmlNode configurationNode = xmlDoc.SelectSingleNode(String.Format("/GME/Configurations/Configuration[@name='{0}']", configuration));
                XmlAttribute rootAttribute = configurationNode.Attributes["root"];
                if (rootAttribute != null)
                {
                    root = rootAttribute.Value;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, String.Format("Failed to get root name in '{1}'", configuration));
            }
            return root;
        }
        override public String GetComponentLinkType(String configuration)
        {
            String linktype = String.Empty;
            try
            {
                XmlNode configurationNode = xmlDoc.SelectSingleNode(String.Format("/GME/Configurations/Configuration[@name='{0}']", configuration));
                XmlAttribute nameAttribute = configurationNode.Attributes["linkType"];
                if (nameAttribute != null)
                {
                    linktype = nameAttribute.Value;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, String.Format("Failed to get linkType in '{1}'", configuration));
            }
            return linktype;
        }
        override public String GetConfigurationControllerName(String configuration)
        {
            try
            {
                XmlNode configurationNode = xmlDoc.SelectSingleNode(String.Format("/GME/Configurations/Configuration[@name='{0}']", configuration));
                XmlAttribute controllerAttribute = configurationNode.Attributes["controller"];
                return controllerAttribute.Value.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, String.Format("Failed to get configuration controller name for '{0}'", configuration));
                return "";
            }
        }
        override public Dictionary<String, Bitmap> GetComponentBitmaps(String configuration)
        {
            Dictionary<String, Bitmap> dictionary = new Dictionary<String, Bitmap>();
            try
            {
                Assembly entryAssembly = Assembly.GetEntryAssembly();

                XmlNodeList componentList = xmlDoc.SelectNodes(String.Format("/GME/Global/Components/Component", configuration));
                foreach (XmlNode componentNode in componentList)
                {
                    XmlAttribute bitmapAttribute = componentNode.Attributes["bitmap"];
                    if (bitmapAttribute != null)
                    {
                        String name = String.Concat(this.ModelConfiguration.imgNamespace, ".", bitmapAttribute.Value);
                        using (StreamReader streamReader = new StreamReader(entryAssembly.GetManifestResourceStream(name)))
                        {
                            Bitmap bitmap = new Bitmap(streamReader.BaseStream);
                            dictionary.Add(componentNode.Attributes["name"].Value, new Bitmap(bitmap));
                            bitmap.Dispose();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                dictionary.Clear();
                MessageBox.Show(ex.Message, String.Format("Failed to get component bitmaps in '{0}'", configuration));
            }
            return dictionary;
        }
        override public XmlNodeList GetSubComponents(String configuration, String component)
        {
            try
            {
                XmlNodeList subcomponentList = xmlDoc.SelectNodes(String.Format("/GME/Configurations/Configuration[@name='{0}']/Components/Component[@name='{1}']/SubComponents/Component", configuration, component));

                return subcomponentList;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, String.Format("Failed to get subcomponents for '{0}' in '{1}'", component, configuration));
            }
            return null;
        }

        [Obsolete("Please use GetComponent(String component): Components are now defined in global space.")] 
        override public IXPathNavigable GetComponent(String configuration, String component)
        {
            try
            {
                XmlNode nodeComponentDoc = xmlDoc.SelectSingleNode(String.Format("/GME/Configurations/Configuration[@name='{0}']/Components/Component[@name='{1}']", configuration, component));
                XmlNode nodeComponent = nodeComponentDoc.Clone();

                // load all global children
                XmlNode globalComponentNode = xmlDoc.SelectSingleNode(String.Format("/GME/Global/Components/Component[@name='{0}']", component));
                if (globalComponentNode != null)
                {
                    foreach (XmlNode child in globalComponentNode.ChildNodes)
                    {
                        XmlNode childClone = child.Clone();
                        nodeComponent.AppendChild(childClone);
                    }
                }

                return nodeComponent.CreateNavigator();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, String.Format("Failed to get component '{0}' in '{1}'", component, configuration));
                return null;
            }
        }

        private int bulkCounter = 1; // increment this so we have a unique format file name

        override public void BulkCreate(String table, FileInfo inputFile, String formatFile, int rowsPerBatch)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            formatFile = Path.Combine(this.formatPath, formatFile);
            FileInfo formatFileInfo = new FileInfo(formatFile);
            formatFileInfo.Refresh();

            try
            {
                String sql = String.Empty;
                SqlCommand command;
                connection.Open();
                //sql = String.Format("BULK INSERT {0} FROM '{1}' WITH (FORMATFILE = '{2}', ROWS_PER_BATCH = {3}, MAXERRORS = 0)", table, inputFile.FullName, formatFile, rowsPerBatch);
                sql = String.Format("BULK INSERT {0} FROM '{1}' WITH (FIELDTERMINATOR = '\\t', ROWTERMINATOR = '\\n', ROWS_PER_BATCH = {3}, MAXERRORS = 0)", table, inputFile.FullName, formatFile, rowsPerBatch);
                command = new SqlCommand(sql, connection);
                command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message + System.Environment.NewLine + String.Format("Failed to bulk create table with name '{0}', on file '{1}' with format '{2}' with ROWS_PER_BATCH = {3}", table, inputFile.FullName, formatFile, rowsPerBatch), "Import Error");
            }
            finally
            {
                inputFile.Refresh();
                if (inputFile.Exists)
                {
                    inputFile.Delete();
                    inputFile.Refresh();
                }

                connection.Close();
                bulkCounter++;
            }

        }

        override public Int32 CreateComponent(String component, String name, String etype, String description)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            Int32 id = -1;
            try
            {
                String sql = String.Empty;
                SqlCommand command;
                connection.Open();
                sql = String.Format("INSERT INTO ComponentTable (type, name, etype, description) VALUES ('{0}', '{1}', '{2}', '{3}')", component, safeSqlLiteral(name), etype, safeSqlLiteral(description));
                command = new SqlCommand(sql, connection);
                command.ExecuteNonQuery();

                sql = "SELECT @@IDENTITY AS 'id'";
                command = new SqlCommand(sql, connection);
                SqlDataReader datareader = command.ExecuteReader();
                datareader.Read();
                id = Convert.ToInt32(datareader.GetValue(0).ToString());
                datareader.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, String.Format("Failed to add component with name '{0}'", name));
            }
            finally
            {
                connection.Close();
            }
            return id;
        }
        override public Int32 GetComponentId(String component, String name)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            DataTable table = new DataTable();
            Int32 id = -1;
            try
            {
                connection.Open();
                String sql = String.Format("SELECT * FROM ComponentTable WHERE type = '{0}' AND name = '{1}'", component, name);
                SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);

                SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
                adapter.Fill(table);

                if (table.Rows.Count > 1 || table.Rows.Count == 0)
                {
                    throw new Exception("Invalid rows retruned!");
                }
                else
                {
                    DataRow row = table.Rows[0];
                    id = (Int32)row["id"];
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, String.Format("Failed to get id for type '{0}' and name '{1}'!", component, name));
            }
            catch (SystemException ex)
            {
                MessageBox.Show(ex.Message, String.Format("Failed to get id for type '{0}' and name '{1}'!", component, name));
            }
            finally
            {
                connection.Close();
            }
            return id;
        }
        override public void DeleteComponent(Int32 id)
        {
            // Delete component paramters
            deleteParameters(id, SchemaConstants.Component);

            // Delete link paramters
            foreach (Int32 linkId in getLinkIds(id))
            {
                deleteParameters(linkId, SchemaConstants.Link);
            }

            // Delete links
            deleteLinks(id);

            // Delete component
            deleteComponent(id);

        }
        override public Int32 UpdateComponent(Int32 id, String columnName, String columnValue)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            Int32 rows = -1;
            try
            {
                connection.Open();
                String sql = String.Format("UPDATE ComponentTable SET {0} = '{1}' WHERE id = '{2}'", columnName, safeSqlLiteral(columnValue), id);
                SqlCommand command = new SqlCommand(sql, connection);
                rows = command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, String.Format("Failed to update component with componentId '{0}' using columName '{1}' and columnValue '{2}'", id, columnName, columnValue));
            }
            finally
            {
                connection.Close();
            }
            return rows;
        }
        override public Int32 CreateLink(Int32? fromComponentId, Int32 toComponentId, String type, String description)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            Int32 id = -1;
            try
            {
                connection.Open();
                String sql = String.Format("INSERT INTO LinkTable (fromComponentId, toComponentId, type, description) VALUES ({0}, '{1}', '{2}', '{3}')", (fromComponentId.HasValue ? Convert.ToString(fromComponentId) : "null"), toComponentId, type, description);
                SqlCommand command = new SqlCommand(sql, connection);
                command.ExecuteNonQuery();

                sql = "SELECT @@IDENTITY AS 'id'";
                command = new SqlCommand(sql, connection);
                SqlDataReader datareader = command.ExecuteReader();
                datareader.Read();
                id = Convert.ToInt32(datareader.GetValue(0).ToString());
                datareader.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, String.Format("Failed to add link with fromComponentId '{0}' and toComponentId '{1}' of type '{2}'", fromComponentId, toComponentId, type));
            }
            finally
            {
                connection.Close();
            }
            return id;
        }
        override public void DeleteLink(Int32 id)
        {
            // Delete parameters
            deleteParameters(id, SchemaConstants.Link);

            // Delete link
            deleteLink(id);
        }
        override public List<Int32> DeleteLinks(String linkType)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            List<Int32> deletedLinkIDs = new List<Int32>();
            SqlDataReader reader;
            try
            {
                connection.Open();
                String sql = String.Format("DELETE FROM linkTable OUTPUT DELETED.id WHERE type = '{0}' ", linkType);
                SqlCommand command = new SqlCommand(sql, connection);
                reader = command.ExecuteReader();
                int linkID;
                while (reader.Read())
                {
                    linkID = reader.GetInt32(0); // single column of ids returned

                    deletedLinkIDs.Add(linkID);
                    deleteParameters(linkID, SchemaConstants.Link);
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, String.Format("Failed to delete link with linktype '{0}'", linkType));
            }
            finally
            {
                connection.Close();
            }
            return deletedLinkIDs;
        }

        override public Int32 CreateParameter(Int32 parentId, String parentType, String name, String value, String description)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            Int32 rows = -1;
            try
            {
                connection.Open();
                //String sql = String.Format("UPDATE ParameterTable (parentId, parentType, name, value, description) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}')" +
                //                           "ON DUPLICATE KEY UPDATE value = '{3}', description = '{4}'", parentId, parentType, name, safeSqlLiteral(value), safeSqlLiteral(description));
                String sql = String.Format("INSERT ParameterTable (parentId, parentType, name, value, description) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}')", parentId, parentType, name, safeSqlLiteral(value), safeSqlLiteral(description));
                SqlCommand command = new SqlCommand(sql, connection);
                rows = command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                if (ex.Number.Equals(2627))
                {
                    String sql = String.Format("UPDATE ParameterTable SET value = '{3}', description = '{4}' WHERE parentId = '{0}' AND parentType = '{1}' AND name = '{2}'", parentId, parentType, name, safeSqlLiteral(value), safeSqlLiteral(description));
                    SqlCommand command = new SqlCommand(sql, connection);
                    rows = command.ExecuteNonQuery();
                }
                else
                    MessageBox.Show(ex.Message + " " + value, String.Format("Failed to add parameter with parentId '{0}', parentType '{1}' and name '{2}'", parentId, parentType, name));
            }
            finally
            {
                connection.Close();
            }
            return rows;
        }

        override public Int32 CreateParameter(Int32 parentId, String parentType, String name, byte[] value, String description)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            Int32 rows = -1;
            try
            {
                connection.Open();
                String sql = String.Format("INSERT ParameterTable (parentId, parentType, name, value, description, binaryvalue) VALUES ('{0}', '{1}', '{2}', '', '{3}', @binary)", parentId, parentType, name, description);
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.Add("@binary", SqlDbType.VarBinary, value.Length).Value = value;
                rows = command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                if (ex.Number.Equals(2627))
                {
                    String sql = String.Format("UPDATE ParameterTable SET value = '', description = '{3}', binaryvalue = @binary WHERE parentId = '{0}' AND parentType = '{1}' AND name = '{2}'", parentId, parentType, name, description);
                    SqlCommand command = new SqlCommand(sql, connection);
                    command.Parameters.Add("@binary", SqlDbType.VarBinary, value.Length).Value = value;
                    rows = command.ExecuteNonQuery();
                }
                else
                {
                    MessageBox.Show(ex.Message + " " + value, String.Format("Failed to add parameter with parentId '{0}', parentType '{1}' and name '{2}'", parentId, parentType, name));
                }
            }
            finally
            {
                connection.Close();
            }
            return rows;
        }

        private List<Int32> getLinkIds(Int32 componentId)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            DataTable table = new DataTable();
            List<Int32> ids = new List<Int32>();
            try
            {
                connection.Open();
                String sql = String.Format("SELECT id FROM LinkTable WHERE fromComponentId = '{0}' OR toComponentId = '{1}' ORDER BY id", componentId, componentId);
                SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
                adapter.Fill(table);

                foreach (DataRow row in table.Rows)
                {
                    ids.Add((Int32)row[0]);
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, String.Format("Failed to get all link ids of component id '{0}'", componentId));
            }
            finally
            {
                connection.Close();
            }
            return ids;
        }
        private void deleteComponent(Int32 id)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            Int32 rows = -1;
            try
            {
                connection.Open();
                String sql = String.Format("DELETE FROM ComponentTable WHERE id = '{0}'", id);
                SqlCommand command = new SqlCommand(sql, connection);
                rows = command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, String.Format("Failed to delete component with id '{0}'", id));
            }
            finally
            {
                connection.Close();
            }
        }
        private void deleteLinks(Int32 componentId)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            Int32 rows = -1;
            try
            {
                connection.Open();
                String sql = String.Format("DELETE FROM LinkTable WHERE fromComponentId = '{0}' OR toComponentId = '{1}'", componentId, componentId);
                SqlCommand command = new SqlCommand(sql, connection);
                rows = command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, String.Format("Failed to delete all links of component id '{0}'", componentId));
            }
            finally
            {
                connection.Close();
            }
        }
        public override void DeleteParameter(int parentId, String parentType, String name)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            Int32 rows = -1;
            try
            {
                connection.Open();
                String sql = String.Format("DELETE FROM ParameterTable WHERE parentId = '{0}' AND parentType = '{1}' AND name = '{2}'", parentId, parentType, name);
                SqlCommand command = new SqlCommand(sql, connection);
                rows = command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, String.Format("Failed to delete parameter parent id '{0}' with parent type '{1}' with name '{2}'", parentId, parentType, name));
            }
            finally
            {
                connection.Close();
            }
        }
        private void deleteParameters(Int32 parentId, String parentType)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            Int32 rows = -1;
            try
            {
                connection.Open();
                String sql = String.Format("DELETE FROM ParameterTable WHERE parentId = '{0}' AND parentType = '{1}'", parentId, parentType);
                SqlCommand command = new SqlCommand(sql, connection);
                rows = command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, String.Format("Failed to delete all parameter of parent id '{0}' of parent type '{1}'", parentId, parentType));
            }
            finally
            {
                connection.Close();
            }
        }
        private void deleteLink(Int32 id)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            Int32 rows = -1;
            try
            {
                connection.Open();
                String sql = String.Format("DELETE FROM LinkTable WHERE id = '{0}'", id);
                SqlCommand command = new SqlCommand(sql, connection);
                rows = command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, String.Format("Failed to delete link with id '{0}'", id));
            }
            finally
            {
                connection.Close();
            }
        }

    } //Model class
} //MVC