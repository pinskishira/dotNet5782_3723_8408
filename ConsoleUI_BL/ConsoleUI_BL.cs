using System;
using IDAL;
using System.Collections.Generic;
using IBL.BO;
using IBL;
using static IBL.BO.Enum;
using BL.IBL.BO;

namespace ConsoleUI_BL
{
    public enum BlMainSwitchFunctions { Add = 1, Update, Display, ListView, Exit };
    public enum AddingFunction { AddStation = 1, AddDrone, AddCustomer, AddParcel };
    public enum UpdateingFunction { AssignParcelToDrone = 1, ParcelCollectionByDrone, ParcelDeliveryToCustomer, SendDroneToChargingStation, DroneReleaseFromChargingStation };
    public enum DisplayingFunction { Station = 1, Drone, Customer, Parcel };
    public enum ListViewFunction { Stations = 1, Drones, Customers, Parcels, ParcelsWithNoDrone, StationWithAvailableChargingStation };

    class ConsoleUI_BL
    {
        public static void Main(string[] args)
        {
            BL.BL ibl = new BL.BL();//
            int ansFromUserInt, input;
            double ansFromUserDouble1;
            double ansFromUserDouble2;
            AddingFunction answerAdd;
            BlMainSwitchFunctions answerMain;
            DisplayingFunction answerDisplay;
            UpdateingFunction answerUpdate;
            ListViewFunction answerListView;

            do
            {
                Console.WriteLine("What would you like to perform?\nEnter 1 to Add \nEnter 2 to Update \n" +
                    "Enter 3 to Display \nEnter 4 to ListView \nEnter 5 to Exit \n");
                int.TryParse(Console.ReadLine(), out input);
                answerMain = (BlMainSwitchFunctions)input;
                try
                {
                    switch (answerMain)
                    {
                        case (BlMainSwitchFunctions)AddingFunction.AddStation://case which adds a new station with data into the Stations array
                            Station newStation = new();
                            Console.Write("Enter a four-digit number in a new station ID number: ");
                            int.TryParse(Console.ReadLine(), out input);
                            newStation.Id = input;
                            Console.Write("Enter new station name: ");
                            newStation.NameOfStation = Console.ReadLine();
                            Console.Write("Please enter you location: ");
                            double.TryParse(Console.ReadLine(), out ansFromUserDouble1);
                            double.TryParse(Console.ReadLine(), out ansFromUserDouble2);
                            newStation.StationLocation.Latitude = ansFromUserDouble1;
                            newStation.StationLocation.Longitude = ansFromUserDouble1;
                            Console.Write("Enter amount of availbale charge slots in new station: ");
                            int.TryParse(Console.ReadLine(), out input);
                            newStation.AvailableChargeSlots = input;
                            newStation.DronesInCharging = null;
                            ibl.AddStation(newStation);
                            break;
                        case (BlMainSwitchFunctions)AddingFunction.AddDrone://case which adds a new drone with data into the Drones array
                            Drone newDrone = new();
                            Console.Write("Enter drone Id: ");
                            int.TryParse(Console.ReadLine(), out input);
                            newDrone.Id = input;
                            Console.Write("Enter drone model: ");
                            newDrone.Model = Console.ReadLine();
                            Console.WriteLine("Enter drones maximum weight:\n1 - Easy\n2 - Medium\n3 - Heavy");
                            int.TryParse(Console.ReadLine(), out input);
                            newDrone.MaxWeight = (WeightCategories)input;
                            Console.Write("Enter station number: ");
                            int.TryParse(Console.ReadLine(), out input);
                            int stationNumber = input;
                            ibl.AddDrone(newDrone, stationNumber);
                            break;
                        case (BlMainSwitchFunctions)AddingFunction.AddCustomer://case which adds a new customer with data into the Customers array
                            Customer NewCustomer = new();
                            Console.Write("Please enter ID: ");
                            int.TryParse(Console.ReadLine(), out ansFromUserInt);
                            NewCustomer.Id = ansFromUserInt;
                            Console.Write("Please enter name: ");
                            NewCustomer.Name = Console.ReadLine();
                            Console.Write("Please enter phone: ");
                            NewCustomer.Phone = Console.ReadLine();
                            Console.Write("Please enter you location: ");
                            double.TryParse(Console.ReadLine(), out ansFromUserDouble1);
                            double.TryParse(Console.ReadLine(), out ansFromUserDouble2);
                            NewCustomer.CustomerLocation.Latitude = ansFromUserDouble1;
                            NewCustomer.CustomerLocation.Longitude = ansFromUserDouble1;
                            ibl.AddCustomer(NewCustomer);
                            break;
                        case (BlMainSwitchFunctions)AddingFunction.AddParcel://case which adds a new parcel with data into the Parcels array
                            Parcel NewParcel = new();
                            NewParcel.Id = 0;
                            Console.Write("Enter sender ID of 9 digits: ");
                            int.TryParse(Console.ReadLine(), out ansFromUserInt);
                            NewParcel.SenderId.Id = ansFromUserInt;
                            Console.Write("Enter target ID of 9 digits: ");
                            int.TryParse(Console.ReadLine(), out ansFromUserInt);
                            NewParcel.TargetId.Id = ansFromUserInt;
                            Console.WriteLine("Enter the weight of your parcel:\n1 - Easy\n2 - Medium\n3 - Heavy");
                            int.TryParse(Console.ReadLine(), out input);
                            NewParcel.Weight = (WeightCategories)input;
                            Console.WriteLine("Enter the urgency of your parcel:\n1 - normal\n2 - Fast\n3 - Emergency");
                            int.TryParse(Console.ReadLine(), out input);
                            NewParcel.Priority = (Priorities)input;
                            NewParcel.DroneParcel = null;
                            ibl.AddParcel(NewParcel);
                            break;
                    }
                    break;
                }
                catch (InvalidInputException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (FailedToAddException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            while (answerMain != BlMainSwitchFunctions.Exit) ;
        }
    }
}