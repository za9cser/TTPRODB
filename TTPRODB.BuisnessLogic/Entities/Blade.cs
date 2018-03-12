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
