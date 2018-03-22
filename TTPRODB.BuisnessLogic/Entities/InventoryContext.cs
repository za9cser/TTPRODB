using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTPRODB.BuisnessLogic.Entities
{
    public class InventoryContext : DataContext
    {
        public InventoryContext(string fileOrServerOrConnection) : base(fileOrServerOrConnection)
        {

        }
    }
}
