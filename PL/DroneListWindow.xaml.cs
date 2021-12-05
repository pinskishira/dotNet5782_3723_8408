
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
//using IBL;
using IBL.BO;
using static IBL.BO.Enum;

namespace PL
{

    public enum DroneStatuses { Available, Maintenance, Delivery, All };
    public enum WeightCategories { Easy, Medium, Heavy, All };
    /// <summary>
    /// Interaction logic for DronesWindow.xaml
    /// </summary>
    public partial class DroneListWindow : Window
    {
        BL.BL bl;
        public ObservableCollection<DroneToList> droneToLists;
        public DroneListWindow(BL.BL ibl)
        {
            InitializeComponent();
            bl = ibl;
            droneToLists = new ObservableCollection<DroneToList>();
            List<DroneToList> tempDroneToList = bl.GetAllDrones().ToList();
            foreach (var indexOfDroneToList in tempDroneToList)
            {
                droneToLists.Add(indexOfDroneToList);
            }
            DronesListView.ItemsSource = droneToLists;
            StatusSelection.ItemsSource = System.Enum.GetValues(typeof(DroneStatuses));
            WeightSelection.ItemsSource = System.Enum.GetValues(typeof(WeightCategories));
            StatusSelection.SelectedIndex = 3;
            droneToLists.CollectionChanged += DroneToLists_CollectionChanged;
        }

        private void DroneToLists_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Selection();
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
            DroneStatuses droneStatuses = (DroneStatuses)StatusSelection.SelectedItem;
            if (WeightSelection.SelectedIndex == -1)
            {
                WeightSelection.SelectedIndex = 3;
            }
            WeightCategories weightCategories = (WeightCategories)WeightSelection.SelectedItem;
            DronesListView.ItemsSource = null;
            if (droneStatuses == DroneStatuses.All && weightCategories == WeightCategories.All)
                DronesListView.ItemsSource = droneToLists;
            if (droneStatuses == DroneStatuses.All && weightCategories != WeightCategories.All)
                DronesListView.ItemsSource = droneToLists.ToList().FindAll(item => item.Weight == (IBL.BO.Enum.WeightCategories)WeightSelection.SelectedItem);
            if (droneStatuses != DroneStatuses.All && weightCategories == WeightCategories.All)
                DronesListView.ItemsSource = droneToLists.ToList().FindAll(item => item.DroneStatus == (IBL.BO.Enum.DroneStatuses)StatusSelection.SelectedItem);
            if (droneStatuses != DroneStatuses.All && weightCategories != WeightCategories.All)
                DronesListView.ItemsSource = droneToLists.ToList().FindAll(item => item.DroneStatus == (IBL.BO.Enum.DroneStatuses)StatusSelection.SelectedItem &&
                  item.Weight == (IBL.BO.Enum.WeightCategories)WeightSelection.SelectedItem);
        }

        private void AddDroneButton_Click(object sender, RoutedEventArgs e)
        {
            new DroneWindow(bl,this, 5).Show();
        }

        private void DronesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            new DroneWindow(bl ,this).Show();
        }

        private void CloseWindowButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}