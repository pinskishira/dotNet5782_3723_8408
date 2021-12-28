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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BL;
using BlApi;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        private static readonly Ibl bl = BlFactory.GetBl();
        public MenuWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Goes to constructor which shows all the drones  
        /// </summary>
        private void ShowDronesButtonClick_Click(object sender, RoutedEventArgs e)
        {
            new DroneListWindow(bl).Show();
        }

        private void ShowStationsButtonClick_Click(object sender, RoutedEventArgs e)
        {
            new StationListWindow(bl).Show();
        }

        private void ShowCustomersButtonClick_Click(object sender, RoutedEventArgs e)
        {
            new CustomerListWindow(bl).Show();
        }

        private void ShowParcelButtonClick_Click(object sender, RoutedEventArgs e)
        {
            new ParcelListWindow(bl).Show();
        }
    }
}
