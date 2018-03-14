using System;
using System.Collections.Generic;
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
    /// Interaction logic for CharacteristicFilter.xaml
    /// </summary>
    public partial class CharacteristicFilter : UserControl
    {
        public double Value1
        {
            get
            {
                bool result = double.TryParse(Value1TextBox.Text, out double value);
                if (!result || value < 0.0)
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
            if (Char.IsDigit(Convert.ToChar(e.Key.ToString())) || e.Key == Key.OemComma || e.Key == Key.Back || e.Key == Key.Delete)
            {
                e.Handled = true;
            }
        }
    }
}
