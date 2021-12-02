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
        BL.BL bl;
        public DroneWindow(BL.BL ibl, IBL.BO.Drone updateDrone)
        {
            InitializeComponent();
            bl = ibl;

        }
        public DroneWindow(BL.BL ibl)
        {
            InitializeComponent();
            bl = ibl;
            ParcelInTranferTextBox.Visibility = Visibility.Hidden;
            ParcelInTransferLabel.Visibility = Visibility.Hidden;
            LatitudeLabel.Visibility = Visibility.Hidden;
            LongitutdeLabel.Visibility = Visibility.Hidden;
            LatitudeTextBox.Visibility = Visibility.Hidden;
            LongitutdeTextBox.Visibility = Visibility.Hidden;
            LocationLabel.Visibility = Visibility.Hidden;
            BatteryLabel.Visibility = Visibility.Hidden;
            BatteryTextBox.Visibility = Visibility.Hidden;
            LongitutdeTextBox.Visibility = Visibility.Hidden;


        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            IBL.BO.Drone newDrone = new();
            newDrone.Id = int.Parse(IDTextBox.Text);
            newDrone.Model = ModelTextBox.Text;
            newDrone.Weight = (IBL.BO.Enum.WeightCategories)WeightComboBox.SelectedItem;
            bl.AddDrone(newDrone, int.Parse(NumOfStationTextBox.Text));
        }
    }
}
