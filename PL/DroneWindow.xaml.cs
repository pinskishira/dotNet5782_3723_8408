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

namespace PL
{
    /// <summary>
    /// Interaction logic for DroneWindow.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {
        public DroneWindow(BL.BL ibl)
        {
            InitializeComponent();
        }
        public DroneWindow()
        {
            InitializeComponent();
            DroneWindow viewDrone = new DroneWindow();
            IBL.BO.Drone newDrone = new();
            newDrone.Id= 

        }

        private void IDLabel_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ModelLabel_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void WeightLabel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void BatteryLabel_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void StatusLable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
