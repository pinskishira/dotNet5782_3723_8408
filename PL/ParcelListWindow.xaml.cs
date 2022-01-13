using System;
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
using BlApi;

namespace PL
{
    public struct StatusWeightAndPriorities
    {
        public BO.Enum.WeightCategories weight { get; set; }
        public BO.Enum.ParcelState status { get; set; }
        public BO.Enum.Priorities priorities { get; set; }
    }

    public enum WeightCategoriesParcel { Easy, Medium, Heavy, All };
    public enum Priorities { Normal, Fast, Emergency, All };
    public enum ParcelState { Created, Paired, PickedUp, Provided, All };

    /// <summary>
    /// Interaction logic for ParcelListWindow.xaml
    /// </summary>
    public partial class ParcelListWindow : Window
    {
        BlApi.Ibl bl;
        public ParcelToList CurrentParcel { get; set; } = new();
        public Dictionary<StatusWeightAndPriorities, List<ParcelToList>> Parcels;
        private bool _close { get; set; } = false;

        /// <summary>
        /// Initializes the list of all the parcels
        /// </summary>
        /// <param name="ibl">From type bl</param>
        public ParcelListWindow(BlApi.Ibl ibl)
        {
            InitializeComponent();
            bl = ibl;
            Parcels = new Dictionary<StatusWeightAndPriorities, List<ParcelToList>>();
            Parcels = (from item in bl.GetAllParcels()
                       group item by
                       new StatusWeightAndPriorities()
                       {
                           status = item.StateOfParcel,
                           weight = item.Weight,
                           priorities = item.Priority,
                       }).ToDictionary(item => item.Key, item => item.ToList());

            ParcelsListView.ItemsSource = Parcels.SelectMany(item => item.Value);//to show all the drones 
            StatusSelection.ItemsSource = System.Enum.GetValues(typeof(ParcelState));
            WeightSelection.ItemsSource = System.Enum.GetValues(typeof(WeightCategories));
            PrioritiesSelection.ItemsSource = System.Enum.GetValues(typeof(Priorities));
            StatusSelection.SelectedIndex = 4;//no filter
        }

        /// <summary>
        /// filters the list of drones that was enterd according to status
        /// </summary>
        private void StatusSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Selection();//to show the list according to the filter that was enterd
        }

