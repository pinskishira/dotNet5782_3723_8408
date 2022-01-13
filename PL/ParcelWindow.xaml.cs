using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using BO;
using System.ComponentModel;

namespace PL
{
    /// <summary>
    /// Interaction logic for WindowParcel.xaml
    /// </summary>
    public partial class ParcelWindow : Window
    {
        BlApi.Ibl bl;
        private ParcelListWindow ParcelListWindow { get; }
        private Parcel Parcel { get; set; } = new();
        private Customer customer { get; set; } = new();
        private bool _close { get; set; } = false;
        StatusWeightAndPriorities _StatusWeightAndPriorities;
        public ParcelWindow(BlApi.Ibl ibl, int id) : this(ibl, null, id) { }

        /// <summary>
        /// constructer-adds a new Parcel   
        /// </summary>
        /// <param name="bl">the accses to IBL</param>
        /// <param name="_windowParcels">the window with all the Parcel</param>
        public ParcelWindow(BlApi.Ibl ibl, ParcelListWindow parcelListWindow)
        {
            InitializeComponent();
            bl = ibl;
            ParcelListWindow = parcelListWindow;
            Parcel.Sender = new();
            Parcel.Target = new();
            Parcel.DroneParcel = new();
            DataContext = Parcel;//updating event 
            WeightADD.ItemsSource = System.Enum.GetValues(typeof(WeightCategories));
            PriorityADD.ItemsSource = System.Enum.GetValues(typeof(Priorities));
            GridParcelADD.Visibility = Visibility.Visible;
            TargetIDADD.ItemsSource = bl.GetAllCustomers().Select(item => item.Id);
            SenderIDADD.ItemsSource = bl.GetAllCustomers().Select(item => item.Id);
            ParcelButton.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// constructer-updates the drone that the mouse clicked twice on
        /// </summary>
        /// <param name="bl">the accses to IBL</param>
        /// <param name="_windowParcels">the window with all the drones</param>
        /// <param name="i">the diffrence between the constractor of add to the constractor of update</param>
        public ParcelWindow(BlApi.Ibl ibl, ParcelListWindow parcelListWindow, int id = 0)
        {
            bl = ibl;
            InitializeComponent();
            ParcelListWindow = parcelListWindow;
            if (id == 0)
                Parcel = ibl.GetParcel(ParcelListWindow.CurrentParcel.Id);//getting parcel with this id
            else
                Parcel = ibl.GetParcel(id);//getting parcel with this id
            DataContext = Parcel;
            GridParcelUP.Visibility = Visibility.Visible;
            //to connect between the text box and the data
            if (Parcel.Scheduled != null)//if the parcel  has a drone 
            {
                DroneInParcel.Visibility = Visibility.Visible;//show the grid of the parcels drone
            }
        }

        /// <summary>
        /// to only allow to enter int in a text box
        /// </summary>
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        /// <summary>
        /// Not be able to close the window with the x on the top
        /// </summary>
        private void WindowClose(object sender, CancelEventArgs e)
        {
            if (!_close)
            {
                e.Cancel = true;
                MessageBox.Show("You can't force the window to close");
            }
        }

        /// <summary>
        /// Goes to the window of the sender customer
        /// </summary>
        private void SenderButton_click(object sender, RoutedEventArgs e)
        {
            if(bl.IsActive(Parcel.Sender.Id))
                new CustomerWindow(bl, this, Parcel.Sender.Id, Parcel).Show();
            else
                MessageBox.Show("THe customer is deleted!");
        }

        /// <summary>
        /// Goes to the window of the target customer
        /// </summary>
        private void TargetButton_Click(object sender, RoutedEventArgs e)
        {
            if (bl.IsActive(Parcel.Target.Id))
                new CustomerWindow(bl, this, Parcel.Target.Id, Parcel).Show();
            else
                MessageBox.Show("THe customer is deleted!");
        }

        /// <summary>
        /// Button that allows user to add a parcel or in some cases to delete it
        /// </summary>
        private void ParcelButton_Click(object sender, RoutedEventArgs e)
        {
            if ((string)ParcelButton.Content == "Add Parcel")
            {
                var result1 = MessageBox.Show($"Are you sure you would like to add this station? \n", "Request Review",
                 MessageBoxButton.OKCancel, MessageBoxImage.Question);
                try
                {
                    switch (result1)
                    {
                        case MessageBoxResult.OK:
                            if (Parcel.Sender.Id == default)
                                throw new MissingInfoException("No information entered for the sender ID");
                            if (Parcel.Target.Id == default)
                                throw new MissingInfoException("No information entered for the target ID");
                            bl.AddParcel(Parcel);//adding new station to list
                            _StatusWeightAndPriorities.priorities = Parcel.Priority;
                            _StatusWeightAndPriorities.weight = Parcel.Weight;
                            _StatusWeightAndPriorities.status = BO.Enum.ParcelState.Created;
                            int idParcel = bl.GetAllParcels().Last().Id;
                            if (ParcelListWindow.Parcels.ContainsKey(_StatusWeightAndPriorities))
                                ParcelListWindow.Parcels[_StatusWeightAndPriorities].Add(bl.GetAllParcels().First(i => i.Id == idParcel));
                            else
                                ParcelListWindow.Parcels.Add(_StatusWeightAndPriorities, bl.GetAllParcels().Where(i => i.Id == idParcel).ToList());
                            ParcelListWindow.Selection();
                            //adding station to list in the window of stations
                            var result2 = MessageBox.Show($"SUCCESSFULY ADDED PARCEL! \nThe new parcel is:\n" + Parcel.ToString(), "Successfuly Added",
                               MessageBoxButton.OK);
                            switch (result2)
                            {
                                case MessageBoxResult.OK:
                                    _close = true;
                                    this.Close();//closes current window after station was added
                                    break;
                            }
                            break;
                        case MessageBoxResult.Cancel://if user presses cancel
                            Parcel = new();//scrathes fields
                            DataContext = Parcel;//updates event
                            break;
                    }
                }
                catch (FailedToAddException ex)
                {
                    var errorMessage = MessageBox.Show("Failed to add parcel: " + ex.GetType().Name + "\n" + ex.Message, "Failed To Add", MessageBoxButton.OK, MessageBoxImage.Error);
                    switch (errorMessage)
                    {
                        case MessageBoxResult.OK:
                            SenderIDADD.Text = "";
                            TargetIDADD.Text = "";
                            DataContext = Parcel;
                            break;
                    }
                }
                catch (FormatException)
                {
                    var errorMessage = MessageBox.Show("Failed to add parcel: " + "\n" + "You need to enter information for all the given fields", "Failed To Add", MessageBoxButton.OK, MessageBoxImage.Error);
                    switch (errorMessage)
                    {
                        case MessageBoxResult.OK:
                            Parcel = new();
                            DataContext = Parcel;
                            break;
                    }
                }
                catch (MissingInfoException ex)
                {
                    var message = MessageBox.Show("Failed to add the parcel: \n" + ex.Message, "Failed To Add",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    switch (message)
                    {
                        case MessageBoxResult.OK:
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Cancels action
        /// </summary>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            _close = true;
            Close();
        }


        /// <summary>
        /// Cant force window to close with x icon
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
        /// Refreshes Page
        /// </summary>
        public void MyRefresh()
        {
            Parcel = bl.GetParcel(Parcel.Id);
            DataContext = Parcel;
            ParcelListWindow.MyRefresh();
        }
    }
}


