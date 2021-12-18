using BO;
using System;
using System.Collections.Generic;
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
            DataContext = Station;//updating event 
            GridAddStation.Visibility = Visibility.Visible;//showing grid of fields needed for adding a station
        }

      

        private void AddStationButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            var result1 = MessageBox.Show($"Are you sure you would like to add this drone? \n", "Request Review",
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
                        if (Station.StationLocation.Longitude == default || Station.StationLocation.Longitude==default)
                            throw new MissingInfoException("No name entered for this station");

                        if (NumOfStationTxtAdd.SelectedItem == null)
                            throw new MissingInfoException("No station ID was entered for this drone");
                        bl.AddDrone(Drone, int.Parse(NumOfStationTxtAdd.Text));//adding new drone to list
                        //adding drone to list in the window of drones
                        DroneListWindow.droneToLists.Add(bl.GetAllDrones().ToList().Find(item => item.Id == int.Parse(IdTxtAdd.Text)));
                        var result2 = MessageBox.Show($"SUCCESSFULY ADDED DRONE! \nThe new drone is:\n" + Drone.ToString(), "Successfuly Added",
                           MessageBoxButton.OK);
                        switch (result2)
                        {
                            case MessageBoxResult.OK:
                                _close = true;
                                this.Close();//closes current window after drone was added
                                break;
                        }
                        break;
                    case MessageBoxResult.Cancel://if user presses cancel
                        Drone = new();//scrathes fields
                        DataContext = Drone;//updates event
                        break;
                }
            }
            catch (FailedToAddException ex)
            {
                var errorMessage = MessageBox.Show("Failed to add drone: " + ex.GetType().Name + "\n" + ex.Message, "Failed To Add", MessageBoxButton.OK, MessageBoxImage.Error);
                switch (errorMessage)
                {
                    case MessageBoxResult.OK:
                        Drone = new();
                        DataContext = Drone;
                        break;
                }
            }
            catch (InvalidInputException ex)
            {
                var errorMessage = MessageBox.Show("Failed to add drone: " + "\n" + ex.Message, "Failed To Add", MessageBoxButton.OK, MessageBoxImage.Error);
                switch (errorMessage)
                {
                    case MessageBoxResult.OK:
                        IdTxtAdd.Text = "";
                        break;
                }
            }
            catch (FormatException)
            {
                var errorMessage = MessageBox.Show("Failed to add drone: " + "\n" + "You need to enter information for all the given fields", "Failed To Add", MessageBoxButton.OK, MessageBoxImage.Error);
                switch (errorMessage)
                {
                    case MessageBoxResult.OK:
                        Drone = new();
                        DataContext = Drone;
                        break;
                }
            }
            catch (MissingInfoException ex)
            {
                var message = MessageBox.Show("Failed to add the drone: \n" + ex.Message, "Failed To Add",
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
    }
}
