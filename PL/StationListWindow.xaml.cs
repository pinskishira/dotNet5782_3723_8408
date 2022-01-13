using BO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for StationListWindow.xaml
    /// </summary>
    public partial class StationListWindow : Window
    {
        BlApi.Ibl bl;
        public Dictionary<int, List<StationToList>> stationToLists;
        public StationToList CurrentStation { get; set; } = new();
        private bool _close { get; set; } = false;

        /// <summary>
        /// Initializes the list of all the stations
        /// </summary>
        /// <param name="ibl">From type bl</param>
        public StationListWindow(BlApi.Ibl ibl)
        {
            InitializeComponent();
            bl = ibl;
            stationToLists = (from item in bl.GetAllStations()
                              group item by item.AvailableChargeSlots).ToDictionary(x => x.Key, x => x.ToList());
            RefreshStations();
        }

        /// <summary>
        /// Refreshes page
        /// </summary>
        public void RefreshStations()
        {
            StationListView.ItemsSource = from item in stationToLists.Values.SelectMany(x => x)
                                          orderby item.AvailableChargeSlots
                                          select item;
        }

        /// <summary>
        /// Can't force the window to close
        /// </summary>
        private void Window_closing(object sender, CancelEventArgs e)
        {
            if (!_close)
            {
                e.Cancel = true;
                MessageBox.Show("You can't force the window to close", "ERROR");
            }
        }

        /// <summary>
        /// Closes current window
        /// </summary>
        private void CloseWindowButton_Click(object sender, RoutedEventArgs e)
        {
            _close = true;
            Close();
        }

        /// <summary>
        /// Goes to constructor to add a station
        /// </summary>
        private void AddStationButton_Click(object sender, RoutedEventArgs e)
        {
            new StationWindow(bl, this, 5).Show();
        }

        /// <summary>
        /// Refreshes list
        private void refersh_Click(object sender, RoutedEventArgs e)
        {
            stationToLists = (from item in bl.GetAllStations()
                              group item by item.AvailableChargeSlots).ToDictionary(x => x.Key, x => x.ToList());
            RefreshStations();
        }

        /// <summary>
        /// Goes to constructor to update a station
        /// </summary>
        private void MouseDoubleClick_SelectionChanged(object sender, MouseButtonEventArgs e)
        {
            CurrentStation = (StationToList)StationListView.SelectedItem;
            if (CurrentStation != null)
                new StationWindow(bl, this).Show();
        }

        /// <summary>
        /// Allows user to delete station using an icon 
        /// </summary>
        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var result1 = MessageBox.Show($"Are you sure you would like to delete this station? \n", "Request Review",
           MessageBoxButton.OKCancel, MessageBoxImage.Question);
            switch (result1)
            {
                case MessageBoxResult.OK:
                    FrameworkElement framework = sender as FrameworkElement;
                    CurrentStation = framework.DataContext as StationToList;
                    bl.DeleteStation(CurrentStation.Id);
                    int AvailableChargeSlotsSort = CurrentStation.AvailableChargeSlots;
                    stationToLists[AvailableChargeSlotsSort].RemoveAll(i => i.Id == CurrentStation.Id);
                    RefreshStations();
                    break;
                case MessageBoxResult.Cancel:
                    break;
            }
        }
    }
}
