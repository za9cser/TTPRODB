using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using TTPRODB.BuisnessLogic.Entities;
using TTPRODB.DatabaseCommunication;

namespace TTPRODB.TTPRODBExecution.Filters
{
    /// <summary>
    /// Interaction logic for ProducersFilter.xaml
    /// </summary>
    public partial class ProducersFilter : UserControl
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

        public SqlParameter[] MakeQuery()
        {
            // get ID of selected producers
            string[] selectedProducers = ProducersStackPanel.Children.OfType<CheckBox>().Where(x => (bool) x.IsChecked)
                .Select(x => x.Content.ToString()).ToArray();
            // if producers didn't choosen return empty string
            if (selectedProducers.Length == 0)
            {
                return null;
            }

            int id;
            SqlParameter[] parameters = new SqlParameter[selectedProducers.Length];
            for (int i = 0; i < selectedProducers.Length; i++)
            {
                id = producers[selectedProducers[i]].Id;
                parameters[i] = new SqlParameter($"@id{i}", id);
            }

            return parameters;
        }
    }
}
