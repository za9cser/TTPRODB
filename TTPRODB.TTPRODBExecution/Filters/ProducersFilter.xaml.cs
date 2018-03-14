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
            // get ID of selected producers
            string[] selectedProducers = ProducersStackPanel.Children.OfType<CheckBox>().Where(x => (bool) x.IsChecked)
                .Select(x => x.Content.ToString()).ToArray();
            // if producers didn't choosen return empty string
            if (selectedProducers.Length == 0)
            {
                query = String.Empty;
                return null;
            }
            int[] producersIds = new int[selectedProducers.Length]; 
            for (int i = 0; i < selectedProducers.Length; i++)
            {
                producersIds[i] = producers[selectedProducers[i]].Id;
            }

            // build query
            SqlParameter[] parameters = new SqlParameter[selectedProducers.Length];
            StringBuilder queryStringBuilder = new StringBuilder("Item.Producer_ID IN (");
            for (int i = 0; i < selectedProducers.Length; i++)
            {
                queryStringBuilder.Append($"@id{i},");
                parameters[i] = new SqlParameter($"@id{i}", producersIds[i]);
            }

            queryStringBuilder = queryStringBuilder.Remove(queryStringBuilder.Length - 1, 1);
            queryStringBuilder.Append(") AND ");
            query = queryStringBuilder.ToString();
            return parameters;
        }
    }
}
