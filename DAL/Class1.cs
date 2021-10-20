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
        public struct Customer

        public struct Station
        {
            int Id;
            string Name;
            String Phone;
            int Name;
            double Longitude;
            double Lattitude;
            int ChargeSlots;
        }
        public struct parcel

        public struct DroneCharge
        {
            int Id;
            int Senderld;
            int Targetld;
            WeightCategories Weight;
            Priorities Priority;
            datetime Requested;
            int Droneld;
            datetime Scheduled;
            datetime PickedUp;
            datetime Delivered;
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
