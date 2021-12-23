using BO;
using System;
using System.Collections.Generic;
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
            GridAddStation.Visibility = Visibility.Visible;//showing grid of fields needed for adding a station
        }

        public StationWindow(BlApi.Ibl ibl, StationListWindow stationListWindow)
        {
            InitializeComponent();
            bl = ibl;
            StationListWindow = stationListWindow;//access to station list
            GridUpdateStation.Visibility = Visibility.Visible;//showing grid of fields needed for updating a staion
            Station = ibl.GetStation(StationListWindow.CurrentStation.Id);//getting station with this id
            DataContext = Station;//updating event
            Station visibleStationButton = Station;//equals chosen drone to update
            if (Station.DronesInCharging.Count() != 0)
                ViewDronesInCharging.Visibility = Visibility.Visible;
        }

        private void AddStationButtonAdd_Click(object sender, RoutedEventArgs e)
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
                     //  if (Station.StationLocation.Longitude == null || Station.StationLocation.Latitude==default)
                     //      throw new MissingInfoException("No location was entered for this station");
                        if(Station.AvailableChargeSlots == default)
                            throw new MissingInfoException("No charge slots was entered for this station");
                        //Station.StationLocation.Longitude = int.Parse(LongituteTxtAdd.Text);
                        //Station.StationLocation.Latitude = int.Parse(LatitudeTxtAdd.Text);
                        bl.AddStation(Station);//adding new station to list
                        //adding station to list in the window of stations
                        StationListWindow.stationToLists.Add(bl.GetAllStations().ToList().Find(item => item.Id == int.Parse(IdTxtAdd.Text)));
                        var result2 = MessageBox.Show($"SUCCESSFULY ADDED STATION! \nThe new station is:\n" + Station.ToString(), "Successfuly Added",
                           MessageBoxButton.OK);
                        switch (result2)
                        {
                            case MessageBoxResult.OK:
                                _close = true;
                                this.Close();//closes current window after station was added
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

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void ViewDronesInCharging_Click(object sender, RoutedEventArgs e)
        {
            DronesInCharging.Visibility = Visibility.Visible;
        }

        private void UpdateStationButtonUD_Click(object sender, RoutedEventArgs e)
        {
            var result1 = MessageBox.Show($"Are you sure you would like to update this station? \n", "Request Review",
               MessageBoxButton.OKCancel, MessageBoxImage.Question);
            try
            {
                switch (result1)
                {
                    case MessageBoxResult.OK:
                        bl.UpdateStation(int.Parse(IdTxtUp.Text), NameTxtUp.Text, int.Parse(ChargeSlotsTxtUp.Text));//udating chosen station
                        StationListWindow.CurrentStation.Name = NameTxtUp.Text;//updating drone name
                        StationListWindow.CurrentStation.AvailableChargeSlots = int.Parse(ChargeSlotsTxtUp.Text);
                        StationListWindow.stationToLists[StationListWindow.StationListView.SelectedIndex] = StationListWindow.CurrentStation;//updating event
                        var result2 = MessageBox.Show($"SUCCESSFULY UPDATED STATION! \n The drones new model name is {NameTxtUp.Text}, and new amount of charge slots is {ChargeSlotsTxtUp.Text}", "Successfuly Updated",
                           MessageBoxButton.OK);
                        switch (result2)
                        {
                            case MessageBoxResult.OK:
                                break;
                        }
                        break;
                    case MessageBoxResult.Cancel:
                        break;
                }
            }
            catch (ItemDoesNotExistException ex)
            {
                var errorMessage = MessageBox.Show("Failed to update station: " + ex.GetType().Name + "\n" + ex.Message, "Failed Warning", MessageBoxButton.OK, MessageBoxImage.Error);
                switch (errorMessage)
                {
                    case MessageBoxResult.OK:
                        NameTxtUp.Text = "";
                        ChargeSlotsTxtUp.Text = "";
                        break;
                }
            }
        }

        private void window_closeing(object sender, CancelEventArgs e)
        {
            if (!_close)
            {
                e.Cancel = true;
                MessageBox.Show("You can't force the window to close");
            }
        }

        private void CancelButtonUD_Click(object sender, RoutedEventArgs e)
        {
            _close = true;
            this.Close();
        }
    }
}
