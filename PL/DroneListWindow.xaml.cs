﻿using System;
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
    public struct DroneStatusesAndWeightCategories
    {
        public BO.Enum.WeightCategories Weight { get; set; }
        public BO.Enum.DroneStatuses DroneStatus { get; set; }
    }

    public enum DroneStatuses { Available, Maintenance, Delivery, All };
    public enum WeightCategories { Easy, Medium, Heavy, All };
    /// <summary>
    /// Interaction logic for DronesWindow.xaml
    /// </summary>
    public partial class DroneListWindow : Window
    {
        BlApi.Ibl bl;
        public Dictionary<DroneStatusesAndWeightCategories, List<DroneToList>> droneToLists;
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
            droneToLists = new Dictionary<DroneStatusesAndWeightCategories, List<DroneToList>>();

            droneToLists = (from item in bl.GetAllDrones()
                            group item by
                            new DroneStatusesAndWeightCategories()
                            {
                                Weight = item.Weight,
                                DroneStatus = item.DroneStatus
                            }).ToDictionary(x => x.Key, x => x.ToList());
            DronesListView.ItemsSource = droneToLists.Values.SelectMany(x => x);
            StatusSelection.ItemsSource = System.Enum.GetValues(typeof(DroneStatuses));//enum values of drone status
            WeightSelection.ItemsSource = System.Enum.GetValues(typeof(WeightCategories));//enum values of weight
            StatusSelection.SelectedIndex = 3;//prints full list
        }

        /// <summary>
        /// sorts list by drone status
        /// </summary>
        private void StatusSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Selection();
        }


        ///// <summary>
        ///// sorts list by drone weight
        ///// </summary>
        ///
        private void WeightSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Selection();
        }

        public void Selection()
        {
            DroneStatuses droneStatuses = (DroneStatuses)StatusSelection.SelectedItem;//gets what the user chose to sort by
            if (WeightSelection.SelectedIndex == -1)//if weigh wasnt chosen 
            {
                WeightSelection.SelectedIndex = 3;//show all list 
            }

            WeightCategories weightCategories = (WeightCategories)WeightSelection.SelectedItem;//gets what the user chose to sort by
            DronesListView.ItemsSource = null;
            if (droneStatuses == DroneStatuses.All && weightCategories == WeightCategories.All)//if all was presses for both status and weight
                DronesListView.ItemsSource = from item in droneToLists.Values.SelectMany(x => x)
                                             orderby item.Weight, item.DroneStatus
                                             select item;
            //sorts list by chosen weight
            if (droneStatuses == DroneStatuses.All && weightCategories != WeightCategories.All)
                DronesListView.ItemsSource = droneToLists.Where(item => item.Key.Weight == (BO.Enum.WeightCategories)weightCategories)
            .SelectMany(x => x.Value);
            //sorts list by chosen status
            if (droneStatuses != DroneStatuses.All && weightCategories == WeightCategories.All)
                DronesListView.ItemsSource = droneToLists.Where(item => item.Key.DroneStatus == (BO.Enum.DroneStatuses)droneStatuses)
            .SelectMany(x => x.Value);
            //sorts list by chosen status and weight
            if (droneStatuses != DroneStatuses.All && weightCategories != WeightCategories.All)
                DronesListView.ItemsSource = droneToLists.Where(item => item.Key.Weight == (BO.Enum.WeightCategories)weightCategories && item.Key.DroneStatus == (BO.Enum.DroneStatuses)droneStatuses)
            .SelectMany(x => x.Value);
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
            this.Close();
        }

        /// <summary>
        /// Goes to the constructor of update 
        /// </summary>
        private void DronesListView_SelectionChanged(object sender, MouseButtonEventArgs e)
        {
            CurrentDrone = (DroneToList)DronesListView.SelectedItem;
            if (CurrentDrone != null)
                new DroneWindow(bl, this,0).Show();
        }

        private void window_closeing(object sender, CancelEventArgs e)
        {
            if (!_close)
            {
                e.Cancel = true;
                MessageBox.Show("You can't force the window to close", "ERROR");
            }
        }

        private void Image_MouseEnter(object sender, MouseEventArgs e)
        {
            FrameworkElement framework = sender as FrameworkElement;
            CurrentDrone = framework.DataContext as DroneToList;
        }
    }
}