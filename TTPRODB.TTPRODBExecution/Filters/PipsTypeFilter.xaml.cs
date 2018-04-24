using System;
using System.Collections.Generic;
using System.Data;
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
using TTPRODB.BuisnessLogic;

namespace TTPRODB.TTPRODBExecution.Filters
{
    /// <summary>
    /// Interaction logic for PipsTypeFilter.xaml
    /// </summary>
    public partial class PipsTypeFilter : UserControl
    {
        public PipsTypeFilter()
        {
            InitializeComponent();
        }

        public SqlParameter[] MakeQuery()
        {
            // selected types
            string[] selectedTypes = PipsTypePanel.Children.OfType<CheckBox>().
                Where(x => x.IsChecked == true).Select(x => x.Content.ToString()).ToArray();
            if (selectedTypes.Length == 0)
            {
                return null;
            }

            // build parametrs
            SqlParameter[] parameters = new SqlParameter[selectedTypes.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                parameters[i] = new SqlParameter($"@pipsType{i}", selectedTypes[i]);
            }

            return parameters;
        }
    }
}
