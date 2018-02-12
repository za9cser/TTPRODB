namespace TTPRODB.BuisnessLogic.Entities
{
    public class Producer
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Producer() { }

        public Producer(int id, string name)
        {
            Id = id;
            Name = name;        
        }
    }
}
