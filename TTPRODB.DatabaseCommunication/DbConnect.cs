using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Resources;

namespace TTPRODB.DatabaseCommunication
{
    public static class DbConnect
    {
        public static string DbConnectionString { get; } = @"Data Source=(localdb)\MSSQLLocalDB;AttachDbFilename =|DataDirectory|\TTPRODB.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private static string dbName = "TTPRODB";        

        // connect to database
        public static void ValidateDatabase()
        {
            // init mdf file parameters
            string outputFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string mdfFilename = dbName + ".mdf";
            string dbFileName = Path.Combine(outputFolder, mdfFilename);

            // If the file doesn't exists, create it
            if (!File.Exists(dbFileName))
            { 
                CreateDatabase(dbFileName);
            }

            // Check db structure

            // Check db data
            CheckDbData();
        }

        /// <summary>
        /// Checks data exists
        /// </summary>
        /// <returns></returns>
        public static bool CheckDbData()
        {
            bool checkFlag = true;
            using (var connection = new SqlConnection(DbConnectionString))
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "SELECT Count(*) FROM Items";
                    if ((int) cmd.ExecuteScalar() == 0)
                    {
                        checkFlag = false;
                    }
                }
            }

            return checkFlag;
        }

        // create local database
        public static void CreateDatabase(string dbFileName)
        {
            try
            {
                // connect to localdb and create database
                string connectionString =
                    @"Data Source=(localdb)\MSSQLLocalDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (var cmd = connection.CreateCommand())
                    {
                        // detach database
                        DetachDatabase(connection);
                        // create new database
                        cmd.CommandText =
                            $"CREATE DATABASE {dbName} ON (NAME = N'{dbName}', FILENAME = '{dbFileName}')";
                        cmd.ExecuteNonQuery();                        
                    }
                }

                // if database created, open connection to it and create structure
                // load and execute sql script to create database's structure and add foreign keys
                using (var connection = new SqlConnection(DbConnectionString))
                {
                    connection.Open();
                    using (var cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = Resources.DbStructure.CreateTables;
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = Resources.DbStructure.AddForeignKeys;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException)
            {
                
            }
        }

        // detach database
        public static void DetachDatabase(SqlConnection connection)
        {
            try
            {
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = $"exec sp_detach_db '{dbName}'";
                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException)
            {

            }
        }

    }
}
