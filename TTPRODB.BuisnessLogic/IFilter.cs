using System.Data.SqlClient;

namespace TTPRODB.BuisnessLogic
{
    public interface IFilter
    {
        SqlParameter[] MakeQuery();
        string Title { get; set; }
    }
}