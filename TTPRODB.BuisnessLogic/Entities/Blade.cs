using System;
using System.Data.Linq.Mapping;
using System.Data.SqlClient;

namespace TTPRODB.BuisnessLogic.Entities
{
    
    public class Blade : Item
    {
        private double speed;
        private double control;
        private double stiffness;
        private double hardness;
        private double consistency;
        private double overall;
        public int Id { get; set; }

        public double Speed
        {
            get => speed;
            set => speed = Math.Round(value, 1, MidpointRounding.AwayFromZero);
        }

        public double Control
        {
            get => control;
            set => control = Math.Round(value, 1, MidpointRounding.AwayFromZero);
        }

        public double Stiffness
        {
            get => stiffness;
            set => stiffness = Math.Round(value, 1, MidpointRounding.AwayFromZero);
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

        public double Overall
        {
            get => overall;
            set => overall = Math.Round(value, 1, MidpointRounding.AwayFromZero);
        }

        public Blade() { }

        public Blade(SqlDataReader sdr) : base(sdr)
        {
            Id = sdr.GetInt32(5);
            Speed =  sdr.GetDouble(7);
            Control = sdr.GetDouble(8);
            Stiffness = sdr.GetDouble(9);
            Hardness = sdr.GetDouble(10);
            Consistency = sdr.GetDouble(11);
            Overall = sdr.GetDouble(12);
        }
    }
}
