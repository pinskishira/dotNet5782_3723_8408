using BO;
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
    /// Interaction logic for CustomerInterfaceWindow.xaml
    /// </summary>
    public partial class CustomerInterfaceWindow : Window
    {
        BlApi.Ibl bl = BL.BlFactory.GetBl();
        public CustomerInterfaceWindow()
        {
            InitializeComponent();
        }

        private void UpdateUserButton_Click(object sender, RoutedEventArgs e)
        {
            //Customer customer = bl.GetCustomer(int.Parse(MainWindow.IdOfNewUser.Text));
            //CustomerListWindow c = new CustomerListWindow(bl);
            //new CustomerWindow(bl, c, customer.Id).Show();
        }
    }
}
