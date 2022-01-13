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
using BO;
using static BO.Enum;

namespace PL
{
    public enum DroneStatuses { Available, Maintenance, Delivery, All };
    public enum WeightCategories { Easy, Medium, Heavy, All };
    /// <summary>
    /// Interaction logic for DronesWindow.xaml
    /// </summary>
    public partial class DroneListWindow : Window
    {
        BlApi.Ibl bl;
        public ObservableCollection<DroneToList> droneToLists;
        public DroneToList CurrentDrone { get; set; } = new();
        private bool _close { get; set; } = false;

        /// <summary>
        /// Initializes the list of all the drones
        /// </summary>
        /// <param name="ibl">From type bl</param>
        public DroneListWindow(BlApi.Ibl ibl)
        {
            InitializeComponent();
            bl = ibl;

            IEnumerable<DroneToList> tempDroneToList = bl.GetAllDrones();
            droneToLists = new();
            foreach (var drones in tempDroneToList)
            {
                droneToLists.Add(drones);
            }

            DronesListView.ItemsSource = droneToLists;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(DronesListView.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("DroneStatus");
            view.GroupDescriptions.Add(groupDescription);

            StatusSelection.ItemsSource = System.Enum.GetValues(typeof(DroneStatuses));//enum values of drone status
            StatusSelection.SelectedIndex = 3;//prints full list
        }

        /// <summary>
        /// sorts list by drone status
        /// </summary>
        private void StatusSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Selection();
        }

        /// <summary>
        /// Sorts list by status
        /// </summary>
        public void Selection()
        {
            DroneStatuses droneStatuses = (DroneStatuses)StatusSelection.SelectedItem;//gets what the user chose to sort by
            DronesListView.ItemsSource = null;
            if (droneStatuses == DroneStatuses.All)//if presses for all
                DronesListView.ItemsSource = droneToLists;
            //sorts list by chosen status
            if (droneStatuses != DroneStatuses.All)
                DronesListView.ItemsSource = droneToLists.Where(item => item.DroneStatus == (BO.Enum.DroneStatuses)droneStatuses);

        }
        /// <summary>
        /// sends to add constructor, which adds drone
        /// </summary>
        private void AddDroneButton_Click(object sender, RoutedEventArgs e)
        {
            new DroneWindow(bl, this).Show();
        }

        /// <summary>
        /// closes current window
        /// </summary>
        private void CloseWindowButton_Click(object sender, RoutedEventArgs e)
        {
            _close = true;
            Close();
        }

        /// <summary>
        /// Goes to the constructor of update 
        /// </summary>
        private void DronesListView_SelectionChanged(object sender, MouseButtonEventArgs e)
        {
            CurrentDrone = (DroneToList)DronesListView.SelectedItem;
            if (CurrentDrone != null)
                new DroneWindow(bl, this, 0).Show();
        }

        /// <summary>
        /// Cant press closing icon
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
        /// Allows user to delete drone using an icon 
        /// </summary>
        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (CurrentDrone.DroneStatus != BO.Enum.DroneStatuses.Delivery)
            {
                var result1 = MessageBox.Show($"Are you sure you would like to delete this drone? \n", "Request Review",
            MessageBoxButton.OKCancel, MessageBoxImage.Question);
                switch (result1)
                {
                    case MessageBoxResult.OK:
                        FrameworkElement framework = sender as FrameworkElement;
                        CurrentDrone = framework.DataContext as DroneToList;
                        bl.DeleteDrone(CurrentDrone.Id);
                        droneToLists.Remove(CurrentDrone);
                        Selection();
                        break;
                    case MessageBoxResult.Cancel:
                        break;
                }
            }
            else
                MessageBox.Show("Cannot delete drone because it is in the middle of performing a delivery.\n", "CANNOT DELETE", MessageBoxButton.OK);
        }
    }
}