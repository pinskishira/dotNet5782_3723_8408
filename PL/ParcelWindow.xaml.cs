﻿using System;
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
        private bool _close { get; set; } = false;
        StatusWeightAndPriorities _StatusWeightAndPriorities;
        /// <summary>
        /// constructer-adds a new Parcel   
        /// </summary>
        /// <param name="bl">the accses to IBL</param>
        /// <param name="_windowParcels">the window with all the Parcel</param>
        public ParcelWindow(BlApi.Ibl ibl, ParcelListWindow parcelListWindow, int i = 0)
        {
            InitializeComponent();
            bl=ibl;
            ParcelListWindow = parcelListWindow;
            Parcel.Sender = new();
            Parcel.Target = new();
            Parcel.DroneParcel = new();
            DataContext = Parcel;//updating event 
            WeightADD.ItemsSource = System.Enum.GetValues(typeof(WeightCategories));
            PriorityADD.ItemsSource = System.Enum.GetValues(typeof(Priorities));
            GridParcelADD.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// constructer-updates the drone that the mouse clicked twice on
        /// </summary>
        /// <param name="bl">the accses to IBL</param>
        /// <param name="_windowParcels">the window with all the drones</param>
        /// <param name="i">the diffrence between the constractor of add to the constractor of update</param>
        public ParcelWindow(BlApi.Ibl ibl, ParcelListWindow parcelListWindow)
        {
            ibl = bl;
            InitializeComponent();
            ParcelListWindow = parcelListWindow;
            DataContext = Parcel;
            GridParcelUP.Visibility = Visibility.Visible;
           //to connect between the text box and the data
            ParcelButton.Visibility = Visibility.Collapsed;
            //if (Parcel.Scheduled == null)
            //{
            //    ParcelButton.Content = "Delete the parcel";
            //    ParcelButton.Visibility = Visibility.Visible;
            //}
            if (Parcel.Scheduled != null)//if the parcel  has a drone 
            {
                if (Parcel.Delivered == null && Parcel.PickedUp == null)
                {
                    ParcelButton.Content = "Pick Up Parcel ";
                }
                if (Parcel.Delivered == null && Parcel.PickedUp != null)
                {
                    ParcelButton.Content = "Deliver Parcel";
                }
                ParcelButton.Visibility = Visibility.Visible;
                DroneInParcel.Visibility = Visibility.Visible;//show the grid of the parcels drone
            }
        }

        /// <summary>
        /// to only allow to enter int in a text box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        /// <summary>
        /// to not be able to close the window with the x on the top
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WindowClose(object sender, CancelEventArgs e)
        {
            if (!_close)
            {
                e.Cancel = true;
                MessageBox.Show("You can't force the window to close");
            }
        }

        Customer _sender = new();
        private void SenderButton_click(object sender, RoutedEventArgs e)
        {
            sender = bl.GetCustomer(int.Parse(SenderIDBlockA.Text));
            CustomerListWindow windowCustomers = new CustomerListWindow(bl);
            windowCustomers.CurrentCustomer.Id = _sender.Id;
            new CustomerWindow(bl, windowCustomers, 0).Show();
        }

        Customer _target= new();
        private void TargetButton_Click(object sender, RoutedEventArgs e)
        {
            _target = bl.GetCustomer(int.Parse(TargetIDBlockA.Text));
            CustomerListWindow windowCustomers = new CustomerListWindow(bl);
            windowCustomers.CurrentCustomer.Id = _target.Id;
            new CustomerWindow(bl, windowCustomers, 0).Show();
        }

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
                            if (Parcel.Id == default)
                                throw new MissingInfoException("No information entered for the ID");
                            if (Parcel.Sender.Name ==default)
                                throw new MissingInfoException("No information entered for the sender ID");
                            if (Parcel.Target.Name == default)
                                throw new MissingInfoException("No information entered for the target ID");
                            bl.AddParcel(Parcel);//adding new station to list
                            _StatusWeightAndPriorities.priorities = Parcel.Priority;
                            _StatusWeightAndPriorities.weight = Parcel.Weight;
                            _StatusWeightAndPriorities.status = BO.Enum.ParcelState.Created;
                            //Parcel = bl.GetParcel(Parcel.Id);
                            if (ParcelListWindow.Parcels.ContainsKey(_StatusWeightAndPriorities))
                                ParcelListWindow.Parcels[_StatusWeightAndPriorities].Add(bl.GetAllParcels().First(i => i.Id == Parcel.Id));
                            else
                                ParcelListWindow.Parcels.Add(_StatusWeightAndPriorities, bl.GetAllParcels().Where(i => i.Id == Parcel.Id).ToList());
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
                            IdTxtADD.Text = "";
                            SenderIDADD.Text = "";
                            TargetIDADD.Text = "";
                            DataContext = Parcel;
                            break;
                    }
                }
                catch (InvalidInputException ex)
                {
                    var errorMessage = MessageBox.Show("Failed to add parcel: " + "\n" + ex.Message, "Failed To Add", MessageBoxButton.OK, MessageBoxImage.Error);
                    switch (errorMessage)
                    {
                        case MessageBoxResult.OK:
                            IdTxtADD.Text = "";
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
            if ((string)ParcelButton.Content == "Pick Up Parcel")
            {
                var result1 = MessageBox.Show($"Are you sure you would like to update that this parcel has been picked up? \n", "Request Review",
               MessageBoxButton.OKCancel, MessageBoxImage.Question);
                try
                {
                    switch (result1)
                    {
                        case MessageBoxResult.OK:
                            bl.UpdateParcelCollectionByDrone(Parcel.DroneParcel.Id);
                            var result2 = MessageBox.Show($"SUCCESSFULY UPDATED PARCEL! \n", "Successfuly Updated", MessageBoxButton.OK);
                            switch (result2)
                            {
                                case MessageBoxResult.OK:
                                    _close = true;
                                    this.Close();//closes current window after station was added
                                    break;
                            }
                            break;
                    }
                }
                catch (FailedToCollectParcelException ex)
                {
                    var errorMessage = MessageBox.Show("Failed to update parcel: " + ex.GetType().Name + "\n" + ex.Message, "Failed Warning", MessageBoxButton.OK, MessageBoxImage.Error);
                    switch (errorMessage)
                    {
                        case MessageBoxResult.OK:
                            _close = true;
                            this.Close();//closes current window after station was added
                            break;
                    }
                }
            }
            if((string)ParcelButton.Content == "Deliver Parcel")
            {
                var result1 = MessageBox.Show($"Are you sure you would like to update that this parcel has been picked up? \n", "Request Review",
              MessageBoxButton.OKCancel, MessageBoxImage.Question);
                try
                {
                    switch (result1)
                    {
                        case MessageBoxResult.OK:
                            bl.UpdateParcelDeliveryToCustomer(Parcel.DroneParcel.Id);
                            var result2 = MessageBox.Show($"SUCCESSFULY UPDATED PARCEL! \n", "Successfuly Updated", MessageBoxButton.OK);
                            switch (result2)
                            {
                                case MessageBoxResult.OK:
                                    _close = true;
                                    this.Close();
                                    break;
                            }
                            break;
                    }
                }
                catch (ParcelDeliveryException ex)
                {
                    var errorMessage = MessageBox.Show("Failed to update parcel: " + ex.GetType().Name + "\n" + ex.Message, "Failed Warning", MessageBoxButton.OK, MessageBoxImage.Error);
                    switch (errorMessage)
                    {
                        case MessageBoxResult.OK:
                            _close = true;
                            this.Close();
                            break;
                    }
                }
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            _close = true;
            Close();
        }
    }
}


