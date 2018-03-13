using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using System.Reflection;
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
using TTPRODB.TTPRODBExecution.Filters;
using static System.Reflection.BindingFlags;
using static TTPRODB.DatabaseCommunication.DbQuering;

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
        public MainWindow()
        {
            InitializeComponent();
            
            if (!DbConnect.ValidateDatabase())
            {
                UpdateMode(Visibility.Collapsed);
            }
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
            //CharacteristicPanel.MaxHeight = 150; 
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
            
            // get items
            List<dynamic> items = DbQuering.GetInventoryByName(SearchTextBox.Text, inventoryTypes[InventorySearchComboBox.SelectedIndex]);
            // build table
            DataGrid table = CreateTable(InventorySearchComboBox.SelectedIndex);
            table.ItemsSource = items;
            Grid.SetRow(table, 2);
            Grid.SetColumn(table, 1);
            ContentGrid.Children.Add(table);
        }

        private DataGrid CreateTable(int selectedIndex)
        {
            DataGrid table = new DataGrid {AutoGenerateColumns = false};
            table.Columns.Add(new DataGridTextColumn() {Header = "Name", Binding = new Binding("Name")});
            foreach (string name in characteristics[selectedIndex])
            {
                table.Columns.Add(new DataGridTextColumn() {Header = name, Binding = new Binding(name)});
            }

            table.Columns.Add(new DataGridTextColumn() {Header = "Ratings", Binding = new Binding("Ratings")});
            
            return table;

        }
    }

    //public class ViewModel
    //{
    //    public string[] InvetoryTypeArray { get; set; } = { "Blade", "Rubber", "Pips" };
    //}
}
