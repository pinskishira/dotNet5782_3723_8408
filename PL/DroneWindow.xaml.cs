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
    /// Interaction logic for DroneWindow.xaml
    /// </summary>

    public partial class DroneWindow : Window
    {
        BL.BL bl;
        public DroneWindow(BL.BL ibl, IBL.BO.DroneToList droneToList)
        {
            InitializeComponent();
            bl = ibl;
            GridUpdateDrone.Visibility = Visibility.Visible;
            IDTextBoxUD.Text = droneToList.Id.ToString();
            WeightTextBoxUD.Text = droneToList.Weight.ToString();
            BatteryTextBoxUD.Text = droneToList.Battery.ToString();
            StatusTextBoxUD.Text = droneToList.DroneStatus.ToString();
            LatitudeTextBoxUD.Text = droneToList.CurrentLocation.Latitude.ToString();
            LongitutdeTextBoxUD.Text = droneToList.CurrentLocation.Longitude.ToString();
            ParcelInTranferTextBoxUD.Text = droneToList.ParcelIdInTransfer.ToString();
        }

        public DroneWindow(BL.BL ibl)
        {
            InitializeComponent();
            bl = ibl;
            WeightComboBox.ItemsSource = Enum.GetValues(typeof(IBL.BO.Enum.WeightCategories));
            GridAddDrone.Visibility = Visibility.Visible;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            IBL.BO.Drone newDrone = new();
            newDrone.Id = int.Parse(IDTextBox.Text);
            newDrone.Model = ModelTextBox.Text;
            newDrone.Weight = (IBL.BO.Enum.WeightCategories)WeightComboBox.SelectedItem;
            bl.AddDrone(newDrone, int.Parse(NumOfStationTextBox.Text));
        }

        private void UpdateDroneButtonUD_Click(object sender, RoutedEventArgs e)
        {
            bl.UpdateDrone(int.Parse(IDTextBoxUD.Text), ModelTextBoxUD.Text);
            var result = MessageBox.Show($"SUCCESSFULY UPDATED DRONE! \n The drones new model name is {ModelTextBoxUD.Text}", "Successfuly updated",
               MessageBoxButton.OKCancel);
            switch (result)
            {
                case MessageBoxResult.OK:
                    this.Close();
                    DroneListWindow. DronesListView.ItemsSource = bl.GetAllDrones();
                    break;
                case MessageBoxResult.Cancel:
                    ModelTextBoxUD.Text = "";
                    break;
            }

        }
    }
}
