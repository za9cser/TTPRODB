using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Controls;
using TTPRODB.BuisnessLogic;
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

        public string Title { get; set; } = "Producers";

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

        /// <summary>
        /// Get selected producer Ids
        /// </summary>
        /// <returns>array of checked producer ids</returns>
        public int[] GetSelectedProdusers()
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
            int[] producerIds = new int[selectedProducers.Length];
            for (int i = 0; i < selectedProducers.Length; i++)
            {
                producerIds[i] = producers[selectedProducers[i]].Id;
            }

            return producerIds;
        }

        

        
    }
}
