using System;
using IDAL;
using System.Collections.Generic;
using IBL.BO;
using static IBL.BO.Enum;

namespace BL
{
    public class BL
    {
        public BL()
        {
            Random rand = new Random();
            IDal dalObject = new DalObject.DalObject();
            double[] elecUse = dalObject.electricityUse();
            double Available = elecUse[0];
            double LightWeight = elecUse[1];
            double MediumWeight = elecUse[2];
            double HeavyWeight = elecUse[3];
            double DroneLoadingRate = elecUse[4];
            List<Drone> BlDrones = new();
            foreach (var indexOfDrones in dalObject.GetAllDrones())
            {
                Drone newDrone = new();
                newDrone.Id = indexOfDrones.Id;
                newDrone.Model = indexOfDrones.Model;
                newDrone.MaxWeight = (WeightCategories)indexOfDrones.MaxWeight;
                bool flag = true;
                foreach (var indexOfParcels in dalObject.GetAllParcels())//going over the parcels
                {
                    if (indexOfParcels.DroneId == indexOfDrones.Id && indexOfParcels.Delivered == DateTime.MinValue)//if parcel was paired but not delivered
                    {
                        newDrone.DroneStatus = (DroneStatuses)3;
                        if (indexOfParcels.Scheduled != DateTime.MinValue && indexOfParcels.PickedUp == DateTime.MinValue)//if parcel was paired but not picked up
                        {
                            ///לשנות את זה
                            newDrone.CurrentLocation.Longitude = 0;
                            newDrone.CurrentLocation.Latitude = 1;
                        }
                        if (indexOfParcels.PickedUp != DateTime.MinValue)//if parcel was picked up but not delivered 
                        {
                            ///לשנות את זה
                            newDrone.CurrentLocation.Longitude = 0;
                            newDrone.CurrentLocation.Latitude = 1;
                        }
                        ///לשנות את זה
                        newDrone.Battery = 0;
                        flag = false;
                        break;
                    }
                }
                if (flag == true)//if drone is not doing a delivery
                    newDrone.DroneStatus = (DroneStatuses)rand.Next(1, 3);
                if (newDrone.DroneStatus == (DroneStatuses)2)
                {
                    List<IDAL.DO.Station> tempStations = (List<IDAL.DO.Station>)dalObject.GetAllStations();
                    
                }

            }
        }
    }

}
