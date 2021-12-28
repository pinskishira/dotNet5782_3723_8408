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
        public StationListWindow(BlApi.Ibl ibl)
        {
            InitializeComponent();
            bl = ibl;
            stationToLists = new Dictionary<int, List<StationToList>>();
            IEnumerable<StationToList> temp = bl.GetAllStations();
            stationToLists = (from item in bl.GetAllStations()
                              group item by item.AvailableChargeSlots).ToDictionary(x => x.Key, x => x.ToList());
            RefreshStations();
        }

        public void RefreshStations()
        {
            StationListView.ItemsSource = from item in stationToLists.Values.SelectMany(x => x)
                                          orderby item.AvailableChargeSlots
                                          select item;
        }

        private void window_closeing(object sender, CancelEventArgs e)
        {
            if (!_close)
            {
                e.Cancel = true;
                MessageBox.Show("You can't force the window to close", "ERROR");
            }
        }

        private void CloseWindowButton_Click(object sender, RoutedEventArgs e)
        {
            _close = true;
            this.Close();
        }


        private void AddStationButton_Click(object sender, RoutedEventArgs e)
        {
            new StationWindow(bl, this, 5).Show();
        }

        private void StationListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrentStation = (StationToList)StationListView.SelectedItem;
            if (CurrentStation != null)
                new StationWindow(bl, this).Show();
        }

        private void refersh_Click(object sender, RoutedEventArgs e)
        {
            IEnumerable<StationToList> temp = bl.GetAllStations();
            stationToLists = (from item in bl.GetAllStations()
                              group item by item.AvailableChargeSlots).ToDictionary(x => x.Key, x => x.ToList());
            RefreshStations();
        }
    }
}
