using System.Data.SqlClient;
using System.Windows.Controls;
using System.Windows.Input;
<<<<<<< refactor_filter_button_click
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
=======
>>>>>>> local
using TTPRODB.BuisnessLogic;

namespace TTPRODB.TTPRODBExecution.Filters
{
    /// <summary>
    /// Interaction logic for CharacteristicFilter.xaml
    /// </summary>
    public partial class CharacteristicFilter : UserControl, IFilter
    {
        public double Value1
        {
            get
            {
                bool result = double.TryParse(Value1TextBox.Text, out double value);
                if (!result || value < 0.0 || value > 10.0)
                {
                    return 0.0;
                }

                return value;
            }
        }

        public double Value2
        {
            get {
                bool result = double.TryParse(Value2TextBox.Text, out double value);
                if (!result || value > 10.0)
                {
                    return 0.0;
                }

                return value;
            }
        }

        public string Title
        {
            get => TitleTextBlock.Text;
            set => TitleTextBlock.Text = value;
        }

        public CharacteristicFilter(string title)
        {
            InitializeComponent();
            TitleTextBlock.Text = title;
            Value1TextBox.Text = "0,0";
            Value2TextBox.Text = "10,0";
        }

        public CharacteristicFilter()
        {
            InitializeComponent();
            Value1TextBox.Text = "0,0";
            Value2TextBox.Text = "10,0";
        }

        private void ValueTextBox_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) ||
                e.Key == Key.Back || e.Key == Key.Delete ||e.Key == Key.OemComma ||
                e.Key == Key.Right || e.Key == Key.Left || e.Key == Key.Tab)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        public SqlParameter[] MakeQuery()
        {
<<<<<<< refactor_filter_button_click
=======
            string characteristic = TitleTextBlock.Text;
>>>>>>> local
            SqlParameter[] parameters = {
                new SqlParameter($"@{Title}lowerLimit", Value1),
                new SqlParameter($"@{Title}upperLimit", Value2),
            };
            return parameters;
        }
    }
}
