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
    /// Interaction logic for RubberTypeFilter.xaml
    /// </summary>
    public partial class RubberTypeFilter : UserControl, IFilter
    {
        public RubberTypeFilter()
        {
            InitializeComponent();
        }

        private void TensorRadioButtonOnChecked(object sender, RoutedEventArgs e)
        {
            if ((bool) AntiRadioButton.IsChecked)
            {
                AntiRadioButton.IsChecked = false;
            }

            TensorRadioButton.IsChecked = true;

        }

        private void AntiRadioButtonOnChecked(object sender, RoutedEventArgs e)
        {
            if ((bool)TensorRadioButton.IsChecked)
            {
                TensorRadioButton.IsChecked = false;
            }

            AntiRadioButton.IsChecked = true;
        }

        public SqlParameter[] MakeQuery(out string query)
        {
            RadioButton selectedType =
                RubberTypePanel.Children.OfType<RadioButton>().FirstOrDefault(x => x.IsChecked == true);
            if (selectedType == null)
            {
                query = String.Empty;
                return null;
            }

            query = $" AND inventory.{selectedType.Content} = @state";

            SqlParameter[] parameters =
            {
                new SqlParameter("@state", true)
            };

            
            return parameters;
        }
    }
}
