using System;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;

namespace TTPRODB.DatabaseCommunication
{
    public class DbConnect
    {
        private static string dbConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;AttachDbFilename =|DataDirectory|\TTPRODB.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
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
        }

        // create local database
        public static void CreateDatabase(string dbFileName)
        {
            try
            {
                // connect to localdb and create database
                const string connectionString =
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
                ExecuteQuery(File.ReadAllText(@"Scripts\CreateDbStructure.sql"),
                             File.ReadAllText(@"Scripts\\AddForeignKeys.sql"));
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

        // execute Query to database
        public static SqlDataReader[] ExecuteQuery(params string[] queries)
        {
            SqlDataReader[] resultSqlDataReaders = new SqlDataReader[queries.Length];
            using (var connection = new SqlConnection(dbConnectionString))
            {
                try
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        try
                        {
                            for (int i = 0; i < queries.Length; i++)
                            {
                                try
                                {
                                    command.CommandText = queries[i];
                                    resultSqlDataReaders[i] = command.ExecuteReader();
                                }
                                catch (SqlException)
                                {
                                }
                            }
                        }
                        catch (NullReferenceException)
                        {
                            
                        }
                    }
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e.Message);
                    throw;
                }
            }
            return resultSqlDataReaders;
        }
    }
}
