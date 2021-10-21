using System;
using IDAL.DO;
Random rand = new Random(DateTime.Now.Millisecond);
/// <summary>
/// 
/// </summary>
namespace ConsoleUI
{

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("What would you like to perform?\n You can choose: \n 1) Add \n 2) Update \n 3) Display \n 4) ListView \n");
            string Answer = Console.ReadLine();
            switch (Answer)
            {
                case "Add":
                    Console.WriteLine("What object would you like to add on? \n 1) AddStation \n 2) AddDrone \n 3) AddCustomer \n 4) AddParcel \n");
                    Answer = Console.ReadLine();
                    switch (Answer)
                    {
                        case "AddStation":
                            Station NewStation = new() { Id = 1234, Name = "Uziel", Longitude = 35.183695, Latitude = 31.772032, ChargeSlots = 9 };
                            DalObject.DalObject.AddStation(NewStation);
                            break;
                        case "AddDrone":
                            Drone NewDrone = new() { Id = 12345, Model = "", MaxWeight = WeightCategories.Heavy, Status = DroneStatuses.Available, Battery = 100 };
                            DalObject.DalObject.AddDrone(NewDrone);
                            break;
                        case "AddCustomer":
                            Console.WriteLine("Please enter ID:");
                            Customer NewCustomer = new();
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
                        case "AddParcel":
                            DalObject.DalObject.UpdateParcelCounter("Increase");
                            Parcel NewParcel = new();
                            NewParcel.Id = 23;
                            Console.WriteLine("Enter sender ID of 3 digits:");
                            NewParcel.SenderId = int.Parse(Console.ReadLine());
                            Console.WriteLine("Enter target ID of 9 digits:");
                            NewParcel.TargetId = int.Parse(Console.ReadLine());
                            Console.WriteLine("Enter the weight of your parcel:\n 1)Easy\n 2) Medium\n 3) Heavy\n");
                            string WeightAnswer = Console.ReadLine();
                            switch (WeightAnswer)
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
                            Console.WriteLine("Enter the weight of your parcel:\n 1)normal\n 2) Fast\n 3) Emergency\n");
                            string PriorityAnswer = Console.ReadLine();
                            switch (PriorityAnswer)
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
                            Console.WriteLine("Enter the date: year, month and day. Time: hour, minutes and seonds:");
                            int year = int.Parse(Console.ReadLine()), month = int.Parse(Console.ReadLine()), day = int.Parse(Console.ReadLine()), hour = int.Parse(Console.ReadLine()), minutes = int.Parse(Console.ReadLine()), seconds = int.Parse(Console.ReadLine());
                            DateTime date = new DateTime(year, month, day, hour, minutes, seconds);//Putting a random date and time
                            NewParcel.Requested = new DateTime(2021, 03, 15, 07, 00, 00);
                            NewParcel.Scheduled = new DateTime(2021, 03, 15, 07, 40, 00);
                            NewParcel.Delivered = new DateTime(2021, 03, 15, 10, 40, 00);
                            NewParcel.PickedUp = date;
                            break;
                    }
                    break;
                case "Update":
                    Console.WriteLine("What object do you want to update?\n 1)AssignParcelToDrone\n 2)ParcelCollectionByDrone\n 3)ParcelDeliveryToCustomer\n 4)SendDroneToChargingStation\n 5)DroneReleaseFromChargingStation");
                    Answer = Console.ReadLine();
                    Console.WriteLine("Enter your Parcel ID:");
                    int ID = int.Parse(Console.ReadLine());
                    Parcel TheParcel = new();
                    TheParcel = DalObject.DalObject.FindParcel(ID);
                    Drone AvailableDrone = new();
                    switch (Answer)
                    {
                        case "AssignParcelToDrone":
                            AvailableDrone = DalObject.DalObject.FindDroneAvailable();
                            AvailableDrone.Status = DroneStatuses.Delivery;
                            TheParcel.Scheduled = DateTime.Now;
                            TheParcel.Droneld = AvailableDrone.Id;
                            break;
                        case "ParcelCollectionByDrone":
                            TheParcel.Delivered = DateTime.Now;
                            TheParcel.Droneld = 0;
                            break;
                        case "ParcelDeliveryToCustomer":
                            TheParcel.PickedUp = DateTime.Now;
                            DalObject.DalObject.UpdateParcelCounter("Decrease");
                            AvailableDrone.Status = DroneStatuses.Available;
                            AvailableDrone.Battery -= 25;
                            break;
                        case "SendDroneToChargingStation":
                            Drone LowBatteryDrone = new();//drone with low battery
                            DroneCharge NewDroneCharge = new();//drone with low battery will go be charged here
                            Console.WriteLine("Enter the ID of the Drone with low battery:\n");
                            int IdOfLowBatteryDrone = int.Parse(Console.ReadLine());//user entering drone with low battery
                            LowBatteryDrone = DalObject.DalObject.FindDroneAvailable();//finding the drone id in the drone stock
                            LowBatteryDrone.Status = DroneStatuses.Maintenance;//saying that the drone is in maintanance and unavailable to deliver
                            NewDroneCharge.DroneId = IdOfLowBatteryDrone;//putting id of low battery drone into its charging station
                            Console.WriteLine("Please enter your desired station:\n");
                            Station[] AvailableStation = new Station[DalObject.DalObject.GetIndexStation()];
                            AvailableStation = DalObject.DalObject.GetStationWithFreeSlots();
                            for (int i = 0; i < DalObject.DalObject.GetIndexStation(); i++)
                            {
                                if (AvailableStation[i].ChargeSlots > 0)
                                    Console.WriteLine(i + 1 + ") " + AvailableStation[i].Name + "\n");
                            }
                            string ChosenStation = Console.ReadLine();
                            int IdStation = DalObject.DalObject.FindStationId(ChosenStation);
                            NewDroneCharge.StationId = IdStation;
                            DalObject.DalObject.AddDroneCharge(NewDroneCharge);
                            DalObject.DalObject.DecreaseChargeSlots(DalObject.DalObject.FindStation(IdStation));

                            break;
                        case "DroneReleaseFromChargingStation":
                            Console.WriteLine("Enter the ID of the Drone with charged battery:\n");
                            int IdOfChargedBatteryDrone = int.Parse(Console.ReadLine());
                            DroneCharge ChargedDrone = new();
                            ChargedDrone = DalObject.DalObject.FindDroneCharge(IdOfChargedBatteryDrone);
                            DalObject.DalObject.IncreaseChargeSlots(DalObject.DalObject.FindStation(ChargedDrone.StationId));
                            //למחוק את הבלה....
                            break;


                    }
                    break;
                case "Display":
                    Console.WriteLine("מה אתה רוצה");
                    Answer = Console.ReadLine();
                    Console.WriteLine("תכניס מספר מזהה");
                    int id = int.Parse(Console.ReadLine());
                    switch (Answer)
                    {
                        case "station":
                            Station s = new();
                            s = DalObject.DalObject.FindStation(id);
                            Console.WriteLine(s.ToString());
                            break;
                        case "drone":
                            Drone d = new();
                            d = DalObject.DalObject.FindDrone(id);
                            Console.WriteLine(d.ToString());
                            break;
                        case "customer":
                            Customer c = new();
                            c = DalObject.DalObject.FindCustomer(id);
                            Console.WriteLine(c.ToString());
                            break;
                        case "parcel":
                            Parcel p = new();
                            p = DalObject.DalObject.FindParcel(id);
                            Console.WriteLine(p.ToString());
                            break;

                    }
                    break;
                case "ListView":
                    Console.WriteLine("What do you want to view?\n 1)Station\n 2)Drones\n 3)Customers\n 4)Parcels\n 5)ParcelsWithNoDrone\n 6)StationWithAvailableChargingStation\n ");
                    Answer = Console.ReadLine();
                    switch (Answer)
                    {
                        case "Station":
                            Station s = new();
                            s = DalObject.DalObject.FindStation(id);
                            Console.WriteLine(s.ToString());
                            break;
                        case "Drones":
                            Drone d = new();
                            d = DalObject.DalObject.FindDrone(id);
                            Console.WriteLine(d.ToString());
                            break;
                        case "Customers":
                            Customer c = new();
                            c = DalObject.DalObject.FindCustomer(id);
                            Console.WriteLine(c.ToString());
                            break;
                        case "Parcels":
                            Parcel p = new();
                            p = DalObject.DalObject.FindParcel(id);
                            Console.WriteLine(p.ToString());
                            break;
                        case "ParcelsWithNoDrone":
                            break;
                        case "StationWithAvailableChargingStation":
                            break;

                    }
                    break;
            }
        }
    }
}