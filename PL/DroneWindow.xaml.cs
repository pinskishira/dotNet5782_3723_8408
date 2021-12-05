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
            DataContext = ibl.GetDrone(((DroneToList)DroneListWindow.DronesListView.SelectedItem).Id);
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var result1 = MessageBox.Show($"Are you sure you would like to add this drone? \n", "???",
               MessageBoxButton.OKCancel);
            switch (result1)
            {
                case MessageBoxResult.OK:
                    bl.AddDrone(Drone, int.Parse(NumOfStationTextBox.Text));
                    DroneListWindow.droneToLists.Add(bl.GetAllDrones().ToList().Find(item => item.Id == int.Parse(IDTextBox.Text)));
                    var result2 = MessageBox.Show($"SUCCESSFULY ADDED DRONE! \n", "Successfuly add",
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
            var result1 = MessageBox.Show($"Are you sure you would like to update this drone? \n", "???",
               MessageBoxButton.OKCancel);
            switch (result1)
            {
                case MessageBoxResult.OK:
                    bl.UpdateDrone(int.Parse(IDTextBoxUD.Text), ModelTextBoxUD.Text);
                    DroneToList droneToList = (DroneToList)DroneListWindow.DronesListView.SelectedItem;
                    droneToList.Model = ModelTextBoxUD.Text;
                    DroneListWindow.droneToLists[DroneListWindow.DronesListView.SelectedIndex] = droneToList;
                    var result2 = MessageBox.Show($"SUCCESSFULY UPDATED DRONE! \n The drones new model name is {ModelTextBoxUD.Text}", "Successfuly updated",
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

        private void SendToChargingButton_Click(object sender, RoutedEventArgs e)
        {
            var result1 = MessageBox.Show($"Are you sure you would like to send this drone to charge? \n", "???",
               MessageBoxButton.OKCancel);
            switch (result1)
            {
                case MessageBoxResult.OK:
                    try
                    {
                        if (StatusTextBoxUD.Text != DroneStatuses.Available.ToString())
                            throw new UnmatchedDroneStatusException("Drone could no be sent to charge because it is not available.\n");
                        bl.SendDroneToChargingStation(int.Parse(IDTextBoxUD.Text));
                    }
                    catch (UnmatchedDroneStatusException ex)
                    {
                        var result2 = MessageBox.Show($"ERROR! {ex.ToString()}", "ERROR");
                    }
                    break;
            }



        }

        private void ReleaseFromChargingButton_Click(object sender, RoutedEventArgs e)
        {
            //bl.DroneReleaseFromChargingStation(int.Parse(IDTextBoxUD.Text));
        }

        private void AssignParcelToDroneButton_Click(object sender, RoutedEventArgs e)
        {
            bl.UpdateAssignParcelToDrone(int.Parse(IDTextBoxUD.Text));
        }

        private void ParcelCollectionByDroneButton_Click(object sender, RoutedEventArgs e)
        {
            bl.UpdateParcelCollectionByDrone(int.Parse(IDTextBoxUD.Text));
        }

        private void ParcelDeliveryToCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            bl.UpdateParcelDeliveryToCustomer(int.Parse(IDTextBoxUD.Text));
        }
    }
}
