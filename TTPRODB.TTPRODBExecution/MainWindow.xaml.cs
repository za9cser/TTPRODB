using System;
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
        private Type[] inventoryTypes = {typeof(Blade), typeof(Rubber), typeof(Pips)};
        private string[][] characteristics;
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
            // init comboboxes
            InventorySearchComboBox.ItemsSource = invetoryTypeArray;
            InventoryFilterComboBox.ItemsSource = invetoryTypeArray;
            InventoryFilterComboBox.SelectedIndex = 0;
            InventorySearchComboBox.SelectedIndex = 0;
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

        // init characteristics of items
        private void InitCharacterisArrays()
        {
            bool SelectDouble(PropertyInfo x) => x.PropertyType == typeof(double);

            characteristics = new[]
            {
                typeof(Blade).GetProperties(Public | Instance | DeclaredOnly).Where(SelectDouble).Select(x => x.Name).ToArray(),
                typeof(Rubber).GetProperties(Public | Instance | DeclaredOnly).Where(SelectDouble).Select(x => x.Name).ToArray(),
                typeof(Pips).GetProperties(Public | Instance | DeclaredOnly).Where(SelectDouble).Select(x => x.Name).ToArray()
            };
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
                //CharacteristicPanel.MaxHeight += 40;
            }

            switch (selectedIndex)
            {
                // Rubber
                case 1:
                    CharacteristicPanel.Children.Add(new RubberTypeFilter());
                    //CharacteristicPanel.MaxHeight += 50; 
                    break;
                // Pipses
                case 2:
                    CharacteristicPanel.Children.Add(new PipsTypeFilter());
                    //CharacteristicPanel.MaxHeight += 64;
                    break;
            }

            //CharacteristicPanel.MaxHeight += 20;
        }

        private void SearchButtonOnClick(object sender, RoutedEventArgs e)
        {
            List<dynamic> items = DbQuering.GetInventoryByName(SearchTextBox.Text, inventoryTypes[InventorySearchComboBox.SelectedIndex]);
        }
    }

    //public class ViewModel
    //{
    //    public string[] InvetoryTypeArray { get; set; } = { "Blade", "Rubber", "Pips" };
    //}
}
