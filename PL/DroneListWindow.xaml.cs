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
public enum DroneStatuses { Available = 1, Maintenance, Delivery, All };
public enum WeightCategories { Easy = 1, Medium, Heavy, All };

namespace PL
{
    /// <summary>
    /// Interaction logic for DronesWindow.xaml
    /// </summary>
    public partial class DroneListWindow : Window
    {
        BL.BL bl;
        public DroneListWindow(BL.BL ibl)
        {
            InitializeComponent();
            bl = ibl;
            DronesListView.ItemsSource = ibl.GetAllDrones();
            StatusSelection.ItemsSource = Enum.GetValues(typeof(DroneStatuses));
            WeightSelection.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            //StatusSelection.SelectedIndex = 4;
            //WeightSelection.SelectedIndex = 4;
        }

        private void StatusSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Selection();
        }

        private void WeightSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Selection();
        }

        private void Selection()
        {
            if ((DroneStatuses)StatusSelection.SelectedItem == null)
                StatusSelection.SelectedItem = 4;
            DroneStatuses droneStatuses = (DroneStatuses)StatusSelection.SelectedItem;
            if ((WeightCategories)WeightSelection.SelectedItem == null)
                WeightSelection.SelectedItem = 4;
            WeightCategories weightCategories = (WeightCategories)WeightSelection.SelectedItem;
            if (droneStatuses == DroneStatuses.All && weightCategories == WeightCategories.All)
                DronesListView.ItemsSource = bl.GetAllDrones();
            if (StatusSelection.SelectedItem != null && weightCategories == WeightCategories.All)
                DronesListView.ItemsSource = bl.GetAllDrones(item => item.DroneStatus == (IBL.BO.Enum.DroneStatuses)StatusSelection.SelectedItem);
            if (droneStatuses == DroneStatuses.All && WeightSelection.SelectedItem != null)
                DronesListView.ItemsSource = bl.GetAllDrones(item => item.Weight == (IBL.BO.Enum.WeightCategories)WeightSelection.SelectedItem);
            if (StatusSelection.SelectedItem == null && WeightSelection.SelectedItem != null)
                    DronesListView.ItemsSource = bl.GetAllDrones(item => item.Weight == (IBL.BO.Enum.WeightCategories)WeightSelection.SelectedItem);
            if (StatusSelection.SelectedItem != null && WeightSelection.SelectedItem == null)
                DronesListView.ItemsSource = bl.GetAllDrones(item => item.DroneStatus == (IBL.BO.Enum.DroneStatuses)StatusSelection.SelectedItem);
            if (StatusSelection.SelectedItem != null && WeightSelection.SelectedItem != null)
                DronesListView.ItemsSource = bl.GetAllDrones(item => item.DroneStatus == (IBL.BO.Enum.DroneStatuses)StatusSelection.SelectedItem &&
                  item.Weight == (IBL.BO.Enum.WeightCategories)WeightSelection.SelectedItem);


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new DroneWindow(bl).Show();
            

        }

        private void DronesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IBL.BO.DroneToList droneToList = (IBL.BO.DroneToList)DronesListView.SelectedItem;
            new DroneWindow(bl, droneToList).Show();
        }
    }
}