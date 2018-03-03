using System.Data.Linq.Mapping;
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

        public Rubber(int itemId, string name, string url, int producerId, int ratings,
            int id, double speed, double spin, double control, double tackiness, double weight, double hardness, double gears, double throwAngle,
            double consistency, double durability,double overall, bool tensor, bool anti)
            : base(itemId, name, url, producerId, ratings)
        {
            Id = id;
            Speed = speed;
            Spin = spin;
            Control = control;
            Tackiness = tackiness;
            Weight = weight;
            Hardness = hardness;
            Gears = gears;
            ThrowAngle = throwAngle;
            Consistency = consistency;
            Durability = durability;
            Overall = overall;
            Tensor = tensor;
            Anti = anti;
        }
    }
}
