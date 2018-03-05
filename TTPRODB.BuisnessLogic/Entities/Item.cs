using System.Data.Linq.Mapping;
using System.Data.SqlClient;

namespace TTPRODB.BuisnessLogic.Entities
{
    [Table(Name = "Items")]
    public abstract class Item
    {
        public int ItemId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int ProducerId { get; set; }
        public int Ratings { get; set; }

        protected Item(int itemId, string name, string url, int producerId, int ratings)
        {
            ItemId = itemId;
            Name = name;
            Url = url;
            ProducerId = producerId;
            Ratings = ratings;
        }

        protected Item() { }

        public Item(SqlDataReader sdr)
        {
            ItemId = sdr.GetInt32(0);
            Name = sdr.GetString(1);
            Url = sdr.GetString(2);
            ProducerId = sdr.GetInt32(3);
            Ratings = sdr.GetInt32(4);

        }
    }
}