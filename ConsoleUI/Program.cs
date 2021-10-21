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
            Console.WriteLine("What would you like to perform?\n You can choose to: \n 1) Add \n 2) Update \n 3) Display \n 4) ListView \n");
            string Answer = Console.ReadLine();
            switch (Answer)
            {
                case "Add"://the user will choose whether he wants to add on a station, drone, customer or parcel
                    Console.WriteLine("What object would you like to add on? \n 1) AddStation \n 2) AddDrone \n 3) AddCustomer \n 4) AddParcel \n");
                    Answer = Console.ReadLine();
                    switch (Answer)
                    {
                        case "AddStation"://case which adds a new station with data into the Stations array
                            Station NewStation = new() { Id = 1234, Name = "Uziel", Longitude = 35.183695, Latitude = 31.772032, ChargeSlots = 9 };
                            DalObject.DalObject.AddStation(NewStation);
                            break;
                        case "AddDrone"://case which adds a new drone with data into the Drones array
                            Drone NewDrone = new() { Id = 12345, Model = "", MaxWeight = WeightCategories.Heavy, Status = DroneStatuses.Available, Battery = 100 };
                            DalObject.DalObject.AddDrone(NewDrone);
                            break;
                        case "AddCustomer"://case which adds a new customer with data into the Customers array
                            Customer NewCustomer = new();
                            Console.WriteLine("Please enter ID:");
                            NewCustomer.Id = int.Parse(Console.ReadLine());
                            Console.WriteLine("Please enter name:");
                            NewCustomer.Name = Console.ReadLine();
                            Console.WriteLine("Please enter phone:");
                            NewCustomer.Phone = Console.ReadLine();
                            Console.WriteLine("Please enter longitude and latitude points:");
                            NewCustomer.Longitude = int.Parse(Console.ReadLine());
                            NewCustomer.Latitude = int.Parse(Console.ReadLine());
                            DalObject.DalObject.AddCustomer(NewCustomer);
                            break;
                        case "AddParcel"://case which adds a new parcel with data into the Parcels array
                            DalObject.DalObject.UpdateParcelCounter("Increase");
                            Parcel NewParcel = new();
                            NewParcel.Id = 23;
                            Console.WriteLine("Enter sender ID of 3 digits:");
                            NewParcel.SenderId = int.Parse(Console.ReadLine());
                            Console.WriteLine("Enter target ID of 9 digits:");
                            NewParcel.TargetId = int.Parse(Console.ReadLine());
                            Console.WriteLine("Enter the weight of your parcel:\n 1)Easy\n 2) Medium\n 3) Heavy\n");
                            string WeightAnswer = Console.ReadLine();
                            switch (WeightAnswer)//user inputs whether his parcel is heavy, medium or easy to carry
                            {
                                case "Easy":
                                    NewParcel.Weight = WeightCategories.Easy;
                                    break;
                                case "Medium":
                                    NewParcel.Weight = WeightCategories.Medium;
                                    break;
                                case "Heavy":
                                    NewParcel.Weight = WeightCategories.Heavy;
                                    break;
                            }
                            Console.WriteLine("Enter the urgency of your parcel:\n 1)normal\n 2) Fast\n 3) Emergency\n");
                            string PriorityAnswer = Console.ReadLine();
                            switch (PriorityAnswer)//user inputs whether his parcel should be delivered normally, fast or if its an emergancy
                            {
                                case "normal":
                                    NewParcel.Priority = Priorities.Normal;
                                    break;
                                case "Fast":
                                    NewParcel.Priority = Priorities.Fast;
                                    break;
                                case "Emergency":
                                    NewParcel.Priority = Priorities.Emergency;
                                    break;
                            }
                            Console.WriteLine("Enter the date: year, month and day. Time: hour, minutes and seonds:");//inputting the time of request
                            int year = int.Parse(Console.ReadLine()), month = int.Parse(Console.ReadLine()), day = int.Parse(Console.ReadLine()), hour = int.Parse(Console.ReadLine()), minutes = int.Parse(Console.ReadLine()), seconds = int.Parse(Console.ReadLine());
                            DateTime date = new DateTime(year, month, day, hour, minutes, seconds);//Putting a random date and time
                            NewParcel.Requested = new DateTime(2021, 03, 15, 07, 00, 00);
                            NewParcel.Scheduled = new DateTime(2021, 03, 15, 07, 40, 00);
                            NewParcel.Delivered = new DateTime(2021, 03, 15, 10, 40, 00);
                            NewParcel.PickedUp = date;
                            break;
                    }
                    break;
                case "Update"://the user will choose whether he wants to update a parcel to a drone, parcel collection by a drone, parcel delivery to customer, sending drone to charge, and release from charge
                    Console.WriteLine("What object do you want to update?\n 1)AssignParcelToDrone\n 2)ParcelCollectionByDrone\n 3)ParcelDeliveryToCustomer\n 4)SendDroneToChargingStation\n 5)DroneReleaseFromChargingStation");
                    Answer = Console.ReadLine();
                    Console.WriteLine("Enter your parcel ID:");
                    int ID = int.Parse(Console.ReadLine());
                    Parcel TheParcel = new();
                    TheParcel = DalObject.DalObject.FindParcel(ID);//finding the parcel in the parcel stock 
                    Drone AvailableDrone = new();
                    switch (Answer)
                    {
                        case "AssignParcelToDrone"://case which assigns a parcel to a suitable drone
                            AvailableDrone = DalObject.DalObject.FindDroneAvailable();
                            AvailableDrone.Status = DroneStatuses.Delivery;
                            TheParcel.Scheduled = DateTime.Now;
                            TheParcel.Droneld = AvailableDrone.Id;
                            break;
                        case "ParcelCollectionByDrone"://case which updates when a parcel is collected by a drone
                            TheParcel.Delivered = DateTime.Now;
                            TheParcel.Droneld = 0;
                            break;
                        case "ParcelDeliveryToCustomer"://case which updates when a parcel is delivered to a customer
                            TheParcel.PickedUp = DateTime.Now;
                            DalObject.DalObject.UpdateParcelCounter("Decrease");
                            AvailableDrone.Status = DroneStatuses.Available;
                            AvailableDrone.Battery -= 25;
                            break;
                        case "SendDroneToChargingStation"://case which sends a low battey drone to be charged 
                            Drone LowBatteryDrone = new();//drone with low battery
                            DroneCharge NewDroneCharge = new();//drone with low battery will go be charged here
                            Console.WriteLine("Enter the ID of the Drone with low battery:\n");
                            int IdOfLowBatteryDrone = int.Parse(Console.ReadLine());//user entering drone with low battery
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
                        case "DroneReleaseFromChargingStation"://case which releases a fully charged drone from charging station
                            Console.WriteLine("Enter the ID of the Drone with charged battery:\n");
                            int IdOfChargedBatteryDrone = int.Parse(Console.ReadLine());
                            DroneCharge ChargedDrone = new();
                            ChargedDrone = DalObject.DalObject.FindDroneCharge(IdOfChargedBatteryDrone);
                            DalObject.DalObject.IncreaseChargeSlots(DalObject.DalObject.FindStation(ChargedDrone.StationId));//increasing amount of places left to charge
                            //למחוק את הבלה....
                            break;


                    }
                    break;
                case "Display"://the user will choose whether he wants to display the stations, drones, customers, or parcels
                    Console.WriteLine("What will you like to display?\n 1)Station\n 2)Drone\n 3)Customer\n 4)Parcel\n");
                    Answer = Console.ReadLine();
                    Console.WriteLine("Enter your ID number:");
                    int id = int.Parse(Console.ReadLine());
                    switch (Answer)
                    {
                        case "Station"://case which displays the requested station
                            Station s = new();
                            s = DalObject.DalObject.FindStation(id);
                            Console.WriteLine(s.ToString());
                            break;
                        case "Drone"://case which displays the requested drone
                            Drone d = new();
                            d = DalObject.DalObject.FindDrone(id);
                            Console.WriteLine(d.ToString());
                            break;
                        case "Customer"://case which displays the requested customer
                            Customer c = new();
                            c = DalObject.DalObject.FindCustomer(id);
                            Console.WriteLine(c.ToString());
                            break;
                        case "Parcel"://case which displays the requested parcel
                            Parcel p = new();
                            p = DalObject.DalObject.FindParcel(id);
                            Console.WriteLine(p.ToString());
                            break;

                    }
                    break;
                case "ListView"://the user will choose whether he wants to view the array of stations, customers, parcels, or parcels with no assigned drones, or stations available to charge
                    Console.WriteLine("What do you want to view?\n 1)Station\n 2)Drones\n 3)Customers\n 4)Parcels\n 5)ParcelsWithNoDrone\n 6)StationWithAvailableChargingStation\n ");
                    Answer = Console.ReadLine();
                    switch (Answer)
                    {
                        case "Station"://case which views the stations array
                            Station[] viewStations = DalObject.DalObject.ListViewStation();
                            viewStations.ToString();
                            break;
                        case "Drones"://case which views the drones array
                            Drone[] viewDrones = DalObject.DalObject.ListViewDrone();
                            for (int i = 0; i < viewDrones.Length; i++)
                            {
                                Console.WriteLine(viewDrones[i].ToString());
                            }
                            break;
                        case "Customers"://case which views the customers array
                            Customer[] viewCustomers = DalObject.DalObject.ListViewCustomer();
                            for (int i = 0; i < viewCustomers.Length; i++)
                            {
                                Console.WriteLine(viewCustomers[i].ToString());
                            }
                            break;
                        case "Parcels"://case which views the parcels array
                            Parcel[] viewParcels = DalObject.DalObject.ListViewParcel();
                            for (int i = 0; i < viewParcels.Length; i++)
                            {
                                Console.WriteLine(viewParcels[i].ToString());
                            }
                            break;
                        case "ParcelsWithNoDrone"://case which views the parcel with no assigned drones
                            Parcel[] ViewParcelsWithNoDrone = DalObject.DalObject.ParcelWithNoDrone();
                            for(int i = 0; i < ViewParcelsWithNoDrone.Length; i++)
                            {
                                Console.WriteLine(ViewParcelsWithNoDrone[i].ToString());
                            }
                            break;
                        case "StationWithAvailableChargingStation"://case which views the station with available charging stations
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
    }
}