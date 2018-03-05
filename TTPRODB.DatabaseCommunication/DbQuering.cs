using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
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

        public static Dictionary<string, dynamic> GetAllInventory(Type inventoryType)
        {
            Dictionary<string, dynamic> producers = new Dictionary<string, dynamic>();
            using (var connection = new SqlConnection(DbConnect.DbConnectionString))
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = $"SELECT * FROM Item, {inventoryType.Name} as inv WHERE Item.ID = inv.Item_ID";
                    SqlDataReader sdr = cmd.ExecuteReader();
                    while (sdr.Read())
                    {

                    }
                }
            }
        }

        public static void InsertProducers(List<Producer> producers)
        {
            using (var connection = new SqlConnection(DbConnect.DbConnectionString))
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "INSERT INTO Producer (Id, Name) VALUES (@Id, @Name)";
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

        /// <summary>
        /// Insert inventory into Db
        /// </summary>
        /// <param name="itemsList">List of inventory</param>
        public static void InsertItems(List<dynamic> itemsList)
        {
            using (var connection = new SqlConnection(DbConnect.DbConnectionString))
            {
                connection.Open();
                // create two command to insert into Item table and specific inventory table
                using (SqlCommand cmd1 = connection.CreateCommand(),
                       cmd2 = connection.CreateCommand())
                {
                    // build query into Item table
                    cmd1.CommandType = CommandType.Text;
                    cmd1.CommandText = "INSERT INTO Item (ID, Name, Producer_ID, Ratings) VALUES (@ItemId, @Name, @Url, @ProducerId, @Ratings)";

                    foreach (PropertyInfo itemProperty in itemProperties)
                    {
                        cmd1.Parameters.Add($"@{itemProperty.Name}");
                    }

                    PropertyInfo[] inventoryProperties = BuidInventoryInsertQuery(itemsList, cmd2);

                    // 
                    foreach (dynamic item in itemsList)
                    {
                        // init isert into Item command parrameters value
                        Item localItem = (Item)item;
                        for (int i = 0; i < cmd1.Parameters.Count; i++)
                        {
                            cmd1.Parameters[i].Value = itemProperties[i].GetValue(localItem);
                        }

                        // init isert into inventory command parrameters value
                        for (int i = 0; i < cmd2.Parameters.Count - 1; i++)
                        {
                            cmd2.Parameters[i].Value = inventoryProperties[i].GetValue(item);
                        }
                        // init Item_Id parametr
                        cmd2.Parameters[cmd2.Parameters.Count - 1].Value = item.ItemId;

                        // Execute queries
                        cmd1.ExecuteNonQuery();
                        cmd2.ExecuteNonQuery();
                    }
                }
            }
        }

        private static PropertyInfo[] BuidInventoryInsertQuery(List<dynamic> itemsList, SqlCommand cmd)
        {
            // Get inventory type and properties
            string table;
            PropertyInfo[] inventoryProperties;
            GetTableName(itemsList, out table, out inventoryProperties);
            // build query into invetory table and init command parameters
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = $"INSERT INTO {table}";
            StringBuilder collumnsBuilder = new StringBuilder(" (");
            StringBuilder valuesBuilder = new StringBuilder(" VALUES(");
            foreach (PropertyInfo inventoryProperty in inventoryProperties)
            {
                string parametr = inventoryProperty.Name;
                collumnsBuilder.Append($"{parametr},");
                valuesBuilder.Append($"@{parametr},");
                cmd.Parameters.Add($"@{parametr}");
            }

            // add to query Item_ID parametr
            collumnsBuilder.Append("Item_ID)");
            valuesBuilder.Append("@ItemId");
            cmd.Parameters.Add("@ItemId");
            cmd.CommandText += collumnsBuilder.ToString() + valuesBuilder.ToString();
            return inventoryProperties;
        }

        private static void GetTableName(List<dynamic> itemsList, out string table, out PropertyInfo[] inventoryProperties)
        {
            Type inventoryType = itemsList[0].GetType();
            table = inventoryType.Name;
            inventoryProperties = inventoryType.GetProperties();
        }

        public static void UpdateItems(List<dynamic> itemsList)
        {
            using (var connection = new SqlConnection(DbConnect.DbConnectionString))
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {
                    string table;
                    PropertyInfo[] inventoryProperties;
                    GetTableName(itemsList, out table, out inventoryProperties);
                    cmd.CommandType = CommandType.Text;
                    PropertyInfo[] invetoryCharacteristicProperties =
                        inventoryProperties.Where(x => x.GetType() != typeof(Int32)).ToArray();
                    StringBuilder updateStringBuilder = new StringBuilder();
                    
                    foreach (PropertyInfo invetoryCharacteristic in invetoryCharacteristicProperties)
                    {
                        updateStringBuilder.Append($"{invetoryCharacteristic.Name}=@{invetoryCharacteristic.Name},");
                        cmd.Parameters.Add($"@{invetoryCharacteristic.Name}");
                    }

                    updateStringBuilder = updateStringBuilder.Remove(updateStringBuilder.Length - 1, 1);
                    string commandText = cmd.CommandText + updateStringBuilder;

                    foreach (dynamic item in itemsList)
                    {
                        
                        for (int i = 0; i < cmd.Parameters.Count; i++)
                        {
                            cmd.Parameters[i].Value = invetoryCharacteristicProperties[i].GetValue(item);
                        }
                        string condition = $" WHERE Name={item.Name}";
                        cmd.CommandText = commandText + condition;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

    }
}
