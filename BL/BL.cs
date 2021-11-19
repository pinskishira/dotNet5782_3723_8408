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
        static Random rand = new Random();
        IDal dal = new DalObject.DalObject();
        List<DroneToList> BlDrones = new();
        public BL()
        {
            dal.GetAllDrones().CopyPropertiesToIEnumerable(BlDrones);
            double[] elecUse = dal.electricityUse();//לשאול את דן
            foreach (var indexOfDrones in BlDrones)
            {
                try
                {
                    IDAL.DO.Parcel parcel = dal.GetAllParcels().First(item => item.DroneId == indexOfDrones.Id && item.Delivered == DateTime.MinValue);//if parcel was paired but not delivered
                    indexOfDrones.DroneStatus = (DroneStatuses)3;//updating status to be in delivery
                    if (parcel.Scheduled != DateTime.MinValue && parcel.PickedUp == DateTime.MinValue)//if parcel was paired but not picked up
                    {
                        IDAL.DO.Parcel tempParcel = dal.GetAllParcels().First(item => item.DroneId == indexOfDrones.Id);//finding parcel with same id
                        IDAL.DO.Station closestStation = smallestDistance(tempParcel.SenderId);//returning samllest distance between the sender of the parcel and the stations 
                        indexOfDrones.CurrentLocation.Longitude = closestStation.Longitude;//updating the loaction of the drone 
                        indexOfDrones.CurrentLocation.Latitude = closestStation.Latitude;
                    }
                    if (parcel.PickedUp != DateTime.MinValue)//if parcel was picked up but not delivered 
                    {
                        IDAL.DO.Parcel tempParcel = dal.GetAllParcels().First(item => item.DroneId == indexOfDrones.Id);//finding parcel with same id
                        IDAL.DO.Customer tempCustomer = dal.FindCustomer(tempParcel.SenderId);//finding the sender in customers
                        indexOfDrones.CurrentLocation.Longitude = tempCustomer.Longitude;//updating the location  of the drone
                        indexOfDrones.CurrentLocation.Latitude = tempCustomer.Latitude;
                    }
                    //לחשב מצב סוללה
                }
                catch (Exception ex)
                {
                    if (indexOfDrones.DroneStatus != (DroneStatuses)3)//if the drone is not performing a delivery
                        indexOfDrones.DroneStatus = (DroneStatuses)rand.Next(1, 3);//his status will be found using random selection  
                    if (indexOfDrones.DroneStatus == (DroneStatuses)2)//if the drone is in maintanance
                    {
                        List<IDAL.DO.Station> tempStations = (List<IDAL.DO.Station>)dal.GetAllStations();//temporary array with all the stations
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
                        //לחשב מצב סוללה
                    }
                }
            }
        }

    }
}
