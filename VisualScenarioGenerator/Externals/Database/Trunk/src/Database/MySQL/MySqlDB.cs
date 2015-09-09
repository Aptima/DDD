using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;
using Microsoft.Win32;
using System.Windows.Forms;
using System.IO;

namespace Database
{
    public class MySqlDB : DB
    {
        //private readonly String DATABASE_NAME;
        private String server;
        private Int32 port;
        private String username;
        private String password;
        private String database;
        private Boolean import;

        private MySqlConnection connection;

        public MySqlDB(String server, Int32 port, String username, String password, String database, Boolean import)
        {
            this.database = database;

            //Console.WriteLine("MySqlDB creation.");
            this.server = server;
            this.port = port;
            this.username = username;
            this.password = password;
            this.import = import;
        }

        private void importSQL(String project)
        {
            String exePath = Directory.GetCurrentDirectory();
            String sqlFilename = Path.Combine(Path.Combine(exePath, @"config\data"), project);
            String xmlSourceFilename = sqlFilename.Replace(".sql", ".xml");
            String xmlDestinationFilename = Path.Combine(exePath,  @"config\xml\Diagrams.xml");

            String mysqlDB = "mysql";
            String connectionString = String.Format("server={0}; port={1}; user id={2}; password={3}; database={4}; pooling=false", server, port, username, password, mysqlDB);

            try
            {
                //Console.WriteLine("Opening connection to the database.");
                connection = new MySqlConnection(connectionString);
                connection.Open();

                FileInfo sql = new FileInfo(sqlFilename);
                TextReader textreader = sql.OpenText();
                string all = textreader.ReadToEnd();
                string[] allSplit = all.Split(new string[] {"INSERT INTO" }, StringSplitOptions.None);

                for (int i = 0; i < allSplit.Length; i++)
                {
                    string splitsql = allSplit[i];
                    if (i > 0)
                    {
                        splitsql = "INSERT INTO" + splitsql;
                    }
                    MySqlCommand command = new MySqlCommand(splitsql, connection);
                    command.ExecuteNonQuery();
                }

                FileInfo xmlSource = new FileInfo(xmlSourceFilename);
                FileInfo xmlDestination = new FileInfo(xmlDestinationFilename);
                //DirectoryInfo currentDir = Directory.CreateDirectory
                DirectoryInfo xmlDestinationDir = new DirectoryInfo(xmlSource.DirectoryName);
                Directory.CreateDirectory(xmlDestination.DirectoryName);
                xmlSource.CopyTo(xmlDestinationFilename, true);
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error connecting to the server: " + ex.Message, "MySql Exception");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Exception");
            }
            finally
            {
                //Console.WriteLine(String.Format("Closing connection to the database."));
                connection.Close();
                //Console.WriteLine();
            }
        }

        private void create(String project)
        {
            String mysqlDB = "mysql";
            String connectionString = String.Format("server={0}; port={1}; user id={2}; password={3}; database={4}; pooling=false", server, port, username, password, mysqlDB);
            DataSet dataset = new DataSet(database);
            DataTable table;
            MySqlCommand command;
            MySqlDataReader datareader;

            try
            {
                //Console.WriteLine("Opening connection to the '{0}' database.", mysqlDB);
                connection = new MySqlConnection(connectionString);
                connection.Open();

                //Console.WriteLine("Deleting the '{0}' database!", database);
                String dropDBQuery = String.Format("DROP DATABASE IF EXISTS {0}", database);
                command = new MySqlCommand(dropDBQuery, connection);
                command.ExecuteNonQuery();

                //Console.WriteLine("Creating the '{0}' database.", database);
                String createDBQuery = String.Format("CREATE DATABASE IF NOT EXISTS {0}", database);
                command = new MySqlCommand(createDBQuery, connection);
                command.ExecuteNonQuery();

                //Console.WriteLine("Switching the connection to the '{0}' database.", database);
                connection.ChangeDatabase(database);

                // Create Tables and schema file.
                String createTableQuery = String.Empty;
                String tableName = String.Empty;
                String sql = String.Empty;

                // ComponentTable
                tableName = "ComponentTable";
                //Console.WriteLine("Creating the '{0}' table.", tableName);
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
                //Console.WriteLine("Creating the '{0}' table.", tableName);
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
                //Console.WriteLine("Creating the '{0}' table.", tableName);
                createTableQuery = String.Format("CREATE TABLE {0} (id INT NOT NULL AUTO_INCREMENT, fromComponentId INT, toComponentId INT, type VARCHAR(80), description VARCHAR(255), PRIMARY KEY (id)) ENGINE = MYISAM", tableName);
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

                //RegistryKey rk = Registry.CurrentUser.CreateSubKey(@"Software\Aptima\MOST");
                //String path = System.Convert.ToString(rk.GetValue("ConfigPath", ""));
                //if (!path.Equals(String.Empty))
                // {
                //    dataset.WriteXmlSchema(path + @"\" + "db.xsd");
                //}               
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error connecting to the server: " + ex.Message + '\n');
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                //Console.WriteLine(String.Format("Closing connection to the '{0}' database.", DATABASE_NAME));
                connection.Close();
                //Console.WriteLine();
            }
        }

        public void CreateDB(String project)
        {
            if (import)
            {
                importSQL(project);
            }
            else
            {
                create(project);
            }
        }
    }
}