        /// <summary>
        ///  filters the list of parcels that was enterd according to weight
        /// </summary>
        private void WeightSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Selection();// to show the list according to the filter that was enterd
        }

        /// <summary>
        ///  filters the list of parcels that was enterd according to priority
        /// </summary>
        private void PrioritiesSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Selection();
        }

        /// <summary>
        /// Shows the list according to the filter that the user chose
        /// </summary>
        public void Selection()
        {
            ParcelState pStatus = (ParcelState)StatusSelection.SelectedItem;
            if (WeightSelection.SelectedIndex == -1)//meens no filter was chosen
                WeightSelection.SelectedIndex = 3;//no filter-shows all the drones
            if (PrioritiesSelection.SelectedIndex == -1)
                PrioritiesSelection.SelectedIndex = 3;
            Priorities pPriorities = (Priorities)PrioritiesSelection.SelectedItem;
            WeightCategories dWeight = (WeightCategories)WeightSelection.SelectedItem;
            //if no filter was chosen-show the all list
            if (pStatus == ParcelState.All && dWeight == WeightCategories.All && pPriorities == Priorities.All)
                //to show the all list
                ParcelsListView.ItemsSource = from item in Parcels.Values.SelectMany(x => x)
                                              orderby item.Weight, item.StateOfParcel, item.Priority
                                              select item;
            //if only he wants to filter the weight category
            if (pStatus == ParcelState.All && dWeight == WeightCategories.All && pPriorities != Priorities.All)
                ParcelsListView.ItemsSource = Parcels.Where(item => item.Key.priorities == (BO.Enum.Priorities)PrioritiesSelection.SelectedItem).SelectMany(item => item.Value);
            //if only he wants to filter the statuse category
            if (pStatus == ParcelState.All && dWeight != WeightCategories.All && pPriorities == Priorities.All)
                ParcelsListView.ItemsSource = Parcels.Where(item => item.Key.weight == (BO.Enum.WeightCategories)WeightSelection.SelectedItem).SelectMany(item => item.Value);
            //if  he wants to filter both the weight category and the status category
            if (pStatus != ParcelState.All && dWeight == WeightCategories.All && pPriorities == Priorities.All)
                ParcelsListView.ItemsSource = Parcels.Where(item => item.Key.status == (BO.Enum.ParcelState)StatusSelection.SelectedItem).SelectMany(item => item.Value);
            if (pStatus == ParcelState.All && dWeight != WeightCategories.All && pPriorities != Priorities.All)
                ParcelsListView.ItemsSource = Parcels.Where(item => item.Key.priorities == (BO.Enum.Priorities)PrioritiesSelection.SelectedItem && item.Key.weight == (BO.Enum.WeightCategories)WeightSelection.SelectedItem).SelectMany(item => item.Value);
            //if only he wants to filter the statuse category
            if (pStatus != ParcelState.All && dWeight == WeightCategories.All && pPriorities != Priorities.All)
                ParcelsListView.ItemsSource = Parcels.Where(item => item.Key.priorities == (BO.Enum.Priorities)PrioritiesSelection.SelectedItem && item.Key.status == (BO.Enum.ParcelState)StatusSelection.SelectedItem).SelectMany(item => item.Value);
            if (pStatus != ParcelState.All && dWeight != WeightCategories.All && pPriorities == Priorities.All)
                ParcelsListView.ItemsSource = Parcels.Where(item => item.Key.weight == (BO.Enum.WeightCategories)WeightSelection.SelectedItem && item.Key.status == (BO.Enum.ParcelState)StatusSelection.SelectedItem).SelectMany(item => item.Value);
            if (pStatus != ParcelState.All && dWeight != WeightCategories.All && pPriorities != Priorities.All)
                ParcelsListView.ItemsSource = Parcels.Where(item => item.Key.weight == (BO.Enum.WeightCategories)WeightSelection.SelectedItem && item.Key.priorities == (BO.Enum.Priorities)PrioritiesSelection.SelectedItem && item.Key.status == (BO.Enum.ParcelState)StatusSelection.SelectedItem).SelectMany(item => item.Value);
            ParcelsListView.Items.Refresh();
        }

        /// <summary>
        /// Adds a new drone to the list
        /// </summary>
        private void AddParcelButton_Click(object sender, RoutedEventArgs e)
        {
            new ParcelWindow(bl, this).Show();
        }

        /// <summary>
        /// Presents the drone that the mouse double clicked  on
        /// </summary>
        private void ParcelsListView_SelectionChanged(object sender, MouseButtonEventArgs e)
        {
            CurrentParcel = (ParcelToList)ParcelsListView.SelectedItem;
            if (CurrentParcel != null)
                new ParcelWindow(bl, this, 0).Show();
        }

        /// <summary>
        /// Can't force the window to close
        /// </summary>
        private void Window_closing(object sender, CancelEventArgs e)
        {
            if (!_close)
            {
                e.Cancel = true;
                MessageBox.Show("You can't force the window to close");
            }
        }

        /// <summary>
        /// Closes current window
        /// </summary>
        private void CloseWindowButton_Click(object sender, RoutedEventArgs e)
        {
            _close = true;
            Close();
        }

        /// <summary>
        /// Refreshes page
        /// </summary>
        public void MyRefresh()
        {
            Parcels = new Dictionary<StatusWeightAndPriorities, List<ParcelToList>>();
            Parcels = (from item in bl.GetAllParcels()
                       group item by
                       new StatusWeightAndPriorities()
                       {
                           status = item.StateOfParcel,
                           weight = item.Weight,
                           priorities = item.Priority,
                       }).ToDictionary(item => item.Key, item => item.ToList());
            ParcelsListView.ItemsSource = from item in Parcels.Values.SelectMany(x => x)
                                          orderby item.Weight, item.StateOfParcel, item.Priority
                                          select item;
        }


        /// <summary>
        /// Allows user to delete parcel using an icon 
        /// </summary>
        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (CurrentParcel.StateOfParcel == BO.Enum.ParcelState.Created)
            {
                var result1 = MessageBox.Show($"Are you sure you would like to delete this parcel? \n", "Request Review",
                 MessageBoxButton.OKCancel, MessageBoxImage.Question);
                switch (result1)
                {
                    case MessageBoxResult.OK:
                        FrameworkElement framework = sender as FrameworkElement;
                        CurrentParcel = framework.DataContext as ParcelToList;
                        bl.DeleteParcel(CurrentParcel.Id);
                        StatusWeightAndPriorities sAndWAndP = new();
                        sAndWAndP.priorities = CurrentParcel.Priority;
                        sAndWAndP.weight = CurrentParcel.Weight;
                        sAndWAndP.status = CurrentParcel.StateOfParcel;
                        Parcels[sAndWAndP].RemoveAll(i => i.Id == CurrentParcel.Id);
                        Selection();
                        break;
                    case MessageBoxResult.Cancel:
                        break;
                }
            }

            else
                MessageBox.Show("Cannot delete parcel because it has already been assigned to a drone to be delivered.\n", "CANNOT DELETE", MessageBoxButton.OK);
        }

    }
}

