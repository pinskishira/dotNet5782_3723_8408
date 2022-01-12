/*Yirat Biton 212628408 and Shira Pinski 214103723
 * Mini Project in Windows Systems
 */

using System;
using System.Collections.Generic;

using BO;
using static BO.Enum;

namespace ConsoleUI_BL
{
    public enum BlMainSwitchFunctions { Add = 1, Update, Display, ListView, Exit };
    public enum AddingFunction { AddStation = 1, AddDrone, AddCustomer, AddParcel, MainMenu };
    public enum UpdatingFunction { UpdateDrone = 1, UpdateStation, UpdateCustomer, SendDroneToChargingStation, DroneReleaseFromChargingStation, AssignParcelToDrone, ParcelCollectionByDrone, ParcelDeliveryToCustomer, MainMenu };
    public enum DisplayingFunction { Station = 1, Drone, Customer, Parcel, MainMenu };
    public enum ListViewFunction { Stations = 1, Drones, Customers, Parcels, ParcelsWithNoDrone, StationWithAvailableChargingStation, MainMenu };

    class ConsoleUI_BL
    {
        public static void Main(string[] args)
        {
            BlApi.Ibl ibl = BlApi.BlFactory.GetBl();
            int ansFromUserInt, input;
            double ansFromUserDouble1;
            double ansFromUserDouble2;
            AddingFunction answerAdd;
            BlMainSwitchFunctions answerMain;
            DisplayingFunction answerDisplay;
            UpdatingFunction answerUpdate;
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
                                "Enter 2 to add a drone \nEnter 3 to add a customer \nEnter 4 to add a parcel \nEnter 5 to return to main menu \n");
                            int.TryParse(Console.ReadLine(), out input);
                            answerAdd = (AddingFunction)input;
                            switch (answerAdd)
                            {
                                case AddingFunction.AddStation://case which adds a new station with data into the Stations list
                                    Station newStation = new Station();
                                    Console.Write("Enter a four-digit number in a new station ID number: ");
                                    int.TryParse(Console.ReadLine(), out input);
                                    newStation.Id = input;
                                    Console.Write("Enter new station name: ");
                                    newStation.Name = Console.ReadLine();
                                    Console.Write("Please enter you location: ");
                                    double.TryParse(Console.ReadLine(), out ansFromUserDouble1);
                                    double.TryParse(Console.ReadLine(), out ansFromUserDouble2);
                                    newStation.StationLocation = new Location();
                                    newStation.StationLocation.Longitude = ansFromUserDouble1;
                                    newStation.StationLocation.Latitude = ansFromUserDouble2;
                                    Console.Write("Enter amount of availbale charge slots in new station: ");
                                    int.TryParse(Console.ReadLine(), out input);
                                    newStation.AvailableChargeSlots = input;
                                    newStation.DronesInCharging = null;
                                    ibl.AddStation(newStation);
                                    break;
                                case AddingFunction.AddDrone://case which adds a new drone with data into the Drones list
                                    Drone newDrone = new Drone();
                                    Console.Write("Enter drone Id: ");
                                    int.TryParse(Console.ReadLine(), out input);
                                    newDrone.Id = input;
                                    Console.Write("Enter drone model: ");
                                    newDrone.Model = Console.ReadLine();
                                    Console.WriteLine("Enter drones maximum weight:\n1 - Easy\n2 - Medium\n3 - Heavy");
                                    int.TryParse(Console.ReadLine(), out input);
                                    newDrone.Weight = (WeightCategories)input;
                                    Console.Write("Enter station number: ");
                                    int.TryParse(Console.ReadLine(), out input);
                                    int stationNumber = input;
                                    ibl.AddDrone(newDrone, stationNumber);
                                    break;
                                case AddingFunction.AddCustomer://case which adds a new customer with data into the Customers list
                                    Customer NewCustomer = new Customer();
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
                                    NewCustomer.CustomerLocation = new Location();
                                    NewCustomer.CustomerLocation.Longitude = ansFromUserDouble1;
                                    NewCustomer.CustomerLocation.Latitude = ansFromUserDouble2;
                                    ibl.AddCustomer(NewCustomer);
                                    break;
                                case AddingFunction.AddParcel://case which adds a new parcel with data into the Parcels list
                                    Parcel NewParcel = new Parcel();
                                    NewParcel.Id = 0;
                                    Console.Write("Enter sender ID of 9 digits: ");
                                    int.TryParse(Console.ReadLine(), out ansFromUserInt);
                                    NewParcel.Sender = new CustomerInParcel();
                                    NewParcel.Sender.Id = ansFromUserInt;
                                    Console.Write("Enter target ID of 9 digits: ");
                                    int.TryParse(Console.ReadLine(), out ansFromUserInt);
                                    NewParcel.Target = new CustomerInParcel();
                                    NewParcel.Target.Id = ansFromUserInt;
                                    Console.WriteLine("Enter the weight of your parcel:\n1 - Easy\n2 - Medium\n3 - Heavy");
                                    int.TryParse(Console.ReadLine(), out input);
                                    NewParcel.Weight = (WeightCategories)input;
                                    Console.WriteLine("Enter the urgency of your parcel:\n1 - normal\n2 - Fast\n3 - Emergency");
                                    int.TryParse(Console.ReadLine(), out input);
                                    NewParcel.Priority = (Priorities)input;
                                    NewParcel.DroneParcel = null;
                                    ibl.AddParcel(NewParcel);
                                    break;
                                case AddingFunction.MainMenu:
                                    break;
                            }
                            break;
                        case BlMainSwitchFunctions.Update://the user will choose whether he wants to update a parcel to a drone, parcel collection by a drone,
                                                          //parcel delivery to customer, sending drone to charge, and release from charge
                            Console.WriteLine("What object do you want to update?\nEnter 1 to update a drone\n" +
                                "Enter 2 to update a station\nEnter 3 to update a customer\n" +
                                "Enter 4 to send drone to charging station\nEnter 5 for drone release from charging station\nEnter 6 to assign a parcel To a drone\n" +
                                "Enter 7 for parcel collection by drone\nEnter 8 for parcel delivery to customer\nEnter 9 to return to main menu \n");
                            int.TryParse(Console.ReadLine(), out input);
                            answerUpdate = (UpdatingFunction)input;
                            int idDrone, idcustomer, chargingSlots, idStation;
                            string name, customerPhone;
                            switch (answerUpdate)
                            {
                                case UpdatingFunction.UpdateDrone:
                                    Console.WriteLine("Enter your drone ID: ");
                                    int.TryParse(Console.ReadLine(), out idDrone);
                                    Console.Write("Enter the new model name for the drone: ");
                                    name = Console.ReadLine();
                                    ibl.UpdateDrone(idDrone, name);
                                    break;
                                case UpdatingFunction.UpdateStation:
                                    Console.WriteLine("Enter your station ID: ");
                                    int.TryParse(Console.ReadLine(), out idStation);
                                    Console.Write("Enter your station name: ");
                                    name = Console.ReadLine();
                                    Console.Write("Enter amount of charging slots: ");
                                    int.TryParse(Console.ReadLine(), out chargingSlots);
                                    ibl.UpdateStation(idStation, name, chargingSlots);
                                    break;
                                case UpdatingFunction.UpdateCustomer:
                                    Console.WriteLine("Enter your customer ID: ");
                                    int.TryParse(Console.ReadLine(), out idcustomer);
                                    Console.WriteLine("Enter your new customer name: ");
                                    name = Console.ReadLine();
                                    Console.WriteLine("Enter your new customer phone: ");
                                    customerPhone = Console.ReadLine();
                                    ibl.UpdateCustomer(idcustomer, name, customerPhone);
                                    break;
                                case UpdatingFunction.SendDroneToChargingStation://case which sends drone to a charging station
                                    Console.Write("Enter your drone ID: ");
                                    int.TryParse(Console.ReadLine(), out idDrone);
                                    ibl.SendDroneToChargingStation(idDrone);
                                    break;
                                case UpdatingFunction.DroneReleaseFromChargingStation://case which releases a drone from its charging slot
                                    Console.Write("Enter your drone ID: ");
                                    int.TryParse(Console.ReadLine(), out idDrone);
                                    ibl.DroneReleaseFromChargingStation(idDrone);
                                    break;
                                case UpdatingFunction.AssignParcelToDrone://case which assigns a parcel to a suitable drone
                                    Console.Write("Enter your drone ID: ");
                                    int.TryParse(Console.ReadLine(), out idDrone);
                                    ibl.UpdateAssignParcelToDrone(idDrone);
                                    break;
                                case UpdatingFunction.ParcelCollectionByDrone://case which updates when a parcel is collected by a drone
                                    Console.Write("Enter drone ID: ");
                                    int.TryParse(Console.ReadLine(), out idDrone);
                                    ibl.UpdateParcelCollectionByDrone(idDrone);
                                    break;
                                case UpdatingFunction.ParcelDeliveryToCustomer://case which updates when a parcel is delivered to a customer
                                    Console.Write("Enter drone ID: ");
                                    int.TryParse(Console.ReadLine(), out idDrone);
                                    ibl.UpdateParcelDeliveryToCustomer(idDrone);
                                    break;
                                case UpdatingFunction.MainMenu:
                                    break;
                            }
                            break;
                        case BlMainSwitchFunctions.Display: //the user will choose whether he wants to display the stations, drones, customers, or parcels
                            Console.WriteLine("What will you like to display?\nEnter 1 for station\nEnter 2 for drone\n" +
                             "Enter 3 for customer\nEnter 4 for parcel\nEnter 5 to return to main menu \n");
                            int.TryParse(Console.ReadLine(), out input);
                            answerDisplay = (DisplayingFunction)input;
                            Console.WriteLine("Enter your ID number: ");
                            int id;
                            int.TryParse(Console.ReadLine(), out id);
                            switch (answerDisplay)
                            {
                                case DisplayingFunction.Station://case which returns the requested station
                                    Console.WriteLine(ibl.GetStation(id));//finds station according to inputted id
                                    break;
                                case DisplayingFunction.Drone://case which returns the requested drone
                                    Console.WriteLine(ibl.GetDrone(id));//finds drone according to inputted id
                                    break;
                                case DisplayingFunction.Customer://case which returns the requested customer
                                    Console.WriteLine(ibl.GetCustomer(id));//finds customer according to inputted id
                                    break;
                                case DisplayingFunction.Parcel://case which returns the requested parcel
                                    Console.WriteLine(ibl.GetParcel(id));//finds parcel according to inputted id
                                    break;
                                case DisplayingFunction.MainMenu:
                                    break;
                            }
                            break;
                        case BlMainSwitchFunctions.ListView://the user will choose whether he wants to view the array of stations, customers, parcels,
                                                            //or parcels with no assigned drones, or stations available to charge
                            Console.WriteLine("What do you want to view?\nEnter 1 for stations\nEnter 2 for drones\n" +
                                "Enter 3 for customers\nEnter 4 for parcels\nEnter 5 for parcels with no drone\n" +
                                "Enter 6 for station with available charging stations\nEnter 7 to return to main menu \n ");
                            int.TryParse(Console.ReadLine(), out input);
                            answerListView = (ListViewFunction)input;
                            switch (answerListView)
                            {
                                case ListViewFunction.Stations://case which views the stations list
                                    IEnumerable<StationToList> viewStations = ibl.GetAllStations();
                                    foreach (var station in viewStations)//prints all stations
                                        Console.WriteLine(station);
                                    break;
                                case ListViewFunction.Drones://case which views the drones list
                                    IEnumerable<DroneToList> viewDrones = ibl.GetAllDrones();
                                    foreach (var drone in viewDrones)//prints all drones
                                        Console.WriteLine(drone);
                                    break;
                                case ListViewFunction.Customers://case which views the customers list
                                    IEnumerable<CustomerToList> viewCustomers = ibl.GetAllCustomers();
                                    foreach (var customer in viewCustomers)//prints all customers
                                        Console.WriteLine(customer);
                                    break;
                                case ListViewFunction.Parcels://case which views the parcels list
                                    IEnumerable<ParcelToList> viewParcels = ibl.GetAllParcels();
                                    foreach (var parcel in viewParcels)//prints all parcels
                                        Console.WriteLine(parcel);
                                    break;
                                case ListViewFunction.ParcelsWithNoDrone://case which views the parcel with no assigned drones
                                    ((List<ParcelToList>)ibl.GetAllParcels(parcel => parcel.StateOfParcel == ParcelState.Created))
                                        .ForEach(parcel => Console.WriteLine(parcel));
                                    break;
                                case ListViewFunction.StationWithAvailableChargingStation://case which views the station with available charging stations
                                    ((List<StationToList>)ibl.GetAllStations(station => station.AvailableChargeSlots > 0))
                                        .ForEach(station => Console.WriteLine(station));
                                    break;
                                case ListViewFunction.MainMenu:
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
                catch (FailedGetException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (DroneMaintananceException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (FailedToCollectParcelException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (ParcelDeliveryException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (ItemDoesNotExistException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            while (answerMain != BlMainSwitchFunctions.Exit);
        }
    }
}
/*
 * What would you like to perform?
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
Enter 7 to return to main menu

1
ID is 5772
Name Of Station is Bayit Vegan
Available Charge Slots is 10
Occupied Charge Slots is 0

ID is 4299
Name Of Station is Givat Shaul
Available Charge Slots is 19
Occupied Charge Slots is 0

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
Enter 7 to return to main menu

2
Id is 88856
Model is 123EST
MaxWeight is Easy
Battery is 78
Drone status is Delivery
Current location is Longitude is 29.88
Latitude is 34.89
Parcel number in transfer is 0

Id is 36053
Model is 234EST
MaxWeight is Heavy
Battery is 89
Drone status is Available
Current location is Longitude is 33.30
Latitude is 36.30
Parcel number in transfer is 0

Id is 84829
Model is 345EST
MaxWeight is Easy
Battery is 25
Drone status is Available
Current location is Longitude is 33.25
Latitude is 34.33
Parcel number in transfer is 0

Id is 33250
Model is 456EST
MaxWeight is Medium
Battery is 2
Drone status is Maintenance
Current location is Longitude is 33.30
Latitude is 36.33
Parcel number in transfer is 0

Id is 26648
Model is 567EST
MaxWeight is Easy
Battery is 54
Drone status is Delivery
Current location is Longitude is 33.30
Latitude is 36.33
Parcel number in transfer is 0

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
Enter 7 to return to main menu

3
Id is 973546612
Name is Avital
Phone is 0586322431
Parcels sent and delivered is 1
Parcels sent but not delivered is 0
Recived parcels is 1
Parcels on the way to customer is 0

Id is 480207142
Name is Hadar
Phone is 0522230982
Parcels sent and delivered is 0
Parcels sent but not delivered is 0
Recived parcels is 0
Parcels on the way to customer is 0

Id is 520861071
Name is Ayala
Phone is 0506876398
Parcels sent and delivered is 1
Parcels sent but not delivered is 1
Recived parcels is 0
Parcels on the way to customer is 0

Id is 166911047
Name is Dasi
Phone is 0506561043
Parcels sent and delivered is 0
Parcels sent but not delivered is 0
Recived parcels is 2
Parcels on the way to customer is 1

Id is 783340236
Name is Moshe
Phone is 0502350982
Parcels sent and delivered is 1
Parcels sent but not delivered is 0
Recived parcels is 1
Parcels on the way to customer is 0

Id is 747639708
Name is Ayalet
Phone is 0534456021
Parcels sent and delivered is 1
Parcels sent but not delivered is 0
Recived parcels is 1
Parcels on the way to customer is 0

Id is 403501779
Name is David
Phone is 0552356731
Parcels sent and delivered is 3
Parcels sent but not delivered is 0
Recived parcels is 1
Parcels on the way to customer is 1

Id is 423156062
Name is Shira
Phone is 0503782099
Parcels sent and delivered is 0
Parcels sent but not delivered is 0
Recived parcels is 0
Parcels on the way to customer is 0

Id is 201075691
Name is Yosef
Phone is 0504310431
Parcels sent and delivered is 0
Parcels sent but not delivered is 1
Recived parcels is 2
Parcels on the way to customer is 0

Id is 281017537
Name is John
Phone is 0506929115
Parcels sent and delivered is 1
Parcels sent but not delivered is 0
Recived parcels is 0
Parcels on the way to customer is 0

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
Enter 7 to return to main menu

4
ID is 10000
Sender name is Avital
Target name is Dasi
Weight is Easy
Priority is Normal
Parcel State is Provided

ID is 10001
Sender name is Ayalet
Target name is David
Weight is Heavy
Priority is Normal
Parcel State is Provided

ID is 10002
Sender name is Ayala
Target name is David
Weight is Medium
Priority is Emergency
Parcel State is Paired

ID is 10003
Sender name is Yosef
Target name is Dasi
Weight is Medium
Priority is Emergency
Parcel State is Paired

ID is 10004
Sender name is David
Target name is Avital
Weight is Medium
Priority is Emergency
Parcel State is Provided

ID is 10005
Sender name is David
Target name is Yosef
Weight is Medium
Priority is Emergency
Parcel State is Provided

ID is 10006
Sender name is Ayala
Target name is Ayalet
Weight is Easy
Priority is Fast
Parcel State is Provided

ID is 10007
Sender name is David
Target name is Moshe
Weight is Medium
Priority is Fast
Parcel State is Provided

ID is 10008
Sender name is Moshe
Target name is Yosef
Weight is Easy
Priority is Emergency
Parcel State is Provided

ID is 10009
Sender name is John
Target name is Dasi
Weight is Medium
Priority is Fast
Parcel State is Provided

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
Enter 7 to return to main menu

5
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
Enter 7 to return to main menu

6
ID is 5772
Name Of Station is Bayit Vegan
Available Charge Slots is 10
Occupied Charge Slots is 0

ID is 4299
Name Of Station is Givat Shaul
Available Charge Slots is 19
Occupied Charge Slots is 0

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
Enter 5 to return to main menu

2
Enter your ID number:
88856
Id is 88856
Model is 123EST
Weight is Easy
Battery is 78
Drone Status is Delivery
Parcel in transfer is
Current location is Longitude is 29.88
Latitude is 34.89

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
Enter 5 to return to main menu

4
Enter your ID number:
10004
Id is 10004
SenderId is
Id is 403501779
Name is David
TargetId is
Id is 973546612
Name is Avital
Weight is Medium
Priority is Emergency
Drone in parcel is
Id is 33250
Battery is 2
Current location is Longitude is 33.30
Latitude is 36.33

Requested is 09/07/2021 13:34:46
Scheduled is 09/07/2021 15:31:28
Delivered is 09/07/2021 16:35:58
Picked Up is 09/07/2021 15:56:19

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
Enter 5 to return to main menu

1
Enter a four-digit number in a new station ID number: 2398
Enter new station name: uziel
Please enter you location: 30
34
Enter amount of availbale charge slots in new station: 12
What would you like to perform?
Enter 1 to Add
Enter 2 to Update
Enter 3 to Display
Enter 4 to ListView
Enter 5 to Exit

2
What object do you want to update?
Enter 1 toupdate a drone
Enter 2 to update a station
Enter 3 to update a customer
Enter 4 to send drone to charging station
Enter 5 for drone release from charging station
Enter 6 to assign a parcel To a drone
Enter 7 for parcel collection by drone
Enter 8 for parcel delivery to customer
Enter 9 to return to main menu

1
Enter your drone ID:
33250
Enter the new model name for the drone 987EST
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
Enter 7 to return to main menu

1
ID is 5772
Name Of Station is Bayit Vegan
Available Charge Slots is 10
Occupied Charge Slots is 0

ID is 4299
Name Of Station is Givat Shaul
Available Charge Slots is 19
Occupied Charge Slots is 0

ID is 2398
Name Of Station is uziel
Available Charge Slots is 12
Occupied Charge Slots is 0

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
Enter 7 to return to main menu

2
Id is 88856
Model is 123EST
MaxWeight is Easy
Battery is 78
Drone status is Delivery
Current location is Longitude is 29.88
Latitude is 34.89
Parcel number in transfer is 0

Id is 36053
Model is 234EST
MaxWeight is Heavy
Battery is 89
Drone status is Available
Current location is Longitude is 33.30
Latitude is 36.30
Parcel number in transfer is 0

Id is 84829
Model is 345EST
MaxWeight is Easy
Battery is 25
Drone status is Available
Current location is Longitude is 33.25
Latitude is 34.33
Parcel number in transfer is 0

Id is 33250
Model is 987EST
MaxWeight is Medium
Battery is 2
Drone status is Maintenance
Current location is Longitude is 33.30
Latitude is 36.33
Parcel number in transfer is 0

Id is 26648
Model is 567EST
MaxWeight is Easy
Battery is 54
Drone status is Delivery
Current location is Longitude is 33.30
Latitude is 36.33
Parcel number in transfer is 0

What would you like to perform?
Enter 1 to Add
Enter 2 to Update
Enter 3 to Display
Enter 4 to ListView
Enter 5 to Exit

2
What object do you want to update?
Enter 1 toupdate a drone
Enter 2 to update a station
Enter 3 to update a customer
Enter 4 to send drone to charging station
Enter 5 for drone release from charging station
Enter 6 to assign a parcel To a drone
Enter 7 for parcel collection by drone
Enter 8 for parcel delivery to customer
Enter 9 to return to main menu

9
What would you like to perform?
Enter 1 to Add
Enter 2 to Update
Enter 3 to Display
Enter 4 to ListView
Enter 5 to Exit

5
 */