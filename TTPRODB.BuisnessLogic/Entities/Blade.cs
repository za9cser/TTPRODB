namespace TTPRODB.BuisnessLogic.Entities
{
    public class Blade
    {
        public new int Id { get; set; }
        public double Speed { get; set; }
        public double Control { get; set; }
        public double Stiffness { get; set; }
        public double Hardness { get; set; }
        public double Consistency { get; set; }
        public double Overall { get; set; }        

        public Blade() { }
        //public Blade(MySqlDataReader reader)
        //{
        //    Id = reader.GetInt32(0);
        //    Url = reader.GetString(1);
        //    Producer_ID = reader.GetInt32(2);
        //    Name = reader.GetString(3);
        //    Speed = reader.GetDouble(4);
        //    Control = reader.GetDouble(5);
        //    Stiffness = reader.GetDouble(6);
        //    Hardness = reader.GetDouble(7);
        //    Consistency = reader.GetDouble(8);
        //    Overall = reader.GetDouble(9);
        //    Ratings = reader.GetInt32(10);
        //}
    }
}
