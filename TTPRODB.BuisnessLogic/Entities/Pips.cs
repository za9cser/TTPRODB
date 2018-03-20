using System;
using System.Data.Linq.Mapping;
using System.Data.SqlClient;

namespace TTPRODB.BuisnessLogic.Entities
{
    
    public class Pips : Item
    {
        private double speed;
        private double spin;
        private double control;
        private double deception;
        private double reversal;
        private double weight;
        private double hardness;
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

        public double Deception
        {
            get => deception;
            set => deception = Math.Round(value, 1, MidpointRounding.AwayFromZero);
        }

        public double Reversal
        {
            get => reversal;
            set => reversal = Math.Round(value, 1, MidpointRounding.AwayFromZero);
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

        public string PipsType { get; set; }

        public Pips() { }

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

        public Pips(SqlDataReader sdr) : base(sdr)
        {
            Id = sdr.GetInt32(5);
            Speed = sdr.GetDouble(7);
            Spin = sdr.GetDouble(8);
            Control = sdr.GetDouble(9);
            Deception = sdr.GetDouble(10);
            Reversal = sdr.GetDouble(11);
            Weight = sdr.GetDouble(12);
            Hardness = sdr.GetDouble(13);
            Consistency = sdr.GetDouble(14);
            Durability = sdr.GetDouble(15);
            Overall = sdr.GetDouble(16);
            PipsType = sdr.GetString(17);
        }
    }
}
