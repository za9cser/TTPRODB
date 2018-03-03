using System.Data.Linq.Mapping;

namespace TTPRODB.BuisnessLogic.Entities
{
    [Table(Name = "Producers")]
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
