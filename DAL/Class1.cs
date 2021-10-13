using System;
using DAL.DO;

namespace DAL
{
    namespace DO
    {
        public struct Drone
        {
            int id;
            string model;
            WeightCategories MaxWeight;
            DroneStatuses Status;
            double Battery;
        }

        public struct Station
        {
            int Id;
            int Name;
            double Longitude;
            double Lattitude;
            int ChargeSlots;
        }

        public struct DroneCharge
        {
            int Droneld;
            int Stationld;
        }
    }
    public class Class1
    {
    }
}

namespace DalObject
{
    public struct DataSource
    {
        static internal Drone[] Drone = new Drone [10];
    }
}
