using BO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for StationWindow.xaml
    /// </summary>
    public partial class StationWindow : Window
    {
        BlApi.Ibl bl;
        private StationListWindow StationListWindow { get; }
        private Station Station { get; set; } = new();
        private bool _close { get; set; } = false;
        public ObservableCollection<DroneInCharging> dronesInCharging { get; set; } = new();
            
        /// <summary>
        /// Constructor to add a station
        /// </summary>
        /// <param name="ibl">Access to BL</param>
        /// <param name="stationListWindow">Access to Station List Window</param>
        public StationWindow(BlApi.Ibl ibl, StationListWindow stationListWindow, int i = 0)
        {
            InitializeComponent();
            bl = ibl;
            StationListWindow = stationListWindow;//access to station list
            Station.StationLocation = new();
            DataContext = Station;//updating event 
            GridStationBoth.Visibility = Visibility.Visible;//showing grid of fields needed for adding a station
            GridStationADD.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Constructor to add a station
        /// </summary>
        /// <param name="ibl">Access to BL</param>
        /// <param name="stationListWindow">Access to Station List Window</param>
        public StationWindow(BlApi.Ibl ibl, StationListWindow stationListWindow)
        {
            InitializeComponent();
            bl = ibl;
            StationListWindow = stationListWindow;//access to station list
            GridStationBoth.Visibility = Visibility.Visible;
            GridStationUp.Visibility = Visibility.Visible;//showing grid of fields needed for updating a staion
            Station = ibl.GetStation(StationListWindow.CurrentStation.Id);//getting station with this id
            DataContext = Station;//updating event
            if (Station.DronesInCharging.Count() != 0)
                ViewDronesInCharging.Visibility = Visibility.Visible;
            StationButton.Content = "Update Station";
            ChargeSlots.Visibility = Visibility.Visible;
            ChargeSlotsTxtUp.Text = (stationListWindow.CurrentStation.AvailableChargeSlots + stationListWindow.CurrentStation.OccupiedChargeSlots).ToString();
            foreach (var item in Station.DronesInCharging)
            {
                dronesInCharging.Add(item);
            }
            DronesInChargingListView.ItemsSource = dronesInCharging;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        /// <summary>
        /// View drones charging in station
        /// </summary>
        private void ViewDronesInCharging_Click(object sender, RoutedEventArgs e)
        {
            if (DronesInChargingListView.Visibility == Visibility.Collapsed)
                DronesInChargingListView.Visibility = Visibility.Visible;
            else
                DronesInChargingListView.Visibility = Visibility.Collapsed;

        }

        /// <summary>
        /// Cant force window to close
        /// </summary>
        private void Window_closing(object sender, CancelEventArgs e)
        {
            if (!_close)
            {
                e.Cancel = true;
                MessageBox.Show("You can't force the window to close");
            }
        }

        /// <summary>
        /// Cancels action
        /// </summary>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            _close = true;
            Close();
        }

        /// <summary>
        /// Button that allows user to add a station
        /// </summary>
        private void StationButton_Click(object sender, RoutedEventArgs e)
        {
            if ((string)StationButton.Content == "Add Station")
            {
                var result1 = MessageBox.Show($"Are you sure you would like to add this station? \n", "Request Review",
                              MessageBoxButton.OKCancel, MessageBoxImage.Question);
                try
                {
                    switch (result1)
                    {
                        case MessageBoxResult.OK:
                            if (Station.Id == default)
                                throw new MissingInfoException("No station ID entered for this station");
                            if (Station.Name == default || Station.Name == null)
                                throw new MissingInfoException("No name entered for this station");
                            if (Station.StationLocation.Longitude == default || Station.StationLocation.Latitude == default)
                                throw new MissingInfoException("No location was entered for this station");
                            if (Station.AvailableChargeSlots == default)
                                throw new MissingInfoException("No charge slots was entered for this station");
                            bl.AddStation(Station);//adding new station to list
                            int keyStation = Station.AvailableChargeSlots;
                            if (StationListWindow.stationToLists.ContainsKey(keyStation))
                                StationListWindow.stationToLists[keyStation].Add(bl.GetAllStations().First(x => x.Id == Station.Id));
                            else
                                StationListWindow.stationToLists.Add(keyStation, bl.GetAllStations().Where(x => x.Id == Station.Id).ToList());
                            StationListWindow.RefreshStations();
                            var result2 = MessageBox.Show($"SUCCESSFULY ADDED STATION! \nThe new station is:\n" + Station.ToString(), "Successfuly Added",
                               MessageBoxButton.OK);
                            switch (result2)
                            {
                                case MessageBoxResult.OK:
                                    _close = true;
                                    Close();//closes current window after station was added
                                    break;
                            }
                            break;
                        case MessageBoxResult.Cancel://if user presses cancel
                            Station = new();//scrathes fields
                            DataContext = Station;//updates event
                            break;
                    }
                }
                catch (FailedToAddException ex)
                {
                    var errorMessage = MessageBox.Show("Failed to add station: " + ex.GetType().Name + "\n" + ex.Message, "Failed To Add", MessageBoxButton.OK, MessageBoxImage.Error);
                    switch (errorMessage)
                    {
                        case MessageBoxResult.OK:
                            Station = new();
                            DataContext = Station;
                            break;
                    }
                }
                catch (InvalidInputException ex)
                {
                    var errorMessage = MessageBox.Show("Failed to add station: " + "\n" + ex.Message, "Failed To Add", MessageBoxButton.OK, MessageBoxImage.Error);
                    switch (errorMessage)
                    {
                        case MessageBoxResult.OK:
                            IdTxtAdd.Text = "";
                            break;
                    }
                }
                catch (FormatException)
                {
                    var errorMessage = MessageBox.Show("Failed to add station: " + "\n" + "You need to enter information for all the given fields", "Failed To Add", MessageBoxButton.OK, MessageBoxImage.Error);
                    switch (errorMessage)
                    {
                        case MessageBoxResult.OK:
                            Station = new();
                            DataContext = Station;
                            break;
                    }
                }
                catch (MissingInfoException ex)
                {
                    var message = MessageBox.Show("Failed to add the station: \n" + ex.Message, "Failed To Add",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    switch (message)
                    {
                        case MessageBoxResult.OK:
                            break;
                    }
                }
            }
            if ((string)StationButton.Content == "Update Station")
            {
                var result1 = MessageBox.Show($"Are you sure you would like to update this station? \n", "Request Review",
               MessageBoxButton.OKCancel, MessageBoxImage.Question);
                string oldName = StationListWindow.CurrentStation.Name;
                try
                {
                    switch (result1)
                    {
                        case MessageBoxResult.OK:
                            bl.UpdateStation(int.Parse(IdTxtUp.Text), NameTxtAdd.Text, int.Parse(ChargeSlotsTxtUp.Text));//udating chosen station
                            StationListWindow.CurrentStation.Name = NameTxtAdd.Text;//updating drone name
                            StationListWindow.CurrentStation.AvailableChargeSlots = int.Parse(ChargeSlotsTxtUp.Text) - StationListWindow.CurrentStation.OccupiedChargeSlots;
                            AvailableChargeSlotsTxtUp.Text = StationListWindow.CurrentStation.AvailableChargeSlots.ToString();
                            StationListWindow.RefreshStations();
                            MessageBox.Show($"SUCCESSFULY UPDATED STATION! \n The stations new name is {NameTxtAdd.Text}, and new amount of charge slots is {ChargeSlotsTxtUp.Text}", "Successfuly Updated", MessageBoxButton.OK);
                            break;
                        case MessageBoxResult.Cancel:
                            NameTxtAdd.Text = oldName;
                            ChargeSlotsTxtUp.Text = "";
                            break;
                    }
                }
                catch (ItemDoesNotExistException ex)
                {
                    var errorMessage = MessageBox.Show("Failed to update station: " + ex.GetType().Name + "\n" + ex.Message, "Failed Warning", MessageBoxButton.OK, MessageBoxImage.Error);
                    switch (errorMessage)
                    {
                        case MessageBoxResult.OK:
                            NameTxtAdd.Text = "";
                            ChargeSlotsTxtUp.Text = "";
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Goes to window of a drone in charging 
        /// </summary>
        private void DronesInChargingListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DroneInCharging droneInCharging;
            droneInCharging = (DroneInCharging)DronesInChargingListView.SelectedItem;
            new DroneWindow(bl, this, droneInCharging.Id, DronesInChargingListView.SelectedIndex).Show();
        }
    }
}
