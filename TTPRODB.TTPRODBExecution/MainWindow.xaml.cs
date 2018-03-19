using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.TextFormatting;
using TTPRODB.BuisnessLogic.Entities;
using TTPRODB.DatabaseCommunication;
using TTPRODB.TTPRODBExecution.Filters;
using static System.Reflection.BindingFlags;

namespace TTPRODB.TTPRODBExecution
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string[] invetoryTypeArray { get; set; } = { "Blade", "Rubber", "Pips" };

        private const BindingFlags OnlyClassBindingFlags = Public | Instance | DeclaredOnly;
        private Type[] inventoryTypes = {typeof(Blade), typeof(Rubber), typeof(Pips)};
        private string[][] characteristics;
        private DataGrid[] resultTables;
        private ViewMode mode = ViewMode.Search;
        private Label notFoundMessageLabel;

        public ProducersFilter ProducersFilterControl;

        public MainWindow()
        {
            InitializeComponent();

            if (!DbConnect.ValidateDatabase())
            {
                UpdateMode(Visibility.Collapsed);
                RunUpdate();
                return;
            }
            InitializeUI();
        }

        // initiate ui controls
        public void InitializeUI()
        {
            // load producers list
            ProducersFilterControl = new ProducersFilter();
            InventoryPanel.Children.Add(ProducersFilterControl);
            // init not found message 
            notFoundMessageLabel = new Label()
            {
                Content = "Nothing found by your query",
                FontSize = 18,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            // init characteristics
            InitCharacterisArrays();
            InitResultTables();
            // init comboboxes
            InventorySearchComboBox.ItemsSource = invetoryTypeArray;
            InventoryFilterComboBox.ItemsSource = invetoryTypeArray;
            InventoryFilterComboBox.SelectedIndex = 0;
            InventorySearchComboBox.SelectedIndex = 0;

            // check for update control
            UpdateDatabase updateDatabase = ContentGrid.Children.OfType<UpdateDatabase>().FirstOrDefault();
            if (updateDatabase != null)
            {
                ContentGrid.Children.Remove(updateDatabase);
            }

            // init database update date
            GetDbUpdateDate();
        }

        // initiates result table for inventory type
        private void InitResultTables()
        {
            // create datagrids
            resultTables = new DataGrid[characteristics.GetLength(0)];
            for (int i = 0; i < characteristics.GetLength(0); i++)
            {
                
                resultTables[i] = new DataGrid();
                // set styles
                //resultTables[i].ColumnHeaderStyle = (Style) Application.Current.Resources["DataGridHeaderStyle"];
                resultTables[i].Style = (Style) Application.Current.Resources["DataGridStyle"];
                resultTables[i].RowStyle = (Style) Application.Current.Resources["DataGridRowStyle"];
                resultTables[i].CellStyle = (Style) Application.Current.Resources["DataGridCellStyle"];
                resultTables[i].AutoGenerateColumns = false;

                // name column
                resultTables[i].Columns.Add(new DataGridTextColumn()
                    { Header = "Name", Binding = new Binding("Name") });
                
                // ratings column
                resultTables[i].Columns.Add(new DataGridTextColumn()
                    { Header = "Ratings", Binding = new Binding("Ratings") });
                
                // init characteristics columns
                foreach (string name in characteristics[i])
                {
                    resultTables[i].Columns.Add(new DataGridTextColumn()
                        { Header = name, Binding = new Binding(name), });
                    
                }

                // colums for inventory type
                switch (i)
                {
                    // Rubber
                    case 1:
                        resultTables[i].Columns.Add(new DataGridCheckBoxColumn()
                            { Header = "Tensor", Binding = new Binding("Tensor"), IsReadOnly = true});
                        
                        resultTables[i].Columns.Add(new DataGridCheckBoxColumn()
                            { Header = "Anti", Binding = new Binding("Anti"), IsReadOnly = true});
                       
                        break;
                    // Pipses
                    case 2:
                        resultTables[i].Columns.Add(new DataGridTextColumn()
                            { Header = "Pips type", Binding = new Binding("PipsType")});
                        
                        break;
                }
                Grid.SetRow(resultTables[i], 2);
                Grid.SetColumn(resultTables[i], 1);
            }
        }

        // hide or show content
        public void UpdateMode(Visibility contentVisibility)
        {
            FirstContentRow.Visibility = contentVisibility;
            BottomPanel.Visibility = contentVisibility;
            LeftSidePanel.Visibility = contentVisibility;
            GetDbUpdateDate();
        }

        // gets database date
        private void GetDbUpdateDate()
        {
            string outputFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string mdfFilename = "TTPRODB.mdf";
            string dbFileName = Path.Combine(outputFolder, mdfFilename);
            DateTime updateDateTime = File.GetLastWriteTime(dbFileName);
            UpdateDateTextBlock.Text = updateDateTime.ToShortDateString();
        }

        // initiates update control and performs update action
        private void RunUpdate()
        {
            UpdateDatabase updateDatabase = new UpdateDatabase();
            Grid.SetRow(updateDatabase, 0);
            Grid.SetColumnSpan(updateDatabase, 2);
            ContentGrid.Children.Add(updateDatabase);
            updateDatabase.RunUpdate();
        }

        // init string array of item characteristics
        private void InitCharacterisArrays()
        {
            characteristics = new[]
            {
                typeof(Blade).GetProperties(OnlyClassBindingFlags).Where(SelectDouble).Select(x => x.Name).ToArray(),
                typeof(Rubber).GetProperties(OnlyClassBindingFlags).Where(SelectDouble).Select(x => x.Name).ToArray(),
                typeof(Pips).GetProperties(OnlyClassBindingFlags).Where(SelectDouble).Select(x => x.Name).ToArray()
            };
        }

        // predicat to select only double
        bool SelectDouble(PropertyInfo x)
        {
            return x.PropertyType == typeof(double);
        }

        // InventoryFilterComboBoxOnSelectionChanged handler to build filters
        private void InventoryFilterComboBoxOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (mode)
            {
                case ViewMode.Search:
                    BuildFilters(((ComboBox) sender).SelectedIndex);
                    break;
            }
        }

        // bullds filters for specific inventory type
        private void BuildFilters(int selectedIndex)
        {
            CharacteristicPanel.Children.Clear();
            
            // add charcteristic filters
            foreach (string characteristic in characteristics[selectedIndex])
            {
                CharacteristicPanel.Children.Add(new CharacteristicFilter(characteristic));
            }

            // filters for CultureSpecificCharacterBufferRange inventory
            switch (selectedIndex)
            {
                // Rubber
                case 1:
                    CharacteristicPanel.Children.Add(new RubberTypeFilter());
                    break;
                // Pipses
                case 2:
                    CharacteristicPanel.Children.Add(new PipsTypeFilter());
                    break;
            }

            CharacteristicPanel.Children.Add(new RatingsFilter());
        }

        // SearchButtonOnClick handler
        private void SearchButtonOnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                ClearResultColumn();

                // get items
                List<dynamic> items = DbQuering.GetInventoryByName(SearchTextBox.Text, inventoryTypes[InventorySearchComboBox.SelectedIndex]);
                if (items == null)
                {
                    ShowNotFoundMessage();
                    return;
                }

                // build table
                DataGrid table = resultTables[InventorySearchComboBox.SelectedIndex];
                table.ItemsSource = null;
                table.ItemsSource = items;
                Grid.SetRow(table, 2);
                Grid.SetColumn(table, 1);
                ContentGrid.Children.Add(table);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        // shows "nothig found by query"
        private void ShowNotFoundMessage()
        {
            Grid.SetRow(notFoundMessageLabel, 2);
            Grid.SetColumn(notFoundMessageLabel, 1);
            ContentGrid.Children.Add(notFoundMessageLabel);
        }

        // clears results panel
        private void ClearResultColumn()
        {
            var item = ContentGrid.Children[ContentGrid.Children.Count - 1];
            if (item.GetType() == typeof(DataGrid) || item.GetType() == typeof(Label))
            {
                ContentGrid.Children.Remove(item);
            }
        }

        // FilterButtonOnClick handler
        private void FilterButtonOnClick(object sender, RoutedEventArgs e)
        {
            ClearResultColumn();
            List<dynamic> items = new List<dynamic>();
            using (var connection = new SqlConnection(DbConnect.DbConnectionString))
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    string InventoryTable = invetoryTypeArray[InventoryFilterComboBox.SelectedIndex];

                    StringBuilder queryStringBuilder = new StringBuilder(
                        $"SELECT * FROM Item inner JOIN {InventoryTable} AS inventory ON Item.ID = inventory.Item_ID WHERE ");

                    // selected producers
                    SqlParameter[] parameters = ProducersFilterControl.MakeQuery(out string query);
                    if (query != String.Empty)
                    {
                        queryStringBuilder.Append(query);
                        cmd.Parameters.AddRange(parameters);
                    }

                    // charactericstics
                    foreach (CharacteristicFilter characteristicFilter in CharacteristicPanel.Children
                        .OfType<CharacteristicFilter>())
                    {
                        parameters = characteristicFilter.MakeQuery(out query);
                        cmd.Parameters.AddRange(parameters);
                        queryStringBuilder.Append(query);
                        queryStringBuilder.Append(" AND ");
                    }

                    queryStringBuilder = queryStringBuilder.Remove(queryStringBuilder.Length - 6, 5);

                    // for rubber and pipses
                    if (InventoryFilterComboBox.SelectedIndex > 0)
                    {
                        IFilter filter = CharacteristicPanel.Children[CharacteristicPanel.Children.Count - 2] as IFilter;
                        parameters = filter.MakeQuery(out query);
                        if (query != String.Empty)
                        {
                            queryStringBuilder.Append(query);
                            cmd.Parameters.AddRange(parameters);
                        }
                    }

                    // ratings filter
                    IFilter ratingFilter = CharacteristicPanel.Children[CharacteristicPanel.Children.Count - 1] as IFilter;
                    parameters = ratingFilter.MakeQuery(out query);
                    queryStringBuilder.Append(query);
                    cmd.Parameters.AddRange(parameters);

                    cmd.CommandText = queryStringBuilder.ToString();

                    SqlDataReader sqlDataReader = cmd.ExecuteReader();

                    ConstructorInfo constructorInfo = inventoryTypes[InventoryFilterComboBox.SelectedIndex].GetConstructor(new[] { typeof(SqlDataReader) });

                    while (sqlDataReader.Read())
                    {
                        object tempItem = constructorInfo.Invoke(new[] { sqlDataReader });
                        dynamic item = Convert.ChangeType(tempItem, inventoryTypes[InventoryFilterComboBox.SelectedIndex]);
                        items.Add(item);
                    }
                }
            }

            if (items.Count == 0)
            {
                ShowNotFoundMessage();
                return;
            }

            // add datagrid to panel
            DataGrid table = resultTables[InventoryFilterComboBox.SelectedIndex];
            table.ItemsSource = null;
            table.ItemsSource = items;
            Grid.SetRow(table, 2);
            Grid.SetColumn(table, 1);
            ContentGrid.Children.Add(table);
        }

        // UpdateButtonOnClick
        private void UpdateButtonOnClick(object sender, RoutedEventArgs e)
        {
            UpdateMode(Visibility.Collapsed);
            RunUpdate();
        }
    }
}
