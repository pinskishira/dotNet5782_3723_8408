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
    public enum ChargeSlots { Available, Occupied, All };

    /// <summary>
    /// Interaction logic for StationListWindow.xaml
    /// </summary>
    public partial class StationListWindow : Window
    {
        BlApi.Ibl bl;
        public ObservableCollection<StationToList> stationToLists;
        public StationToList CurrentStation { get; set; } = new();
        private bool _close { get; set; } = false;
        public StationListWindow(BlApi.Ibl ibl)
        {
            InitializeComponent();
            bl = ibl;
            stationToLists = new ObservableCollection<StationToList>();

            List<StationToList> tempStationToList = bl.GetAllStations().ToList();//getting list of drones from bl

            foreach (var indexOfStationToList in tempStationToList)//going through list and inserting it into drone to list of type ObservableCollection
            {
                stationToLists.Add(indexOfStationToList);
            }

            StationListView.ItemsSource = stationToLists;
            AvailableChargeSlots.ItemsSource = System.Enum.GetValues(typeof(ChargeSlots));//enum values of charge slots
            //WeightSelection.ItemsSource = System.Enum.GetValues(typeof(WeightCategories));//enum values of weight
            //StatusSelection.SelectedIndex = 3;//prints full list
            //stationToLists.CollectionChanged += StationToLists_CollectionChanged;//updating event 

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

        private void StationListView_SelectionChanged(object sender, MouseButtonEventArgs e)
        {
            //CurrentDrone = (DroneToList)DronesListView.SelectedItem;
            //if (CurrentDrone != null)
            //    new DroneWindow(bl, this).Show();
        }
        private void AddStationButton_Click(object sender, RoutedEventArgs e)
        {
            //new StationWindow(bl, this, 5).Show();
        }

        private void AvailableChargeSlots_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
