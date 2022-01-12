using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Controls;
using BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for DroneWindow.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {
        BlApi.Ibl bl;
        private DroneListWindow DroneListWindow { get; set; }
        private Parcel parcel { get; set; } = new();
        private Drone Drone { get; set; } = new();
        private bool _close { get; set; } = false;
        private StationWindow Station { get; set; }
        private int Index { get; set; }
        public DroneWindow(BlApi.Ibl ibl, StationWindow station, int id, int index) : this(ibl, null, id)
        {
            Station = station;
            Index = index;
        }

        /// <summary>
        /// Constructor to add a drone
        /// </summary>
        /// <param name="ibl">Access to BL</param>
        /// <param name="droneListWindow">Access to Drone List Window</param>
        public DroneWindow(BlApi.Ibl ibl, DroneListWindow droneListWindow)
        {
            InitializeComponent();
            bl = ibl;
            DroneListWindow = droneListWindow;//access to drone list
            DataContext = Drone;//updating event 
            WeightCmbxAdd.ItemsSource = System.Enum.GetValues(typeof(BO.Enum.WeightCategories));//getting chosen weight
            GridAddDrone.Visibility = Visibility.Visible;//showing grid of fields needed for adding a drone
            NumOfStationTxtAdd.ItemsSource = bl.GetAllStations(index => index.AvailableChargeSlots > 0).Select(s => s.Id);//choosing a station from a combo box
        }

        /// <summary>
        /// Constructor to update a drone
        /// </summary>
        /// <param name="ibl">Access to BL</param>
        /// <param name="droneListWindow">Access to Drone List Window</param>
        public DroneWindow(BlApi.Ibl ibl, DroneListWindow droneListWindow, int idDrone = 0)
        {
            InitializeComponent();
            bl = ibl;
            DroneListWindow = droneListWindow;//access to drone list
            GridUpdateDrone.Visibility = Visibility.Visible;//showing grid of fields needed for updating a drone
            if (idDrone == 0)
                Drone = ibl.GetDrone(DroneListWindow.CurrentDrone.Id);//getting drone with this id
            else
                Drone = ibl.GetDrone(idDrone);//getting drone with this id
            DataContext = Drone;//updating event
            WindowUp();
        }

        /// <summary>
        /// Shows changes in display
        /// </summary>
        private void WindowUp()
        {
            if (Drone.ParcelInTransfer == null)//if its got no parcel assigned to it
            {
                GridParcelInTransfer.Visibility = Visibility.Collapsed;//all the fields linked to parcel are hidden - because are not needed
            }
            if (Drone.DroneStatus == BO.Enum.DroneStatuses.Available)//if drone status is - available
            {
                ChargeDroneUD.Content = "Send Drone to Charging";//input button content
                DroneStatusChangeUD.Content = "Assign drone to a parcel";//input button content
            }

            if (Drone.DroneStatus == BO.Enum.DroneStatuses.Maintenance)//if drone status is - maintanance
            {
                ChargeDroneUD.Content = "Release Drone from Charging";//input button content
                DroneStatusChangeUD.Visibility = Visibility.Hidden;//hiding button uneeded for this status
            }

            else
            {
                //if drone status is - Delivery, and parcel has not yet been delivered 
                if (Drone.DroneStatus == BO.Enum.DroneStatuses.Delivery && Drone.ParcelInTransfer.ParcelState == false)
                {
                    DroneStatusChangeUD.Content = "Drone Collects Parcel";//input button content
                    ChargeDroneUD.Visibility = Visibility.Hidden;//hiding button uneeded for this status
                }
                //if drone status is - Delivery, and parcel has been deliveerd
                if (Drone.DroneStatus == BO.Enum.DroneStatuses.Delivery && Drone.ParcelInTransfer.ParcelState == true)
                {
                    DroneStatusChangeUD.Content = "Drone Delivers Parcel";//input button content
                    ChargeDroneUD.Visibility = Visibility.Hidden;//hiding button uneeded for this status
                }
            }
            if (DroneListWindow != null)
                DroneListWindow.Selection();
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
                        if (Drone.Id == default)
                            throw new MissingInfoException("No Drone ID entered for this drone");
                        if (Drone.Model == default || Drone.Model == null)
                            throw new MissingInfoException("No Model entered for this drone");
                        if (NumOfStationTxtAdd.SelectedItem == null)
                            throw new MissingInfoException("No station ID was entered for this drone");
                        bl.AddDrone(Drone, int.Parse(NumOfStationTxtAdd.Text));
                        DroneListWindow.droneToLists.Add(bl.GetAllDrones().First(x => x.Id == Drone.Id));
                        DroneListWindow.Selection();
                        MessageBox.Show($"SUCCESSFULY ADDED DRONE! \nThe new drone is:\n" + Drone.ToString(), "Successfuly Added",
                          MessageBoxButton.OK);
                        _close = true;
                        this.Close();//closes current window after drone was added
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

        private void Window_closing(object sender, CancelEventArgs e)
        {
            if (!_close)
            {
                e.Cancel = true;
                MessageBox.Show("You can't force the window to close");
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
            string oldName = ModelTxtUD.Text;
            oldName = DroneListWindow.CurrentDrone.Model;
            try
            {
                switch (result1)
                {
                    case MessageBoxResult.OK:
                        bl.UpdateDrone(int.Parse(IDTxtUD.Text), ModelTxtUD.Text);//updating chosen drone
                        DroneListWindow.Selection();
                        IEditableCollectionView items = DroneListWindow.DronesListView.Items as IEditableCollectionView;
                        if (items != null)
                        {
                            items.EditItem(DroneListWindow.CurrentDrone);
                            items.CommitEdit();
                        }
                        MessageBox.Show($"SUCCESSFULY UPDATED DRONE! \n The drones new model name is {ModelTxtUD.Text}", "Successfuly Updated",
                           MessageBoxButton.OK);
                        break;
                    case MessageBoxResult.Cancel:
                        ModelTxtUD.Text = oldName;
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
        }

        /// <summary>
        /// Closes current window
        /// </summary>
        private void CloseWidowButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            _close = true;
            this.Close();
        }

        /// <summary>
        /// Closes current window
        /// </summary>
        private void CancelButtonUD_Click(object sender, RoutedEventArgs e)
        {
            _close = true;
            this.Close();

        }

        /// <summary>
        /// Sorts the different update buttons according to their content and sending to upadate in bl and updating list in pl
        /// </summary>
        private void DroneStatusChangeUD_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((string)DroneStatusChangeUD.Content == "Assign drone to a parcel")
                {
                    var request = MessageBox.Show($"Are you sure you would like to send drone to collect a parcel? \n", "Request Review",
                    MessageBoxButton.OKCancel, MessageBoxImage.Question);
                    switch (request)
                    {
                        case MessageBoxResult.OK:
                            bl.UpdateAssignParcelToDrone(int.Parse(IDTxtUD.Text));//assign parcel to drone - updating in bl 
                            ChargeDroneUD.Visibility = Visibility.Hidden;//hiding uneccessary buttons after update
                            DroneStatusChangeUD.Content = "Drone Collects Parcel";//changing to button content to fit past update
                            MessageBox.Show($"SUCCESSFULY ASIGNED DRONE TO A PARCEL! \n", "Successfuly Updated", MessageBoxButton.OK);
                            GridParcelInTransfer.Visibility = Visibility.Visible;
                            break;
                        case MessageBoxResult.Cancel:
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
                                MessageBox.Show($"DRONE SUCCESSFULY COLLECTED PARCEL! \n", "Successfuly Updated", MessageBoxButton.OK);
                                break;
                            case MessageBoxResult.Cancel:
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
                                    DroneStatusChangeUD.Content = "Assign drone to a parcel";//changing to button content to fit past update
                                    MessageBox.Show($"DRONE SUCCESSFULY DELICVERED PARCEL! \n", "Successfuly Updated", MessageBoxButton.OK);
                                    GridParcelInTransfer.Visibility = Visibility.Collapsed;
                                    break;
                                case MessageBoxResult.Cancel:
                                    break;
                            }
                        }
                    }
                }
                DataContext = bl.GetDrone(Drone.Id);
                if (DroneListWindow != null)
                {
                    IEditableCollectionView items = DroneListWindow.DronesListView.Items as IEditableCollectionView;//רשימת תצוגה
                    if (items != null)
                    {
                        items.EditItem(DroneListWindow.CurrentDrone);
                        items.CommitEdit();
                    }
                    DroneListWindow.Selection();
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
                            MessageBox.Show($"SUCCESSFULY SENT DRONE TO CHARGE! \n", "Successfuly Updated", MessageBoxButton.OK);
                            break;
                        case MessageBoxResult.Cancel:
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
                                if (Station != null)
                                    Station.dronesInCharging.RemoveAt(Index);
                                DroneStatusChangeUD.Visibility = Visibility.Visible;//showing neccessary buttons after update
                                ChargeDroneUD.Content = "Send Drone to Charging";//changing to button content to fit past update
                                DroneStatusChangeUD.Content = "Assign drone to a parcel";//changing to button content to fit past update
                                MessageBox.Show($"SUCCESSFULY RELEASED DRONE FROM CHARGE! \n", "Successfuly Updated",
                                MessageBoxButton.OK);
                                break;
                            case MessageBoxResult.Cancel:
                                break;
                        }
                    }
                }
                DataContext = bl.GetDrone(Drone.Id);
                if (DroneListWindow != null)
                {
                    IEditableCollectionView items = DroneListWindow.DronesListView.Items as IEditableCollectionView;//רשימת תצוגה
                    if (items != null)
                    {
                        items.EditItem(DroneListWindow.CurrentDrone);
                        items.CommitEdit();
                    }
                    DroneListWindow.Selection();
                }
            }
            catch (BO.DroneMaintananceException ex)
            {
                var errorMessage = MessageBox.Show($"ERROR! {ex.ToString()}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ShowMore_Click(object sender, RoutedEventArgs e)
        {
            parcel = bl.GetParcel(int.Parse(IdParcelTxtUD.Text));
            ParcelListWindow windowParcels = new ParcelListWindow(bl);
            windowParcels.CurrentParcel.Id = parcel.Id;
            new ParcelWindow(bl, windowParcels, 0).Show();
        }

        BackgroundWorker worker;
        private void UpdateWidowDrone()
        {
            Drone = bl.GetDrone(Drone.Id);
            DataContext = Drone;
            WindowUp();
            if(DroneListWindow!=null)
            {
                IEditableCollectionView items = DroneListWindow.DronesListView.Items;
                if (items != null)
                {
                    items.EditItem(DroneListWindow.CurrentDrone);
                    items.CommitEdit();
                }
            }
        }
        private void Automatic_Click(object sender, RoutedEventArgs e)
        {
            worker = new() { WorkerReportsProgress = true, WorkerSupportsCancellation = true, };
            //report progress reports changes made in the display
            worker.DoWork += (sender, args) => bl.StartSimulator((int)args.Argument, () => worker.ReportProgress(0), () => worker.CancellationPending);
            worker.ProgressChanged += (sender, args) => UpdateWidowDrone();
            worker.RunWorkerAsync(Drone.Id);//runs the process
        }
        private void Regular_Click(object sender, RoutedEventArgs e)
        {
            worker.CancelAsync();
        }
    }
}