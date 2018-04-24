using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TTPRODB.BuisnessLogic;

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

        public SqlParameter[] MakeQuery()
        {
            RadioButton selectedType =
                RubberTypePanel.Children.OfType<RadioButton>().FirstOrDefault(x => x.IsChecked == true);
            if (selectedType == null)
            {
                return null;
            }
           
            SqlParameter[] parameters =
            {
                new SqlParameter("@state", true)
            };

            
            return parameters;
        }

        public string Title { get; set; } = "Rubber type";
    }
}
