using System.Data.SqlClient;

namespace TTPRODB.TTPRODBExecution.Filters
{
    public interface IFilter
    {
        SqlParameter[] MakeQuery(out string query);
    }
}