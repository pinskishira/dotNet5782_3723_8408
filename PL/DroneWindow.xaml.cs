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
        public DroneWindow(BL.BL ibl, DroneListWindow droneListWindow)
        {
            InitializeComponent();
            bl = ibl;
            DroneListWindow = droneListWindow;
            GridUpdateDrone.Visibility = Visibility.Visible;
            Drone drone = ibl.GetDrone(((DroneToList)DroneListWindow.DronesListView.SelectedItem).Id);
            DataContext = drone;
            Drone visibleDroneButton = drone;
            if (visibleDroneButton.DroneStatus == IBL.BO.Enum.DroneStatuses.Available)
            {
                ChargeDroneUD.Content = "Send Drone to Charging";
                DroneStatusChangeUD.Content = "Send Drone to Collect Parcel";
            }
            if (visibleDroneButton.DroneStatus == IBL.BO.Enum.DroneStatuses.Maintenance)
            {
                ChargeDroneUD.Content = "Release Drone from Charging";
                DroneStatusChangeUD.Visibility = Visibility.Hidden;
            }
            else
            {
                if (visibleDroneButton.DroneStatus == IBL.BO.Enum.DroneStatuses.Delivery && visibleDroneButton.ParcelInTransfer.ParcelState == false)
                {
                    DroneStatusChangeUD.Content = "Drone Collects Parcel";
                    ChargeDroneUD.Visibility = Visibility.Hidden;
                }
                if (visibleDroneButton.DroneStatus == IBL.BO.Enum.DroneStatuses.Delivery && visibleDroneButton.ParcelInTransfer.ParcelState == true)
                {
                    DroneStatusChangeUD.Content = "Drone Delivers Parcel";
                    ChargeDroneUD.Visibility = Visibility.Hidden;
                }
            }
        }

        public DroneWindow(BL.BL ibl, DroneListWindow droneListWindow, int i = 0)
        {
            InitializeComponent();
            bl = ibl;
            DroneListWindow = droneListWindow;
            DataContext = Drone;
            WeightComboBox.ItemsSource = System.Enum.GetValues(typeof(IBL.BO.Enum.WeightCategories));
            GridAddDrone.Visibility = Visibility.Visible;
        }

        private void AddDroneButton_Click(object sender, RoutedEventArgs e)
        {
            var result1 = MessageBox.Show($"Are you sure you would like to add this drone? \n", "Request Review",
               MessageBoxButton.OKCancel, MessageBoxImage.Question);
            switch (result1)
            {
                case MessageBoxResult.OK:
                    bl.AddDrone(Drone, int.Parse(NumOfStationTextBox.Text));
                    DroneListWindow.droneToLists.Add(bl.GetAllDrones().ToList().Find(item => item.Id == int.Parse(IDTextBox.Text)));
                    var result2 = MessageBox.Show($"SUCCESSFULY ADDED DRONE! \n", "Successfuly Added",
                       MessageBoxButton.OK);
                    switch (result2)
                    {
                        case MessageBoxResult.OK:
                            this.Close();
                            break;
                    }
                    break;
                case MessageBoxResult.Cancel:
                    IDTextBox.Text = "";
                    ModelTextBox.Text = "";
                    WeightComboBox.SelectedItem = 0;
                    NumOfStationTextBox.Text = "";
                    break;
            }
        }

        private void UpdateDroneButtonUD_Click(object sender, RoutedEventArgs e)
        {
            var result1 = MessageBox.Show($"Are you sure you would like to update this drone? \n", "Request Review",
               MessageBoxButton.OKCancel, MessageBoxImage.Question);
            switch (result1)
            {
                case MessageBoxResult.OK:
                    bl.UpdateDrone(int.Parse(IDTextBoxUD.Text), ModelTextBoxUD.Text);
                    DroneToList droneToList = (DroneToList)DroneListWindow.DronesListView.SelectedItem;
                    droneToList.Model = ModelTextBoxUD.Text;
                    DroneListWindow.droneToLists[DroneListWindow.DronesListView.SelectedIndex] = droneToList;
                    var result2 = MessageBox.Show($"SUCCESSFULY UPDATED DRONE! \n The drones new model name is {ModelTextBoxUD.Text}", "Successfuly Updated",
                       MessageBoxButton.OK);
                    switch (result2)
                    {
                        case MessageBoxResult.OK:
                            this.Close();
                            break;
                    }
                    break;
                case MessageBoxResult.Cancel:
                    ModelTextBoxUD.Text = "";
                    break;
            }
        }

        private void CloseWindowButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CancelButtonUD_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DroneStatusChangeUD_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var success = MessageBox.Show("");
                if (DroneStatusChangeUD.Content.ToString() == "Send Drone to Collect Parcel")
                {
                    var request = MessageBox.Show($"Are you sure you would like to send drone to collect a parcel? \n", "Request Review",
                    MessageBoxButton.OKCancel, MessageBoxImage.Question);
                    switch (request)
                    {
                        case MessageBoxResult.OK:
                            bl.UpdateAssignParcelToDrone(int.Parse(IDTextBoxUD.Text));
                            success = MessageBox.Show($"SUCCESSFULY ASIGNED DRONE TO A PARCEL! \n", "Successfuly Updated",
                            MessageBoxButton.OK);
                            break;
                    }

                }
                if (DroneStatusChangeUD.Content.ToString() == "Drone Collects Parcel")
                {
                    var request = MessageBox.Show($"Are you sure you would like to drone to collect parcel? \n", "Request Review",
                    MessageBoxButton.OKCancel, MessageBoxImage.Question);
                    switch (request)
                    {
                        case MessageBoxResult.OK:
                            bl.UpdateParcelCollectionByDrone(int.Parse(IDTextBoxUD.Text));
                            success = MessageBox.Show($"DRONE SUCCESSFULY COLLECTED PARCEL! \n", "Successfuly Updated",
                            MessageBoxButton.OK);
                            break;
                    }
                }
                if (DroneStatusChangeUD.Content.ToString() == "Drone Delivers Parcel")
                {
                    var request = MessageBox.Show($"Are you sure you would like to drone to deliver parcel? \n", "Request Review",
                   MessageBoxButton.OKCancel, MessageBoxImage.Question);
                    switch (request)
                    {
                        case MessageBoxResult.OK:
                            bl.UpdateParcelDeliveryToCustomer(int.Parse(IDTextBoxUD.Text));
                            success = MessageBox.Show($"DRONE SUCCESSFULY DELICVERED PARCEL! \n", "Successfuly Updated",
                            MessageBoxButton.OK);
                            break;
                    }
                }
                switch (success)
                {
                    case MessageBoxResult.OK:
                        DroneToList droneToList = (DroneToList)DroneListWindow.DronesListView.SelectedItem;
                        DroneListWindow.droneToLists[bl.GetAllDrones().ToList().FindIndex(item => item.Id == int.Parse(IDTextBoxUD.Text))] = droneToList;
                        this.Close();
                        break;
                }
            }
            catch (ParcelDeliveryException ex)
            {
                var errorMessage = MessageBox.Show($"ERROR! {ex.ToString()}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Warning);
                switch (errorMessage)
                {
                    case MessageBoxResult.OK:
                        this.Close();
                        break;
                }
            }
            catch (InvalidOperationException ex)
            {
                var errorMessage = MessageBox.Show($"ERROR! {ex.ToString()}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Warning);
                switch (errorMessage)
                {
                    case MessageBoxResult.OK:
                        this.Close();
                        break;
                }
            }

        }
        private void ChargeDroneUD_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var success = MessageBox.Show("");
                if (ChargeDroneUD.Content.ToString() == "Send Drone to Charging")
                {
                    var request = MessageBox.Show($"Are you sure you would like to send drone to charge? \n", "Request Review",
                   MessageBoxButton.OKCancel, MessageBoxImage.Question);
                    switch (request)
                    {
                        case MessageBoxResult.OK:
                            bl.SendDroneToChargingStation(int.Parse(IDTextBoxUD.Text));
                            success = MessageBox.Show($"SUCCESSFULY SENT DRONE TO CHARGE! \n", "Successfuly Updated",
                            MessageBoxButton.OK);
                            break;
                    }
                }
                if(ChargeDroneUD.Content.ToString() == "Release Drone from Charging")
                {
                    var request = MessageBox.Show($"Are you sure you would like to release drone from charge? \n", "Request Review",
                MessageBoxButton.OKCancel, MessageBoxImage.Question);
                    switch (request)
                    {
                        case MessageBoxResult.OK:
                            //bl.DroneReleaseFromChargingStation(int.Parse(IDTextBoxUD.Text));
                            success = MessageBox.Show($"SUCCESSFULY RELEASED DRONE FROM CHARGE! \n", "Successfuly Updated",
                            MessageBoxButton.OK);
                            break;
                    }
                }
                switch (success)
                {
                    case MessageBoxResult.OK:
                        DroneToList droneToList = (DroneToList)DroneListWindow.DronesListView.SelectedItem;
                        DroneListWindow.droneToLists[bl.GetAllDrones().ToList().FindIndex(item => item.Id == int.Parse(IDTextBoxUD.Text))] = droneToList;
                        this.Close();
                        break;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        //private void SendToChargingButton_Click(object sender, RoutedEventArgs e)
        //{
        //    var request = MessageBox.Show($"Are you sure you would like to send this drone to charge? \n", "Request Review",
        //       MessageBoxButton.OKCancel, MessageBoxImage.Question);
        //    switch (request)
        //    {
        //        case MessageBoxResult.OK:
        //            try
        //            {
        //                if (StatusTextBoxUD.Text != DroneStatuses.Available.ToString())
        //                    throw new UnmatchedDroneStatusException("Drone could no be sent to charge because it is not available.\n");
        //                bl.SendDroneToChargingStation(int.Parse(IDTextBoxUD.Text));
        //                var success = MessageBox.Show($"SUCCESSFULY SENT DRONE TO CHARGE! \n", "Successfuly Updated",
        //              MessageBoxButton.OK);
        //                switch (success)
        //                {
        //                    case MessageBoxResult.OK:
        //                        DroneToList droneToList = (DroneToList)DroneListWindow.DronesListView.SelectedItem;
        //                        DroneListWindow.droneToLists[bl.GetAllDrones().ToList().FindIndex(item=>item.Id==int.Parse(IDTextBoxUD.Text))] = droneToList;
        //                        this.Close();
        //                        break;
        //                }
        //                break;
        //            }
        //            catch (UnmatchedDroneStatusException ex)
        //            {
        //                var errorMessage = MessageBox.Show($"ERROR! {ex.ToString()}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Warning);
        //                switch (errorMessage)
        //                {
        //                    case MessageBoxResult.OK:
        //                        this.Close();
        //                        break;
        //                }
        //                break;
        //            }
        //            catch(DroneMaintananceException ex)
        //            {
        //                var errorMessage = MessageBox.Show($"ERROR! {ex.ToString()}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Warning);
        //                switch (errorMessage)
        //                {
        //                    case MessageBoxResult.OK:
        //                        this.Close();
        //                        break;
        //                }
        //                break;
        //            }
        //    }
        //}

        //private void ReleaseFromChargingButton_Click(object sender, RoutedEventArgs e)
        //{
        //    //bl.DroneReleaseFromChargingStation(int.Parse(IDTextBoxUD.Text));
        //}

        //private void AssignParcelToDroneButton_Click(object sender, RoutedEventArgs e)
        //{
        //    var request = MessageBox.Show($"Are you sure you would like to assign this drone to a parcel? \n", "Request Review",
        //      MessageBoxButton.OKCancel, MessageBoxImage.Question);
        //    switch (request)
        //    {
        //        case MessageBoxResult.OK:
        //            try
        //            {
        //                if (StatusTextBoxUD.Text != DroneStatuses.Available.ToString())
        //                    throw new UnmatchedDroneStatusException("Drone could no be assigned to a parcel because it is not available.\n");
        //                bl.UpdateAssignParcelToDrone(int.Parse(IDTextBoxUD.Text));
        //                var success = MessageBox.Show($"SUCCESSFULY AASIGNED DRONE TO A PARCEL! \n", "Successfuly Updated",
        //              MessageBoxButton.OK);
        //                switch (success)
        //                {
        //                    case MessageBoxResult.OK:
        //                        DroneToList droneToList = (DroneToList)DroneListWindow.DronesListView.SelectedItem;
        //                        DroneListWindow.droneToLists[bl.GetAllDrones().ToList().FindIndex(item => item.Id == int.Parse(IDTextBoxUD.Text))] = droneToList;
        //                        this.Close();
        //                        break;
        //                }
        //                break;
        //            }
        //            catch (UnmatchedDroneStatusException ex)
        //            {
        //                var errorMessage = MessageBox.Show($"ERROR! {ex.ToString()}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Warning);
        //                switch (errorMessage)
        //                {
        //                    case MessageBoxResult.OK:
        //                        this.Close();
        //                        break;
        //                }
        //                break;
        //            }
        //            catch (ParcelDeliveryException ex)
        //            {
        //                var errorMessage = MessageBox.Show($"ERROR! {ex.ToString()}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Warning);
        //                switch (errorMessage)
        //                {
        //                    case MessageBoxResult.OK:
        //                        this.Close();
        //                        break;
        //                }
        //                break;
        //            }
        //            catch (InvalidOperationException ex)
        //            {
        //                var errorMessage = MessageBox.Show($"ERROR! {ex.ToString()}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Warning);
        //                switch (errorMessage)
        //                {
        //                    case MessageBoxResult.OK:
        //                        this.Close();
        //                        break;
        //                }
        //                break;
        //            }
        //    }
        //}

        //private void ParcelCollectionByDroneButton_Click(object sender, RoutedEventArgs e)
        //{
        //    var request = MessageBox.Show($"Are you sure you would like drone to collect parcel? \n", "Request Review",
        //     MessageBoxButton.OKCancel, MessageBoxImage.Question);
        //    switch (request)
        //    {
        //        case MessageBoxResult.OK:
        //            try
        //            {
        //                if (StatusTextBoxUD.Text != DroneStatuses.Delivery.ToString())
        //                    throw new UnmatchedDroneStatusException("Drone could not collect a parcel because it is not available.\n");
        //                bl.UpdateAssignParcelToDrone(int.Parse(IDTextBoxUD.Text));
        //                var success = MessageBox.Show($"SUCCESSFULY AASIGNED DRONE TO A PARCEL! \n", "Successfuly Updated",
        //              MessageBoxButton.OK);
        //                switch (success)
        //                {
        //                    case MessageBoxResult.OK:
        //                        DroneToList droneToList = (DroneToList)DroneListWindow.DronesListView.SelectedItem;
        //                        DroneListWindow.droneToLists[bl.GetAllDrones().ToList().FindIndex(item => item.Id == int.Parse(IDTextBoxUD.Text))] = droneToList;
        //                        this.Close();
        //                        break;
        //                }
        //                break;
        //            }
        //            catch (UnmatchedDroneStatusException ex)
        //            {
        //                var errorMessage = MessageBox.Show($"ERROR! {ex.ToString()}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Warning);
        //                switch (errorMessage)
        //                {
        //                    case MessageBoxResult.OK:
        //                        this.Close();
        //                        break;
        //                }
        //                break;
        //            }
        //            catch (ParcelDeliveryException ex)
        //            {
        //                var errorMessage = MessageBox.Show($"ERROR! {ex.ToString()}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Warning);
        //                switch (errorMessage)
        //                {
        //                    case MessageBoxResult.OK:
        //                        this.Close();
        //                        break;
        //                }
        //                break;
        //            }
        //            catch (InvalidOperationException ex)
        //            {
        //                var errorMessage = MessageBox.Show($"ERROR! {ex.ToString()}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Warning);
        //                switch (errorMessage)
        //                {
        //                    case MessageBoxResult.OK:
        //                        this.Close();
        //                        break;
        //                }
        //                break;
        //            }
        //    }
        //    bl.UpdateParcelCollectionByDrone(int.Parse(IDTextBoxUD.Text));
        //}

        //private void ParcelDeliveryToCustomerButton_Click(object sender, RoutedEventArgs e)
        //{
        //    bl.UpdateParcelDeliveryToCustomer(int.Parse(IDTextBoxUD.Text));
        //}

    }

}