using System.Linq;
using System.Windows.Controls;
using TTPRODB.BuisnessLogic;

namespace TTPRODB.TTPRODBExecution.Filters
{
    /// <summary>
    /// Interaction logic for PipsTypeFilter.xaml
    /// </summary>
    public partial class PipsTypeFilter : UserControl, IRubberType
    {
        public PipsTypeFilter()
        {
            InitializeComponent();
        }

        public string[] GetRubberType()
        {
            // selected types
            string[] selectedTypes = PipsTypePanel.Children.OfType<CheckBox>().
                Where(x => x.IsChecked == true).Select(x => x.Content.ToString()).ToArray();
            if (selectedTypes.Length == 0)
            {
                return null;
            }

            return selectedTypes;
        }
    }
}
