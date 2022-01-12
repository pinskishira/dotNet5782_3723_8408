using System;
using System.Collections.Generic;
using DO;
using static DO.Enum;

/// <summary>
/// DalObject defines lists for the stations, drones, customers, parcels and drone charges and then updates them and fills them with data.
/// It also includes adding functions for all lists as well as searching functions.
/// </summary>
namespace Dal
{
    static class DataSource
    {
        static Random rand = new Random();
        static internal List<Drone> Drones = new List<Drone>();//Defining a list for the drones
        static internal List<Station> Stations = new List<Station>();//Defining a list for the stations
        static internal List<Customer> Customers = new List<Customer>();//Defining a list for the customers
        static internal List<Parcel> Parcels = new List<Parcel>();//Defining a list for the parcels
        static internal List<DroneCharge> DroneCharges = new List<DroneCharge>();//Defining a list for the drone charges

        internal static class Config
        {
            static internal double BatteryConsumptionPowerUsageEmpty = 1;//Amount of battery used per km with no parcel
            static internal double BatteryConsumptionLightWeight = 3;//Amount of battery used per km with a light parcel
            static internal double BatteryConsumptionMediumWeight = 5;//Amount of battery used per km with a medium parcel
            static internal double BatteryConsumptionHeavyWeight = 7;//Amount of battery used per km with a heavy parcel
            static internal double DroneChargingRatePH = 1000;//Amount of battery charged hour/km
            static internal int NextParcelNumber = 10000;
        }

        /// <summary>
        /// Functions that updates and fills with data the information in the arrays
        /// </summary>
        public static void Initialize()
        {
            Station newStation = new Station();
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
                newStation.Longitude = rand.Next(29, 34) + rand.NextDouble();//Updating longitude   
                newStation.Latitude = rand.Next(33, 37) + rand.NextDouble();//Updating latitude 
                newStation.AvailableChargeSlots = rand.Next(10, 31);//Updating charging slots
                newStation.Name = stationArrayNames[loopStation];
                Stations.Add(newStation);
            }

            Drone newDrone = new Drone();
            string[] droneArayNames = new string[5];
            droneArayNames[0] = "123EST";
            droneArayNames[1] = "234EST";
            droneArayNames[2] = "345EST";
            droneArayNames[3] = "456EST";
            droneArayNames[4] = "567EST";
            for (int loopDrone = 0; loopDrone < 5; loopDrone++)//Updating 5 drones
            {
                newDrone.Id = rand.Next(10000, 100000);//Updating 5-digit ID name
                //int indexDrone = Drones.FindIndex(d => d.Id == newDrone.Id);
                for (int j = 0; j < loopDrone; j++)//Checking if it already appears in array
                {
                    while (newDrone.Id == Drones[j].Id)
                        newDrone.Id = rand.Next(10000, 100000);
                }
                newDrone.Weight = (WeightCategories)rand.Next(0, 3);//Updating the weight category
                newDrone.Model = droneArayNames[loopDrone];//Updating model
                Drones.Add(newDrone);
            }

