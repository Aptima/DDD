using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;
using System.IO;
using System.Collections;
using System.Drawing;
using GME.Controllers;
using System.Collections.Specialized;

namespace GME.Model
{
    public class MySqlModel : Model
    {
        //private String configurationPath = String.Empty;
        //private String documentationPath = String.Empty;
        //private String licensePath = String.Empty;
        //private XmlDocument xmlDoc;

        private String connectionString;
        private String database;

        public MySqlModel(String server, Int32 port, String username, String password, String database)
        {
            connectionString = String.Format("server={0}; port={1}; user id={2}; password={3}; database={4}; Max Pool Size=50; Min Pool Size=5; Pooling=True; Reset Pooled Connections=False; Cache Server Configuration=True;", server, port, username, password, database);
            this.database = database;

            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();
            }
            catch (MySqlException ex)
            {
                // error codes from http://dev.mysql.com/doc/refman/5.0/en/error-messages-server.html 

                switch (ex.Number)
                {
                    case 1049:  // unknown / bad DB, create the database for them
                        {
                            String previousConnectionString = connectionString;
                            // change connection string to use 'mysql' as database
                            connectionString = String.Format("server={0}; port={1}; user id={2}; password={3}; database={4}; Max Pool Size=50; Min Pool Size=5; Pooling=True; Reset Pooled Connections=False; Cache Server Configuration=True;", server, port, username, password, "mysql");

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
            }
        }

        override public void DropDatabase()
        {
        }
        override public void InitializeDB()
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand command;
            MySqlDataReader datareader;
            DataTable table;
            DataSet dataset = new DataSet("dataset");

            try
            {
                connection.Open();

                String dropDBQuery = String.Format("DROP DATABASE IF EXISTS {0}", database);
                command = new MySqlCommand(dropDBQuery, connection);
                command.ExecuteNonQuery();

                String createDBQuery = String.Format("CREATE DATABASE IF NOT EXISTS {0}", database);
                command = new MySqlCommand(createDBQuery, connection);
                command.ExecuteNonQuery();

                connection.ChangeDatabase(database);

                // Create Tables and schema file.
                String createTableQuery = String.Empty;
                String tableName = String.Empty;
                String sql = String.Empty;

                // ComponentTable
                tableName = "ComponentTable";
                createTableQuery = String.Format("CREATE TABLE {0} (id INT NOT NULL AUTO_INCREMENT, type VARCHAR(80), name VARCHAR(80), description VARCHAR(255), etype VARCHAR(80), PRIMARY KEY (id)) ENGINE = MYISAM", tableName);
                command = new MySqlCommand(createTableQuery, connection);
                command.ExecuteNonQuery();
                // For schema file.
                sql = String.Format("SELECT * FROM {0}", tableName);
                command = new MySqlCommand(sql, connection);
                datareader = command.ExecuteReader();
                table = new DataTable(tableName);
                table.Load(datareader);
                dataset.Tables.Add(table);
                datareader.Close();

                // ParameterTable
                tableName = "ParameterTable";
                createTableQuery = String.Format("CREATE TABLE {0} (id INT NOT NULL AUTO_INCREMENT, parentId INT NOT NULL, parentType VARCHAR(80), name VARCHAR(80), value VARCHAR(512), description VARCHAR(256), PRIMARY KEY (id), UNIQUE (parentId, parentType, name)) ENGINE = MYISAM", tableName);
                command = new MySqlCommand(createTableQuery, connection);
                command.ExecuteNonQuery();
                // For schema file.
                sql = String.Format("SELECT * FROM {0}", tableName);
                command = new MySqlCommand(sql, connection);
                datareader = command.ExecuteReader();
                table = new DataTable(tableName);
                table.Load(datareader);
                dataset.Tables.Add(table);
                datareader.Close();

                // LinkTable
                tableName = "LinkTable";
                createTableQuery = String.Format("CREATE TABLE {0} (id INT NOT NULL AUTO_INCREMENT, fromComponentId INT, toComponentId INT, type VARCHAR(80), description VARCHAR(255), PRIMARY KEY (id), UNIQUE (fromComponentId, toComponentId, type)) ENGINE = MYISAM", tableName);
                command = new MySqlCommand(createTableQuery, connection);
                command.ExecuteNonQuery();
                // For schema file.
                sql = String.Format("SELECT * FROM {0}", tableName);
                command = new MySqlCommand(sql, connection);
                datareader = command.ExecuteReader();
                table = new DataTable(tableName);
                table.Load(datareader);
                dataset.Tables.Add(table);
                datareader.Close();
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
        override public void ImportSql(String filename)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);

            DataTable table = new DataTable();
            try
            {
                connection.Open();
                FileInfo sql = new FileInfo(filename);
                TextReader textreader = sql.OpenText();
                MySqlCommand command = new MySqlCommand(textreader.ReadToEnd(), connection);
                command.ExecuteNonQuery();
            }
            catch (MySqlException ex)
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
            MySqlConnection connection = new MySqlConnection(connectionString);

            DataTable table = new DataTable();
            try
            {
                connection.Open();
                String sql = "SELECT * FROM ComponentTable";
                MySqlDataAdapter adapter = new MySqlDataAdapter(sql, connection);
                MySqlCommandBuilder builder = new MySqlCommandBuilder(adapter);
                adapter.Fill(table);
            }
            catch (MySqlException ex)
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
            MySqlConnection connection = new MySqlConnection(connectionString);

            DataTable table = new DataTable();
            try
            {
                connection.Open();
                String sql = String.Format("SELECT * FROM ComponentTable WHERE {0} = '{1}'", columnName, columnValue);
                MySqlDataAdapter adapter = new MySqlDataAdapter(sql, connection);
                MySqlCommandBuilder builder = new MySqlCommandBuilder(adapter);
                adapter.Fill(table);
            }
            catch (MySqlException ex)
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
            MySqlConnection connection = new MySqlConnection(connectionString);

            DataTable table = new DataTable();
            try
            {
                connection.Open();
                String sql = String.Format("SELECT * FROM ComponentTable WHERE id = '{0}'", id);
                MySqlDataAdapter adapter = new MySqlDataAdapter(sql, connection);
                MySqlCommandBuilder builder = new MySqlCommandBuilder(adapter);
                adapter.Fill(table);
            }
            catch (MySqlException ex)
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
            MySqlConnection connection = new MySqlConnection(connectionString);

            DataTable table = new DataTable();
            try
            {
                connection.Open();
                String sql = String.Format("SELECT * FROM LinkTable WHERE id = '{0}' ORDER BY id", id);
                MySqlDataAdapter adapter = new MySqlDataAdapter(sql, connection);
                MySqlCommandBuilder builder = new MySqlCommandBuilder(adapter);
                adapter.Fill(table);
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, String.Format("Failed to get Link for id '{0}'", id));
            }
            finally
            {
                connection.Close();
            }
            return table;
        }
        override public DataTable GetLinkTable()
        {
            MySqlConnection connection = new MySqlConnection(connectionString);

            DataTable table = new DataTable();
            try
            {
                connection.Open();
                String sql = "SELECT * FROM LinkTable ORDER BY id";
                MySqlDataAdapter adapter = new MySqlDataAdapter(sql, connection);
                MySqlCommandBuilder builder = new MySqlCommandBuilder(adapter);
                adapter.Fill(table);
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Failed to get LinkTable");
            }
            finally
            {
                connection.Close();
            }
            return table;
        }
        override public List<String> GetDynamicLinkTypes(String linkType)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);

            DataTable table = new DataTable();
            List<String> dynamicLinkTypes = new List<String>();
            try
            {
                connection.Open();
                String sql = String.Format("SELECT type FROM LinkTable WHERE type LIKE '{0}%' GROUP BY type", linkType);
                MySqlDataAdapter adapter = new MySqlDataAdapter(sql, connection);
                MySqlCommandBuilder builder = new MySqlCommandBuilder(adapter);
                adapter.Fill(table);

                foreach (DataRow row in table.Rows)
                {
                    dynamicLinkTypes.Add((String)row[0]);
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, String.Format("Failed to get Dynamic Link Types of {0}", linkType));
            }
            finally
            {
                connection.Close();
            }
            return dynamicLinkTypes;
        }
        override public DataTable GetLinkTable(String linkType)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);

            DataTable table = new DataTable();
            try
            {
                connection.Open();
                String sql = String.Format("SELECT * FROM LinkTable WHERE {0} = '{1}' ORDER BY id", "type", linkType);
                MySqlDataAdapter adapter = new MySqlDataAdapter(sql, connection);
                MySqlCommandBuilder builder = new MySqlCommandBuilder(adapter);
                adapter.Fill(table);
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Failed to get LinkTable for linktype " + linkType);
            }
            finally
            {
                connection.Close();
            }
            return table;
        }
        override public DataTable GetParameterTable(Int32 parentId, String parentType)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);

            DataTable table = new DataTable();
            try
            {
                connection.Open();
                String sql = String.Format("SELECT * FROM ParameterTable WHERE parentId = '{0}' AND parentType = '{1}'", parentId, parentType);
                MySqlDataAdapter adapter = new MySqlDataAdapter(sql, connection);
                MySqlCommandBuilder builder = new MySqlCommandBuilder(adapter);
                adapter.Fill(table);
            }
            catch (MySqlException ex)
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
            MySqlConnection connection = new MySqlConnection(connectionString);

            DataTable table = new DataTable();
            try
            {
                connection.Open();
                String sql = "SELECT * FROM ParameterTable";
                MySqlDataAdapter adapter = new MySqlDataAdapter(sql, connection);
                MySqlCommandBuilder builder = new MySqlCommandBuilder(adapter);
                adapter.Fill(table);
            }
            catch (MySqlException ex)
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
            MySqlConnection connection = new MySqlConnection(connectionString);

            String sql = String.Empty;
            DataTable tableSource = new DataTable();
            DataTable tableTemp = new DataTable();
            DataTable table = new DataTable();
            try
            {
                connection.Open();
                sql = String.Format("SELECT l.id, l.toComponentId FROM linktable l WHERE l.fromComponentId = '{0}' ORDER BY l.id", id);
                //String sql = String.Format("SELECT * FROM componenttable c WHERE c.id IN (SELECT l.toComponentId FROM linktable l WHERE l.fromComponentId = '{0}' AND l.type = '{1}')", id, linkType);
                MySqlDataAdapter adapter = new MySqlDataAdapter(sql, connection);
                MySqlCommandBuilder builder = new MySqlCommandBuilder(adapter);
                adapter.Fill(tableSource);

                foreach (DataRow row in tableSource.Rows)
                {
                    tableTemp = new DataTable();
                    sql = String.Format("SELECT * FROM componenttable c WHERE c.id = '{0}'", row["toComponentId"].ToString());
                    //String sql = String.Format("SELECT * FROM componenttable c WHERE c.id IN (SELECT l.toComponentId FROM linktable l WHERE l.fromComponentId = '{0}' AND l.type = '{1}')", id, linkType);
                    adapter = new MySqlDataAdapter(sql, connection);
                    builder = new MySqlCommandBuilder(adapter);
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
            //    adapter = new MySqlDataAdapter(sql, connection);
            //    builder = new MySqlCommandBuilder(adapter);
            //    adapter.Fill(table);
            //}
            catch (MySqlException ex)
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
            MySqlConnection connection = new MySqlConnection(connectionString);

            String sql = String.Empty;
            DataTable tableSource = new DataTable();
            DataTable tableTemp = new DataTable();
            DataTable table = new DataTable();
            try
            {
                connection.Open();
                sql = String.Format("SELECT l.id, l.toComponentId FROM linktable l WHERE l.fromComponentId = '{0}' AND l.type = '{1}' ORDER BY l.id", id, linkType);
                //String sql = String.Format("SELECT * FROM componenttable c WHERE c.id IN (SELECT l.toComponentId FROM linktable l WHERE l.fromComponentId = '{0}' AND l.type = '{1}')", id, linkType);
                MySqlDataAdapter adapter = new MySqlDataAdapter(sql, connection);
                MySqlCommandBuilder builder = new MySqlCommandBuilder(adapter);
                adapter.Fill(tableSource);

                foreach (DataRow row in tableSource.Rows)
                {
                    tableTemp = new DataTable();
                    sql = String.Format("SELECT * FROM componenttable c WHERE c.id = '{0}'", row["toComponentId"].ToString());
                    //String sql = String.Format("SELECT * FROM componenttable c WHERE c.id IN (SELECT l.toComponentId FROM linktable l WHERE l.fromComponentId = '{0}' AND l.type = '{1}')", id, linkType);
                    adapter = new MySqlDataAdapter(sql, connection);
                    builder = new MySqlCommandBuilder(adapter);
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
            catch (MySqlException ex)
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
            MySqlConnection connection = new MySqlConnection(connectionString);

            String sql = String.Empty;
            DataTable tableSource = new DataTable();
            DataTable tableTemp = new DataTable();
            DataTable table = new DataTable();
            try
            {
                connection.Open();
                sql = String.Format("SELECT l.id, l.toComponentId FROM linktable l WHERE l.fromComponentId = '{0}' ORDER BY l.id", id);
                //String sql = String.Format("SELECT * FROM componenttable c WHERE c.id IN (SELECT l.toComponentId FROM linktable l WHERE l.fromComponentId = '{0}' AND l.type = '{1}')", id, linkType);
                MySqlDataAdapter adapter = new MySqlDataAdapter(sql, connection);
                MySqlCommandBuilder builder = new MySqlCommandBuilder(adapter);
                adapter.Fill(tableSource);

                foreach (DataRow row in tableSource.Rows)
                {
                    tableTemp = new DataTable();
                    sql = String.Format("SELECT * FROM componenttable c WHERE c.id = '{0}' AND {1} = '{2}'", row["toComponentId"].ToString(), columnName, columnValue);
                    //String sql = String.Format("SELECT * FROM componenttable c WHERE c.id IN (SELECT l.toComponentId FROM linktable l WHERE l.fromComponentId = '{0}' AND l.type = '{1}')", id, linkType);
                    adapter = new MySqlDataAdapter(sql, connection);
                    builder = new MySqlCommandBuilder(adapter);
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
            //    adapter = new MySqlDataAdapter(sql, connection);
            //    builder = new MySqlCommandBuilder(adapter);
            //    adapter.Fill(table);
            //}
            catch (MySqlException ex)
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
            MySqlConnection connection = new MySqlConnection(connectionString);

            String sql = String.Empty;
            DataTable tableSource = new DataTable();
            DataTable tableTemp = new DataTable();
            DataTable table = new DataTable();
            try
            {
                connection.Open();
                sql = String.Format("SELECT l.id, l.toComponentId FROM linktable l WHERE l.fromComponentId = '{0}' AND l.type = '{1}' ORDER BY l.id", id, linkType);
                //String sql = String.Format("SELECT * FROM componenttable c WHERE c.id IN (SELECT l.toComponentId FROM linktable l WHERE l.fromComponentId = '{0}' AND l.type = '{1}')", id, linkType);
                MySqlDataAdapter adapter = new MySqlDataAdapter(sql, connection);
                MySqlCommandBuilder builder = new MySqlCommandBuilder(adapter);
                adapter.Fill(tableSource);

                foreach (DataRow row in tableSource.Rows)
                {
                    tableTemp = new DataTable();
                    sql = String.Format("SELECT * FROM componenttable c WHERE c.id = '{0}' AND {1} = '{2}'", row["toComponentId"].ToString(), columnName, columnValue);
                    //String sql = String.Format("SELECT * FROM componenttable c WHERE c.id IN (SELECT l.toComponentId FROM linktable l WHERE l.fromComponentId = '{0}' AND l.type = '{1}')", id, linkType);
                    adapter = new MySqlDataAdapter(sql, connection);
                    builder = new MySqlCommandBuilder(adapter);
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
            //    adapter = new MySqlDataAdapter(sql, connection);
            //    builder = new MySqlCommandBuilder(adapter);
            //    adapter.Fill(table);
            //}
            catch (MySqlException ex)
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

            MySqlConnection connection = new MySqlConnection(connectionString);

            String sql = String.Empty;
            DataTable tableTypes = new DataTable();
            DataTable tableFrom = new DataTable();
            DataTable tableTo = new DataTable();
            DataTable table = new DataTable();
            try
            {
                connection.Open();
                sql = String.Format("SELECT l.type FROM vsg.linktable l GROUP BY l.type");
                MySqlDataAdapter adapter = new MySqlDataAdapter(sql, connection);
                MySqlCommandBuilder builder = new MySqlCommandBuilder(adapter);
                adapter.Fill(tableTypes);

                foreach (DataRow rowTypes in tableTypes.Rows)
                {
                    tableFrom = new DataTable();
                    sql = String.Format("SELECT l.fromComponentId FROM vsg.linktable l WHERE l.type = '{0}' GROUP BY l.fromComponentId", rowTypes["type"].ToString());
                    adapter = new MySqlDataAdapter(sql, connection);
                    builder = new MySqlCommandBuilder(adapter);
                    adapter.Fill(tableFrom);

                    foreach (DataRow rowFrom in tableFrom.Rows)
                    {
                        tableTo = new DataTable();
                        sql = String.Format("SELECT l.fromComponentId FROM vsg.linktable l WHERE l.type = '{0}' AND l.toComponentId = '{1}'", rowTypes["type"].ToString(), rowFrom["fromComponentId"].ToString());
                        adapter = new MySqlDataAdapter(sql, connection);
                        builder = new MySqlCommandBuilder(adapter);
                        adapter.Fill(tableTo);

                        if (tableTo.Rows.Count == 0)
                            collection.Add(rowFrom["fromComponentId"].ToString(), rowTypes["type"].ToString());
                        else if (rowFrom["fromComponentId"].ToString().Equals(rowTypes["type"].ToString()))
                            collection.Add(rowFrom["fromComponentId"].ToString(), rowTypes["type"].ToString());
                    }
                }
            }
            catch (MySqlException ex)
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
            MySqlConnection connection = new MySqlConnection(connectionString);

            String sql = String.Empty;
            DataTable tableSource = new DataTable();
            DataTable tableTemp = new DataTable();
            DataTable table = new DataTable();
            try
            {
                connection.Open();
                sql = String.Format("SELECT l.fromComponentId FROM linktable l WHERE l.toComponentId = '{0}' ORDER BY l.id", id);
                //    String sql = String.Format("SELECT * FROM componenttable c WHERE c.id IN (SELECT l.fromComponentId FROM linktable l WHERE l.toComponentId = '{0}')", id);
                MySqlDataAdapter adapter = new MySqlDataAdapter(sql, connection);
                MySqlCommandBuilder builder = new MySqlCommandBuilder(adapter);
                adapter.Fill(tableSource);

                foreach (DataRow row in tableSource.Rows)
                {
                    tableTemp = new DataTable();
                    sql = String.Format("SELECT * FROM componenttable c WHERE c.id = '{0}'", row["fromComponentId"].ToString());
                    adapter = new MySqlDataAdapter(sql, connection);
                    builder = new MySqlCommandBuilder(adapter);
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
            catch (MySqlException ex)
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
            //    adapter = new MySqlDataAdapter(sql, connection);
            //    builder = new MySqlCommandBuilder(adapter);
            //    adapter.Fill(table);
            //}
            //catch (MySqlException ex)
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
            MySqlConnection connection = new MySqlConnection(connectionString);

            String sql = String.Empty;
            DataTable tableSource = new DataTable();
            DataTable tableTemp = new DataTable();
            DataTable table = new DataTable();
            try
            {
                connection.Open();
                sql = String.Format("SELECT l.fromComponentId FROM linktable l WHERE l.toComponentId = '{0}' AND l.type = '{1}' ORDER BY l.id", id, linkType);
                //String sql = String.Format("SELECT * FROM componenttable c WHERE c.id IN (SELECT l.toComponentId FROM linktable l WHERE l.fromComponentId = '{0}' AND l.type = '{1}')", id, linkType);
                MySqlDataAdapter adapter = new MySqlDataAdapter(sql, connection);
                MySqlCommandBuilder builder = new MySqlCommandBuilder(adapter);
                adapter.Fill(tableSource);

                foreach (DataRow row in tableSource.Rows)
                {
                    tableTemp = new DataTable();
                    sql = String.Format("SELECT * FROM componenttable c WHERE c.id = '{0}'", row["fromComponentId"].ToString());
                    //String sql = String.Format("SELECT * FROM componenttable c WHERE c.id IN (SELECT l.toComponentId FROM linktable l WHERE l.fromComponentId = '{0}' AND l.type = '{1}')", id, linkType);
                    adapter = new MySqlDataAdapter(sql, connection);
                    builder = new MySqlCommandBuilder(adapter);
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
            catch (MySqlException ex)
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
            MySqlConnection connection = new MySqlConnection(connectionString);

            DataTable table = new DataTable();
            try
            {
                connection.Open();
                String sql = String.Format("SELECT * FROM LinkTable WHERE fromComponentId = '{0}' ORDER BY id", id);
                MySqlDataAdapter adapter = new MySqlDataAdapter(sql, connection);
                MySqlCommandBuilder builder = new MySqlCommandBuilder(adapter);
                adapter.Fill(table);
            }
            catch (MySqlException ex)
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
            MySqlConnection connection = new MySqlConnection(connectionString);

            DataTable table = new DataTable();
            try
            {
                connection.Open();
                String sql = String.Format("SELECT * FROM LinkTable WHERE fromComponentId = '{0}' AND type = '{1}' ORDER BY id", id, linkType);
                MySqlDataAdapter adapter = new MySqlDataAdapter(sql, connection);
                MySqlCommandBuilder builder = new MySqlCommandBuilder(adapter);
                adapter.Fill(table);
            }
            catch (MySqlException ex)
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
            try
            {
                String outputPath = Path.GetFullPath(configurationPath + @"\output");
                String outputFile = Path.Combine(outputPath, filename + ".xml");
                if (filename.Length > 0)
                {
                    document.Load(outputFile);
                }
                return (IXPathNavigable)document.Clone();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, String.Format("Failed to get output xml '{0}'", filename));
                return null;
            }
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
                XmlNodeList componentList = xmlDoc.SelectNodes(String.Format("/GME/Global/Components/Component", configuration));
                foreach (XmlNode componentNode in componentList)
                {
                    XmlAttribute bitmapAttribute = componentNode.Attributes["bitmap"];
                    if (bitmapAttribute != null)
                    {
                        FileInfo fileInfo = new FileInfo(configurationPath + @"\" + bitmapAttribute.Value);
                        FileStream streamFromFileInfo = fileInfo.Open(FileMode.Open);
                        Bitmap bitmap = new Bitmap(streamFromFileInfo);
                        dictionary.Add(componentNode.Attributes["name"].Value, new Bitmap(bitmap));
                        streamFromFileInfo.Close();
                        bitmap.Dispose();
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

        override public Int32 CreateComponent(String component, String name, String etype, String description)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);

            Int32 id = -1;
            try
            {
                String sql = String.Empty;
                MySqlCommand command;
                connection.Open();
                sql = String.Format("INSERT INTO ComponentTable (type, name, etype, description) VALUES ('{0}', '{1}', '{2}', '{3}')", component, safeSqlLiteral(name), etype, safeSqlLiteral(description));
                command = new MySqlCommand(sql, connection);
                command.ExecuteNonQuery();

                sql = "SELECT @@IDENTITY AS 'id'";
                command = new MySqlCommand(sql, connection);
                MySqlDataReader datareader = command.ExecuteReader();
                datareader.Read();
                id = Convert.ToInt32(datareader.GetValue(0).ToString());
                datareader.Close();
            }
            catch (MySqlException ex)
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
            MySqlConnection connection = new MySqlConnection(connectionString);

            DataTable table = new DataTable();
            Int32 id = -1;
            try
            {
                connection.Open();
                String sql = String.Format("SELECT * FROM ComponentTable WHERE type = '{0}' AND name = '{1}'", component, name);
                MySqlDataAdapter adapter = new MySqlDataAdapter(sql, connection);

                MySqlCommandBuilder builder = new MySqlCommandBuilder(adapter);
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
            catch (MySqlException ex)
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
            MySqlConnection connection = new MySqlConnection(connectionString);

            Int32 rows = -1;
            try
            {
                connection.Open();
                String sql = String.Format("UPDATE ComponentTable SET {0} = '{1}' WHERE id = '{2}'", columnName, safeSqlLiteral(columnValue), id);
                MySqlCommand command = new MySqlCommand(sql, connection);
                rows = command.ExecuteNonQuery();
            }
            catch (MySqlException ex)
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
            MySqlConnection connection = new MySqlConnection(connectionString);

            Int32 id = -1;
            try
            {
                connection.Open();
                String sql = String.Format("INSERT INTO LinkTable (fromComponentId, toComponentId, type, description) VALUES ({0}, '{1}', '{2}', '{3}')", (fromComponentId.HasValue ? Convert.ToString(fromComponentId) : "null"), toComponentId, type, description);
                MySqlCommand command = new MySqlCommand(sql, connection);
                command.ExecuteNonQuery();

                sql = "SELECT @@IDENTITY AS 'id'";
                command = new MySqlCommand(sql, connection);
                MySqlDataReader datareader = command.ExecuteReader();
                datareader.Read();
                id = Convert.ToInt32(datareader.GetValue(0).ToString());
                datareader.Close();
            }
            catch (MySqlException ex)
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
        override public Int32 CreateParameter(Int32 parentId, String parentType, String name, String value, String description)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);

            Int32 rows = -1;
            try
            {
                connection.Open();
                String sql = String.Format("INSERT INTO ParameterTable (parentId, parentType, name, value, description) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}')" +
                                           "ON DUPLICATE KEY UPDATE value = '{3}', description = '{4}'", parentId, parentType, name, safeSqlLiteral(value), safeSqlLiteral(description));
                MySqlCommand command = new MySqlCommand(sql, connection);
                rows = command.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message + " " + value, String.Format("Failed to add parameter with parentId '{0}', parentType '{1}' and name '{2}'", parentId, parentType, name));
            }
            finally
            {
                connection.Close();
            }
            return rows;
        }
        private List<Int32> getLinkIds(Int32 componentId)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);

            DataTable table = new DataTable();
            List<Int32> ids = new List<Int32>();
            try
            {
                connection.Open();
                String sql = String.Format("SELECT id FROM LinkTable WHERE fromComponentId = '{0}' OR toComponentId = '{1}' ORDER BY id", componentId, componentId);
                MySqlDataAdapter adapter = new MySqlDataAdapter(sql, connection);
                MySqlCommandBuilder builder = new MySqlCommandBuilder(adapter);
                adapter.Fill(table);

                foreach (DataRow row in table.Rows)
                {
                    ids.Add((Int32)row[0]);
                }
            }
            catch (MySqlException ex)
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
            MySqlConnection connection = new MySqlConnection(connectionString);

            Int32 rows = -1;
            try
            {
                connection.Open();
                String sql = String.Format("DELETE FROM ComponentTable WHERE id = '{0}'", id);
                MySqlCommand command = new MySqlCommand(sql, connection);
                rows = command.ExecuteNonQuery();
            }
            catch (MySqlException ex)
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
            MySqlConnection connection = new MySqlConnection(connectionString);

            Int32 rows = -1;
            try
            {
                connection.Open();
                String sql = String.Format("DELETE FROM LinkTable WHERE fromComponentId = '{0}' OR toComponentId = '{1}'", componentId, componentId);
                MySqlCommand command = new MySqlCommand(sql, connection);
                rows = command.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, String.Format("Failed to delete all links of component id '{0}'", componentId));
            }
            finally
            {
                connection.Close();
            }
        }
        private void deleteParameters(Int32 parentId, String parentType)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);

            Int32 rows = -1;
            try
            {
                connection.Open();
                String sql = String.Format("DELETE FROM ParameterTable WHERE parentId = '{0}' AND parentType = '{1}'", parentId, parentType);
                MySqlCommand command = new MySqlCommand(sql, connection);
                rows = command.ExecuteNonQuery();
            }
            catch (MySqlException ex)
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
            MySqlConnection connection = new MySqlConnection(connectionString);

            Int32 rows = -1;
            try
            {
                connection.Open();
                String sql = String.Format("DELETE FROM LinkTable WHERE id = '{0}'", id);
                MySqlCommand command = new MySqlCommand(sql, connection);
                rows = command.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, String.Format("Failed to delete link with id '{0}'", id));
            }
            finally
            {
                connection.Close();
            }
        }
        public override List<Int32> DeleteLinks(String linkType)
        {
            throw new Exception("Delete links call not currently supported by this model");
        }

    } //Model class
} //MVC
