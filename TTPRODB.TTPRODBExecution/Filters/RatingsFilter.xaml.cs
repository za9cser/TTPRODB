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
using Xceed.Wpf.Toolkit;

namespace TTPRODB.TTPRODBExecution.Filters
{
    /// <summary>
    /// Interaction logic for RatingsFilter.xaml
    /// </summary>
    public partial class RatingsFilter : UserControl, IFilter
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
            if (Char.IsNumber(e.Key.ToString()[0]) || e.Key == Key.Back || e.Key == Key.Delete)
            {
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
                return;
            }

            IntegerUpDown upDown = sender as IntegerUpDown;
            if (upDown.Value < 1)
            {
                upDown.Value = upDown.Minimum;
            }
        }

        public SqlParameter[] MakeQuery(out string query)
        {
            throw new NotImplementedException();
        }
    }
}
