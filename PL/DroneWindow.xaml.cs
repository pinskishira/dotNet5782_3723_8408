using System;
using System.Collections.Generic;
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
using System.Text.RegularExpressions;


using static IBL.BO.Enum;
using IBL.BO;
using static PL.PLExceptions;

namespace PL
{
    /// <summary>
    /// Interaction logic for DroneWindow.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {
        BL.BL bl;
        private DroneListWindow DroneListWindow { get; }
        private Drone Drone { get; set; } = new();

        /// <summary>
        /// Constructor to add a drone
        /// </summary>
        /// <param name="ibl">Access to BL</param>
        /// <param name="droneListWindow">Access to Drone List Window</param>
        public DroneWindow(BL.BL ibl, DroneListWindow droneListWindow, int i = 0)
        {
            InitializeComponent();
            bl = ibl;
            DroneListWindow = droneListWindow;//access to drone list
            DataContext = Drone;//updating event 
            WeightCmbxAdd.ItemsSource = System.Enum.GetValues(typeof(IBL.BO.Enum.WeightCategories));//getting chosen weight
            GridAddDrone.Visibility = Visibility.Visible;//showing grid of fields needed for adding a drone
            NumOfStationTxtAdd.ItemsSource = bl.GetAllStations().Select(s => s.Id);//choosing a station from a combo box
        }

        /// <summary>
        /// Constructor to update a drone
        /// </summary>
        /// <param name="ibl">Access to BL</param>
        /// <param name="droneListWindow">Access to Drone List Window</param>
        public DroneWindow(BL.BL ibl, DroneListWindow droneListWindow)
        {
            InitializeComponent();
            bl = ibl;
            DroneListWindow = droneListWindow;//access to drone list
            GridUpdateDrone.Visibility = Visibility.Visible;//showing grid of fields needed for updating a drone
            Drone = ibl.GetDrone(DroneListWindow.CurrentDrone.Id);//getting drone with this id
            DataContext = Drone;//updating event
            Drone visibleDroneButton = Drone;//equals chosen drone to update
            if (Drone.ParcelInTransfer == null)//if its got no parcel assigned to it
            {
                GridParcelInTransfer.Visibility = Visibility.Collapsed;//all the fields linked to parcel are hidden - because are not needed
            }

            if (visibleDroneButton.DroneStatus == IBL.BO.Enum.DroneStatuses.Available)//if drone status is - available
            {
                ChargeDroneUD.Content = "Send Drone to Charging";//input button content
                DroneStatusChangeUD.Content = "Send Drone to Collect Parcel";//input button content
            }

            if (visibleDroneButton.DroneStatus == IBL.BO.Enum.DroneStatuses.Maintenance)//if drone status is - maintanance
            {
                ChargeDroneUD.Content = "Release Drone from Charging";//input button content
                DroneStatusChangeUD.Visibility = Visibility.Hidden;//hiding button uneeded for this status
            }

            else
            {
                //if drone status is - Delivery, and parcel has not yet been delivered 
                if (visibleDroneButton.DroneStatus == IBL.BO.Enum.DroneStatuses.Delivery && visibleDroneButton.ParcelInTransfer.ParcelState == false)
                {
                    DroneStatusChangeUD.Content = "Drone Collects Parcel";//input button content
                    ChargeDroneUD.Visibility = Visibility.Hidden;//hiding button uneeded for this status
                }
                //if drone status is - Delivery, and parcel has been deliveerd
                if (visibleDroneButton.DroneStatus == IBL.BO.Enum.DroneStatuses.Delivery && visibleDroneButton.ParcelInTransfer.ParcelState == true)
                {
                    DroneStatusChangeUD.Content = "Drone Delivers Parcel";//input button content
                    ChargeDroneUD.Visibility = Visibility.Hidden;//hiding button uneeded for this status
                }
            }
        }

