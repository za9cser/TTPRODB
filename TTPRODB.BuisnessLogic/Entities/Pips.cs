using System.Data.Linq.Mapping;

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

        public Pips(int itemId, string name, string url, int producerId, int ratings, int id, double speed, 
            double spin, double control, double deception, double reversal, double weight, double hardness, 
            double consistency, double durability, double overall, string pipsType) 
            : base(itemId, name, url, producerId, ratings)
        {
            Id = id;
            Speed = speed;
            Spin = spin;
            Control = control;
            Deception = deception;
            Reversal = reversal;
            Weight = weight;
            Hardness = hardness;
            Consistency = consistency;
            Durability = durability;
            Overall = overall;
            PipsType = pipsType;
        }

        public Pips(int id, double speed, double spin, double control, double deception, double reversal, 
            double weight, double hardness, double consistency, double durability, double overall, string pipsType)
        {
            Id = id;
            Speed = speed;
            Spin = spin;
            Control = control;
            Deception = deception;
            Reversal = reversal;
            Weight = weight;
            Hardness = hardness;
            Consistency = consistency;
            Durability = durability;
            Overall = overall;
            PipsType = pipsType;
        }
    }
}
