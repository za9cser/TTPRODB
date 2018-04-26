using System.Windows.Controls;
using System.Windows.Input;
using Xceed.Wpf.Toolkit;

namespace TTPRODB.TTPRODBExecution.Filters
{
    /// <summary>
    /// Interaction logic for RatingsFilter.xaml
    /// </summary>
    public partial class RatingsFilter : UserControl
    {
        public int RatingCount
        {
            get
            {
                if (RatingsUpDown.Value == null)
                {
                    return (int)RatingsUpDown.Minimum;
                }
                return (int)RatingsUpDown.Value;
            }
        }

        public RatingsFilter()
        {
            InitializeComponent();
        }


        private void RatingsUpDownOnKeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) ||
                e.Key == Key.Back || e.Key == Key.Delete ||
                e.Key == Key.Right || e.Key == Key.Left)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }

            IntegerUpDown upDown = sender as IntegerUpDown;
            if (upDown.Value < 1)
            {
                upDown.Value = upDown.Minimum;
            }
        }
    }
}
