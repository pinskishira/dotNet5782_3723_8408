using System;
using IDAL.DO;
Random rand = new Random(DateTime.Now.Millisecond);
/// <summary>
/// Program main which gives us 4 main options: To add, update, display and view the: drones, stations, parcels, customers,
/// and drone charges. 
/// </summary>
namespace ConsoleUI
{ 
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("What would you like to perform?\n You can choose to: \n 1) Add \n 2) Update \n 3) Display \n 4) ListView \n 5) Exit");
            int AnsFromUserInt, Input;
            double AnsFromUserDouble;
            AddingFunction AnswerAdd;
            MainSwitchFunctions AnswerMain;
            DisplayingFunction AnswerDisplay;
            UpdateingFunction AnswerUpdate;
            ListViewFunction AnswerListView;
            int.TryParse(Console.ReadLine(), out Input);
            AnswerMain = (MainSwitchFunctions)Input;
            while(Input!=5)
            {
                switch (AnswerMain)
                {
                    case MainSwitchFunctions.Add://the user will choose whether he wants to add on a station, drone, customer or parcel
                        Console.WriteLine("What object would you like to add on? \n 1) AddStation \n 2) AddDrone \n 3) AddCustomer \n 4) AddParcel \n");
                        int.TryParse(Console.ReadLine(), out Input);
                        AnswerAdd = (AddingFunction)Input;
                        switch (AnswerAdd)
                        {
                            case AddingFunction.AddStation://case which adds a new station with data into the Stations array
                                Station NewStation = new() { Id = 1234, Name = "Uziel", Longitude = 35.183695, Latitude = 31.772032, ChargeSlots = 9 };
                                DalObject.DalObject.AddStation(NewStation);
                                break;
                            case AddingFunction.AddDrone://case which adds a new drone with data into the Drones array
                                Drone NewDrone = new() { Id = 12345, Model = "", MaxWeight = WeightCategories.Heavy, Status = DroneStatuses.Available, Battery = 100 };
                                DalObject.DalObject.AddDrone(NewDrone);
                                break;
                            case AddingFunction.AddCustomer://case which adds a new customer with data into the Customers array
                                Customer NewCustomer = new();
                                Console.WriteLine("Please enter ID:");
                                int.TryParse(Console.ReadLine(), out AnsFromUserInt);
                                NewCustomer.Id = AnsFromUserInt;
                                Console.WriteLine("Please enter name:");
                                NewCustomer.Name = Console.ReadLine();
                                Console.WriteLine("Please enter phone:");
                                NewCustomer.Phone = Console.ReadLine();
                                Console.WriteLine("Please enter longitude and latitude points:");
                                double.TryParse(Console.ReadLine(), out AnsFromUserDouble);
                                NewCustomer.Longitude = AnsFromUserDouble;
                                double.TryParse(Console.ReadLine(), out AnsFromUserDouble);
                                NewCustomer.Latitude = AnsFromUserDouble;
                                DalObject.DalObject.AddCustomer(NewCustomer);
                                break;
                            case AddingFunction.AddParcel://case which adds a new parcel with data into the Parcels array
                                DalObject.DalObject.UpdateParcelCounter("Increase");
                                Parcel NewParcel = new();
                                NewParcel.Id = 23;//////
                                Console.WriteLine("Enter sender ID of 3 digits:\n");
                                int.TryParse(Console.ReadLine(), out AnsFromUserInt);
                                NewParcel.SenderId = AnsFromUserInt;
                                Console.WriteLine("Enter target ID of 9 digits:\n");
                                int.TryParse(Console.ReadLine(), out AnsFromUserInt);
                                NewParcel.TargetId = AnsFromUserInt;
                                Console.WriteLine("Enter the weight of your parcel:\n 1)Easy\n 2) Medium\n 3) Heavy\n");
                                int.TryParse(Console.ReadLine(), out Input);
                                NewParcel.Weight = (WeightCategories)Input;
                                Console.WriteLine("Enter the urgency of your parcel:\n 1)normal\n 2) Fast\n 3) Emergency\n");
                                int.TryParse(Console.ReadLine(), out Input);
                                NewParcel.Priority = (Priorities)Input;
                                NewParcel.Requested = DateTime.Now;
                                NewParcel.Scheduled = DateTime.Now;
                                NewParcel.Delivered = DateTime.Now;
                                NewParcel.PickedUp = DateTime.Now;
                                break;
                        }
                        break;
                    case MainSwitchFunctions.Update://the user will choose whether he wants to update a parcel to a drone, parcel collection by a drone, parcel delivery to customer, sending drone to charge, and release from charge
                        Console.WriteLine("What object do you want to update?\n 1)AssignParcelToDrone\n 2)ParcelCollectionByDrone\n 3)ParcelDeliveryToCustomer\n 4)SendDroneToChargingStation\n 5)DroneReleaseFromChargingStation");
                        int.TryParse(Console.ReadLine(), out Input);
                        AnswerUpdate = (UpdateingFunction)Input;
                        int ID;
                        Console.WriteLine("Enter your parcel ID:\n");
                        int.TryParse(Console.ReadLine(), out ID);
                        Parcel TheParcel = new();
                        TheParcel = DalObject.DalObject.FindParcel(ID);//finding the parcel in the parcel stock 
                        Drone AvailableDrone = new();
                        switch (AnswerUpdate)
                        {
                            case UpdateingFunction.AssignParcelToDrone://case which assigns a parcel to a suitable drone
                                AvailableDrone = DalObject.DalObject.FindDroneAvailable();
                                AvailableDrone.Status = DroneStatuses.Delivery;
                                TheParcel.Scheduled = DateTime.Now;
                                TheParcel.Droneld = AvailableDrone.Id;
                                break;
                            case UpdateingFunction.ParcelCollectionByDrone://case which updates when a parcel is collected by a drone
                                TheParcel.Delivered = DateTime.Now;
                                TheParcel.Droneld = 0;
                                break;
                            case UpdateingFunction.ParcelDeliveryToCustomer://case which updates when a parcel is delivered to a customer
                                TheParcel.PickedUp = DateTime.Now;
                                DalObject.DalObject.UpdateParcelCounter("Decrease");
                                AvailableDrone.Status = DroneStatuses.Available;
                                AvailableDrone.Battery -= 25;
                                break;
                            case UpdateingFunction.SendDroneToChargingStation://case which sends a low battey drone to be charged 
                                Drone LowBatteryDrone = new();//drone with low battery
                                DroneCharge NewDroneCharge = new();//drone with low battery will go be charged here
                                Console.WriteLine("Enter the ID of the Drone with low battery:\n");
                                int IdOfLowBatteryDrone;
                                int.TryParse(Console.ReadLine(), out IdOfLowBatteryDrone);//user entering drone with low battery
                                LowBatteryDrone = DalObject.DalObject.FindDroneAvailable();//finding the drone id in the drone stock
                                LowBatteryDrone.Status = DroneStatuses.Maintenance;//saying that the drone is in maintanance and unavailable to deliver
                                NewDroneCharge.DroneId = IdOfLowBatteryDrone;//putting id of low battery drone into its charging station
                                Console.WriteLine("Please enter your desired station:\n");
                                Station[] AvailableStation = new Station[DalObject.DalObject.GetIndexStation()];
                                AvailableStation = DalObject.DalObject.GetStationWithFreeSlots();//finding available station
                                for (int i = 0; i < DalObject.DalObject.GetIndexStation(); i++)//user will have a few charging stations to choose from 
                                {
                                    if (AvailableStation[i].ChargeSlots > 0)
                                        Console.WriteLine(i + 1 + ") " + AvailableStation[i].Name + "\n");
                                }
                                string ChosenStation = Console.ReadLine();
                                int IdStation = DalObject.DalObject.FindStationId(ChosenStation);
                                NewDroneCharge.StationId = IdStation;
                                DalObject.DalObject.AddDroneCharge(NewDroneCharge);//updating that a drone is charging 
                                DalObject.DalObject.DecreaseChargeSlots(DalObject.DalObject.FindStation(IdStation));//decreasing amount of places left to charge

                                break;
                            case UpdateingFunction.DroneReleaseFromChargingStation://case which releases a fully charged drone from charging station
                                Console.WriteLine("Enter the ID of the Drone with charged battery:\n");
                                int IdOfChargedBatteryDrone;
                                int.TryParse(Console.ReadLine(), out IdOfChargedBatteryDrone);
                                DroneCharge ChargedDrone = new();
                                ChargedDrone = DalObject.DalObject.FindDroneCharge(IdOfChargedBatteryDrone);
                                DalObject.DalObject.IncreaseChargeSlots(DalObject.DalObject.FindStation(ChargedDrone.StationId));//increasing amount of places left to charge
                                DalObject.DalObject.DeleteDroneCharge(ChargedDrone);
                                break;
                        }
                        break;
                    case MainSwitchFunctions.Display://the user will choose whether he wants to display the stations, drones, customers, or parcels
                        Console.WriteLine("What will you like to display?\n 1)Station\n 2)Drone\n 3)Customer\n 4)Parcel\n");
                        int.TryParse(Console.ReadLine(), out Input);
                        AnswerDisplay = (DisplayingFunction)Input;
                        Console.WriteLine("Enter your ID number:");
                        int id;
                        int.TryParse(Console.ReadLine(), out id);
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
                        Console.WriteLine("What do you want to view?\n 1)Station\n 2)Drones\n 3)Customers\n 4)Parcels\n 5)ParcelsWithNoDrone\n 6)StationWithAvailableChargingStation\n ");
                        int.TryParse(Console.ReadLine(), out Input);
                        AnswerListView = (ListViewFunction)Input;
                        switch (AnswerListView)
                        {
                            case ListViewFunction.Stations://case which views the stations array
                                Station[] viewStations = DalObject.DalObject.ListViewStation();
                                viewStations.ToString();
                                break;
                            case ListViewFunction.Drones://case which views the drones array
                                Drone[] viewDrones = DalObject.DalObject.ListViewDrone();
                                for (int i = 0; i < viewDrones.Length; i++)
                                {
                                    Console.WriteLine(viewDrones[i].ToString());
                                }
                                break;
                            case ListViewFunction.Customers://case which views the customers array
                                Customer[] viewCustomers = DalObject.DalObject.ListViewCustomer();
                                for (int i = 0; i < viewCustomers.Length; i++)
                                {
                                    Console.WriteLine(viewCustomers[i].ToString());
                                }
                                break;
                            case ListViewFunction.Parcels://case which views the parcels array
                                Parcel[] viewParcels = DalObject.DalObject.ListViewParcel();
                                for (int i = 0; i < viewParcels.Length; i++)
                                {
                                    Console.WriteLine(viewParcels[i].ToString());
                                }
                                break;
                            case ListViewFunction.ParcelsWithNoDrone://case which views the parcel with no assigned drones
                                Parcel[] ViewParcelsWithNoDrone = DalObject.DalObject.ParcelWithNoDrone();
                                for (int i = 0; i < ViewParcelsWithNoDrone.Length; i++)
                                {
                                    Console.WriteLine(ViewParcelsWithNoDrone[i].ToString());
                                }
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
                Console.WriteLine("What would you like to perform?\n You can choose to: \n 1) Add \n 2) Update \n 3) Display \n 4) ListView \n 5) Exit");
                int.TryParse(Console.ReadLine(), out Input);
            }
        }
    }
}