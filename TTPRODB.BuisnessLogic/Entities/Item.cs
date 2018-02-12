namespace TTPRODB.BuisnessLogic.Entities
{
    public abstract class Item
    {
        public int ItemId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int ProducerId { get; set; }
        public int Ratings { get; set; }

        public Item(int itemId, string name, string url, int producerId, int ratings)
        {
            ItemId = itemId;
            Name = name;
            Url = url;
            ProducerId = producerId;
            Ratings = ratings;
        }

        public Item() { }
    }
}