            Customer newCustomer = new Customer();
            //Updating customer names
            string[] customerArrayName = new string[10];
            customerArrayName[0] = "Avital";
            customerArrayName[1] = "Hadar";
            customerArrayName[2] = "Ayala";
            customerArrayName[3] = "Dasi";
            customerArrayName[4] = "Moshe";
            customerArrayName[5] = "Ayalet";
            customerArrayName[6] = "David";
            customerArrayName[7] = "Shira";
            customerArrayName[8] = "Yosef";
            customerArrayName[9] = "John";
            string[] customerArrayPhone = new string[10];
            //Updating customers phone numbers
            customerArrayPhone[0] = "0586322431";
            customerArrayPhone[1] = "0522230982";
            customerArrayPhone[2] = "0506876398";
            customerArrayPhone[3] = "0506561043";
            customerArrayPhone[4] = "0502350982";
            customerArrayPhone[5] = "0534456021";
            customerArrayPhone[6] = "0552356731";
            customerArrayPhone[7] = "0503782099";
            customerArrayPhone[8] = "0504310431";
            customerArrayPhone[9] = "0506929115";
            for (int i = 0; i < 10; i++)//Updating 10 customers
            {
                newCustomer.Id = rand.Next(100000000, 1000000000);//Updating ID name randomly
                for (int j = 0; j < i; j++)//Checking if it already appears in array
                {
                    while (newCustomer.Id == Customers[j].Id)
                        newCustomer.Id = rand.Next(100000000, 1000000000);
                }
                newCustomer.Longitude = rand.Next(29, 34) + rand.NextDouble();//Updating longitude   
                newCustomer.Latitude = rand.Next(33, 37) + rand.NextDouble();//Updating latitude
                newCustomer.Name = customerArrayName[i];
                newCustomer.Phone = customerArrayPhone[i];
                Customers.Add(newCustomer);
            }

            for (int index = 0; index < 10; index++)//Updating 10 parcels
            {
                Parcel newParcel = new Parcel();
                newParcel.Id = Config.NextParcelNumber++;//Updating the ID number of the package
                newParcel.SenderId = Customers[rand.Next(10)].Id;//Updating the ID number of the sender
                newParcel.DroneId = 0;//Updating the ID number of the drone
                do
                {
                    newParcel.TargetId = Customers[rand.Next(10)].Id;
                }
                while (newParcel.SenderId == newParcel.TargetId);

                newParcel.Weight = (WeightCategories)rand.Next(0, 3);//Updating the weight
                newParcel.Priority = (Priorities)rand.Next(0, 3);//Updating the urgency of the shipment
                //Putting a random date and time
                newParcel.Requested = new DateTime(2021, rand.Next(1, 13), rand.Next(1, 29),
                    rand.Next(24), rand.Next(60), rand.Next(60));
                int status = rand.Next(100);
                int drone = -1;
                if (status >= 10)
                {
                    //Scheduling a time to deliver parcel
                    newParcel.Scheduled = newParcel.Requested +
                        new TimeSpan(rand.Next(5), rand.Next(60), rand.Next(60));
                    if (status >= 15)
                    {
                        //Time drone came to deliver parcel
                        newParcel.PickedUp = newParcel.Scheduled +
                            new TimeSpan(0, rand.Next(1, 60), rand.Next(60));
                        if (status >= 20)
                        {
                            //Time customer recieved parcel
                            newParcel.Delivered = newParcel.PickedUp +
                                new TimeSpan(0, rand.Next(1, 60), rand.Next(60));
                            do
                            {
                                drone = rand.Next(5);
                                newParcel.DroneId = Drones[drone].Id;
                            }
                            while (Drones[drone].Weight < newParcel.Weight);
                        }
                    }
                    if (drone == -1)
                    {
                        do
                        {
                            drone = rand.Next(5);
                            newParcel.DroneId = Drones[drone].Id;
                        }
                        while (Drones[drone].Weight > newParcel.Weight);
                    }
                }
                Parcels.Add(newParcel);
            }
            //XMLTools.SaveListToXMLSerializer(Drones, DroneXml);
            //XMLTools.SaveListToXMLSerializer(Stations, StationXml);
            //XMLTools.SaveListToXMLSerializer(Customers, CustomerXml);
            //XMLTools.SaveListToXMLSerializer(Parcels, ParcelXml);
            //XMLTools.SaveListToXMLSerializer(new List<DroneCharge>(), DroneChargeXml);

        }
        //private static string DroneXml = "@DroneXml.xml";
        //private static string StationXml = "@StationXml.xml";
        //private static string CustomerXml = "@CustomerXml.xml";
        //private static string ParcelXml = "@ParcelXml.xml";
        //private static string DroneChargeXml = "@DroneChargeXml.xml";

    }
}