using System;
using System.Collections.Generic;
using System.Data.Common;
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
using TTPRODB.BuisnessLogic;
using TTPRODB.BuisnessLogic.Entities;
using TTPRODB.DatabaseCommunication;
using static TTPRODB.DatabaseCommunication.DbQuering;

namespace TTPRODB.TTPRODBExecution
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            if (DbConnect.ValidateDatabase())
            {
                GetDataFromSite();
            }
        }

        private void GetDataFromSite()
        {
            ParseTTDb parseTTDb = new ParseTTDb(GetAllProducers(), GetItemCount());
            
            DataToSave[] dataToSave = new DataToSave[parseTTDb.Pages.Length];
            
            for (int i = 0 ; i < parseTTDb.Pages.Length; i++)
            {
                dataToSave[i] =  parseTTDb.ParseItems(parseTTDb.Pages[i], parseTTDb.Types[i], null, null);
            }

            InsertProducers(parseTTDb.ProducersToInsert);

            foreach (DataToSave data in dataToSave)
            {
                InsertItems(data.ItemsToInsert);
                UpdateItems(data.ItemsToUpdate);
            }


        }
    }
}
