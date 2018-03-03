using System.Data.SqlClient;

namespace TTPRODB.BuisnessLogic.Entities
{
    public class Rubber : Item
    {
        public int Id { get; set; }
        public double Speed { get; set; }
        public double Spin { get; set; }
        public double Control { get; set; }
        public double Tackiness { get; set; }
        public double Weight { get; set; }
        public double Hardness { get; set; }
        public double Gears { get; set; }
        public double ThrowAngle { get; set; }
        public double Consistency { get; set; }
        public double Durability { get; set; }
        public double Overall { get; set; }
        public bool Tensor { get; set; }
        public bool Anti { get; set; }

        public Rubber() { }

        public Rubber(SqlDataReader reader)
        {
            
            Id = reader.GetInt32(0);
            Url = reader.GetString(1);
            ProducerId = reader.GetInt32(2);
            Name = reader.GetString(3);
            Speed = reader.GetDouble(4);
            Spin = reader.GetDouble(5);
            Control = reader.GetDouble(6);
            Tackiness = reader.GetDouble(7);
            Weight = reader.GetDouble(8);
            Hardness = reader.GetDouble(9);
            Gears = reader.GetDouble(10);
            ThrowAngle = reader.GetDouble(11);
            Consistency = reader.GetDouble(12);
            Durability = reader.GetDouble(13);
            Overall = reader.GetDouble(14);
            Ratings = reader.GetInt32(15);
        }
             
    }
}
