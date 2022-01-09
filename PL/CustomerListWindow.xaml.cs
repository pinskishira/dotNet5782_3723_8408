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
        public ObservableCollection<CustomerToList> customerToLists;

        public CustomerToList CurrentCustomer { get; set; } = new();
        private bool _close { get; set; } = false;

        /// <summary>
        /// Initializes the list of all the drones
        /// </summary>
        /// <param name="ibl">From type bl</param>
        public CustomerListWindow(BlApi.Ibl ibl)
        {
            InitializeComponent();
            bl = ibl;
            customerToLists = new ObservableCollection<CustomerToList>();
            List<CustomerToList> tempCustomerToList = bl.GetAllCustomers().ToList();//getting list of drones from bl

            foreach (var indexOfCustomerToList in tempCustomerToList)//going through list and inserting it into drone to list of type ObservableCollection
            {
                customerToLists.Add(indexOfCustomerToList);
            }
            CustomerListView.ItemsSource = customerToLists;
            customerToLists.CollectionChanged += CustomerToLists_CollectionChanged;
        }

        private void CustomerToLists_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            CustomerListView.Items.Refresh();
        }

        /// <summary>
        /// closes current window
        /// </summary>
        private void CloseWindowButton_Click(object sender, RoutedEventArgs e)
        {
            _close = true;
            this.Close();
        }

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
            CurrentCustomer = (CustomerToList)CustomerListView.SelectedItem;
            if (CurrentCustomer != null)
                new CustomerWindow(bl, this,0).Show();
        }

        private void CloseWindowCustomer_Click(object sender, RoutedEventArgs e)
        {
            _close = true;
            this.Close();
        }

        private void AddCustomerButton_Click(object sender, RoutedEventArgs e)
        {
           new CustomerWindow(bl, this).Show();
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            customerToLists.OrderBy(customer => customer.Id);
        }

        public void MyRefresh()
        {
            customerToLists.OrderBy(customer => customer.Id);
            CustomerListView.Items.Refresh();
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement framework = sender as FrameworkElement;
            CurrentCustomer= framework.DataContext as CustomerToList;
            bl.DeleteCustomer(CurrentCustomer.Id);
            customerToLists.Remove(CurrentCustomer);
            MyRefresh();
        }
    }
}
