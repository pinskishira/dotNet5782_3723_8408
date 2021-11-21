using System;
using IDAL;
using System.Collections;
using IBL.BO;
using System.Collections.Generic;
using IBL;
using static IBL.BO.Enum;
using DalObject;
using BL.IBL.BO;
using System.Linq;

namespace BL
{
    public partial class BL : Ibl
    {
        static Random rand = new Random();//זה בסדר לאתחל ככה את השדות ולעשות אותו סטטיק?
        static IDal dal = new DalObject.DalObject();
        List<DroneToList> BlDrones = new();
        double[] elecUse = dal.electricityUse();//לשאול את דן
        public BL()
        {
            dal.GetAllDrones().CopyPropertiesToIEnumerable(BlDrones);
            foreach (var indexOfDrones in BlDrones)
            {
                try
                {
                    IDAL.DO.Parcel parcel = dal.GetAllParcels().First(item => item.DroneId == indexOfDrones.Id && item.Delivered == DateTime.MinValue);//if parcel was paired but not delivered
                    indexOfDrones.DroneStatus = (DroneStatuses)3;//updating status to be in delivery
                    indexOfDrones.MaxWeight = (WeightCategories)parcel.Weight;//לשאול את דן אם זה בסדר שהוספנו את זה מרצוננו החופשי
                    if (parcel.Scheduled != DateTime.MinValue && parcel.PickedUp == DateTime.MinValue)//if parcel was paired but not picked up
                    {
                        IDAL.DO.Customer sender = dal.FindCustomer(parcel.SenderId);//finding the customer usind id number
                        IDAL.DO.Station closestStation = smallestDistance(sender.Longitude, sender.Latitude);//returning samllest distance between the sender of the parcel and the stations 
                        indexOfDrones.CurrentLocation.Longitude = closestStation.Longitude;//updating the loaction of the drone 
                        indexOfDrones.CurrentLocation.Latitude = closestStation.Latitude;
                        double batteryConsumption = BatteryConsumption(indexOfDrones, parcel) + Distance.Haversine
                            (indexOfDrones.CurrentLocation.Longitude, indexOfDrones.CurrentLocation.Latitude, sender.Longitude, sender.Latitude);
                        indexOfDrones.Battery = rand.Next((int)batteryConsumption, 101);

                    }
                    if (parcel.PickedUp != DateTime.MinValue)//if parcel was picked up but not delivered 
                    {
                        IDAL.DO.Customer tempCustomer = dal.FindCustomer(parcel.SenderId);//finding the sender in customers
                        indexOfDrones.CurrentLocation.Longitude = tempCustomer.Longitude;//updating the location  of the drone
                        indexOfDrones.CurrentLocation.Latitude = tempCustomer.Latitude;
                        indexOfDrones.Battery = rand.Next(BatteryConsumption(indexOfDrones, parcel), 101);
                    }
                }
                catch (ArgumentNullException ex)       //לשאול מה צריך להיות בשגיאה הזאת
                {
                    if (indexOfDrones.DroneStatus != (DroneStatuses)3)//if the drone is not performing a delivery
                        indexOfDrones.DroneStatus = (DroneStatuses)rand.Next(1, 3);//his status will be found using random selection  
                    if (indexOfDrones.DroneStatus == (DroneStatuses)2)//if the drone is in maintanance
                    {
                        List<IDAL.DO.Station> tempStations = dal.GetAllStations().ToList();//temporary array with all the stations
                        int idStation = rand.Next(0, tempStations.Count());//finding a random index from the array of stations
                        IDAL.DO.Station station = tempStations[idStation];//placing the index returned into the stations list 
                        indexOfDrones.CurrentLocation.Longitude = station.Longitude;//updating the location of the drone
                        indexOfDrones.CurrentLocation.Latitude = station.Latitude;
                        indexOfDrones.Battery = rand.Next(0, 21);
                    }
                    if (indexOfDrones.DroneStatus == (DroneStatuses)1)//if the drone is available for delivery
                    {
                        List<int> deliveredParcels = new();//creating a new list 
                        foreach (var indexOfParcels in dal.GetAllParcels())//going through parcel list
                            if (indexOfParcels.Delivered != DateTime.MinValue)//finding parcels that have been delivered 
                                deliveredParcels.Add(indexOfParcels.TargetId);//placing their targetId in new list
                        int idCustomer = rand.Next(0, deliveredParcels.Count());//finding a random index from the new list of deliveredParcels
                        IDAL.DO.Customer customer = dal.FindCustomer(idCustomer);//finding the customer with the index found with random selection
                        indexOfDrones.CurrentLocation.Longitude = customer.Longitude;//updating the location of the drone
                        indexOfDrones.CurrentLocation.Latitude = customer.Latitude;
                        IDAL.DO.Station smallestStation = smallestDistance(indexOfDrones.CurrentLocation.Longitude, indexOfDrones.CurrentLocation.Latitude);
                        double distanceFromStation = Distance.Haversine
                            (indexOfDrones.CurrentLocation.Longitude, indexOfDrones.CurrentLocation.Latitude, smallestStation.Longitude, smallestStation.Latitude);
                        indexOfDrones.Battery = rand.Next((int)(distanceFromStation*elecUse[0]),101);
                    }
                }
            }
        }

        public int BatteryConsumption(DroneToList droneToList, IDAL.DO.Parcel parcel)//פונרקציה שמחשבת כמה בטריה צריך הרחפן כדי לעשות את המשלוח
        {
            IDAL.DO.Customer target = dal.FindCustomer(parcel.TargetId);
            IDAL.DO.Customer sender = dal.FindCustomer(parcel.SenderId);
            double distanceFromTarget = Distance.Haversine(sender.Longitude, sender.Latitude, target.Longitude, target.Latitude);
            IDAL.DO.Station smallestStation = smallestDistance(target.Longitude, target.Latitude);
            double distanceFromStation = Distance.Haversine(target.Longitude, target.Latitude, smallestStation.Longitude, smallestStation.Latitude);
            return (int)(distanceFromTarget * elecUse[Weight(droneToList.MaxWeight)] + distanceFromStation * elecUse[0]);
        }

        public int Weight(WeightCategories maxWeight)//פונקציה שמחזירה מה כמות הצריכה של הרחפן
        {
            if (maxWeight == (WeightCategories)1)
                return 1;
            if (maxWeight == (WeightCategories)2)
                return 2;
            if (maxWeight == (WeightCategories)3)
                return 3;
            return 0;
        }

        public IDAL.DO.Station smallestDistance(double longitude,double latitude)
        {
            double minDistance = double.PositiveInfinity;//starting with an unlimited value
            IDAL.DO.Station station = new();
            double tempDistance = -1;
            foreach (var indexOfStations in dal.GetAllStations())//goes through all the stations 
            {
                //calculating the distance between the sender and the station
                tempDistance = Distance.Haversine(indexOfStations.Longitude, indexOfStations.Latitude, longitude, latitude);
                if (tempDistance < minDistance)//compares which distance is smaller
                {
                    minDistance = tempDistance;
                    station = indexOfStations;
                }
            }
            return station;//returns closest station to sender
        }
    }
}
