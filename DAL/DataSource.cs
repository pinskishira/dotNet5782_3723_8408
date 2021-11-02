using System;
using IDAL.DO;

/// <summary>
/// DalObject defines arrays for the stations, drones, customers, parcels and drone charges and then updates them and fills them with data.
/// It also includes adding functions for all arrays as well as searching functions.
/// </summary>
namespace DalObject
{
    public static class DataSource
    {
        static Random rand = new Random();
        static internal Drone[] Drones = new Drone[10];//Defining an array of size 10 for the drones
        static internal Station[] Stations = new Station[5];//Defining an array of size 5 for the stations
        static internal Customer[] Customers = new Customer[100];//Defining an array of size 100 for the customers
        static internal Parcel[] Parcels = new Parcel[1000];//Defining an array of size 10000 for the parcels
        static internal DroneCharge[] DroneCharges = new DroneCharge[100];//Defining an array of size 100 for the drone charges
        /// <summary>
        /// Has all my static indexes
        /// </summary>
        internal static class Config
        {
            static internal int IndexDrone = 0;
            static internal int IndexStation = 0;
            static internal int IndexCustomer = 0;
            static internal int IndexParcel = 0;
            static internal int IndexDroneCharge = 0;
            static internal int NextParcelNumber = 10000000;
        }

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
                Stations[loopStation].Longitude = 35 + rand.NextDouble();//Updating longitude                                     
                Stations[loopStation].Latitude = 31 + rand.NextDouble();//Updating latitude    
                Stations[loopStation].ChargeSlots = rand.Next(10, 30);//Updating charging slots
            }
            Stations[0].Name = "Bayit Vegan";//Updating names
            Stations[1].Name = "Givat Shaul";
            Config.IndexStation = 2;

            for (int i = 0; i < 5; i++)//Updating 5 drones
            {
                Drones[i].Id = rand.Next(10000, 100000);//Updating 5-digit ID name 
                for (int j = 0; j < i; j++)//Checking if it already appears in array
                {
                    while (Drones[i].Id == Drones[j].Id)
                        Drones[i].Id = rand.Next(10000, 100000);
                }
                Drones[i].MaxWeight = (WeightCategories)rand.Next(1, 4);//Updating the weight category
                Drones[i].Battery = rand.Next(1, 101);//Updating battery status
                // Updating drone status
                if (Drones[i].Battery < 20)
                {
                    Drones[i].Status = DroneStatuses.Maintenance;
                    int station = rand.Next(2);
                    DroneCharges[Config.IndexDroneCharge].StationId = Stations[station].Id;
                    Stations[station].ChargeSlots--;
                }
                else
                    Drones[i].Status = DroneStatuses.Available;
            }
            Drones[0].Model = "123EST";//Updating model
            Drones[1].Model = "234EST";
            Drones[2].Model = "345EST";
            Drones[3].Model = "456EST";
            Drones[4].Model = "567EST";
            DataSource.Config.IndexDrone = 5;//Promoting the index
            for (int i = 0; i < 10; i++)//Updating 10 customers
            {
                Customers[i].Id = rand.Next(100000000, 1000000000);//Updating ID name randomly
                for (int j = 0; j < i; j++)//Checking if it already appears in array
                {
                    while (Customers[i].Id == Customers[j].Id)
                        Customers[i].Id = rand.Next(100, 1000);
                } 
                Customers[i].Longitude = 35 + rand.NextDouble();//U//Updating longitude   
                Customers[i].Latitude = 31 + rand.NextDouble();//Updating latitude    
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
            Customers[0].Phone = "0586322431";
            Customers[1].Phone = "0522230982";
            Customers[2].Phone = "0506876398";
            Customers[3].Phone = "0506561043";
            Customers[4].Phone = "0502350982";
            Customers[5].Phone = "0534456021";
            Customers[6].Phone = "0552356731";
            Customers[7].Phone = "0503782099";
            Customers[8].Phone = "0504310431";
            Customers[9].Phone = "0506929115";

            ref int parcelNum = ref Config.NextParcelNumber;
            for (int index = 0; index < 10; index++)//Updating 10 parcels
            {
                Parcels[index].Id = Config.NextParcelNumber++;//Updating the ID number of the package
                Parcels[index].SenderId = Customers[rand.Next(10)].Id;//Updating the ID number of the sender
                Parcels[index].DroneId = 0;//Updating the ID number of the drone
                do
                {
                    Parcels[index].TargetId = Customers[rand.Next(10)].Id;
                }
                while (Parcels[index].SenderId == Parcels[index].TargetId);

                Parcels[index].Weight = (WeightCategories)rand.Next(1,4);//Updating the weight
                Parcels[index].Priority = (Priorities)rand.Next(1,4);//Updating the urgency of the shipment
                //Putting a random date and time
                Parcels[index].Requested = new DateTime(2021, rand.Next(1, 13), rand.Next(1, 30),
                    rand.Next(24), rand.Next(60), rand.Next(60));
                int status = rand.Next(100);
                int drone = -1;
                if (status >= 10)
                {
                    //Scheduling a time to deliver parcel
                    Parcels[index].Scheduled = Parcels[index].Requested +
                        new TimeSpan(rand.Next(5), rand.Next(60), rand.Next(60));
                    if (status >= 15)
                    {
                        //Time drone came to deliver parcel
                        Parcels[index].PickedUp = Parcels[index].Scheduled +
                            new TimeSpan(0, rand.Next(1, 60), rand.Next(60));
                        if (status >= 20)
                        {
                            //Time customer recieved parcel
                            Parcels[index].Delivered = Parcels[index].PickedUp +
                                new TimeSpan(0, rand.Next(1, 60), rand.Next(60));
                            do
                            {
                                drone = rand.Next(10);
                                Parcels[index].DroneId = Drones[drone].Id;
                            }
                            while (Drones[drone].MaxWeight < Parcels[index].Weight);
                        }
                    }
                    if (drone == -1)
                    {
                        do
                        {
                            drone = rand.Next(10);
                            Parcels[index].DroneId = Drones[drone].Id;
                        }
                        while (Drones[drone].Status == DroneStatuses.Available &&
                            Drones[drone].MaxWeight >= Parcels[index].Weight);
                    }
                    Drones[drone].Status = DroneStatuses.Delivery;
                }
            }
            DataSource.Config.IndexParcel = 10;//Promoting the index
        }
    }
}