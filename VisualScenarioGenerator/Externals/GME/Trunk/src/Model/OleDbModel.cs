using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OleDb;
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
    public class OleDbModel : Model
    {
        //private String configurationPath = String.Empty;
        //private String documentationPath = String.Empty;
        //private String licensePath = String.Empty;
        //private XmlDocument xmlDoc;

        private String connectionString;
        private String database;
        private String filename;

        public OleDbModel(String server, Int32 port, String username, String password, String database)
        {
            //connectionString = String.Format("server={0}; port={1}; user id={2}; password={3}; database={4}; Max Pool Size=50; Min Pool Size=5; Pooling=True; Reset Pooled Connections=False; Cache Server Configuration=True;", server, port, username, password, database);
            filename = Path.Combine(Directory.GetCurrentDirectory(), @"config\data\vsg.mdb");
            connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filename;
            this.database = database;

            OleDbConnection connection = new OleDbConnection(connectionString);

            try
            {
                connection.Open();
            }
            catch (OleDbException ex)
            {
                // error codes from http://dev.mysql.com/doc/refman/5.0/en/error-messages-server.html 

                switch (ex.ErrorCode)
                {
                    case -2147467259:  // unknown / bad DB, create the database for them
                        {
                            ADOX.CatalogClass mdb = new ADOX.CatalogClass();
                            try
                            {
                                mdb.Create("Provider=Microsoft.Jet.OLEDB.4.0;" +
                                       "Data Source=" + filename + ";" +
                                       "Jet OLEDB:Engine Type=5");

                                ADOX.Table componentTable = new ADOX.Table();
                                componentTable.Name = "ComponentTable";
                                mdb.Tables.Append(componentTable);
                                ADOX.Table linkTable = new ADOX.Table();
                                linkTable.Name = "LinkTable";
                                mdb.Tables.Append(linkTable);
                                ADOX.Table parameterTable = new ADOX.Table();
                                parameterTable.Name = "ParameterTable";
                                mdb.Tables.Append(parameterTable);
                            }
                            catch (Exception e)
                            {
                                // Let the user know what went wrong.
                                MessageBox.Show(e.Message, "File error");
                            }
                            finally
                            {
                                mdb = null;
                            }

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

            OleDbConnection connection = new OleDbConnection(connectionString);
            OleDbCommand command;
            OleDbDataReader datareader;
            DataTable table;
            DataSet dataset = new DataSet("dataset");

            try
            {
                connection.Open();


                String dropTableQuery = String.Format("DROP TABLE ComponentTable");
                command = new OleDbCommand(dropTableQuery, connection);
                command.ExecuteNonQuery();

                dropTableQuery = String.Format("DROP TABLE LinkTable");
                command = new OleDbCommand(dropTableQuery, connection);
                command.ExecuteNonQuery();

                dropTableQuery = String.Format("DROP TABLE ParameterTable");
                command = new OleDbCommand(dropTableQuery, connection);
                command.ExecuteNonQuery();

                //String createDBQuery = String.Format("CREATE DATABASE IF NOT EXISTS {0}", database);
                //command = new OleDbCommand(createDBQuery, connection);
                //command.ExecuteNonQuery();

                //connection.ChangeDatabase(database);

                // Create Tables and schema file.
                String createTableQuery = String.Empty;
                String tableName = String.Empty;
                String sql = String.Empty;

                // ComponentTable
                tableName = "ComponentTable";
                createTableQuery = String.Format("CREATE TABLE {0} ([id] COUNTER PRIMARY KEY, [type] TEXT(80), [name] TEXT(80), [description] TEXT(255), [etype] TEXT(80))", tableName);
                command = new OleDbCommand(createTableQuery, connection);
                command.ExecuteNonQuery();
                // For schema file.
                sql = String.Format("SELECT * FROM {0}", tableName);
                command = new OleDbCommand(sql, connection);
                datareader = command.ExecuteReader();
                table = new DataTable(tableName);
                table.Load(datareader);
                dataset.Tables.Add(table);
                datareader.Close();

                // ParameterTable
                tableName = "ParameterTable";
                createTableQuery = String.Format("CREATE TABLE {0} ([id] COUNTER PRIMARY KEY, [parentId] INT, [parentType] TEXT(80), [name] TEXT(80), [value] MEMO, [description] TEXT(255), CONSTRAINT {0}_Unique UNIQUE (parentId, parentType, name))", tableName);
                command = new OleDbCommand(createTableQuery, connection);
                command.ExecuteNonQuery();
                // For schema file.
                sql = String.Format("SELECT * FROM {0}", tableName);
                command = new OleDbCommand(sql, connection);
                datareader = command.ExecuteReader();
                table = new DataTable(tableName);
                table.Load(datareader);
                dataset.Tables.Add(table);
                datareader.Close();

                // LinkTable
                tableName = "LinkTable";
                createTableQuery = String.Format("CREATE TABLE {0} ([id] COUNTER PRIMARY KEY, [fromComponentId] INT, [toComponentId] INT, [type] TEXT(80), [description] TEXT(255), CONSTRAINT {0}_Unique UNIQUE (fromComponentId, toComponentId, type))", tableName);
                command = new OleDbCommand(createTableQuery, connection);
                command.ExecuteNonQuery();
                // For schema file.
                sql = String.Format("SELECT * FROM {0}", tableName);
                command = new OleDbCommand(sql, connection);
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
            OleDbConnection connection = new OleDbConnection(connectionString);

            DataTable table = new DataTable();
            try
            {
                connection.Open();
                FileInfo sql = new FileInfo(filename);
                TextReader textreader = sql.OpenText();
                OleDbCommand command = new OleDbCommand(textreader.ReadToEnd(), connection);
                command.ExecuteNonQuery();
            }
            catch (OleDbException ex)
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
            OleDbConnection connection = new OleDbConnection(connectionString);

            DataTable table = new DataTable();
            try
            {
                connection.Open();
                String sql = "SELECT * FROM ComponentTable";
                OleDbDataAdapter adapter = new OleDbDataAdapter(sql, connection);
                OleDbCommandBuilder builder = new OleDbCommandBuilder(adapter);
                adapter.Fill(table);
            }
            catch (OleDbException ex)
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
            OleDbConnection connection = new OleDbConnection(connectionString);

            DataTable table = new DataTable();
            try
            {
                connection.Open();
                String sql = String.Format("SELECT * FROM ComponentTable WHERE {0} = '{1}'", columnName, columnValue);
                OleDbDataAdapter adapter = new OleDbDataAdapter(sql, connection);
                OleDbCommandBuilder builder = new OleDbCommandBuilder(adapter);
                adapter.Fill(table);
            }
            catch (OleDbException ex)
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
            OleDbConnection connection = new OleDbConnection(connectionString);

            DataTable table = new DataTable();
            try
            {
                connection.Open();
                String sql = String.Format("SELECT * FROM ComponentTable WHERE id = {0}", id);
                OleDbDataAdapter adapter = new OleDbDataAdapter(sql, connection);
                OleDbCommandBuilder builder = new OleDbCommandBuilder(adapter);
                adapter.Fill(table);
            }
            catch (OleDbException ex)
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
            OleDbConnection connection = new OleDbConnection(connectionString);

            DataTable table = new DataTable();
            try
            {
                connection.Open();
                String sql = String.Format("SELECT * FROM LinkTable WHERE id = {0} ORDER BY id", id);
                OleDbDataAdapter adapter = new OleDbDataAdapter(sql, connection);
                OleDbCommandBuilder builder = new OleDbCommandBuilder(adapter);
                adapter.Fill(table);
            }
            catch (OleDbException ex)
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
            OleDbConnection connection = new OleDbConnection(connectionString);

            DataTable table = new DataTable();
            try
            {
                connection.Open();
                String sql = "SELECT * FROM LinkTable ORDER BY id";
                OleDbDataAdapter adapter = new OleDbDataAdapter(sql, connection);
                OleDbCommandBuilder builder = new OleDbCommandBuilder(adapter);
                adapter.Fill(table);
            }
            catch (OleDbException ex)
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
            OleDbConnection connection = new OleDbConnection(connectionString);

            DataTable table = new DataTable();
            List<String> dynamicLinkTypes = new List<String>();
            try
            {
                connection.Open();
                String sql = String.Format("SELECT type FROM LinkTable WHERE type LIKE '{0}%' GROUP BY type", linkType);
                OleDbDataAdapter adapter = new OleDbDataAdapter(sql, connection);
                OleDbCommandBuilder builder = new OleDbCommandBuilder(adapter);
                adapter.Fill(table);

                foreach (DataRow row in table.Rows)
                {
                    dynamicLinkTypes.Add((String)row[0]);
                }
            }
            catch (OleDbException ex)
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
            OleDbConnection connection = new OleDbConnection(connectionString);

            DataTable table = new DataTable();
            try
            {
                connection.Open();
                String sql = String.Format("SELECT * FROM LinkTable WHERE {0} = '{1}' ORDER BY id", "type", linkType);
                OleDbDataAdapter adapter = new OleDbDataAdapter(sql, connection);
                OleDbCommandBuilder builder = new OleDbCommandBuilder(adapter);
                adapter.Fill(table);
            }
            catch (OleDbException ex)
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
            OleDbConnection connection = new OleDbConnection(connectionString);

            DataTable table = new DataTable();
            try
            {
                connection.Open();
                String sql = String.Format("SELECT * FROM ParameterTable WHERE parentId = {0} AND parentType = '{1}'", parentId, parentType);
                OleDbDataAdapter adapter = new OleDbDataAdapter(sql, connection);
                OleDbCommandBuilder builder = new OleDbCommandBuilder(adapter);
                adapter.Fill(table);
            }
            catch (OleDbException ex)
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
            OleDbConnection connection = new OleDbConnection(connectionString);

            DataTable table = new DataTable();
            try
            {
                connection.Open();
                String sql = "SELECT * FROM ParameterTable";
                OleDbDataAdapter adapter = new OleDbDataAdapter(sql, connection);
                OleDbCommandBuilder builder = new OleDbCommandBuilder(adapter);
                adapter.Fill(table);
            }
            catch (OleDbException ex)
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
            OleDbConnection connection = new OleDbConnection(connectionString);

            String sql = String.Empty;
            DataTable tableSource = new DataTable();
            DataTable tableTemp = new DataTable();
            DataTable table = new DataTable();
            try
            {
                connection.Open();
                sql = String.Format("SELECT l.id, l.toComponentId FROM linktable l WHERE l.fromComponentid = {0} ORDER BY l.id", id);
                //String sql = String.Format("SELECT * FROM componenttable c WHERE c.id IN (SELECT l.toComponentId FROM linktable l WHERE l.fromComponentid = {0} AND l.type = '{1}')", id, linkType);
                OleDbDataAdapter adapter = new OleDbDataAdapter(sql, connection);
                OleDbCommandBuilder builder = new OleDbCommandBuilder(adapter);
                adapter.Fill(tableSource);

                foreach (DataRow row in tableSource.Rows)
                {
                    tableTemp = new DataTable();
                    sql = String.Format("SELECT * FROM componenttable c WHERE c.id = {0}", row["toComponentId"].ToString());
                    //String sql = String.Format("SELECT * FROM componenttable c WHERE c.id IN (SELECT l.toComponentId FROM linktable l WHERE l.fromComponentid = {0} AND l.type = '{1}')", id, linkType);
                    adapter = new OleDbDataAdapter(sql, connection);
                    builder = new OleDbCommandBuilder(adapter);
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
            //    String sql = String.Format("SELECT * FROM componenttable c WHERE c.id IN (SELECT l.toComponentId FROM linktable l WHERE l.fromComponentid = {0})", id);
            //    adapter = new OleDbDataAdapter(sql, connection);
            //    builder = new OleDbCommandBuilder(adapter);
            //    adapter.Fill(table);
            //}
            catch (OleDbException ex)
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
            OleDbConnection connection = new OleDbConnection(connectionString);

            String sql = String.Empty;
            DataTable tableSource = new DataTable();
            DataTable tableTemp = new DataTable();
            DataTable table = new DataTable();
            try
            {
                connection.Open();
                sql = String.Format("SELECT l.id, l.toComponentId FROM linktable l WHERE l.fromComponentid = {0} AND l.type = '{1}' ORDER BY l.id", id, linkType);
                //String sql = String.Format("SELECT * FROM componenttable c WHERE c.id IN (SELECT l.toComponentId FROM linktable l WHERE l.fromComponentid = {0} AND l.type = '{1}')", id, linkType);
                OleDbDataAdapter adapter = new OleDbDataAdapter(sql, connection);
                OleDbCommandBuilder builder = new OleDbCommandBuilder(adapter);
                adapter.Fill(tableSource);

                foreach (DataRow row in tableSource.Rows)
                {
                    tableTemp = new DataTable();
                    sql = String.Format("SELECT * FROM componenttable c WHERE c.id = {0}", row["toComponentId"].ToString());
                    //String sql = String.Format("SELECT * FROM componenttable c WHERE c.id IN (SELECT l.toComponentId FROM linktable l WHERE l.fromComponentid = {0} AND l.type = '{1}')", id, linkType);
                    adapter = new OleDbDataAdapter(sql, connection);
                    builder = new OleDbCommandBuilder(adapter);
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
            catch (OleDbException ex)
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
            OleDbConnection connection = new OleDbConnection(connectionString);

            String sql = String.Empty;
            DataTable tableSource = new DataTable();
            DataTable tableTemp = new DataTable();
            DataTable table = new DataTable();
            try
            {
                connection.Open();
                sql = String.Format("SELECT l.id, l.toComponentId FROM linktable l WHERE l.fromComponentid = {0} ORDER BY l.id", id);
                //String sql = String.Format("SELECT * FROM componenttable c WHERE c.id IN (SELECT l.toComponentId FROM linktable l WHERE l.fromComponentid = {0} AND l.type = '{1}')", id, linkType);
                OleDbDataAdapter adapter = new OleDbDataAdapter(sql, connection);
                OleDbCommandBuilder builder = new OleDbCommandBuilder(adapter);
                adapter.Fill(tableSource);

                foreach (DataRow row in tableSource.Rows)
                {
                    tableTemp = new DataTable();
                    sql = String.Format("SELECT * FROM componenttable c WHERE c.id = {0} AND {1} = '{2}'", row["toComponentId"].ToString(), columnName, columnValue);
                    //String sql = String.Format("SELECT * FROM componenttable c WHERE c.id IN (SELECT l.toComponentId FROM linktable l WHERE l.fromComponentid = {0} AND l.type = '{1}')", id, linkType);
                    adapter = new OleDbDataAdapter(sql, connection);
                    builder = new OleDbCommandBuilder(adapter);
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
            //    String sql = String.Format("SELECT * FROM componenttable WHERE id IN (SELECT l.toComponentId FROM linktable l WHERE l.fromComponentid = {0}) AND {1} = '{2}'", id, columnName, columnValue);
            //    adapter = new OleDbDataAdapter(sql, connection);
            //    builder = new OleDbCommandBuilder(adapter);
            //    adapter.Fill(table);
            //}
            catch (OleDbException ex)
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
            OleDbConnection connection = new OleDbConnection(connectionString);

            String sql = String.Empty;
            DataTable tableSource = new DataTable();
            DataTable tableTemp = new DataTable();
            DataTable table = new DataTable();
            try
            {
                connection.Open();
                sql = String.Format("SELECT l.id, l.toComponentId FROM linktable l WHERE l.fromComponentid = {0} AND l.type = '{1}' ORDER BY l.id", id, linkType);
                //String sql = String.Format("SELECT * FROM componenttable c WHERE c.id IN (SELECT l.toComponentId FROM linktable l WHERE l.fromComponentid = {0} AND l.type = '{1}')", id, linkType);
                OleDbDataAdapter adapter = new OleDbDataAdapter(sql, connection);
                OleDbCommandBuilder builder = new OleDbCommandBuilder(adapter);
                adapter.Fill(tableSource);

                foreach (DataRow row in tableSource.Rows)
                {
                    tableTemp = new DataTable();
                    sql = String.Format("SELECT * FROM componenttable c WHERE c.id = {0} AND {1} = '{2}'", row["toComponentId"].ToString(), columnName, columnValue);
                    //String sql = String.Format("SELECT * FROM componenttable c WHERE c.id IN (SELECT l.toComponentId FROM linktable l WHERE l.fromComponentid = {0} AND l.type = '{1}')", id, linkType);
                    adapter = new OleDbDataAdapter(sql, connection);
                    builder = new OleDbCommandBuilder(adapter);
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
            //    String sql = String.Format("SELECT * FROM componenttable WHERE id IN (SELECT l.toComponentId FROM linktable l WHERE l.fromComponentid = {0} AND l.type = '{1}') AND {2} = '{3}'", id, linkType, columnName, columnValue);
            //    adapter = new OleDbDataAdapter(sql, connection);
            //    builder = new OleDbCommandBuilder(adapter);
            //    adapter.Fill(table);
            //}
            catch (OleDbException ex)
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

            OleDbConnection connection = new OleDbConnection(connectionString);

            String sql = String.Empty;
            DataTable tableTypes = new DataTable();
            DataTable tableFrom = new DataTable();
            DataTable tableTo = new DataTable();
            DataTable table = new DataTable();
            try
            {
                connection.Open();
                sql = String.Format("SELECT l.type FROM vsg.linktable l GROUP BY l.type");
                OleDbDataAdapter adapter = new OleDbDataAdapter(sql, connection);
                OleDbCommandBuilder builder = new OleDbCommandBuilder(adapter);
                adapter.Fill(tableTypes);

                foreach (DataRow rowTypes in tableTypes.Rows)
                {
                    tableFrom = new DataTable();
                    sql = String.Format("SELECT l.fromComponentId FROM vsg.linktable l WHERE l.type = '{0}' GROUP BY l.fromComponentId", rowTypes["type"].ToString());
                    adapter = new OleDbDataAdapter(sql, connection);
                    builder = new OleDbCommandBuilder(adapter);
                    adapter.Fill(tableFrom);

                    foreach (DataRow rowFrom in tableFrom.Rows)
                    {
                        tableTo = new DataTable();
                        sql = String.Format("SELECT l.fromComponentId FROM vsg.linktable l WHERE l.type = '{0}' AND l.toComponentId = '{1}'", rowTypes["type"].ToString(), rowFrom["fromComponentId"].ToString());
                        adapter = new OleDbDataAdapter(sql, connection);
                        builder = new OleDbCommandBuilder(adapter);
                        adapter.Fill(tableTo);

                        if (tableTo.Rows.Count == 0)
                            collection.Add(rowFrom["fromComponentId"].ToString(), rowTypes["type"].ToString());
                        else if (rowFrom["fromComponentId"].ToString().Equals(rowTypes["type"].ToString()))
                            collection.Add(rowFrom["fromComponentId"].ToString(), rowTypes["type"].ToString());
                    }
                }
            }
            catch (OleDbException ex)
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
            OleDbConnection connection = new OleDbConnection(connectionString);

            String sql = String.Empty;
            DataTable tableSource = new DataTable();
            DataTable tableTemp = new DataTable();
            DataTable table = new DataTable();
            try
            {
                connection.Open();
                sql = String.Format("SELECT l.fromComponentId FROM linktable l WHERE l.toComponentid = {0} ORDER BY l.id", id);
                //    String sql = String.Format("SELECT * FROM componenttable c WHERE c.id IN (SELECT l.fromComponentId FROM linktable l WHERE l.toComponentid = {0})", id);
                OleDbDataAdapter adapter = new OleDbDataAdapter(sql, connection);
                OleDbCommandBuilder builder = new OleDbCommandBuilder(adapter);
                adapter.Fill(tableSource);

                foreach (DataRow row in tableSource.Rows)
                {
                    tableTemp = new DataTable();
                    sql = String.Format("SELECT * FROM componenttable c WHERE c.id = {0}", row["fromComponentId"].ToString());
                    adapter = new OleDbDataAdapter(sql, connection);
                    builder = new OleDbCommandBuilder(adapter);
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
            catch (OleDbException ex)
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
            //    String sql = String.Format("SELECT * FROM componenttable c WHERE c.id IN (SELECT l.fromComponentId FROM linktable l WHERE l.toComponentid = {0})", id);
            //    adapter = new OleDbDataAdapter(sql, connection);
            //    builder = new OleDbCommandBuilder(adapter);
            //    adapter.Fill(table);
            //}
            //catch (OleDbException ex)
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
            OleDbConnection connection = new OleDbConnection(connectionString);

            String sql = String.Empty;
            DataTable tableSource = new DataTable();
            DataTable tableTemp = new DataTable();
            DataTable table = new DataTable();
            try
            {
                connection.Open();
                sql = String.Format("SELECT l.fromComponentId FROM linktable l WHERE l.toComponentid = {0} AND l.type = '{1}' ORDER BY l.id", id, linkType);
                //String sql = String.Format("SELECT * FROM componenttable c WHERE c.id IN (SELECT l.toComponentId FROM linktable l WHERE l.fromComponentid = {0} AND l.type = '{1}')", id, linkType);
                OleDbDataAdapter adapter = new OleDbDataAdapter(sql, connection);
                OleDbCommandBuilder builder = new OleDbCommandBuilder(adapter);
                adapter.Fill(tableSource);

                foreach (DataRow row in tableSource.Rows)
                {
                    tableTemp = new DataTable();
                    sql = String.Format("SELECT * FROM componenttable c WHERE c.id = {0}", row["fromComponentId"].ToString());
                    //String sql = String.Format("SELECT * FROM componenttable c WHERE c.id IN (SELECT l.toComponentId FROM linktable l WHERE l.fromComponentid = {0} AND l.type = '{1}')", id, linkType);
                    adapter = new OleDbDataAdapter(sql, connection);
                    builder = new OleDbCommandBuilder(adapter);
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
            catch (OleDbException ex)
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
            OleDbConnection connection = new OleDbConnection(connectionString);

            DataTable table = new DataTable();
            try
            {
                connection.Open();
                String sql = String.Format("SELECT * FROM LinkTable WHERE fromComponentid = {0} ORDER BY id", id);
                OleDbDataAdapter adapter = new OleDbDataAdapter(sql, connection);
                OleDbCommandBuilder builder = new OleDbCommandBuilder(adapter);
                adapter.Fill(table);
            }
            catch (OleDbException ex)
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
            OleDbConnection connection = new OleDbConnection(connectionString);

            DataTable table = new DataTable();
            try
            {
                connection.Open();
                String sql = String.Format("SELECT * FROM LinkTable WHERE fromComponentid = {0} AND type = '{1}' ORDER BY id", id, linkType);
                OleDbDataAdapter adapter = new OleDbDataAdapter(sql, connection);
                OleDbCommandBuilder builder = new OleDbCommandBuilder(adapter);
                adapter.Fill(table);
            }
            catch (OleDbException ex)
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
            String outputFilepath = Path.GetFullPath(configurationPath + @"\output");
            String simRunFile = Path.Combine(outputFilepath, filename + ".xml");
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
            OleDbConnection connection = new OleDbConnection(connectionString);

            Int32 id = -1;
            try
            {
                String sql = String.Empty;
                OleDbCommand command;
                connection.Open();
                sql = String.Format("INSERT INTO ComponentTable (type, name, etype, description) VALUES ('{0}', '{1}', '{2}', '{3}')", component, safeSqlLiteral(name), etype, safeSqlLiteral(description));
                command = new OleDbCommand(sql, connection);
                command.ExecuteNonQuery();

                sql = "SELECT @@IDENTITY AS 'id'";
                command = new OleDbCommand(sql, connection);
                OleDbDataReader datareader = command.ExecuteReader();
                datareader.Read();
                id = Convert.ToInt32(datareader.GetValue(0).ToString());
                datareader.Close();
            }
            catch (OleDbException ex)
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
            OleDbConnection connection = new OleDbConnection(connectionString);

            DataTable table = new DataTable();
            Int32 id = -1;
            try
            {
                connection.Open();
                String sql = String.Format("SELECT * FROM ComponentTable WHERE type = '{0}' AND name = '{1}'", component, name);
                OleDbDataAdapter adapter = new OleDbDataAdapter(sql, connection);

                OleDbCommandBuilder builder = new OleDbCommandBuilder(adapter);
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
            catch (OleDbException ex)
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
            OleDbConnection connection = new OleDbConnection(connectionString);

            Int32 rows = -1;
            try
            {
                connection.Open();
                String sql = String.Format("UPDATE ComponentTable SET {0} = '{1}' WHERE id = '{2}'", columnName, safeSqlLiteral(columnValue), id);
                OleDbCommand command = new OleDbCommand(sql, connection);
                rows = command.ExecuteNonQuery();
            }
            catch (OleDbException ex)
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
            OleDbConnection connection = new OleDbConnection(connectionString);

            Int32 id = -1;
            try
            {
                connection.Open();
                String sql = String.Format("INSERT INTO LinkTable (fromComponentId, toComponentId, type, description) VALUES ({0}, '{1}', '{2}', '{3}')", (fromComponentId.HasValue ? Convert.ToString(fromComponentId) : "null"), toComponentId, type, description);
                OleDbCommand command = new OleDbCommand(sql, connection);
                command.ExecuteNonQuery();

                sql = "SELECT @@IDENTITY AS 'id'";
                command = new OleDbCommand(sql, connection);
                OleDbDataReader datareader = command.ExecuteReader();
                datareader.Read();
                id = Convert.ToInt32(datareader.GetValue(0).ToString());
                datareader.Close();
            }
            catch (OleDbException ex)
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
            OleDbConnection connection = new OleDbConnection(connectionString);

            Int32 rows = -1;
            try
            {
                connection.Open();
                String sql = String.Format("INSERT INTO ParameterTable ([parentId], [parentType], [name], [value], [description]) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}')", parentId, parentType, name, safeSqlLiteral(value), safeSqlLiteral(description));
                OleDbCommand command = new OleDbCommand(sql, connection);
                rows = command.ExecuteNonQuery();
            }
            catch (OleDbException ex)
            {
                if (ex.ErrorCode.Equals(-2147467259))
                {
                    String sql = String.Format("UPDATE ParameterTable SET [value] = '{3}', [description] = '{4}' WHERE [parentId] = {0} AND [parentType] = '{1}' AND [name] = '{2}'", parentId, parentType, name, safeSqlLiteral(value), safeSqlLiteral(description));
                    OleDbCommand command = new OleDbCommand(sql, connection);
                    rows = command.ExecuteNonQuery();
                }
                else
                    MessageBox.Show(ex.Message + " " + value, String.Format("Failed to add parameter with parentId '{0}', parentType '{1}' and name '{2}'", parentId, parentType, name));

                //MessageBox.Show(ex.Message + " " + value, String.Format("Failed to add parameter with parentId '{0}', parentType '{1}' and name '{2}'", parentId, parentType, name));
            }
            finally
            {
                connection.Close();
            }
            return rows;
        }
        private List<Int32> getLinkIds(Int32 componentId)
        {
            OleDbConnection connection = new OleDbConnection(connectionString);

            DataTable table = new DataTable();
            List<Int32> ids = new List<Int32>();
            try
            {
                connection.Open();
                String sql = String.Format("SELECT id FROM LinkTable WHERE fromComponentid = {0} OR toComponentId = '{1}' ORDER BY id", componentId, componentId);
                OleDbDataAdapter adapter = new OleDbDataAdapter(sql, connection);
                OleDbCommandBuilder builder = new OleDbCommandBuilder(adapter);
                adapter.Fill(table);

                foreach (DataRow row in table.Rows)
                {
                    ids.Add((Int32)row[0]);
                }
            }
            catch (OleDbException ex)
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
            OleDbConnection connection = new OleDbConnection(connectionString);

            Int32 rows = -1;
            try
            {
                connection.Open();
                String sql = String.Format("DELETE FROM ComponentTable WHERE id = {0}", id);
                OleDbCommand command = new OleDbCommand(sql, connection);
                rows = command.ExecuteNonQuery();
            }
            catch (OleDbException ex)
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
            OleDbConnection connection = new OleDbConnection(connectionString);

            Int32 rows = -1;
            try
            {
                connection.Open();
                String sql = String.Format("DELETE FROM LinkTable WHERE fromComponentid = {0} OR toComponentId = '{1}'", componentId, componentId);
                OleDbCommand command = new OleDbCommand(sql, connection);
                rows = command.ExecuteNonQuery();
            }
            catch (OleDbException ex)
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
            OleDbConnection connection = new OleDbConnection(connectionString);

            Int32 rows = -1;
            try
            {
                connection.Open();
                String sql = String.Format("DELETE FROM ParameterTable WHERE parentId = {0} AND parentType = '{1}'", parentId, parentType);
                OleDbCommand command = new OleDbCommand(sql, connection);
                rows = command.ExecuteNonQuery();
            }
            catch (OleDbException ex)
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
            OleDbConnection connection = new OleDbConnection(connectionString);

            Int32 rows = -1;
            try
            {
                connection.Open();
                String sql = String.Format("DELETE FROM LinkTable WHERE id = {0}", id);
                OleDbCommand command = new OleDbCommand(sql, connection);
                rows = command.ExecuteNonQuery();
            }
            catch (OleDbException ex)
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
