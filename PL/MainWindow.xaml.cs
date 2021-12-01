﻿using System;
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

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public BL.BL bl = new BL.BL();
        public MainWindow(BL.BL bl)
        {
            InitializeComponent();
        }

        private void ShowDronesButtonClick(object sender, RoutedEventArgs e)
        {
            new DroneListWindow(bl).Show();
        }
    }
}
