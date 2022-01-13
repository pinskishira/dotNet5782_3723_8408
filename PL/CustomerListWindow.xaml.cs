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

        /// <summary>
        /// Refreshes list
        /// </summary>
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
            Close();
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
        /// Goes to the constructor of update 
        /// </summary>
        private void CustomerListView_SelectionChanged(object sender, MouseButtonEventArgs e)
        {
            CurrentCustomer = (CustomerToList)CustomerListView.SelectedItem;
            if (CurrentCustomer != null)
                new CustomerWindow(bl, this, 0, false).Show();
        }

        /// <summary>
        /// Closes window
        /// </summary>
        private void CloseWindowCustomer_Click(object sender, RoutedEventArgs e)
        {
            _close = true;
            Close();
        }

        /// <summary>
        /// Goes to constructor off add
        /// </summary>
        private void AddCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            new CustomerWindow(bl, this).Show();
        }

        /// <summary>
        /// Refreshes data
        /// </summary>
        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            customerToLists.OrderBy(customer => customer.Id);
        }

        /// <summary>
        /// Refreshes data
        /// </summary>
        public void MyRefresh()
        {
            customerToLists.OrderBy(customer => customer.Id);
            CustomerListView.Items.Refresh();
        }

        /// <summary>
        /// Allows user to delete customer using an icon 
        /// </summary>
        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var result1 = MessageBox.Show($"Are you sure you would like to delete this customer? \n", "Request Review",
            MessageBoxButton.OKCancel, MessageBoxImage.Question);
            switch (result1)
            {
                case MessageBoxResult.OK:
                    FrameworkElement framework = sender as FrameworkElement;
                    CurrentCustomer = framework.DataContext as CustomerToList;
                    bl.DeleteCustomer(CurrentCustomer.Id);
                    customerToLists.Remove(CurrentCustomer);
                    MyRefresh();
                    break;
                case MessageBoxResult.Cancel:
                    break;
            }
        }
    }
}
