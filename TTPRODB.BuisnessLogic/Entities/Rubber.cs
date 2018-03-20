using System;
using System.Data.Linq.Mapping;
using System.Data.SqlClient;

namespace TTPRODB.BuisnessLogic.Entities
{
    
    public class Rubber : Item
    {
        private double speed;
        private double spin;
        private double control;
        private double tackiness;
        private double weight;
        private double hardness;
        private double gears;
        private double throwAngle;
        private double consistency;
        private double durability;
        private double overall;

        public int Id { get; set; }

        public double Speed
        {
            get => speed;
            set => speed = Math.Round(value, 1, MidpointRounding.AwayFromZero);
        }

        public double Spin
        {
            get => spin;
            set => spin = Math.Round(value, 1, MidpointRounding.AwayFromZero);
        }

        public double Control
        {
            get => control;
            set => control = Math.Round(value, 1, MidpointRounding.AwayFromZero);
        }

        public double Tackiness
        {
            get => tackiness;
            set => tackiness = Math.Round(value, 1, MidpointRounding.AwayFromZero);
        }

        public double Weight
        {
            get => weight;
            set => weight = Math.Round(value, 1, MidpointRounding.AwayFromZero);
        }

        public double Hardness
        {
            get => hardness;
            set => hardness = Math.Round(value, 1, MidpointRounding.AwayFromZero);
        }

        public double Gears
        {
            get => gears;
            set => gears = Math.Round(value, 1, MidpointRounding.AwayFromZero);
        }

        public double ThrowAngle
        {
            get => throwAngle;
            set => throwAngle = Math.Round(value, 1, MidpointRounding.AwayFromZero);
        }

        public double Consistency
        {
            get => consistency;
            set => consistency = Math.Round(value, 1, MidpointRounding.AwayFromZero);
        }

        public double Durability
        {
            get => durability;
            set => durability = Math.Round(value, 1, MidpointRounding.AwayFromZero);
        }

        public double Overall
        {
            get => overall;
            set => overall = Math.Round(value, 1, MidpointRounding.AwayFromZero);
        }

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
        
        public Rubber(SqlDataReader sdr) : base(sdr)
        {
            Id = sdr.GetInt32(5);
            Speed = sdr.GetDouble(7);
            Spin = sdr.GetDouble(8);
            Control = sdr.GetDouble(9);
            Tackiness = sdr.GetDouble(10);
            Weight = sdr.GetDouble(11);
            Hardness = sdr.GetDouble(12);
            Gears = sdr.GetDouble(13);
            ThrowAngle = sdr.GetDouble(14);
            Consistency = sdr.GetDouble(15);
            Durability = sdr.GetDouble(16);
            Overall = sdr.GetDouble(17);
            Tensor = sdr.GetBoolean(18);
            Anti = sdr.GetBoolean(19);
        }
    }
}
