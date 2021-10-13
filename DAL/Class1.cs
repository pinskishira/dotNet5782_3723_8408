using System;
using DAL.DO;

namespace DAL
{
    namespace DO
    {
        public struct Drone
        {

        }
        public struct Customer
        {
            int Id;
            string Name;
            String Phone;
            double Longitude;
            double Lattitude;
        }
        public struct parcel
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
        }
    }
    public class Class1
    {
    }
}
namespace DalObject
{
    public class DataSource
    {
        static internal Drone[] drone=new Drone[10];
        //int[] arr=new int[8];
    }
}
