using System.Data.Linq.Mapping;
using System.Data.SqlClient;

namespace TTPRODB.BuisnessLogic.Entities
{
    
    public class Blade : Item
    {
        public int Id { get; set; }
        public double Speed { get; set; }
        public double Control { get; set; }
        public double Stiffness { get; set; }
        public double Hardness { get; set; }
        public double Consistency { get; set; }
        public double Overall { get; set; }

        public Blade() { }

        public Blade(int itemId, string name, string url, int producerId, int ratings, int id,
            double speed, double control, double stiffness, double hardness, double consistency, double overall)
            : base(itemId, name, url, producerId, ratings)
        {
            Id = id;
            Speed = speed;
            Control = control;
            Stiffness = stiffness;
            Hardness = hardness;
            Consistency = consistency;
            Overall = overall;
        }

        public Blade(int id, double speed, double control, double stiffness, double hardness,
            double consistency, double overall)
        {
            Id = id;
            Speed = speed;
            Control = control;
            Stiffness = stiffness;
            Hardness = hardness;
            Consistency = consistency;
            Overall = overall;
        }

        public Blade(SqlDataReader sdr) : base(sdr)
        {
            Id = sdr.GetInt32(5);
            Speed = sdr.GetDouble(7);
            Control = sdr.GetDouble(8);
            Stiffness = sdr.GetDouble(9);
            Hardness = sdr.GetDouble(10);
            Consistency = sdr.GetDouble(11);
            Overall = sdr.GetDouble(12);
        }
    }
}
