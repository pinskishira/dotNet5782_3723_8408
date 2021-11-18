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
                        case BlMainSwitchFunctions.Add: //the user will choose whether he wants to add on a station, drone, customer or parcel
                            Console.WriteLine("What object would you like to add on? \nEnter 1 to add a station \n" +
                                "Enter 2 to add a drone \nEnter 3 to add a customer \nEnter 4 to add a parcel \n");
                            int.TryParse(Console.ReadLine(), out input);
                            answerAdd = (AddingFunction)input;
                            switch (answerAdd)
                            {
                                case AddingFunction.AddStation://case which adds a new station with data into the Stations array
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
                                case AddingFunction.AddDrone://case which adds a new drone with data into the Drones array
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
                                case AddingFunction.AddCustomer://case which adds a new customer with data into the Customers array
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
                                case AddingFunction.AddParcel://case which adds a new parcel with data into the Parcels array
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
                        case BlMainSwitchFunctions.Display: //the user will choose whether he wants to display the stations, drones, customers, or parcels
                            Console.WriteLine("What will you like to display?\nEnter 1 for station\nEnter 2 for drone\n" +
                             "Enter 3 for customer\nEnter 4 for parcel\n");
                            int.TryParse(Console.ReadLine(), out input);
                            answerDisplay = (DisplayingFunction)input;
                            Console.WriteLine("Enter your ID number: ");
                            int id;
                            int.TryParse(Console.ReadLine(), out id);
                            switch (answerDisplay)
                            {
                                case DisplayingFunction.Station://case which displays the requested station
                                    Console.WriteLine(ibl.DisplayStation(id));//finds station according to inputted id
                                    break;
                                case DisplayingFunction.Drone://case which displays the requested drone
                                    Console.WriteLine(ibl.DisplayDrone(id));//finds drone according to inputted id
                                    break;
                                case DisplayingFunction.Customer://case which displays the requested customer
                                    Console.WriteLine(ibl.DisplayCustomer(id));//finds customer according to inputted id
                                    break;
                                case DisplayingFunction.Parcel://case which displays the requested parcel
                                    Console.WriteLine(ibl.DisplayParcel(id));//finds parcel according to inputted id
                                    break;
                            }
                            break;
                        case BlMainSwitchFunctions.ListView://the user will choose whether he wants to view the array of stations, customers, parcels,
                                                          //or parcels with no assigned drones, or stations available to charge
                            Console.WriteLine("What do you want to view?\nEnter 1 for stations\nEnter 2 for drones\n" +
                                "Enter 3 for customers\nEnter 4 for parcels\nEnter 5 for parcels with no drone\n" +
                                "Enter 6 for station with available charging stations\n ");
                            int.TryParse(Console.ReadLine(), out input);
                            answerListView = (ListViewFunction)input;
                            switch (answerListView)
                            {
                                case ListViewFunction.Stations://case which views the stations array
                                    
                                    IEnumerable<StationToList> viewStations = ibl.ListViewStations(); 
                                    foreach (var station in viewStations)//prints all stations
                                        Console.WriteLine(station);
                                    break;
                                case ListViewFunction.Drones://case which views the drones array
                                    IEnumerable<DroneToList> viewDrones = ibl.ListViewDrones();
                                    foreach (var drone in viewDrones)//prints all drones
                                        Console.WriteLine(drone);
                                    break;
                                case ListViewFunction.Customers://case which views the customers array
                                    IEnumerable<CustomerToList> viewCustomers = ibl.ListViewCustomers();
                                    foreach (var customer in viewCustomers)//prints all customers
                                        Console.WriteLine(customer);
                                    break;
                                case ListViewFunction.Parcels://case which views the parcels array
                                    IEnumerable<ParcelToList> viewParcels = ibl.ListViewParcels();
                                    foreach (var parcel in viewParcels)//prints all parcels
                                        Console.WriteLine(parcel);
                                    break;
                                case ListViewFunction.ParcelsWithNoDrone://case which views the parcel with no assigned drones
                                    IEnumerable<Parcel> ViewParcelsWithNoDrone = ibl.ParcelWithNoDrone();
                                    foreach (var parcel in ViewParcelsWithNoDrone)//printing
                                        Console.WriteLine(parcel);
                                    break;
                                case ListViewFunction.StationWithAvailableChargingStation://case which views the station with available charging stations
                                    IEnumerable<Station> viewStationWithAvailableChargingStation = ibl.GetStationWithFreeSlots();
                                    foreach (var station in viewStationWithAvailableChargingStation)//prints all parcels
                                        Console.WriteLine(station);
                                    break;
                            }
                            break;
                    }
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
            while (answerMain != BlMainSwitchFunctions.Exit);
        }
    }
}