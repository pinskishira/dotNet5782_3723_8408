//Yirat Biton 212628408 and Shira Pinski 214103723
// * Mini Project in Windows Systems
// */
using System;
using DO;
using System.Collections.Generic;
using DalApi;
using static DO.Enum;

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
            IDal dalObj = DalApi.DLFactory.GetDL();
            int ansFromUserInt, input;
            double ansFromUserDouble;
            AddingFunction answerAdd;
            MainSwitchFunctions answerMain;
            DisplayingFunction answerDisplay;
            UpdateingFunction answerUpdate;
            ListViewFunction answerListView;

            do
            {
                Console.WriteLine("What would you like to perform?\nEnter 1 to Add \nEnter 2 to Update \n" +
                    "Enter 3 to Display \nEnter 4 to ListView \nEnter 5 to Exit \n");
                int.TryParse(Console.ReadLine(), out input);
                answerMain = (MainSwitchFunctions)input;
                try
                {

                    switch (answerMain)
                    {
                        case MainSwitchFunctions.Add://the user will choose whether he wants to add on a station, drone, customer or parcel
                            Console.WriteLine("What object would you like to add on? \nEnter 1 to add a station \n" +
                                "Enter 2 to add a drone \nEnter 3 to add a customer \nEnter 4 to add a parcel \n");
                            int.TryParse(Console.ReadLine(), out input);
                            answerAdd = (AddingFunction)input;
                            switch (answerAdd)
                            {
                                case AddingFunction.AddStation://case which adds a new station with data into the Stations array
                                    Station newStation = new();
                                    Console.Write("Enter new station Id: ");
                                    int.TryParse(Console.ReadLine(), out input);
                                    newStation.Id = input;
                                    Console.Write("Enter new station name: ");
                                    newStation.Name = Console.ReadLine();
                                    Console.Write("Please enter longitude: ");
                                    double.TryParse(Console.ReadLine(), out ansFromUserDouble);
                                    newStation.Longitude = ansFromUserDouble;
                                    Console.Write("Please enter Latitude: ");
                                    double.TryParse(Console.ReadLine(), out ansFromUserDouble);
                                    newStation.Latitude = ansFromUserDouble;
                                    Console.Write("Enter amount of charge slots in new station : ");
                                    int.TryParse(Console.ReadLine(), out input);
                                    newStation.AvailableChargeSlots = input;
                                    dalObj.AddStation(newStation);
                                    break;
                                case AddingFunction.AddDrone://case which adds a new drone with data into the Drones array
                                    Drone newDrone = new();
                                    Console.Write("Enter drone Id: ");
                                    int.TryParse(Console.ReadLine(), out input);
                                    newDrone.Id = input;
                                    Console.Write("Enter drone model: ");
                                    newDrone.Model = Console.ReadLine();
                                    Console.WriteLine("Enter drones' maximum weight:\n1 - Easy\n2 - Medium\n3 - Heavy");
                                    int.TryParse(Console.ReadLine(), out input);
                                    newDrone.Weight = (WeightCategories)input;
                                    Console.WriteLine("Enter drones' status:\n1 - Available\n2 - Maintanance\n3 - Delivery");
                                    int.TryParse(Console.ReadLine(), out input);
                                    //newDrone.Status = (DroneStatuses)input;
                                    Console.Write("Enter the drones' battery status: ");
                                    int.TryParse(Console.ReadLine(), out input);
                                    //newDrone.Battery = input;
                                    dalObj.AddDrone(newDrone);
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
                                    Console.Write("Please enter longitude: ");
                                    double.TryParse(Console.ReadLine(), out ansFromUserDouble);
                                    NewCustomer.Longitude = ansFromUserDouble;
                                    Console.Write("Please enter latitude: ");
                                    double.TryParse(Console.ReadLine(), out ansFromUserDouble);
                                    NewCustomer.Latitude = ansFromUserDouble;
                                    dalObj.AddCustomer(NewCustomer);
                                    break;
                                case AddingFunction.AddParcel://case which adds a new parcel with data into the Parcels array
                                    Parcel NewParcel = new();
                                    NewParcel.Id = 0;
                                    Console.Write("Enter sender ID of 9 digits: ");
                                    int.TryParse(Console.ReadLine(), out ansFromUserInt);
                                    NewParcel.SenderId = ansFromUserInt;
                                    Console.Write("Enter target ID of 9 digits: ");
                                    int.TryParse(Console.ReadLine(), out ansFromUserInt);
                                    NewParcel.TargetId = ansFromUserInt;
                                    Console.WriteLine("Enter the weight of your parcel:\n1 - Easy\n2 - Medium\n3 - Heavy");
                                    int.TryParse(Console.ReadLine(), out input);
                                    NewParcel.Weight = (WeightCategories)input;
                                    Console.WriteLine("Enter the urgency of your parcel:\n1 - normal\n2 - Fast\n3 - Emergency");
                                    int.TryParse(Console.ReadLine(), out input);
                                    NewParcel.Priority = (Priorities)input;
                                    NewParcel.DroneId = 0;
                                    NewParcel.Requested = DateTime.MinValue;
                                    NewParcel.Scheduled = DateTime.MinValue;
                                    NewParcel.Delivered = DateTime.MinValue;
                                    NewParcel.PickedUp = DateTime.MinValue;
                                    dalObj.AddParcel(NewParcel);
                                    break;
                            }
                            break;
                        case MainSwitchFunctions.Update://the user will choose whether he wants to update a parcel to a drone, parcel collection by a drone,
                                                        //parcel delivery to customer, sending drone to charge, and release from charge
                            Console.WriteLine("What object do you want to update?\nEnter 1 to assign a parcel to a drone\n" +
                                "Enter 2 for parcel collection by drone\nEnter 3 for parcel delivery to customer\n" +
                                "Enetr 4 to send drone to charging station\nEnter 5 for drone release from charging station\n");
                            int.TryParse(Console.ReadLine(), out input);
                            answerUpdate = (UpdateingFunction)input;
                            int IdParcel, IdDrone;
                            switch (answerUpdate)
                            {
                                case UpdateingFunction.AssignParcelToDrone://case which assigns a parcel to a suitable drone
                                    Console.Write("Enter your parcel ID: ");
                                    int.TryParse(Console.ReadLine(), out IdParcel);
                                    Console.Write("Enter your drone ID: ");
                                    int.TryParse(Console.ReadLine(), out IdDrone);
                                    dalObj.UpdateAssignParcelToDrone(IdParcel, IdDrone);
                                    break;
                                case UpdateingFunction.ParcelCollectionByDrone://case which updates when a parcel is collected by a drone
                                    Console.Write("Enter your parcel ID: ");
                                    int.TryParse(Console.ReadLine(), out IdParcel);
                                    dalObj.UpdateParcelCollectionByDrone(IdParcel);
                                    break;
                                case UpdateingFunction.ParcelDeliveryToCustomer://case which updates when a parcel is delivered to a customer
                                    Console.Write("Enter your parcel ID: ");
                                    int.TryParse(Console.ReadLine(), out IdParcel);
                                    dalObj.UpdateParcelDeliveryToCustomer(IdParcel);
                                    break;
                                case UpdateingFunction.SendDroneToChargingStation://case which sends a low battey drone to be charged 
                                    Console.Write("Enter the ID of the Drone with low battery: ");
                                    int IdOfLowBatteryDrone;
                                    int.TryParse(Console.ReadLine(), out IdOfLowBatteryDrone);//user entering drone with low battery
                                    Console.Write("Please enter your desired station: ");
                                    IEnumerable<Station> AvailableStation = dalObj.GetAllStations(item => item.AvailableChargeSlots > 0);//finding available station
                                    Console.Write("\n");
                                    int count = 1;
                                    foreach (var indexStation in AvailableStation)//user will have a few charging stations to choose from
                                    {
                                        if (indexStation.AvailableChargeSlots > 0)
                                            Console.WriteLine((count++) + " - " + indexStation.Name);
                                    }
                                    string ChosenStation = Console.ReadLine();
                                    dalObj.UpdateSendDroneToChargingStation(IdOfLowBatteryDrone, ChosenStation);
                                    break;
                                case UpdateingFunction.DroneReleaseFromChargingStation://case which releases a fully charged drone from charging station
                                    Console.Write("Enter the ID of the Drone with charged battery: ");
                                    int IdOfChargedBatteryDrone;
                                    int.TryParse(Console.ReadLine(), out input);
                                    IdOfChargedBatteryDrone = input;
                                    dalObj.DroneReleaseFromChargingStation(IdOfChargedBatteryDrone);
                                    break;
                            }
                            break;
                        case MainSwitchFunctions.Display://the user will choose whether he wants to display the stations, drones, customers, or parcels
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
                                    Station s = new();
                                    s = dalObj.FindStation(id);//finds station according to inputted id
                                    Console.WriteLine(s);
                                    break;
                                case DisplayingFunction.Drone://case which displays the requested drone
                                    Drone d = new();
                                    d = dalObj.FindDrone(id);//finds drone according to inputted id
                                    Console.WriteLine(d);
                                    break;
                                case DisplayingFunction.Customer://case which displays the requested customer
                                    Customer c = new();
                                    c = dalObj.FindCustomer(id);//finds customer according to inputted id
                                    Console.WriteLine(c);
                                    break;
                                case DisplayingFunction.Parcel://case which displays the requested parcel
                                    Parcel p = new();
                                    p = dalObj.FindParcel(id);//finds parcel according to inputted id
                                    Console.WriteLine(p);
                                    break;

                            }
                            break;
                        case MainSwitchFunctions.ListView://the user will choose whether he wants to view the array of stations, customers, parcels,
                                                          //or parcels with no assigned drones, or stations available to charge
                            Console.WriteLine("What do you want to view?\nEnter 1 for stations\nEnter 2 for drones\n" +
                                "Enter 3 for customers\nEnter 4 for parcels\nEnter 5 for parcels with no drone\n" +
                                "Enter 6 for station with available charging stations\n ");
                            int.TryParse(Console.ReadLine(), out input);
                            answerListView = (ListViewFunction)input;
                            switch (answerListView)
                            {
                                case ListViewFunction.Stations://case which views the stations array
                                    IEnumerable<Station> viewStations = dalObj.GetAllStations();
                                    foreach (var station in viewStations)//prints all stations
                                        Console.WriteLine(station);
                                    break;
                                case ListViewFunction.Drones://case which views the drones array
                                    IEnumerable<Drone> viewDrones = dalObj.GetAllDrones();
                                    foreach (var drone in viewDrones)//prints all drones
                                        Console.WriteLine(drone);
                                    break;
                                case ListViewFunction.Customers://case which views the customers array
                                    IEnumerable<Customer> viewCustomers = dalObj.GetAllCustomers();
                                    foreach (var customer in viewCustomers)//prints all customers
                                        Console.WriteLine(customer);
                                    break;
                                case ListViewFunction.Parcels://case which views the parcels array
                                    IEnumerable<Parcel> viewParcels = dalObj.GetAllParcels();
                                    foreach (var parcel in viewParcels)//prints all parcels
                                        Console.WriteLine(parcel);
                                    break;
                                case ListViewFunction.ParcelsWithNoDrone://case which views the parcel with no assigned drones
                                    IEnumerable<Parcel> ViewParcelsWithNoDrone = dalObj.GetAllParcels(item => item.DroneId == 0);
                                    foreach (var parcel in ViewParcelsWithNoDrone)//printing
                                        Console.WriteLine(parcel);
                                    break;
                                case ListViewFunction.StationWithAvailableChargingStation://case which views the station with available charging stations
                                    IEnumerable<Station> viewStationWithAvailableChargingStation = dalObj.GetAllStations(item => item.AvailableChargeSlots > 0);
                                    foreach (var station in viewStationWithAvailableChargingStation)//prints all parcels
                                        Console.WriteLine(station);
                                    break;
                            }
                            break;
                    }
                }
                catch (ItemExistsException message)
                {
                    Console.WriteLine(message);
                }
                catch (ItemDoesNotExistException message)
                {
                    Console.WriteLine(message);
                }
            }
            while (answerMain != MainSwitchFunctions.Exit);
        }
    }
}
/*
What would you like to perform?
Enter 1 to Add
Enter 2 to Update
Enter 3 to Display
Enter 4 to ListView
Enter 5 to Exit

1
What object would you like to add on?
Enter 1 to add a station
Enter 2 to add a drone
Enter 3 to add a customer
Enter 4 to add a parcel

1
Enter new station Id: 9287
Enter new station name: Ramot
Please enter longitude: 35.22
Please enter Latitude: 31.09
Enter amount of charge slots in new station : 30
What would you like to perform?
Enter 1 to Add
Enter 2 to Update
Enter 3 to Display
Enter 4 to ListView
Enter 5 to Exit

1
What object would you like to add on?
Enter 1 to add a station
Enter 2 to add a drone
Enter 3 to add a customer
Enter 4 to add a parcel

2
Enter drone Id: 29873
Enter drone model: 376EST
Enter drones' maximum weight:
1 - Easy
2 - Medium
3 - Heavy
1
Enter drones' status:
1 - Available
2 - Maintanance
3 - Delivery
1
Enter the drones' battery status: 100
What would you like to perform?
Enter 1 to Add
Enter 2 to Update
Enter 3 to Display
Enter 4 to ListView
Enter 5 to Exit

1
What object would you like to add on?
Enter 1 to add a station
Enter 2 to add a drone
Enter 3 to add a customer
Enter 4 to add a parcel

3
Please enter ID: 212628408
Please enter name: Yirat
Please enter phone: 0586310321
Please enter longitude: 35.99
Please enter latitude: 31.57
What would you like to perform?
Enter 1 to Add
Enter 2 to Update
Enter 3 to Display
Enter 4 to ListView
Enter 5 to Exit

1
What object would you like to add on?
Enter 1 to add a station
Enter 2 to add a drone
Enter 3 to add a customer
Enter 4 to add a parcel

4
Enter sender ID of 9 digits: 928716553
Enter target ID of 9 digits: 212765223
Enter the weight of your parcel:
1 - Easy
2 - Medium
3 - Heavy
1
Enter the urgency of your parcel:
1 - normal
2 - Fast
3 - Emergency
1
What would you like to perform?
Enter 1 to Add
Enter 2 to Update
Enter 3 to Display
Enter 4 to ListView
Enter 5 to Exit

4
What do you want to view?
Enter 1 for stations
Enter 2 for drones
Enter 3 for customers
Enter 4 for parcels
Enter 5 for parcels with no drone
Enter 6 for station with available charging stations

1
ID is 6439
Name is Bayit Vegan
Longitude is 35.56
Latitude is 31.81
ChargeSlots is 12

ID is 6868
Name is Givat Shaul
Longitude is 35.25
Latitude is 31.22
ChargeSlots is 21

ID is 9287
Name is Ramot
Longitude is 35.22
Latitude is 31.09
ChargeSlots is 30

What would you like to perform?
Enter 1 to Add
Enter 2 to Update
Enter 3 to Display
Enter 4 to ListView
Enter 5 to Exit

4
What do you want to view?
Enter 1 for stations
Enter 2 for drones
Enter 3 for customers
Enter 4 for parcels
Enter 5 for parcels with no drone
Enter 6 for station with available charging stations

2
ID is 48114
Model is 123EST
MaxWeight is Medium
Status is Delivery
Battery is 88

ID is 90386
Model is 234EST
MaxWeight is Heavy
Status is Delivery
Battery is 91

ID is 71626
Model is 345EST
MaxWeight is Heavy
Status is Delivery
Battery is 39

ID is 48145
Model is 456EST
MaxWeight is Medium
Status is Delivery
Battery is 4

ID is 99381
Model is 567EST
MaxWeight is Heavy
Status is Available
Battery is 40

ID is 29873
Model is 376EST
MaxWeight is Easy
Status is Available
Battery is 100

What would you like to perform?
Enter 1 to Add
Enter 2 to Update
Enter 3 to Display
Enter 4 to ListView
Enter 5 to Exit

4
What do you want to view?
Enter 1 for stations
Enter 2 for drones
Enter 3 for customers
Enter 4 for parcels
Enter 5 for parcels with no drone
Enter 6 for station with available charging stations

3
ID is 898948673
Name is Avital
Telephone is 058-6322431
Longitude is 35.85
Latitude is 31.32

ID is 979352977
Name is Hadar
Telephone is 052-2230982
Longitude is 35.03
Latitude is 31.57

ID is 389091757
Name is Ayala
Telephone is 050-6876398
Longitude is 35.03
Latitude is 31.00

ID is 618833406
Name is Dasi
Telephone is 050-6561043
Longitude is 35.77
Latitude is 31.54

ID is 490670271
Name is Moshe
Telephone is 050-2350982
Longitude is 35.46
Latitude is 31.13

ID is 921964131
Name is Ayalet
Telephone is 053-4456021
Longitude is 35.05
Latitude is 31.84

ID is 383251532
Name is David
Telephone is 055-2356731
Longitude is 35.29
Latitude is 31.07

ID is 694998025
Name is Shira
Telephone is 050-3782099
Longitude is 35.95
Latitude is 31.30

ID is 369953181
Name is Yosef
Telephone is 050-4310431
Longitude is 35.74
Latitude is 31.93

ID is 365982985
Name is John
Telephone is 050-6929115
Longitude is 35.90
Latitude is 31.19

ID is 212628408
Name is Yirat
Telephone is 058-6310321
Longitude is 35.99
Latitude is 31.57

What would you like to perform?
Enter 1 to Add
Enter 2 to Update
Enter 3 to Display
Enter 4 to ListView
Enter 5 to Exit

4
What do you want to view?
Enter 1 for stations
Enter 2 for drones
Enter 3 for customers
Enter 4 for parcels
Enter 5 for parcels with no drone
Enter 6 for station with available charging stations

4
ID is 10000000
SenderId is 898948673
TargetId is 383251532
Weight is Heavy
Priority is Fast
Droneld is 71626
Requested is 24/04/2021 22:14:31
Scheduled is 24/04/2021 23:05:24
PickedUp is 24/04/2021 23:27:32
Delivered is 24/04/2021 23:36:55

ID is 10000001
SenderId is 921964131
TargetId is 365982985
Weight is Easy
Priority is Emergency
Droneld is 48114
Requested is 10/06/2021 05:54:01
Scheduled is 10/06/2021 07:31:02
PickedUp is 10/06/2021 07:39:50
Delivered is 10/06/2021 08:09:59

ID is 10000002
SenderId is 979352977
TargetId is 618833406
Weight is Heavy
Priority is Emergency
Droneld is 90386
Requested is 14/04/2021 02:13:46
Scheduled is 14/04/2021 02:23:07
PickedUp is 14/04/2021 02:24:07
Delivered is 14/04/2021 02:46:19

ID is 10000003
SenderId is 898948673
TargetId is 369953181
Weight is Heavy
Priority is Fast
Droneld is 71626
Requested is 07/03/2021 03:42:05
Scheduled is 07/03/2021 08:08:27
PickedUp is 07/03/2021 08:53:24
Delivered is 07/03/2021 09:26:34

ID is 10000004
SenderId is 618833406
TargetId is 898948673
Weight is Medium
Priority is Normal
Droneld is 48114
Requested is 16/11/2021 22:37:01
Scheduled is 17/11/2021 01:18:14
PickedUp is 17/11/2021 01:38:08
Delivered is 17/11/2021 01:47:34

ID is 10000005
SenderId is 389091757
TargetId is 898948673
Weight is Easy
Priority is Emergency
Droneld is 48114
Requested is 20/07/2021 17:11:42
Scheduled is 20/07/2021 18:14:31
PickedUp is 20/07/2021 18:18:20
Delivered is 20/07/2021 19:00:23

ID is 10000006
SenderId is 921964131
TargetId is 898948673
Weight is Easy
Priority is Normal
Droneld is 48145
Requested is 04/08/2021 22:29:09
Scheduled is 04/08/2021 22:57:43
PickedUp is 04/08/2021 23:29:11
Delivered is 05/08/2021 00:25:05

ID is 10000007
SenderId is 921964131
TargetId is 694998025
Weight is Medium
Priority is Emergency
Droneld is 71626
Requested is 28/10/2021 08:58:56
Scheduled is 28/10/2021 13:30:11
PickedUp is 28/10/2021 14:23:45
Delivered is 28/10/2021 15:14:46

ID is 10000008
SenderId is 694998025
TargetId is 383251532
Weight is Heavy
Priority is Fast
Droneld is 71626
Requested is 17/03/2021 10:34:30
Scheduled is 17/03/2021 12:54:37
PickedUp is 17/03/2021 13:50:20
Delivered is 17/03/2021 14:30:47

ID is 10000009
SenderId is 618833406
TargetId is 389091757
Weight is Easy
Priority is Emergency
Droneld is 0
Requested is 09/12/2021 05:52:34
Scheduled is 01/01/0001 00:00:00
PickedUp is 01/01/0001 00:00:00
Delivered is 01/01/0001 00:00:00

ID is 10000010
SenderId is 928716553
TargetId is 212765223
Weight is Easy
Priority is Normal
Droneld is 0
Requested is 01/01/0001 00:00:00
Scheduled is 01/01/0001 00:00:00
PickedUp is 01/01/0001 00:00:00
Delivered is 01/01/0001 00:00:00

What would you like to perform?
Enter 1 to Add
Enter 2 to Update
Enter 3 to Display
Enter 4 to ListView
Enter 5 to Exit

2
What object do you want to update?
Enter 1 to assign a parcel to a drone
Enter 2 for parcel collection by drone
Enter 3 for parcel delivery to customer
Enetr 4 to send drone to charging station
Enter 5 for drone release from charging station

1
Enter your parcel ID: 10000009
Enter your drone ID: 99381
What would you like to perform?
Enter 1 to Add
Enter 2 to Update
Enter 3 to Display
Enter 4 to ListView
Enter 5 to Exit

2
What object do you want to update?
Enter 1 to assign a parcel to a drone
Enter 2 for parcel collection by drone
Enter 3 for parcel delivery to customer
Enetr 4 to send drone to charging station
Enter 5 for drone release from charging station

2
Enter your parcel ID: 10000009
What would you like to perform?
Enter 1 to Add
Enter 2 to Update
Enter 3 to Display
Enter 4 to ListView
Enter 5 to Exit

2
What object do you want to update?
Enter 1 to assign a parcel to a drone
Enter 2 for parcel collection by drone
Enter 3 for parcel delivery to customer
Enetr 4 to send drone to charging station
Enter 5 for drone release from charging station

3
Enter your parcel ID: 10000009
What would you like to perform?
Enter 1 to Add
Enter 2 to Update
Enter 3 to Display
Enter 4 to ListView
Enter 5 to Exit

2
What object do you want to update?
Enter 1 to assign a parcel to a drone
Enter 2 for parcel collection by drone
Enter 3 for parcel delivery to customer
Enetr 4 to send drone to charging station
Enter 5 for drone release from charging station

4
Enter the ID of the Drone with low battery: 48145
Please enter your desired station:
1 - Bayit Vegan
2 - Givat Shaul
3 - Ramot
Ramot
What would you like to perform?
Enter 1 to Add
Enter 2 to Update
Enter 3 to Display
Enter 4 to ListView
Enter 5 to Exit

2
What object do you want to update?
Enter 1 to assign a parcel to a drone
Enter 2 for parcel collection by drone
Enter 3 for parcel delivery to customer
Enetr 4 to send drone to charging station
Enter 5 for drone release from charging station

5
Enter the ID of the Drone with charged battery: 48145
What would you like to perform?
Enter 1 to Add
Enter 2 to Update
Enter 3 to Display
Enter 4 to ListView
Enter 5 to Exit

3
What will you like to display?
Enter 1 for station
Enter 2 for drone
Enter 3 for customer
Enter 4 for parcel

3
Enter your ID number:
212628408

ID is 212628408
Name is Yirat
Telephone is 058-6310321
Longitude is 35.99
Latitude is 31.57

What would you like to perform?
Enter 1 to Add
Enter 2 to Update
Enter 3 to Display
Enter 4 to ListView
Enter 5 to Exit

3
What will you like to display?
Enter 1 for station
Enter 2 for drone
Enter 3 for customer
Enter 4 for parcel

1
Enter your ID number:
6439

ID is 6439
Name is Bayit Vegan
Longitude is 35.56
Latitude is 31.81
ChargeSlots is 12

What would you like to perform?
Enter 1 to Add
Enter 2 to Update
Enter 3 to Display
Enter 4 to ListView
Enter 5 to Exit

4
What do you want to view?
Enter 1 for stations
Enter 2 for drones
Enter 3 for customers
Enter 4 for parcels
Enter 5 for parcels with no drone
Enter 6 for station with available charging stations

1
ID is 6439
Name is Bayit Vegan
Longitude is 35.56
Latitude is 31.81
ChargeSlots is 12

ID is 6868
Name is Givat Shaul
Longitude is 35.25
Latitude is 31.22
ChargeSlots is 21

ID is 9287
Name is Ramot
Longitude is 35.22
Latitude is 31.09
ChargeSlots is 30

What would you like to perform?
Enter 1 to Add
Enter 2 to Update
Enter 3 to Display
Enter 4 to ListView
Enter 5 to Exit

4
What do you want to view?
Enter 1 for stations
Enter 2 for drones
Enter 3 for customers
Enter 4 for parcels
Enter 5 for parcels with no drone
Enter 6 for station with available charging stations

2
ID is 48114
Model is 123EST
MaxWeight is Medium
Status is Delivery
Battery is 88

ID is 90386
Model is 234EST
MaxWeight is Heavy
Status is Delivery
Battery is 91

ID is 71626
Model is 345EST
MaxWeight is Heavy
Status is Delivery
Battery is 39

ID is 48145
Model is 456EST
MaxWeight is Medium
Status is Available
Battery is 100

ID is 99381
Model is 567EST
MaxWeight is Heavy
Status is Available
Battery is 40

ID is 29873
Model is 376EST
MaxWeight is Easy
Status is Available
Battery is 100

What would you like to perform?
Enter 1 to Add
Enter 2 to Update
Enter 3 to Display
Enter 4 to ListView
Enter 5 to Exit

4
What do you want to view?
Enter 1 for stations
Enter 2 for drones
Enter 3 for customers
Enter 4 for parcels
Enter 5 for parcels with no drone
Enter 6 for station with available charging stations

3
ID is 898948673
Name is Avital
Telephone is 058-6322431
Longitude is 35.85
Latitude is 31.32

ID is 979352977
Name is Hadar
Telephone is 052-2230982
Longitude is 35.03
Latitude is 31.57

ID is 389091757
Name is Ayala
Telephone is 050-6876398
Longitude is 35.03
Latitude is 31.00

ID is 618833406
Name is Dasi
Telephone is 050-6561043
Longitude is 35.77
Latitude is 31.54

ID is 490670271
Name is Moshe
Telephone is 050-2350982
Longitude is 35.46
Latitude is 31.13

ID is 921964131
Name is Ayalet
Telephone is 053-4456021
Longitude is 35.05
Latitude is 31.84

ID is 383251532
Name is David
Telephone is 055-2356731
Longitude is 35.29
Latitude is 31.07

ID is 694998025
Name is Shira
Telephone is 050-3782099
Longitude is 35.95
Latitude is 31.30

ID is 369953181
Name is Yosef
Telephone is 050-4310431
Longitude is 35.74
Latitude is 31.93

ID is 365982985
Name is John
Telephone is 050-6929115
Longitude is 35.90
Latitude is 31.19

ID is 212628408
Name is Yirat
Telephone is 058-6310321
Longitude is 35.99
Latitude is 31.57

What would you like to perform?
Enter 1 to Add
Enter 2 to Update
Enter 3 to Display
Enter 4 to ListView
Enter 5 to Exit

4
What do you want to view?
Enter 1 for stations
Enter 2 for drones
Enter 3 for customers
Enter 4 for parcels
Enter 5 for parcels with no drone
Enter 6 for station with available charging stations

4
ID is 10000000
SenderId is 898948673
TargetId is 383251532
Weight is Heavy
Priority is Fast
Droneld is 71626
Requested is 24/04/2021 22:14:31
Scheduled is 24/04/2021 23:05:24
PickedUp is 24/04/2021 23:27:32
Delivered is 24/04/2021 23:36:55

ID is 10000001
SenderId is 921964131
TargetId is 365982985
Weight is Easy
Priority is Emergency
Droneld is 48114
Requested is 10/06/2021 05:54:01
Scheduled is 10/06/2021 07:31:02
PickedUp is 10/06/2021 07:39:50
Delivered is 10/06/2021 08:09:59

ID is 10000002
SenderId is 979352977
TargetId is 618833406
Weight is Heavy
Priority is Emergency
Droneld is 90386
Requested is 14/04/2021 02:13:46
Scheduled is 14/04/2021 02:23:07
PickedUp is 14/04/2021 02:24:07
Delivered is 14/04/2021 02:46:19

ID is 10000003
SenderId is 898948673
TargetId is 369953181
Weight is Heavy
Priority is Fast
Droneld is 71626
Requested is 07/03/2021 03:42:05
Scheduled is 07/03/2021 08:08:27
PickedUp is 07/03/2021 08:53:24
Delivered is 07/03/2021 09:26:34

ID is 10000004
SenderId is 618833406
TargetId is 898948673
Weight is Medium
Priority is Normal
Droneld is 48114
Requested is 16/11/2021 22:37:01
Scheduled is 17/11/2021 01:18:14
PickedUp is 17/11/2021 01:38:08
Delivered is 17/11/2021 01:47:34

ID is 10000005
SenderId is 389091757
TargetId is 898948673
Weight is Easy
Priority is Emergency
Droneld is 48114
Requested is 20/07/2021 17:11:42
Scheduled is 20/07/2021 18:14:31
PickedUp is 20/07/2021 18:18:20
Delivered is 20/07/2021 19:00:23

ID is 10000006
SenderId is 921964131
TargetId is 898948673
Weight is Easy
Priority is Normal
Droneld is 48145
Requested is 04/08/2021 22:29:09
Scheduled is 04/08/2021 22:57:43
PickedUp is 04/08/2021 23:29:11
Delivered is 05/08/2021 00:25:05

ID is 10000007
SenderId is 921964131
TargetId is 694998025
Weight is Medium
Priority is Emergency
Droneld is 71626
Requested is 28/10/2021 08:58:56
Scheduled is 28/10/2021 13:30:11
PickedUp is 28/10/2021 14:23:45
Delivered is 28/10/2021 15:14:46

ID is 10000008
SenderId is 694998025
TargetId is 383251532
Weight is Heavy
Priority is Fast
Droneld is 71626
Requested is 17/03/2021 10:34:30
Scheduled is 17/03/2021 12:54:37
PickedUp is 17/03/2021 13:50:20
Delivered is 17/03/2021 14:30:47

ID is 10000009
SenderId is 618833406
TargetId is 389091757
Weight is Easy
Priority is Emergency
Droneld is 0
Requested is 09/12/2021 05:52:34
Scheduled is 02/11/2021 19:36:44
PickedUp is 02/11/2021 19:37:08
Delivered is 02/11/2021 19:37:23

ID is 10000010
SenderId is 928716553
TargetId is 212765223
Weight is Easy
Priority is Normal
Droneld is 0
Requested is 01/01/0001 00:00:00
Scheduled is 01/01/0001 00:00:00
PickedUp is 01/01/0001 00:00:00
Delivered is 01/01/0001 00:00:00

What would you like to perform?
Enter 1 to Add
Enter 2 to Update
Enter 3 to Display
Enter 4 to ListView
Enter 5 to Exit

4
What do you want to view?
Enter 1 for stations
Enter 2 for drones
Enter 3 for customers
Enter 4 for parcels
Enter 5 for parcels with no drone
Enter 6 for station with available charging stations

5
ID is 10000009
SenderId is 618833406
TargetId is 389091757
Weight is Easy
Priority is Emergency
Droneld is 0
Requested is 09/12/2021 05:52:34
Scheduled is 02/11/2021 19:36:44
PickedUp is 02/11/2021 19:37:08
Delivered is 02/11/2021 19:37:23

ID is 10000010
SenderId is 928716553
TargetId is 212765223
Weight is Easy
Priority is Normal
Droneld is 0
Requested is 01/01/0001 00:00:00
Scheduled is 01/01/0001 00:00:00
PickedUp is 01/01/0001 00:00:00
Delivered is 01/01/0001 00:00:00

What would you like to perform?
Enter 1 to Add
Enter 2 to Update
Enter 3 to Display
Enter 4 to ListView
Enter 5 to Exit

4
What do you want to view?
Enter 1 for stations
Enter 2 for drones
Enter 3 for customers
Enter 4 for parcels
Enter 5 for parcels with no drone
Enter 6 for station with available charging stations

6
ID is 6439
Name is Bayit Vegan
Longitude is 35.56
Latitude is 31.81
ChargeSlots is 12

ID is 6868
Name is Givat Shaul
Longitude is 35.25
Latitude is 31.22
ChargeSlots is 21

ID is 9287
Name is Ramot
Longitude is 35.22
Latitude is 31.09
ChargeSlots is 30

What would you like to perform?
Enter 1 to Add
Enter 2 to Update
Enter 3 to Display
Enter 4 to ListView
Enter 5 to Exit

5
*/
