using BL;
using BlApi;
using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static PL.PLExceptions;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BlApi.Ibl bl = BlApi.BlFactory.GetBl();
        public MainWindow()
        {
            InitializeComponent();
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
        /// User presses to sign up and gives him information to sign in to the system 
        /// </summary>
        private void SignUpPicButtonMain_Click(object sender, RoutedEventArgs e)
        {
            CustomerListWindow c = new CustomerListWindow(bl);
            new CustomerWindow(bl, c).Show();
        }

        /// <summary>
        /// User enters his Id inorder to sign in to the system
        /// </summary>
        private void SignInButton_click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IdOfNewUser.Text == default)
                    throw new MissingInfoException("No Id Entered");
                Customer customer = bl.GetCustomer(int.Parse(IdOfNewUser.Text));
                CustomerListWindow c = new CustomerListWindow(bl);
                new CustomerWindow(bl, c, customer.Id,true).Show();
            }
            catch (MissingInfoException ex)
            {
                var errorMessage = MessageBox.Show(ex.Message, "Failed To Login", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (InvalidInputExceptionPL ex)
            {
                var errorMessage = MessageBox.Show(ex.Message, "Failed To Login", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch(ItemDoesNotExistException ex)
            {
                var errorMessage = MessageBox.Show(ex.Message, "Failed To Login", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            catch (OverflowException)
            {
                var errorMessage = MessageBox.Show("Id must be 9 digits long \n", "Failed To Login", MessageBoxButton.OK, MessageBoxImage.Error);
                switch (errorMessage)
                {
                    case MessageBoxResult.OK:
                        IdOfNewUser.Text = " ";
                        break;
                }
            }
        }


        /// <summary>
        /// Manger or worker press button which gives them an option to sign in with a special password
        /// </summary>
        private void ManagerOrWorkerButton_Click(object sender, RoutedEventArgs e)
        {
            if (CompanySecretPassword.Visibility == Visibility.Collapsed)
            {
                CompanySecretPassword.Visibility = Visibility.Visible;
                ManagerOrWorkerSignInButton.Visibility = Visibility.Visible;
                PasswordLbl.Visibility = Visibility.Visible;
            }
            else
            {
                CompanySecretPassword.Visibility = Visibility.Collapsed;
                ManagerOrWorkerSignInButton.Visibility = Visibility.Collapsed;
                PasswordLbl.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Manager or worker enter password to get into main system 
        /// </summary>
        private void ManagerOrWorkerSignInButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CompanySecretPassword.Password == "bestdrones")
                {
                    new MenuWindow().Show();
                }
                else
                    throw new InvalidInputExceptionPL("WRONG PASSWORD");
            }
            catch(InvalidInputExceptionPL ex)
            {
                var errorMessage = MessageBox.Show( ex.Message + "\n" , "Failed To Login", MessageBoxButton.OK, MessageBoxImage.Error);
                switch (errorMessage)
                {
                    case MessageBoxResult.OK:
                        CompanySecretPassword.Password = "";
                        break;
                }
            }

        }
    }
}
