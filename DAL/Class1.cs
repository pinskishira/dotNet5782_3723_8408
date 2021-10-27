using System;
using IDAL.DO;
Random rand = new Random(DateTime.Now.Millisecond);
/// <summary>
/// DalObject defines arrays for the stations, drones, customers, parcels and drone charges and then updates them and fills them with data.
/// It also includes adding functions for all arrays as well as searching functions.
/// </summary>
namespace DalObject
{
    public class DataSource
    {
        static internal Drone[] Drones = new Drone[10];//Defining an array of size 10 for the drones
        static internal Station[] Stations = new Station[5];//Defining an array of size 5 for the stations
        static internal Customer[] Customers = new Customer[100];//Defining an array of size 100 for the customers
        static internal Parcel[] Parcels = new Parcel[1000];//Defining an array of size 10000 for the parcels
        static internal DroneCharge[] DroneCharges = new DroneCharge[100];//Defining an array of size 100 for the drone charges
        /// <summary>
        /// Has all my static indexes
        /// </summary>
        internal class Config
        {
            static internal int IndexDrone = 0, IndexStation = 0, IndexCustomer = 0, IndexParcel = 0, IndexDroneCharge = 0, ParcelCounter = 0;
        }
        public static Random rand = new Random(DateTime.Now.Millisecond);
        /// <summary>
        /// Functions that updates and fills with data the information in the arrays
        /// </summary>
        public static void Initialize()
        { 
            for (int loopStation = 0; loopStation < 2; loopStation++)//Updating 2 base stations
            {
                Stations[loopStation].Id = rand.Next(1000, 10000);//Updating 4-digit ID name 
                for (int help = 0; help < loopStation; help++)//Checking if it already appears in array
                {
                    while (Stations[loopStation].Id == Stations[help].Id)
                        Stations[loopStation].Id = rand.Next(1000, 10000);
                }
                Stations[loopStation].Longitude = rand.Next(100, 1000);//Updating longitude                                     
                Stations[loopStation].Latitude = rand.Next(100, 1000);//Updating latitude    
                Stations[loopStation].ChargeSlots = rand.Next(0, 30);//Updating charging slots
                DataSource.Config.IndexStation++;//Promoting the index
            }
            Stations[0].Name = "Bayit Vegan";//Updating names
            Stations[1].Name = "Givat Shaul";
            for (int i = 0; i < 5; i++)//Updating 5 drones
            {
                Drones[i].Id = rand.Next(10000, 100000);//Updating 5-digit ID name 
                for (int j = 0; j < i; j++)//Checking if it already appears in array
                {
                    while (Drones[i].Id == Drones[j].Id)
                        Drones[i].Id = rand.Next(10000, 100000);
                }
                Drones[i].MaxWeight = (WeightCategories)rand.Next(0, 3);//Updating the weight category
                Drones[i].Status = (DroneStatuses)rand.Next(0, 2);//Updating drone status
                Drones[i].Battery = rand.Next(0, 101);//Updating battery status
                Drones[i].Model = "";//Updating model
                DataSource.Config.IndexDrone++;//Promoting the index
            }
            for (int i = 0; i < 10; i++)//Updating 10 customers
            {
                Customers[i].Id = rand.Next(100000000, 1000000000);//Updating ID name randomly
                for (int j = 0; j < i; j++)//Checking if it already appears in array
                {
                    while (Customers[i].Id == Customers[j].Id)
                        Customers[i].Id = rand.Next(100, 1000);
                }
                Customers[i].Longitude = rand.Next(100, 1000);//Updating longitude   
                Customers[i].Latitude = rand.Next(100, 1000);//Updating latitude    
                DataSource.Config.IndexCustomer++;//Promoting the index
            }
            //Updating customer names
            Customers[0].Name = "Avital";
            Customers[1].Name = "Hadar";
            Customers[2].Name = "Ayala";
            Customers[3].Name = "Dasi";
            Customers[4].Name = "Moshe";
            Customers[5].Name = "Ayalet";
            Customers[6].Name = "David";
            Customers[7].Name = "Shira";
            Customers[8].Name = "Yosef";
            Customers[9].Name = "John";
            //Updating customers phone numbers
            Customers[0].Phone = "058-6322431";
            Customers[1].Phone = "052-2230982";
            Customers[2].Phone = "050-6876398";
            Customers[3].Phone = "050-6561043";
            Customers[4].Phone = "050-2350982";
            Customers[5].Phone = "053-4456021";
            Customers[6].Phone = "055-2356731";
            Customers[7].Phone = "050-3782099";
            Customers[8].Phone = "050-4310431";
            Customers[9].Phone = "050-6929115";
            for (int i = 0; i < 10; i++)//Updating 10 parcels
            {
                Parcels[i].Id = rand.Next(1, 1000);//Updating the ID number of the package
                Parcels[i].SenderId = rand.Next(100, 1000);//Updating the ID number of the sender
                Parcels[i].Droneld = 0;//Updating the ID number of the drone
                Parcels[i].TargetId = rand.Next(100000000, 1000000000);//Updating the ID number of the customer
                for (int j = 0; j < i; j++)//Checking if it already appears in array
                {
                    while (Parcels[i].Id == Parcels[j].Id)
                        Parcels[i].Id = rand.Next(100, 1000);
                }
                Parcels[i].Weight = (WeightCategories)rand.Next(0, 3);//Updating the weight
                Parcels[i].Priority = (Priorities)rand.Next(0, 3);//Updating the urgency of the shipment
                DateTime date = new DateTime(2021, rand.Next(1, 13), rand.Next(1, 30), rand.Next(1, 25), rand.Next(1, 61), rand.Next(1, 61));//Putting a random date and time
                Parcels[i].Requested = date;
                Parcels[i].Scheduled = date.AddMinutes(rand.Next(1, 61));//Scheduling a time to deliver parcel
                Parcels[i].Delivered = date.AddHours(rand.Next(1, 61));//Time drone came to deliver parcel
                Parcels[i].PickedUp = date.AddDays(rand.Next(1, 15));//Time customer recieved parcel
                DataSource.Config.IndexParcel++;//Promoting the index
                DataSource.Config.ParcelCounter++;//Promoting the index
            }
        }
    }
    /// <summary>
    /// Functions that add and search on the arrays
    /// </summary>
    public class DalObject
    {
        /// <summary>
        /// Adding a new station to the array of stations
        /// </summary>
        /// <param name="NewStation"></param>
        public static void AddStation(Station NewStation)
        {
            DataSource.Stations[DataSource.Config.IndexStation] = NewStation;
            DataSource.Config.IndexStation++;//Promoting the index
        }
        /// <summary>
        /// Adding a new  drone to the array of drones
        /// </summary>
        /// <param name="NewDrone"></param>
        public static void AddDrone(Drone NewDrone)
        {
            DataSource.Drones[DataSource.Config.IndexDrone] = NewDrone;
            DataSource.Config.IndexDrone++;//Promoting the index
        }
        /// <summary>
        /// Adding a new customer to the array of customers
        /// </summary>
        /// <param name="NewCustomer"></param>
        public static void AddCustomer(Customer NewCustomer)
        {
            DataSource.Customers[DataSource.Config.IndexCustomer] = NewCustomer;
            DataSource.Config.IndexCustomer++;//Promoting the index
        }
        /// <summary>
        /// Adding a new parcel to the array of parcels
        /// </summary>
        /// <param name="NewParcel"></param>
        public static void AddParcel(Parcel NewParcel)
        {
            DataSource.Parcels[DataSource.Config.IndexParcel].Id = DataSource.Config.ParcelCounter++;
            DataSource.Parcels[DataSource.Config.IndexParcel].SenderId = NewParcel.SenderId;
            DataSource.Parcels[DataSource.Config.IndexParcel].TargetId = NewParcel.TargetId;
            DataSource.Parcels[DataSource.Config.IndexParcel].Weight = NewParcel.Weight;
            DataSource.Parcels[DataSource.Config.IndexParcel].Priority = NewParcel.Priority;
            DataSource.Parcels[DataSource.Config.IndexParcel].Requested = NewParcel.Requested;
            DataSource.Parcels[DataSource.Config.IndexParcel].Scheduled = NewParcel.Scheduled;
            DataSource.Parcels[DataSource.Config.IndexParcel].Delivered = NewParcel.Delivered;
            DataSource.Parcels[DataSource.Config.IndexParcel].PickedUp = NewParcel.PickedUp;
            DataSource.Parcels[DataSource.Config.IndexParcel].Droneld = NewParcel.Droneld;
            DataSource.Config.IndexParcel++;//Promoting the index
        }
        /// <summary>
        /// Adding a new drone charge to the array of drone charges
        /// </summary>
        /// <param name="NewDroneCharge"></param>
        public static void AddDroneCharge(DroneCharge NewDroneCharge)
        {
            DataSource.DroneCharges[DataSource.Config.IndexDroneCharge] = NewDroneCharge;
            DataSource.Config.IndexDroneCharge++;//Promoting the index
        }
        public static void DeleteDroneCharge(DroneCharge DeleteDroneCharge)
        {
            for (int i = 0; i < DataSource.Config.IndexDroneCharge; i++)
            {
                if(DataSource.DroneCharges[i].DroneId== DeleteDroneCharge.DroneId)
                {
                    DataSource.DroneCharges[i].DroneId = 0;
                    DataSource.DroneCharges[i].StationId = 0;
                }
                DataSource.Config.IndexDroneCharge--;//Decreasing the index
            }
        }
        /// <summary>
        /// Finding a drone available for delivery
        /// </summary>
        /// <returns></returns>
        public static Drone FindDroneAvailable()
        {
            Drone tempDrone = new();
            for (int i = 0; i < DataSource.Config.IndexDrone; i++)//Going through drones array
            {
                if (DataSource.Drones[i].Status == DroneStatuses.Available)//Checking status whether its available
                {
                    tempDrone = DataSource.Drones[i];//placing in a temporary array
                    break;
                }
            }
            return tempDrone;
        }
        /// <summary>
        /// Finding requested station according to its ID name
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Station FindStation(int id)
        {
            Station tempStation = new();
            for (int i = 0; i < DataSource.Config.IndexStation; i++)//Going through stations array
            {
                if (DataSource.Stations[i].Id == id)//finding requested station 
                {
                    tempStation = DataSource.Stations[i];//placing in a temporary array
                    break;
                }
            }
            return tempStation;
        }
        /// <summary>
        /// Finding requested station according to its name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static int FindStationId(string name)
        {
            Station tempStation = new();
            for (int i = 0; i < DataSource.Config.IndexStation; i++)
            {
                if (DataSource.Stations[i].Name == name)
                {
                    tempStation = DataSource.Stations[i];
                    break;
                }
            }
            return tempStation.Id;
        }
        /// <summary>
        /// Finding requested parcel according to its ID name
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static Parcel FindParcel(int ID)
        {
            Parcel tempParcel = new();
            for (int i = 0; i < DataSource.Config.IndexParcel; i++)
            {
                if (DataSource.Drones[i].Id == ID)
                {
                    tempParcel = DataSource.Parcels[i];
                    break;
                }
            }
            return tempParcel;
        }
        /// <summary>
        /// Finding requested custmer according to its ID name
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static Customer FindCustomer(int ID)
        {
            Customer tempCustomer = new();
            for (int i = 0; i < DataSource.Config.IndexCustomer; i++)
            {
                if (DataSource.Customers[i].Id == ID)
                {
                    tempCustomer = DataSource.Customers[i];
                    break;
                }
            }
            return tempCustomer;
        }
        /// <summary>
        /// Finding requested drone according to its ID name
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static Drone FindDrone(int ID)
        {
            Drone tempDrone = new();
            for (int i = 0; i < DataSource.Config.IndexDrone; i++)
            {
                if (DataSource.Drones[i].Id == ID)
                {
                    tempDrone = DataSource.Drones[i];
                    break;
                }
            }
            return tempDrone;
        }
        /// <summary>
        /// Finding requested drone charge according to its ID name
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static DroneCharge FindDroneCharge(int ID)
        {
            DroneCharge tempDroneCharge = new();
            for (int i = 0; i < DataSource.Config.IndexDroneCharge; i++)
            {
                if (DataSource.DroneCharges[i].DroneId == ID)
                {
                    tempDroneCharge = DataSource.DroneCharges[i];
                    break;
                }
            }
            return tempDroneCharge;
        }
        /// <summary>
        /// Increasing or decreasing index according to the flag 
        /// </summary>
        /// <param name="flag"></param>
        public static void UpdateParcelCounter(string flag)////////////////////////////////
        {
            if (flag == "Decrease")
                DataSource.Config.ParcelCounter--;
        }
        /// <summary>
        /// A function that returns an array of stationswhose load position is greater than 0
        /// </summary>
        /// <returns></returns>
        public static Station[] GetStationWithFreeSlots()
        {
            Station[] StationChargingSlot = new Station[DataSource.Config.IndexStation];
            int count = 0, i;
            for (i = 0; i < DataSource.Config.IndexStation; i++)
            {
                if (DataSource.Stations[i].ChargeSlots > 0)
                    StationChargingSlot[count] = DataSource.Stations[i];
                count++;
            }
            StationChargingSlot[i + 1].Id = -1;//Placing -1 in the last place in the array
            return StationChargingSlot;
        }
        /// <summary>
        /// Returns place of index in station array
        /// </summary>
        /// <returns></returns>
        public static int GetIndexStation() 
        {
            return DataSource.Config.IndexStation;
        }
        /// <summary>
        /// Increasing the charging slots
        /// </summary>
        /// <param name="UpdateStation"></param>
        public static void IncreaseChargeSlots(Station UpdateStation)
        {
            UpdateStation.ChargeSlots++;
        }
        /// <summary>
        /// Decreasing the charging slots
        /// </summary>
        /// <param name="UpdateStation"></param>
        public static void DecreaseChargeSlots(Station UpdateStation)
        {
            UpdateStation.ChargeSlots--;
        }
        /// <summary>
        /// Gives a view of the array of stations
        /// </summary>
        /// <returns></returns>
        public static Station[] ListViewStation()
        {
            Station[] ViewStation = new Station[DataSource.Config.IndexStation - 1];//placing array in another array which will be sent back to main
            for (int i = 0; i < DataSource.Config.IndexStation - 1; i++)
            {
                ViewStation[i] = DataSource.Stations[i];
            }
            return ViewStation;
        }
        /// <summary>
        /// Gives a view of the array of parcels
        /// </summary>
        /// <returns></returns>
        public static Parcel[] ListViewParcel()
        {
            Parcel[] ViewParcel = new Parcel[DataSource.Config.IndexParcel - 1];
            for (int i = 0; i < DataSource.Config.IndexParcel - 1; i++)
            {
                ViewParcel[i] = DataSource.Parcels[i];
            }
            return ViewParcel;
        }
        /// <summary>
        /// Gives a view of the array of drones
        /// </summary>
        /// <returns></returns>
        public static Drone[] ListViewDrone()
        {
            Drone[] ViewDrone = new Drone[DataSource.Config.IndexDrone - 1];
            for (int i = 0; i < DataSource.Config.IndexDrone - 1; i++)
            {
                ViewDrone[i] = DataSource.Drones[i];
            }
            return ViewDrone;
        }
        /// <summary>
        /// Gives a view of the array of customers
        /// </summary>
        /// <returns></returns>
        public static Customer[] ListViewCustomer()
        {
            Customer[] ViewCustomer = new Customer[DataSource.Config.IndexCustomer - 1];
            for (int i = 0; i < DataSource.Config.IndexCustomer - 1; i++)
            {
                ViewCustomer[i] = DataSource.Customers[i];
            }
            return ViewCustomer;
        }
        /// <summary>
        /// Gives a view of the an array of parcels with no assigned drones
        /// </summary>
        /// <returns></returns>
        public static Parcel[] ParcelWithNoDrone()
        {
            int count = 0;
            for (int i = 0; i < DataSource.Config.IndexParcel; i++)//counts how many parcels dont have an assigned drone
            {
                if (DataSource.Parcels[i].Droneld != 0)//if the drone ID doesnt equal 0
                    count++;
            }
            Parcel[] ViewParcel = new Parcel[count];
            for (int i = 0; i < count; i++)//places the parcels that dont have an assigned drone into an array
            {
                if (DataSource.Parcels[i].Droneld != 0)
                    ViewParcel[i] = DataSource.Parcels[i];
            }
            return ViewParcel;
        }
        /// <summary>
        /// Gives a view of the an array stations with available charging slots
        /// </summary>
        /// <returns></returns>
        public static Station[] AvailableChargingSlots()
        {
            int count = 0;
            for (int i = 0; i < DataSource.Config.IndexStation; i++)
            {
                if (DataSource.Stations[i].ChargeSlots > 0)//if the charge slots if greather than zero
                    count++;
            }
            Station[] ViewStation = new Station[count];
            for (int i = 0; i < count; i++)
            {
                if (DataSource.Stations[i].ChargeSlots > 0)
                    ViewStation[i] = DataSource.Stations[i];
            }
            return ViewStation;           
        }
    }
}