using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using TTPRODB.BuisnessLogic.Entities;

namespace TTPRODB.DatabaseCommunication
{
    public static class DbQuering
    {
        private static PropertyInfo[] itemProperties = typeof(Item).GetProperties();

        public static int GetItemCount()
        {
            int count;
            using (var connection = new SqlConnection(DbConnect.DbConnectionString))
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = @"SELECT COUNT(*) FROM Items";
                    count = (int) cmd.ExecuteScalar();
                }
            }

            return count;
        }

        public static Dictionary<string, Producer> GetAllProducers()
        {
            Dictionary<string, Producer> producers = new Dictionary<string, Producer>();
            using (var connection = new SqlConnection(DbConnect.DbConnectionString))
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = @"SELECT * FROM Producers";
                    SqlDataReader sqlDataReader = cmd.ExecuteReader();
                    while (sqlDataReader.Read())
                    {
                        producers.Add(sqlDataReader.GetString(1),
                            new Producer(sqlDataReader.GetInt32(0), sqlDataReader.GetString(1)));
                    }
                }
            }

            return producers.Count == 0 ? null : producers;
        }

        public static void InsertProducers(List<Producer> producers)
        {
            using (var connection = new SqlConnection(DbConnect.DbConnectionString))
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "INSERT INTO Producers (Id, Name) VALUES (@Id, @Name)";
                    cmd.Parameters.Add("@Id", DbType.Int32);
                    cmd.Parameters.Add("@Name", DbType.String);

                    foreach (Producer producer in producers)
                    {
                        cmd.Parameters[0].Value = producer.Id;
                        cmd.Parameters[1].Value = producer.Name;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public static void InsertItems(List<dynamic> itemsList)
        {
            using (var connection = new SqlConnection(DbConnect.DbConnectionString))
            {
                connection.Open();
                using (SqlCommand cmd1 = connection.CreateCommand(),
                       cmd2 = connection.CreateCommand())
                {
                    cmd1.CommandType = CommandType.Text;
                    cmd1.CommandText = "INSERT INTO Items (ID, Name, Producer_ID, Ratings) VALUES (@ItemId, @Name, @Url, @ProducerId, @Ratings)";

                    foreach (PropertyInfo itemProperty in itemProperties)
                    {
                        cmd1.Parameters.Add("@" + itemProperty.Name);
                    }

                    

                }
            }
        }
    }
}
