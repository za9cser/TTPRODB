using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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
        public MainWindow()
        {
            InitializeComponent();
            
            if (!DbConnect.ValidateDatabase())
            {
                UpdateMode(Visibility.Collapsed);
            }
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
        }

        private void InitResultTables()
        {
            resultTables = new DataGrid[characteristics.GetLength(0)];
            for (int i = 0; i < characteristics.GetLength(0); i++)
            {
                
                resultTables[i] = new DataGrid();
                //resultTables[i].ColumnHeaderStyle = (Style) Application.Current.Resources["DataGridHeaderStyle"];
                resultTables[i].Style = (Style) Application.Current.Resources["DataGridStyle"];
                resultTables[i].RowStyle = (Style) Application.Current.Resources["DataGridRowStyle"];
                resultTables[i].CellStyle = (Style) Application.Current.Resources["DataGridCellStyle"];
                resultTables[i].AutoGenerateColumns = false;
                resultTables[i].Columns.Add(new DataGridTextColumn()
                    { Header = "Name", Binding = new Binding("Name") });
                
                resultTables[i].Columns.Add(new DataGridTextColumn()
                    { Header = "Ratings", Binding = new Binding("Ratings") });
                
                foreach (string name in characteristics[i])
                {
                    resultTables[i].Columns.Add(new DataGridTextColumn()
                        { Header = name, Binding = new Binding(name), });
                    
                }

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

        public void UpdateMode(Visibility contentVisibility)
        {
            SearchPanel.Visibility = contentVisibility;
            BottomPanel.Visibility = contentVisibility;
            LeftSidePanel.Visibility = contentVisibility;
            UpdateDatabase updateDatabase = new UpdateDatabase();
            Grid.SetRow(updateDatabase, 0);
            Grid.SetColumnSpan(updateDatabase, 2);
            ContentGrid.Children.Add(updateDatabase);
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

        bool SelectDouble(PropertyInfo x)
        {
            return x.PropertyType == typeof(double);
        }

        private void InventoryFilterComboBoxOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (mode)
            {
                case ViewMode.Search:
                    BuildFilters(((ComboBox) sender).SelectedIndex);
                    break;
            }
        }

        private void BuildFilters(int selectedIndex)
        {
            CharacteristicPanel.Children.Clear();
            
            foreach (string characteristic in characteristics[selectedIndex])
            {
                CharacteristicPanel.Children.Add(new CharacteristicFilter(characteristic));
            }

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

            
        }

        private void SearchButtonOnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var item = ContentGrid.Children[ContentGrid.Children.Count - 1];
                if (item.GetType() == typeof(DataGrid) || item.GetType() == typeof(Label))
                {
                    ContentGrid.Children.Remove(item);
                }

                // get items
                List<dynamic> items = DbQuering.GetInventoryByName(SearchTextBox.Text, inventoryTypes[InventorySearchComboBox.SelectedIndex]);
                if (items == null)
                {
                    Grid.SetRow(notFoundMessageLabel, 2);
                    Grid.SetColumn(notFoundMessageLabel, 1);
                    ContentGrid.Children.Add(notFoundMessageLabel);
                    return;
                }

                // build table
                DataGrid table = resultTables[InventorySearchComboBox.SelectedIndex];
                table.Items.Clear();
                table.ItemsSource = items;
                Grid.SetRow(table, 2);
                Grid.SetColumn(table, 1);
                ContentGrid.Children.Add(table);
                //table.Width = table.Columns.Sum(x => x.ActualWidth);
                //table.MaxWidth = 500;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }
    }
}