        /// <summary>
        /// When user presses on add button, it gives him an option to change his mind, or add drone
        /// </summary>
        private void AddDroneButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            var result1 = MessageBox.Show($"Are you sure you would like to add this drone? \n", "Request Review",
               MessageBoxButton.OKCancel, MessageBoxImage.Question);
            try
            {
                switch (result1)
                {
                    case MessageBoxResult.OK:
                        bl.AddDrone(Drone, int.Parse(NumOfStationTxtAdd.Text));//adding new drone to list
                        //adding drone to list in the window of drones
                        DroneListWindow.droneToLists.Add(bl.GetAllDrones().ToList().Find(item => item.Id == int.Parse(IdTxtAdd.Text)));
                        var result2 = MessageBox.Show($"SUCCESSFULY ADDED DRONE! \nThe new drone is:\n"+Drone.ToString(), "Successfuly Added",
                           MessageBoxButton.OK);
                        switch (result2)
                        {
                            case MessageBoxResult.OK:
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
                var errorMessage=MessageBox.Show("Failed to add drone: " + ex.GetType().Name + "\n" + ex.Message, "Failed To Add",MessageBoxButton.OK, MessageBoxImage.Error);
                switch (errorMessage)
                {
                    case MessageBoxResult.OK:
                        Drone = new();
                        DataContext = Drone;
                        break;
                }
            }
            catch(InvalidInputException ex)
            {
                var errorMessage = MessageBox.Show("Failed to add drone: "+ "\n" + ex.Message, "Failed To Add", MessageBoxButton.OK, MessageBoxImage.Error);
                switch (errorMessage)
                {
                    case MessageBoxResult.OK:
                        Drone = new();
                        DataContext = Drone;
                        break;
                }
            }
            catch (FormatException)
            {
                var errorMessage = MessageBox.Show("Failed to add drone: "+ "\n" + "You need to enter information for all the given fields", "Failed To Add", MessageBoxButton.OK, MessageBoxImage.Error);
                switch (errorMessage)
                {
                    case MessageBoxResult.OK:
                        Drone = new();
                        DataContext = Drone;
                        break;
                }
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void UpdateDroneButtonUD_Click(object sender, RoutedEventArgs e)
        {
            var result1 = MessageBox.Show($"Are you sure you would like to update this drone? \n", "Request Review",
               MessageBoxButton.OKCancel, MessageBoxImage.Question);
            try
            {
                //if (ModelTxtUD.Text == Drone.Model)
                //    throw new SameFieldDataException("Drone model name remained the same");
                switch (result1)
                {
                    case MessageBoxResult.OK:
                        bl.UpdateDrone(int.Parse(IDTxtUD.Text), ModelTxtUD.Text);//udating chosen drone
                        DroneListWindow.CurrentDrone.Model = ModelTxtUD.Text;//updating drone name
                        DroneListWindow.droneToLists[DroneListWindow.DronesListView.SelectedIndex] = DroneListWindow.CurrentDrone;//updating event
                        var result2 = MessageBox.Show($"SUCCESSFULY UPDATED DRONE! \n The drones new model name is {ModelTxtUD.Text}", "Successfuly Updated",
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
                var errorMessage = MessageBox.Show("Failed to update drone: " + ex.GetType().Name + "\n" + ex.Message, "Failed Warning", MessageBoxButton.OK, MessageBoxImage.Error);
                switch (errorMessage)
                {
                    case MessageBoxResult.OK:
                        ModelTxtUD.Text = "";
                        break;
                }
            }
            //catch(SameFieldDataException ex)
            //{
            //    var errorMessage = MessageBox.Show("Attention: " + ex.GetType().Name + "\n" + ex.Message, "Attention", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    switch (errorMessage)
            //    {
            //        case MessageBoxResult.OK:
            //            ModelTxtUD.Text = "";
            //            break;
            //    }
            //}
        }

        /// <summary>
        /// Closes current window
        /// </summary>
        private void CloseWidowButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Closes current window
        /// </summary>
        private void CancelButtonUD_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Sorts the different update buttons according to their content and sending to upadate in bl and updating list in pl
        /// </summary>
        private void DroneStatusChangeUD_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                object success = null;
                if ((string)DroneStatusChangeUD.Content == "Send Drone to Collect Parcel")
                {
                    var request = MessageBox.Show($"Are you sure you would like to send drone to collect a parcel? \n", "Request Review",
                    MessageBoxButton.OKCancel, MessageBoxImage.Question);
                    switch (request)
                    {
                        case MessageBoxResult.OK:
                            bl.UpdateAssignParcelToDrone(int.Parse(IDTxtUD.Text));//assign parcel to drone - updating in bl 
                            ChargeDroneUD.Visibility = Visibility.Hidden;//hiding uneccessary buttons after update
                            DroneStatusChangeUD.Content = "Drone Collects Parcel";//changing to button content to fit past update
                            success = MessageBox.Show($"SUCCESSFULY ASIGNED DRONE TO A PARCEL! \n", "Successfuly Updated",
                            MessageBoxButton.OK);
                            GridParcelInTransfer.Visibility = Visibility.Visible;
                            break;
                    }

                }
                else
                {
                    if ((string)DroneStatusChangeUD.Content == "Drone Collects Parcel")
                    {
                        var request = MessageBox.Show($"Are you sure you would like to drone to collect parcel? \n", "Request Review",
                        MessageBoxButton.OKCancel, MessageBoxImage.Question);
                        switch (request)
                        {
                            case MessageBoxResult.OK:
                                bl.UpdateParcelCollectionByDrone(int.Parse(IDTxtUD.Text));//drone collects parcel - updating in bl
                                ChargeDroneUD.Visibility = Visibility.Hidden;//hiding uneccessary buttons after update
                                DroneStatusChangeUD.Content = "Drone Delivers Parcel";//changing to button content to fit past update
                                success = MessageBox.Show($"DRONE SUCCESSFULY COLLECTED PARCEL! \n", "Successfuly Updated",
                                MessageBoxButton.OK);
                                break;
                        }
                    }
                    else
                    {
                        if ((string)DroneStatusChangeUD.Content == "Drone Delivers Parcel")
                        {
                            var request = MessageBox.Show($"Are you sure you would like to drone to deliver parcel? \n", "Request Review",
                           MessageBoxButton.OKCancel, MessageBoxImage.Question);
                            switch (request)
                            {
                                case MessageBoxResult.OK:
                                    bl.UpdateParcelDeliveryToCustomer(int.Parse(IDTxtUD.Text));//drone delivers parcel - updating in bl
                                    ChargeDroneUD.Visibility = Visibility.Visible;//showing neccessary buttons after update
                                    ChargeDroneUD.Content = "Send Drone to Charging";//changing to button content to fit past update
                                    DroneStatusChangeUD.Content = "Drone Collects Parcel";//changing to button content to fit past update
                                    success = MessageBox.Show($"DRONE SUCCESSFULY DELICVERED PARCEL! \n", "Successfuly Updated",
                                    MessageBoxButton.OK);
                                    GridParcelInTransfer.Visibility = Visibility.Collapsed;
                                    break;
                            }
                        }
                    }
                }
                switch (success)
                {
                    case MessageBoxResult.OK:
                        int index = DroneListWindow.droneToLists.ToList().FindIndex(item => item.Id == Drone.Id);
                        DroneListWindow.droneToLists[index] = bl.GetAllDrones().First(item => item.Id == Drone.Id);
                        Drone = bl.GetDrone(Drone.Id);
                        DataContext = Drone;
                        break;
                }
            }
            catch (ParcelDeliveryException ex)
            {
                var errorMessage = MessageBox.Show($"ERROR! {ex.ToString()}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Warning);
                switch (errorMessage)
                {
                    case MessageBoxResult.OK:
                        break;
                }
            }
            catch (FailedToCollectParcelException ex)
            {
                var errorMessage = MessageBox.Show($"ERROR! {ex.ToString()}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Warning);
                switch (errorMessage)
                {
                    case MessageBoxResult.OK:
                        break;
                }
            }
        }

        /// <summary>
        /// Sorts the different update buttons according to their content and sending to upadate in bl and updating list in pl
        /// </summary>
        private void ChargeDroneUD_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                object success = null;
                if (ChargeDroneUD.Content.ToString() == "Send Drone to Charging")
                {
                    var request = MessageBox.Show($"Are you sure you would like to send drone to charge? \n", "Request Review",
                   MessageBoxButton.OKCancel, MessageBoxImage.Question);
                    switch (request)
                    {
                        case MessageBoxResult.OK:
                            bl.SendDroneToChargingStation(int.Parse(IDTxtUD.Text));//send drone to charging - updating in bl
                            DroneStatusChangeUD.Visibility = Visibility.Hidden;//hiding uneccessary buttons after update
                            ChargeDroneUD.Content = "Release Drone from Charging";//changing to button content to fit past update
                            success = MessageBox.Show($"SUCCESSFULY SENT DRONE TO CHARGE! \n", "Successfuly Updated",
                            MessageBoxButton.OK);
                            break;
                    }
                }
                else
                {
                    if (ChargeDroneUD.Content.ToString() == "Release Drone from Charging")
                    {
                        var request = MessageBox.Show($"Are you sure you would like to release drone from charge? \n", "Request Review",
                    MessageBoxButton.OKCancel, MessageBoxImage.Question);
                        switch (request)
                        {
                            case MessageBoxResult.OK:
                                bl.DroneReleaseFromChargingStation(int.Parse(IDTxtUD.Text));//release drone from charging - updating in bl
                                DroneStatusChangeUD.Visibility = Visibility.Visible;//showing neccessary buttons after update
                                ChargeDroneUD.Content = "Send Drone to Charging";//changing to button content to fit past update
                                DroneStatusChangeUD.Content = "Drone Collects Parcel";//changing to button content to fit past update
                                success = MessageBox.Show($"SUCCESSFULY RELEASED DRONE FROM CHARGE! \n", "Successfuly Updated",
                                MessageBoxButton.OK);
                                break;
                        }
                    }
                }
                switch (success)
                {
                    case MessageBoxResult.OK:
                        int index = DroneListWindow.droneToLists.ToList().FindIndex(item => item.Id == Drone.Id);
                        DroneListWindow.droneToLists[index] = bl.GetAllDrones().First(item => item.Id == Drone.Id);
                        Drone = bl.GetDrone(Drone.Id);
                        DataContext = Drone;
                        break;
                }
            }
            catch (DroneMaintananceException ex)
            {
                var errorMessage = MessageBox.Show($"ERROR! {ex.ToString()}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Warning);
                switch (errorMessage)
                {
                    case MessageBoxResult.OK:
                        break;
                }
            }
        }

    }

}