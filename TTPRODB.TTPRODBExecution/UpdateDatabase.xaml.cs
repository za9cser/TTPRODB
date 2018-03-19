using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using TTPRODB.TTPRODBExecution.Filters;
using static TTPRODB.DatabaseCommunication.DbQuering;

namespace TTPRODB.TTPRODBExecution
{
    /// <summary>
    /// Interaction logic for UpdateDatabase.xaml
    /// </summary>
    public partial class UpdateDatabase : UserControl
    {
        private BackgroundWorker bw;
        private int itemCount;

        public UpdateDatabase()
        {
            InitializeComponent();
            InitBGworker();
        }

        // Init background thread
        public void InitBGworker()
        {
            bw = new BackgroundWorker();
            bw.WorkerSupportsCancellation = true;       // разрешение отмены
            bw.WorkerReportsProgress = true;            // разрешение прогресса
            bw.DoWork += bw_DoWork;                     // метод фонового потока
            bw.ProgressChanged += bw_ProgressChanged;   // изменение UI
            bw.RunWorkerCompleted += bw_RunWorkerCompleted; // поток завершен
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            info.Content = "Готово";
            MainWindow hwnd = App.Current.MainWindow as MainWindow;
            hwnd.UpdateMode(Visibility.Visible);
            if (hwnd.InventoryPanel.Children.OfType<ProducersFilter>().FirstOrDefault() == null)
            {
                hwnd.InitializeUI();
            }
        }

        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // начинаем новый процесс
            if (e.ProgressPercentage == 0)
            {
                url.Content = "";
                optype.Content = e.UserState;
                prgbar.Value = 0;
                info.Content = "0 / 0";
                return;
            }

            prgbar.Value = ((double)e.ProgressPercentage / itemCount) * 100;
            info.Content = e.ProgressPercentage.ToString() + " / " + itemCount.ToString();
            url.Content = e.UserState.ToString();
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            ParseTTDb parseTTDb = new ParseTTDb(GetAllProducers(), GetItemCount());

            DataToSave[] dataToSave = new DataToSave[parseTTDb.Pages.Length];

            Dictionary<string, dynamic>[] invenoryList =
                {
                    GetAllInventory(parseTTDb.Types[0]), GetAllInventory(parseTTDb.Types[1]),
                    GetAllInventory(parseTTDb.Types[2])
                };
            
            for (int i = 0; i < parseTTDb.Pages.Length; i++)
            {
                bw.ReportProgress(0, parseTTDb.Pages[i] + " parsing");
                dataToSave[i] = parseTTDb.ParseItems(parseTTDb.Pages[i], parseTTDb.Types[i], invenoryList[i], bw, out itemCount);
            }

            bw.ReportProgress(0, "Insert producers");
            InsertProducers(parseTTDb.ProducersToInsert);

            for (int i = 0; i < dataToSave.Length; i++)
            {
                bw.ReportProgress(0, $"Insert {parseTTDb.Pages[i]}");
                InsertItems(dataToSave[i].ItemsToInsert);
                bw.ReportProgress(0, $"Update {parseTTDb.Pages[i]}");
                UpdateItems(dataToSave[i].ItemsToUpdate);
            }
        }

        public void RunUpdate()
        {
            bw.RunWorkerAsync();
        }
    }
}
