using System;
using IDAL.DO;

/// <summary>
/// Program main which gives us 4 main options: To add, update, display and view the: drones, stations, parcels, customers,
/// and drone charges. 
/// </summary>
namespace ConsoleUI
{
    public enum MainSwitchFunctions { Add = 1, Update, Display, ListView, Exit };
    public enum AddingFunction { AddStation = 1, AddDrone, AddCustomer, AddParcel };
    public enum UpdateingFunction { AssignParcelToDrone = 1, ParcelCollectionByDrone, ParcelDeliveryToCustomer, SendDroneToChargingStation, DroneReleaseFromChargingStation };
    public enum DisplayingFunction { Station = 1, Drone, Customer, Parcel };
    public enum ListViewFunction { Stations = 1, Drones, Customers, Parcels, ParcelsWithNoDrone, StationWithAvailableChargingStation };

    class Program
    {
        public static void Main(string[] args)
        { 
            int AnsFromUserInt, input;
            double AnsFromUserDouble;
            AddingFunction AnswerAdd;
            MainSwitchFunctions AnswerMain;
            DisplayingFunction AnswerDisplay;
            UpdateingFunction AnswerUpdate;
            ListViewFunction AnswerListView;

            do
            {
                Console.WriteLine("What would you like to perform?\nEnter 1 to Add \nEnter 2 to Update \n" +
                    "Enter 3 to Display \nEnter 4 to ListView \nEnter 5 to Exit \n");
                int.TryParse(Console.ReadLine(), out input);
                AnswerMain = (MainSwitchFunctions)input;
                switch (AnswerMain)
                {
                    case MainSwitchFunctions.Add://the user will choose whether he wants to add on a station, drone, customer or parcel
                        Console.WriteLine("What object would you like to add on? \nEnter 1 to add a station \n" +
                            "Enter 2 to add a drone \nEnter 3 to add a customer \nEnter 4 to add a parcel \n");
                        int.TryParse(Console.ReadLine(), out input);
                        AnswerAdd = (AddingFunction)input;
                        switch (AnswerAdd)
                        {
                            case AddingFunction.AddStation://case which adds a new station with data into the Stations array
                                Station newStation = new();
                                // Must input data from user!!!
                                Console.Write("Enter new station Id: ");
                                int.TryParse(Console.ReadLine(), out input);
                                newStation.Id = input;
                                Console.Write("Enter new station name: ");
                                newStation.Name = Console.ReadLine();
                                Console.Write("Please enter longitude: ");
                                double.TryParse(Console.ReadLine(), out AnsFromUserDouble);
                                newStation.Longitude = AnsFromUserDouble;
                                Console.Write("Please enter Latitude: ");
                                double.TryParse(Console.ReadLine(), out AnsFromUserDouble);
                                newStation.Latitude = AnsFromUserDouble;
                                Console.Write("Enter amount of charge slots in new station : ");
                                int.TryParse(Console.ReadLine(), out input);
                                newStation.ChargeSlots = input;
                                DalObject.DalObject.AddStation(newStation);
                                break;
                            case AddingFunction.AddDrone://case which adds a new drone with data into the Drones array
                                Drone newDrone = new();
                                Console.Write("Enter drone Id: ");
                                int.TryParse(Console.ReadLine(), out input);
                                newDrone.Id = input;
                                Console.Write("Enter drone model: ");
                                newDrone.Model = Console.ReadLine();
                                Console.WriteLine("Enter drones' maximum weight:\n1 - Easy\n2 - Medium\n3 - Heavy");
                                int.TryParse(Console.ReadLine(),out input);
                                newDrone.MaxWeight = (WeightCategories)input;
                                Console.WriteLine("Enter drones' status:\n1 - Available\n2 - Maintanance\n3 - Delivery");
                                int.TryParse(Console.ReadLine(), out input);
                                newDrone.Status = (DroneStatuses)input;
                                Console.Write("Enter the drones' battery status: ");
                                int.TryParse(Console.ReadLine(), out input);
                                newDrone.Battery = input;
                                DalObject.DalObject.AddDrone(newDrone);
                                break;
                            case AddingFunction.AddCustomer://case which adds a new customer with data into the Customers array
                                Customer NewCustomer = new();
                                Console.Write("Please enter ID: ");
                                int.TryParse(Console.ReadLine(), out AnsFromUserInt);
                                NewCustomer.Id = AnsFromUserInt;
                                Console.Write("Please enter name: ");
                                NewCustomer.Name = Console.ReadLine();
                                Console.Write("Please enter phone: ");
                                NewCustomer.Phone = Console.ReadLine();
                                Console.Write("Please enter longitude: ");
                                double.TryParse(Console.ReadLine(), out AnsFromUserDouble);
                                NewCustomer.Longitude = AnsFromUserDouble;
                                Console.Write("Please enter latitude: ");
                                double.TryParse(Console.ReadLine(), out AnsFromUserDouble);
                                NewCustomer.Latitude = AnsFromUserDouble;
                                DalObject.DalObject.AddCustomer(NewCustomer);
                                break;
                            case AddingFunction.AddParcel://case which adds a new parcel with data into the Parcels array
                                Parcel NewParcel = new();
                                NewParcel.Id = 0;
                                Console.Write("Enter sender ID of 3 digits: ");
                                int.TryParse(Console.ReadLine(), out AnsFromUserInt);
                                NewParcel.SenderId = AnsFromUserInt;
                                Console.Write("Enter target ID of 9 digits: ");
                                int.TryParse(Console.ReadLine(), out AnsFromUserInt);
                                NewParcel.TargetId = AnsFromUserInt;
                                Console.WriteLine("Enter the weight of your parcel:\n1 - Easy\n2 - Medium\n3 - Heavy");
                                int.TryParse(Console.ReadLine(), out input);
                                NewParcel.Weight = (WeightCategories)input;
                                Console.WriteLine("Enter the urgency of your parcel:\n1 - normal\n2 - Fast\n3 - Emergency");
                                int.TryParse(Console.ReadLine(), out input);
                                NewParcel.Priority = (Priorities)input;
                                NewParcel.DroneId = 0;
                                NewParcel.Requested = new(0);
                                NewParcel.Scheduled = new(0);
                                NewParcel.Delivered = new(0);
                                NewParcel.PickedUp = new(0);
                                DalObject.DalObject.AddParcel(NewParcel);
                                break;
                        }
                        break;
                    case MainSwitchFunctions.Update://the user will choose whether he wants to update a parcel to a drone, parcel collection by a drone, parcel delivery to customer, sending drone to charge, and release from charge
                        Console.WriteLine("What object do you want to update?\nEnter 1 to assign a parcel to a drone\n" +
                            "Enter 2 for parcel collection by drone\nEnter 3 for parcel delivery to customer\n" +
                            "Enetr 4 to send drone to charging station\nEnter 5 for drone release from charging station\n");
                        int.TryParse(Console.ReadLine(), out input);
                        AnswerUpdate = (UpdateingFunction)input;
                        int IdParcel, IdDrone;
                        switch (AnswerUpdate)
                        {
                            case UpdateingFunction.AssignParcelToDrone://case which assigns a parcel to a suitable drone
                                Console.Write("Enter your parcel ID: ");
                                int.TryParse(Console.ReadLine(), out IdParcel);
                                Console.Write("Enter your drone ID: ");
                                int.TryParse(Console.ReadLine(), out IdDrone);
                                DalObject.DalObject.UpdateAssignParcelToDrone(IdParcel, IdDrone);
                                break;
                            case UpdateingFunction.ParcelCollectionByDrone://case which updates when a parcel is collected by a drone
                                Console.Write("Enter your parcel ID: ");
                                int.TryParse(Console.ReadLine(), out IdParcel);
                                DalObject.DalObject.UpdateParcelCollectionByDrone(IdParcel);
                                break;
                            case UpdateingFunction.ParcelDeliveryToCustomer://case which updates when a parcel is delivered to a customer
                                Console.Write("Enter your parcel ID: ");
                                int.TryParse(Console.ReadLine(), out IdParcel);
                                DalObject.DalObject.UpdateParcelDeliveryToCustomer(IdParcel);
                                break;
                            case UpdateingFunction.SendDroneToChargingStation://case which sends a low battey drone to be charged 
                                Console.Write("Enter the ID of the Drone with low battery: ");
                                int IdOfLowBatteryDrone;
                                int.TryParse(Console.ReadLine(), out IdOfLowBatteryDrone);//user entering drone with low battery
                                Console.Write("Please enter your desired station: ");
                                Station[] AvailableStation = new Station[DalObject.DalObject.GetIndexStation()];
                                AvailableStation = DalObject.DalObject.GetStationWithFreeSlots();//finding available station
                                for (int i = 0; i < DalObject.DalObject.GetIndexStation(); i++)//user will have a few charging stations to choose from 
                                {
                                    if (AvailableStation[i].ChargeSlots > 0)
                                        Console.WriteLine(i + 1 + ") " + AvailableStation[i].Name + "\n");
                                }
                                string ChosenStation = Console.ReadLine();
                                DalObject.DalObject.UpdateSendDroneToChargingStation(IdOfLowBatteryDrone, ChosenStation);
                                break;
                            case UpdateingFunction.DroneReleaseFromChargingStation://case which releases a fully charged drone from charging station
                                Console.Write("Enter the ID of the Drone with charged battery: ");
                                int IdOfChargedBatteryDrone;
                                int.TryParse(Console.ReadLine(), out IdOfChargedBatteryDrone);
                                DalObject.DalObject.DroneReleaseFromChargingStation(IdOfChargedBatteryDrone);
                                break;
                        }
                        break;
                    case MainSwitchFunctions.Display://the user will choose whether he wants to display the stations, drones, customers, or parcels
                        Console.WriteLine("What will you like to display?\nEnter 1 for station\nEnter 2 for drone\n " +
                            "Enter 3 for customer\nEnter 4 for parcel\n");
                        int.TryParse(Console.ReadLine(), out input);
                        AnswerDisplay = (DisplayingFunction)input;
                        Console.WriteLine("Enter your ID number: ");
                        int id;
                        int.TryParse(Console.ReadLine(), out id);
                        Console.WriteLine("\n");
                        switch (AnswerDisplay)
                        {
                            case DisplayingFunction.Station://case which displays the requested station
                                Station s = new();
                                s = DalObject.DalObject.FindStation(id);
                                Console.WriteLine(s.ToString());
                                break;
                            case DisplayingFunction.Drone://case which displays the requested drone
                                Drone d = new();
                                d = DalObject.DalObject.FindDrone(id);
                                Console.WriteLine(d.ToString());
                                break;
                            case DisplayingFunction.Customer://case which displays the requested customer
                                Customer c = new();
                                c = DalObject.DalObject.FindCustomer(id);
                                Console.WriteLine(c.ToString());
                                break;
                            case DisplayingFunction.Parcel://case which displays the requested parcel
                                Parcel p = new();
                                p = DalObject.DalObject.FindParcel(id);
                                Console.WriteLine(p.ToString());
                                break;

                        }
                        break;
                    case MainSwitchFunctions.ListView://the user will choose whether he wants to view the array of stations, customers, parcels, or parcels with no assigned drones, or stations available to charge
                        Console.WriteLine("What do you want to view?\nEnter 1 for stations\nEnter 2 for drones\n" +
                            "Enter 3 for customers\nEnter 4 for parcels\nEnter 5 for parcels with no drone\n" +
                            "Enter 6 for station with available charging stations\n ");
                        int.TryParse(Console.ReadLine(), out input);
                        AnswerListView = (ListViewFunction)input;
                        switch (AnswerListView)
                        {
                            case ListViewFunction.Stations://case which views the stations array
                                Station[] viewStations = DalObject.DalObject.GetAllStations();
                                foreach(var station in viewStations)
                                    Console.WriteLine(station);
                                break;
                            case ListViewFunction.Drones://case which views the drones array
                                Drone[] viewDrones = DalObject.DalObject.GetAllDrones();
                                for (int i = 0; i < viewDrones.Length; i++)
                                    Console.WriteLine(viewDrones[i]);
                                break;
                            case ListViewFunction.Customers://case which views the customers array
                                Customer[] viewCustomers = DalObject.DalObject.GetAllCustomers();
                                for (int i = 0; i < viewCustomers.Length; i++)
                                    Console.WriteLine(viewCustomers[i].ToString());
                                break;
                            case ListViewFunction.Parcels://case which views the parcels array
                                Parcel[] viewParcels = DalObject.DalObject.GetAllParcels();
                                for (int i = 0; i < viewParcels.Length; i++)
                                    Console.WriteLine(viewParcels[i].ToString());
                                break;
                            case ListViewFunction.ParcelsWithNoDrone://case which views the parcel with no assigned drones
                                Parcel[] ViewParcelsWithNoDrone = DalObject.DalObject.ParcelWithNoDrone();
                                for (int i = 0; i < ViewParcelsWithNoDrone.Length; i++)
                                    Console.WriteLine(ViewParcelsWithNoDrone[i].ToString());
                                break;
                            case ListViewFunction.StationWithAvailableChargingStation://case which views the station with available charging stations
                                Station[] viewStationWithAvailableChargingStation = DalObject.DalObject.AvailableChargingSlots();
                                for (int i = 0; i < viewStationWithAvailableChargingStation.Length; i++)
                                {
                                    Console.WriteLine(viewStationWithAvailableChargingStation[i].ToString());
                                }
                                break;
                        }
                        break;
                }
            }
            while (AnswerMain != MainSwitchFunctions.Exit);
        }
    }
}