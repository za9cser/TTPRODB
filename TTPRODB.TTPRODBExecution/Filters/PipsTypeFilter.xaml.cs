using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TTPRODB.TTPRODBExecution.Filters
{
    /// <summary>
    /// Interaction logic for PipsTypeFilter.xaml
    /// </summary>
    public partial class PipsTypeFilter : UserControl, IFilter
    {
        public PipsTypeFilter()
        {
            InitializeComponent();
        }

        public SqlParameter[] MakeQuery(out string query)
        {
            string[] selectedTypes = PipsTypePanel.Children.OfType<CheckBox>().
                Where(x => x.IsChecked == true).Select(x => x.Content.ToString()).ToArray();
            if (selectedTypes.Length == 0)
            {
                query = string.Empty;
                return null;
            }

            SqlParameter[] parameters = new SqlParameter[selectedTypes.Length];
            StringBuilder queryStringBuilder = new StringBuilder(" AND (");
            query = " AND (";
            for (int i = 0; i < parameters.Length; i++)
            {
                queryStringBuilder.Append($"PipsType = @pipsType{i} OR ");
                parameters[i] = new SqlParameter($"@pipsType{i}", selectedTypes[i]);
            }

            queryStringBuilder = queryStringBuilder.Remove(queryStringBuilder.Length - 4, 4);
            queryStringBuilder.Append(")");
            query = queryStringBuilder.ToString();
            return parameters;
        }
    }
}
