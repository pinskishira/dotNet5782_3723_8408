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

namespace PL
{
    /// <summary>
    /// Interaction logic for CustomerListWindow.xaml
    /// </summary>
    public partial class CustomerListWindow : Window
    {
        BlApi.Ibl bl;
        //  public ObservableCollection<IGrouping<BO.Enum.DroneStatuses, DroneToList>> droneToLists;
        public ObservableCollection<CustomerToList> customerToList;

        public CustomerToList CurrentDrone { get; set; } = new();
        private bool _close { get; set; } = false;

        /// <summary>
        /// Initializes the list of all the drones
        /// </summary>
        /// <param name="ibl">From type bl</param>
        public CustomerListWindow(BlApi.Ibl ibl)
        {
            InitializeComponent();
            bl = ibl;
            customerToList = new ObservableCollection<CustomerToList>();
            List<CustomerToList> tempCustomerToList = bl.GetAllCustomers().ToList();//getting list of drones from bl

            foreach (var indexOfCustomerToList in tempCustomerToList)//going through list and inserting it into drone to list of type ObservableCollection
            {
                customerToList.Add(indexOfCustomerToList);
            }
            CustomerListView.ItemsSource = customerToList;
        }

        /// <summary>
        /// sends to add constructor, which adds drone
        /// </summary>
        //private void AddDroneButton_Click(object sender, RoutedEventArgs e)
        //{
        //    new DroneWindow(bl, this, 0).Show();
        //}

        /// <summary>
        /// closes current window
        /// </summary>
        private void CloseWindowButton_Click(object sender, RoutedEventArgs e)
        {
            _close = true;
            this.Close();
        }

        /// <summary>
        /// Goes to the constructor of update 
        /// </summary>
        //private void DronesListView_SelectionChanged(object sender, MouseButtonEventArgs e)
        //{
        //    CurrentDrone = (DroneToList)DronesListView.SelectedItem;
        //    if (CurrentDrone != null)
        //        new DroneWindow(bl, this).Show();
        //}

        private void window_closeing(object sender, CancelEventArgs e)
        {
            if (!_close)
            {
                e.Cancel = true;
                MessageBox.Show("You can't force the window to close", "ERROR");
            }
        }

        /// <summary>
        /// Goes to the constructor of update 
        /// </summary>
        private void CustomerListView_SelectionChanged(object sender, MouseButtonEventArgs e)
        {
            //CurrentDrone = (DroneToList)StationListView.SelectedItem;
            //if (CurrentDrone != null)
            //    new DroneWindow(bl, this).Show();
        }

    }
}
