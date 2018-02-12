namespace TTPRODB.BuisnessLogic.Entities
{
    public class Pips : Item
    {
        public int Id { get; set; }
        public double Speed { get; set; }
        public double Spin { get; set; }
        public double Control { get; set; }
        public double Deception { get; set; }
        public double Reversal { get; set; }
        public double Weight { get; set; }
        public double Hardness { get; set; }
        public double Consistency { get; set; }
        public double Durability { get; set; }
        public double Overall { get; set; }
        public string PipsType { get; set; }

        public Pips() { }
        //public Pips(MySqlDataReader reader)
        //{
        //    Id = reader.GetInt32(0);
        //    Url = reader.GetString(1);
        //    Producer_ID = reader.GetInt32(2);
        //    Name = reader.GetString(3);
        //    Speed = reader.GetDouble(4);
        //    Spin = reader.GetDouble(5);
        //    Control = reader.GetDouble(6);
        //    Deception = reader.GetDouble(7);
        //    Reversal = reader.GetDouble(8);
        //    Weight = reader.GetDouble(9);
        //    Hardness = reader.GetDouble(10);            
        //    Consistency = reader.GetDouble(11);
        //    Durability = reader.GetDouble(12);
        //    Overall = reader.GetDouble(13);
        //    Type = reader.GetString(14);
        //    Ratings = reader.GetInt32(15);
        //}

    }
}
