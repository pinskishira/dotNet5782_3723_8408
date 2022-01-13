using BO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for CustomerWindow.xaml
    /// </summary>
    public partial class CustomerWindow : Window
    {
        BlApi.Ibl bl;
        private CustomerListWindow CustomerListWindow { get; }
        private Customer Customer { get; set; } = new();
        private bool _close { get; set; } = false;
        public ObservableCollection<ParcelAtCustomer> parcelsFromCustomers { get; set; } = new();
        public ObservableCollection<ParcelAtCustomer> parcelsToCustomers { get; set; } = new();
        public ParcelWindow parcelWindow { get; set; }

        public CustomerWindow(BlApi.Ibl ibl, ParcelWindow _parcelWindow, int id,Parcel parcel) : this(ibl, null, id, false)
        {
            parcelWindow = _parcelWindow;
        }

        /// <summary>
        /// Constructor to add a customer
        /// </summary>
        /// <param name="ibl">Access to BL</param>
        /// <param name="customerListWindow">Access to Customer List Window</param>
        public CustomerWindow(BlApi.Ibl ibl, CustomerListWindow customerListWindow)
        {
            InitializeComponent();
            bl = ibl;
            CustomerListWindow = customerListWindow;//access to customer list
            Customer.CustomerLocation = new();
            DataContext = Customer;//updating event 
            GridCustomerBoth.Visibility = Visibility.Visible;
            GridCustomerADD.Visibility = Visibility.Visible;//showing grid of fields needed for adding a customer
        }

        /// <summary>
        /// constructor to update customer
        /// </summary>
        /// <param name="ibl">Access to BL</param>
        /// <param name="customerListWindow">Access to Customer List Window</param>
        /// <param name="id">temp</param>
        public CustomerWindow(BlApi.Ibl ibl, CustomerListWindow customerListWindow, int id = 0, bool flagt=false)
        {
            
            InitializeComponent();
            bl = ibl;
            CustomerListWindow = customerListWindow;//access to station list
            if (id == 0)
                Customer = bl.GetCustomer(customerListWindow.CurrentCustomer.Id);
            else
                Customer = bl.GetCustomer(id);//getting station with this id
            CustomerButton.Content = "Update Customer";
            GridCustomerBoth.Visibility = Visibility.Visible;
            GridCustomerUP.Visibility = Visibility.Visible;//showing grid of fields needed for updating a staion
            if (flagt == true)
                sighparcel.Visibility = Visibility.Visible;
            DataContext = Customer;//updating event
            if (Customer.ParcelsFromCustomers.Any())
                ShowParcelsFromCustomer.Visibility = Visibility.Visible;
            if (Customer.ParcelsToCustomers.Any())
                ShowParcelsToCustomer.Visibility = Visibility.Visible;
            foreach (var item in Customer.ParcelsFromCustomers)
            {
                parcelsFromCustomers.Add(item);
            }
            foreach (var item in Customer.ParcelsToCustomers)
            {
                parcelsToCustomers.Add(item);
            }
            ViewParcelsFromCustomer.ItemsSource = parcelsFromCustomers;
            ViewParcelsToCustomer.ItemsSource = parcelsToCustomers;
        }

        /// <summary>
        /// Button that allows user to add or update a customer according to his choice
        /// </summary>
        private void CustomerButtonUD_Click(object sender, RoutedEventArgs e)
        {
            if ((string)CustomerButton.Content == "Add Customer")
            {
                var result1 = MessageBox.Show($"Are you sure you would like to add this customer? \n", "Request Review",
            MessageBoxButton.OKCancel, MessageBoxImage.Question);
                try
                {
                    switch (result1)
                    {
                        case MessageBoxResult.OK:
                            if (Customer.Id == default)
                                throw new MissingInfoException("No station ID entered for this customer");
                            if (Customer.Name == default || Customer.Name == null)
                                throw new MissingInfoException("No name entered for this customer");
                            if (Customer.Phone == default)
                                throw new MissingInfoException("No phone was entered for this station");
                            if (Customer.CustomerLocation.Longitude == default || Customer.CustomerLocation.Latitude == default)
                                throw new MissingInfoException("No location was entered for this customer");
                            bl.AddCustomer(Customer);//adding new station to list
                                                     //adding station to list in the window of stations
                            CustomerListWindow.customerToLists.Add(bl.GetAllCustomers().ToList().Find(item => item.Id == int.Parse(IdTxtADD.Text)));
                            var result2 = MessageBox.Show($"SUCCESSFULY ADDED CUSTOMER! \nThe new customer is:\n" + Customer.ToString(), "Successfuly Added",
                               MessageBoxButton.OK);
                            switch (result2)
                            {
                                case MessageBoxResult.OK:
                                    _close = true;
                                    this.Close();//closes current window after customer was added
                                    break;
                            }
                            break;
                        case MessageBoxResult.Cancel://if user presses cancel
                            Customer = new();//scrathes fields
                            DataContext = Customer;//updates event
                            break;
                    }
                }
                catch (FailedToAddException ex)
                {
                    var errorMessage = MessageBox.Show("Failed to add customer: " + ex.GetType().Name + "\n" + ex.Message, "Failed To Add", MessageBoxButton.OK, MessageBoxImage.Error);
                    switch (errorMessage)
                    {
                        case MessageBoxResult.OK:
                            Customer = new();//scrathes fields
                            DataContext = Customer;//updates event
                            break;
                    }
                }
                catch (InvalidInputException ex)
                {
                    var errorMessage = MessageBox.Show("Failed to add customer: " + "\n" + ex.Message, "Failed To Add", MessageBoxButton.OK, MessageBoxImage.Error);
                    switch (errorMessage)
                    {
                        case MessageBoxResult.OK:
                            IdTxtADD.Text = "";
                            break;
                    }
                }
                catch (FormatException)
                {
                    var errorMessage = MessageBox.Show("Failed to add customer: " + "\n" + "You need to enter information for all the given fields", "Failed To Add", MessageBoxButton.OK, MessageBoxImage.Error);
                    switch (errorMessage)
                    {
                        case MessageBoxResult.OK:
                            break;
                    }
                }
                catch (MissingInfoException ex)
                {
                    var message = MessageBox.Show("Failed to add the customer: \n" + ex.Message, "Failed To Add",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    switch (message)
                    {
                        case MessageBoxResult.OK:
                            break;
                    }
                }
            }
            if ((string)CustomerButton.Content == "Update Customer")
            {
                var result1 = MessageBox.Show($"Are you sure you would like to update this customer? \n", "Request Review",
              MessageBoxButton.OKCancel, MessageBoxImage.Question);
                try
                {
                    switch (result1)
                    {
                        case MessageBoxResult.OK:
                            bl.UpdateCustomer(int.Parse(IdTxtUP.Text), NameTxt.Text, PhoneTxt.Text);//udating chosen station
                            if (CustomerListWindow != null)
                            {
                                int i = CustomerListWindow.CustomerListView.SelectedIndex;
                                if (i == -1)
                                    CustomerListWindow.MyRefresh();
                                else
                                {
                                    CustomerListWindow.CurrentCustomer.Name = NameTxt.Text;//updating drone name
                                    CustomerListWindow.CurrentCustomer.Phone = PhoneTxt.Text;
                                    CustomerListWindow.customerToLists[i] = CustomerListWindow.CurrentCustomer;//updating event
                                    CustomerListWindow.CustomerListView.SelectedIndex = i;
                                }
                            }
                            else
                                parcelWindow.MyRefresh();
                            var result2 = MessageBox.Show($"SUCCESSFULY UPDATED CUSTOMER! \n The customers new name is {NameTxt.Text}, and new phone is {PhoneTxt.Text}", "Successfuly Updated",
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
                    var errorMessage = MessageBox.Show("Failed to update customer: " + ex.GetType().Name + "\n" + ex.Message, "Failed Warning", MessageBoxButton.OK, MessageBoxImage.Error);
                    switch (errorMessage)
                    {
                        case MessageBoxResult.OK:
                            NameTxt.Text = "";
                            PhoneTxt.Text = "";
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Function that doesnt allow to insert the alphabet, only numbers
        /// </summary>
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        /// <summary>
        /// Closing window
        /// </summary>
        private void CloseWindowButtonAdd_Click(object sender, RoutedEventArgs e)
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
                MessageBox.Show("You can't force the window to close");
            }
        }

        /// <summary>
        /// Closes window
        /// </summary>
        private void CancelButtonUD_Click(object sender, RoutedEventArgs e)
        {
            _close = true;
            Close();
        }

        /// <summary>
        /// If customer has Parcels To Customer, shows them
        /// </summary>
        private void ShowParcelsToCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (ViewParcelsToCustomer.Visibility == Visibility.Collapsed)
                ViewParcelsToCustomer.Visibility = Visibility.Visible;
            else
                ViewParcelsToCustomer.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// If customer has Parcels From Customer, shows them
        /// </summary>
        private void ShowParcelsFromCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (ViewParcelsFromCustomer.Visibility == Visibility.Collapsed)
                ViewParcelsFromCustomer.Visibility = Visibility.Visible;
            else
                ViewParcelsFromCustomer.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Double Click on a parcel and you can update it
        /// </summary>
        private void ViewParcelsFromCustomer_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ParcelAtCustomer parcelAtCustomer;
            parcelAtCustomer = (ParcelAtCustomer)ViewParcelsFromCustomer.SelectedItem;
            new ParcelWindow(bl, parcelAtCustomer.Id).Show();
        }


        /// <summary>
        /// Double Click on a parcel and you can update it
        /// </summary>
        private void ViewParcelsToCustomer_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ParcelAtCustomer parcelAtCustomer;
            parcelAtCustomer = (ParcelAtCustomer)ViewParcelsToCustomer.SelectedItem;
            new ParcelWindow(bl, parcelAtCustomer.Id).Show();
        }

        private void sighparcel_Click(object sender, RoutedEventArgs e)
        {
            ParcelListWindow parcelListWindow = new ParcelListWindow(bl);
            new ParcelWindow(bl, parcelListWindow).Show();
        }
    }

}
