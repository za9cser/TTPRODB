using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Controls;
using TTPRODB.BuisnessLogic.Entities;
using TTPRODB.DatabaseCommunication;

namespace TTPRODB.TTPRODBExecution.Filters
{
    /// <summary>
    /// Interaction logic for ProducersFilter.xaml
    /// </summary>
    public partial class ProducersFilter : UserControl, IFilter
    {
        private Dictionary<string, Producer> producers;
        public ProducersFilter()
        {
            InitializeComponent();
            FillProducerList();
        }

        public void FillProducerList()
        {
            producers = DbQuering.GetAllProducers();
            foreach (KeyValuePair<string, Producer> producer in producers)
            {
                CheckBox producerCb = new CheckBox() { Content = producer.Value.Name };
                ProducersStackPanel.Children.Add(producerCb);
            }
        }

        public SqlParameter[] MakeQuery(out string query)
        {
            throw new System.NotImplementedException();
        }
    }
}
