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
            if (StatusSelection.SelectedItem == null && WeightSelection.SelectedItem != null)
                DronesListView.ItemsSource = bl.GetAllDrones(item => item.Weight == (WeightCategories)WeightSelection.SelectedItem);
            if (StatusSelection.SelectedItem != null && WeightSelection.SelectedItem == null)
                DronesListView.ItemsSource = bl.GetAllDrones(item => item.DroneStatus == (DroneStatuses)StatusSelection.SelectedItem);
            if (StatusSelection.SelectedItem != null && WeightSelection.SelectedItem != null)
                DronesListView.ItemsSource = bl.GetAllDrones(item => item.DroneStatus == (DroneStatuses)StatusSelection.SelectedItem && 
                  item.Weight == (WeightCategories)WeightSelection.SelectedItem);

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new DroneWindow(bl).Show();
        }
    }
}
