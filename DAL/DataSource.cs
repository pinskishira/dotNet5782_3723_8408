using System;
using IDAL.DO;
using System.Collections.Generic;
/// <summary>
/// DalObject defines arrays for the stations, drones, customers, parcels and drone charges and then updates them and fills them with data.
/// It also includes adding functions for all arrays as well as searching functions.
/// </summary>
namespace DalObject
{
    public static class DataSource
    {
        static Random rand = new Random();
        static List<Drone> Drones = new();//Defining a list for the drones
        static List<Station> Stations = new();//Defining a list for the stations
        static List<Customer> Customers = new();//Defining a list for the customers
        static List<Parcel> Parcels = new();//Defining a list for the parcels
        static List<DroneCharge> DroneCharges = new();//Defining a list for the drone charges
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
            Station newStation = new();
            string[] stationArrayNames = new string[2];
            stationArrayNames[0] = "Bayit Vegan";
            stationArrayNames[1] = "Givat Shaul";
            for (int loopStation = 0; loopStation < 2; loopStation++)//Updating 2 base stations
            {
                newStation.Id = rand.Next(1000, 10000);//Updating 4-digit ID name                                   
                for (int help = 0; help < loopStation; help++)//Checking if it already appears in array
                {
                    while (newStation.Id == Stations[help].Id)
                        newStation.Id = rand.Next(1000, 10000);
                }
                newStation.Longitude = 35 + rand.NextDouble();//Updating longitude                                     
                newStation.Latitude = 31 + rand.NextDouble();//Updating latitude    
                newStation.ChargeSlots = rand.Next(10, 30);//Updating charging slots
                newStation.Name = stationArrayNames[loopStation];
                Stations.Add(newStation);
            }
            Config.IndexStation = 2;

            Drone newDrone = new();
            string[] droneArayNames = new string[5];
            droneArayNames[0] = "123EST";
            droneArayNames[1] = "234EST";
            droneArayNames[2] = "345EST";
            droneArayNames[3] = "456EST";
            droneArayNames[4] = "567EST";
            for (int loopDrone = 0; loopDrone < 5; loopDrone++)//Updating 5 drones
            {
                newDrone.Id = rand.Next(10000, 100000);//Updating 5-digit ID name 
                for (int j = 0; j < loopDrone; j++)//Checking if it already appears in array
                {
                    while (newDrone.Id == Drones[j].Id)
                        newDrone.Id = rand.Next(10000, 100000);
                }
                newDrone.MaxWeight = (WeightCategories)rand.Next(1, 4);//Updating the weight category
                newDrone.Model = droneArayNames[loopDrone];//Updating model
                //newDrone.Battery = rand.Next(1, 101);//Updating battery status
                // Updating drone status
                //if (newDrone.Battery < 20)
                //{
                //    newDrone.Status = DroneStatuses.Maintenance;
                //    int station = rand.Next(2);
                //    DroneCharges[Config.IndexDroneCharge].StationId = Stations[station].Id;
                //    Stations[station].ChargeSlots--;
                //}
                //else
                //    newDrone.Status = DroneStatuses.Available;
            }
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
            Parcel newParcel = new();
            for (int index = 0; index < 10; index++)//Updating 10 parcels
            {
                newParcel.Id = Config.NextParcelNumber++;//Updating the ID number of the package
                newParcel.SenderId = Customers[rand.Next(10)].Id;//Updating the ID number of the sender
                newParcel.DroneId = 0;//Updating the ID number of the drone
                do
                {
                    newParcel.TargetId = Customers[rand.Next(10)].Id;
                }
                while (Parcels[index].SenderId == Parcels[index].TargetId);

                newParcel.Weight = (WeightCategories)rand.Next(1,4);//Updating the weight
                newParcel.Priority = (Priorities)rand.Next(1,4);//Updating the urgency of the shipment
                //Putting a random date and time
                newParcel.Requested = new DateTime(2021, rand.Next(1, 13), rand.Next(1, 30),
                    rand.Next(24), rand.Next(60), rand.Next(60));
                int status = rand.Next(100);
                int drone = -1;
                if (status >= 10)
                {
                    //Scheduling a time to deliver parcel
                    newParcel.Scheduled = Parcels[index].Requested +
                        new TimeSpan(rand.Next(5), rand.Next(60), rand.Next(60));
                    if (status >= 15)
                    {
                        //Time drone came to deliver parcel
                        newParcel.PickedUp = Parcels[index].Scheduled +
                            new TimeSpan(0, rand.Next(1, 60), rand.Next(60));
                        if (status >= 20)
                        {
                            //Time customer recieved parcel
                            newParcel.Delivered = Parcels[index].PickedUp +
                                new TimeSpan(0, rand.Next(1, 60), rand.Next(60));
                            do
                            {
                                drone = rand.Next(10);
                                newParcel.DroneId = Drones[drone].Id;
                            }
                            while (Drones[drone].MaxWeight < Parcels[index].Weight);
                        }
                    }
                    if (drone == -1)
                    {
                        do
                        {
                            drone = rand.Next(10);
                            newParcel.DroneId = Drones[drone].Id;
                        }
                        while (/*Drones[drone].Status == DroneStatuses.Available &&*/
                            Drones[drone].MaxWeight >= Parcels[index].Weight);
                    }
                    //Drones[drone].Status = DroneStatuses.Delivery;
                }
                Parcels.Add(newParcel);
            }
            DataSource.Config.IndexParcel = 10;//Promoting the index
        }
    }
}