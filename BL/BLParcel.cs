﻿using System;
using System.Collections.Generic;
using System.Linq;

using IBL.BO;
using static IBL.BO.Enum;

namespace BL
{
    public partial class BL
    {
        public void AddParcel(Parcel newParcel)
        {
            if ((Math.Round(Math.Floor(Math.Log10(newParcel.Sender.Id))) + 1) != 9)//if id inputted is not 9 digits long
                throw new InvalidInputException("The identification number of sender should be 9 digits long\n");
            if ((Math.Round(Math.Floor(Math.Log10(newParcel.Target.Id))) + 1) != 9)//if id inputted is not 9 digits long
                throw new InvalidInputException("The identification number of target should be 9 digits long\n");
            if (newParcel.Weight != (WeightCategories)1 && newParcel.Weight != (WeightCategories)2 && newParcel.Weight != (WeightCategories)3)//if 1,2 or 3 werent inputted
                throw new InvalidInputException("You need to select 1- for Easy 2- for Medium 3- for Heavy\n");
            if (newParcel.Priority != (Priorities)1 && newParcel.Priority != (Priorities)2 && newParcel.Priority != (Priorities)3)//if 1,2 or 3 were inputted
                throw new InvalidInputException("You need to select 1- for Normal 2- for Fast 3- for Emergency\n");
            //updating times
            newParcel.Requested = DateTime.Now;
            newParcel.Scheduled = DateTime.MinValue;
            newParcel.PickedUp = DateTime.MinValue;
            newParcel.Delivered = DateTime.MinValue;
            try
            {

                //converting BL parcel to dal
                IDAL.DO.Parcel tempParcel = new();
                object obj = tempParcel;
                newParcel.CopyPropertiesTo(obj);
                tempParcel = (IDAL.DO.Parcel)obj;
                newParcel.CopyPropertiesTo(tempParcel);
                tempParcel.SenderId = newParcel.Sender.Id;
                tempParcel.TargetId = newParcel.Target.Id;               
                dal.AddParcel(tempParcel);//adding to station list in dal
            }
            catch (IDAL.DO.ItemExistsException ex)
            {
                throw new FailedToAddException("The parcel already exists.\n", ex);
            }
        }

        public Parcel GetParcel(int parcelId)
        {
            Parcel blParcel = new();
            try
            {
                IDAL.DO.Parcel dalParcel = dal.FindParcel(parcelId);//finding parcel
                blParcel.Sender = new();
                blParcel.Target = new();
                blParcel.DroneParcel = new();
                dalParcel.CopyPropertiesTo(blParcel);//converting to BL
                Customer target = GetCustomer(dalParcel.TargetId);//finding the target who will recieve parcel
                target.CopyPropertiesTo(blParcel.Target);//converting to BL
                Customer sender = GetCustomer(dalParcel.SenderId);//finding the sender who sends the parcel
                sender.CopyPropertiesTo(blParcel.Sender);//converting to BL
                if (dalParcel.DroneId == 0)//if parcel isnt assigned to a drone
                    blParcel.DroneParcel = default;
                else
                {
                    Drone drone = GetDrone(dalParcel.DroneId);//finding its assogned drone
                    blParcel.DroneParcel.CurrentLocation = new();
                    drone.CopyPropertiesTo(blParcel.DroneParcel);//converting to BL
                    blParcel.DroneParcel.CurrentLocation = CopyLocation(drone.CurrentLocation.Longitude, drone.CurrentLocation.Latitude);
                }
            }
            catch (IDAL.DO.ItemDoesNotExistException ex)
            {
                throw new FailedDisplayException("The Id does not exist.\n", ex);
            }
            return blParcel;
        }

        public IEnumerable<ParcelToList> GetAllParcels()
        {
            Parcel tempParcel = new();
            ParcelToList tempParcelToList = new();
            List<ParcelToList> parcelToLists = new List<ParcelToList>();
            foreach (var indexOfParcels in dal.GetAllParcels())//goes through dals list of parcels
            {
                tempParcel = GetParcel(indexOfParcels.Id);//finding parcel
                tempParcel.CopyPropertiesTo(tempParcelToList);//converting to parcelToList
                tempParcelToList.SenderName = tempParcel.Sender.Name;
                tempParcelToList.TargetName = tempParcel.Target.Name;
                if (tempParcel.Delivered != DateTime.MinValue)//if parcel was delivered
                    tempParcelToList.StateOfParcel = (ParcelState)4;//state -> provided
                else
                {
                    if (tempParcel.Scheduled != DateTime.MinValue)//if parcel was picked up by drone
                        tempParcelToList.StateOfParcel = (ParcelState)3;//state -> picked up
                    else
                    {
                        if (tempParcel.PickedUp != DateTime.MinValue)//if if parcel was assigned to drone
                            tempParcelToList.StateOfParcel = (ParcelState)2;//state -> paired
                        else//if parcel was requested
                            tempParcelToList.StateOfParcel = (ParcelState)1;//state -> created
                    }
                }
                parcelToLists.Add(tempParcelToList);
                tempParcelToList = new();

            }
            return parcelToLists;
        }

        public IEnumerable<ParcelToList> GetAllParcelsWithNoDrone()
        {
            List<ParcelToList> parcelToList = new();
            foreach (var indexOfParcelToList in GetAllParcels())//goes thorugh parcels
            {
                if (indexOfParcelToList.StateOfParcel == (ParcelState)1)//if they have not been assigned a drone
                    parcelToList.Add(indexOfParcelToList);//add to list
            }
            return parcelToList;
        }

        public void UpdateAssignParcelToDrone(int droneId)
        {
            try
            {
                DroneToList droneToList = BlDrones.Find(indexOfDroneToList => indexOfDroneToList.Id == droneId);//Looking for the drone you want to associate
                IDAL.DO.Parcel parcel = new();
                int maxPriorities = 0, maxWeight = 0, indexParcel = -1;
                double maxDistance = 0.0;
                if (droneToList.DroneStatus == (DroneStatuses)1)//Check that the drone is free
                {
                    foreach (var indexOfParcel in dal.GetAllParcels())//Go through all the parcel to look for a suitable parcel
                    {
                        IDAL.DO.Customer sender = dal.GetAllCustomers().First(index => index.Id == indexOfParcel.SenderId);//Looking for the customer of the parcle
                        //Finds the distance between the drone and the sender
                        double distance = Distance.Haversine(sender.Longitude, sender.Latitude, droneToList.CurrentLocation.Longitude, droneToList.CurrentLocation.Latitude);
                        //Looking for a parcel with high priority Maximum weight and close to the drone
                        if ((int)indexOfParcel.Priority <= maxPriorities && (int)indexOfParcel.Weight <= maxWeight && distance <= maxDistance)
                        {
                            double batteryConsumption = BatteryConsumption(droneToList, indexOfParcel) + Distance.Haversine
                                (droneToList.CurrentLocation.Longitude, droneToList.CurrentLocation.Latitude, sender.Longitude, sender.Latitude) * PowerUsageEmpty;
                            if (droneToList.Battery <= batteryConsumption)//Checks if the drone can make the sending
                            {
                                maxPriorities = (int)indexOfParcel.Priority;
                                maxWeight = (int)indexOfParcel.Weight;
                                maxDistance = distance;
                                parcel = indexOfParcel;//Updating the parcel
                            }
                        }
                    }
                    if (indexParcel == -1)//No suitable drone found
                        throw new ParcelDeliveryException("There is no parcel that can belong to this drone.\n");
                    dal.UpdateAssignParcelToDrone(parcel.Id, droneToList.Id);//Updating the parcel
                    droneToList.DroneStatus = (DroneStatuses)2;//Update the drone status
                    int indexOfDroneToList = BlDrones.FindIndex(indexOfDroneToList => indexOfDroneToList.Id == droneId);
                    BlDrones[indexOfDroneToList] = droneToList;
                }
            }
            catch (ParcelDeliveryException ex)
            {
                throw new ParcelDeliveryException(ex.ToString());
            }
            catch (ArgumentNullException ex)
            {
                throw new ParcelDeliveryException(ex.ToString());
            }
            catch (IDAL.DO.ItemDoesNotExistException ex)
            {
                throw new FailedToAddException("The drone in charge does not exist.\n", ex);
            }
        }

        /// <summary>
        /// Finds distance between the drone and the sender of the parcel.
        /// </summary>
        /// <param name="droneToList">List of drones</param>
        /// <param name="parcelSenderId">Id of the parcel sender</param>
        /// <returns></returns>
        public double DroneDistanceFromParcel(DroneToList droneToList, int parcelSenderId)
        {
            try
            {
                IDAL.DO.Customer sender = dal.GetAllCustomers().First(indexOfParcel => indexOfParcel.Id == parcelSenderId);
                return Distance.Haversine(sender.Longitude, sender.Latitude, droneToList.CurrentLocation.Longitude, droneToList.CurrentLocation.Latitude);
            }
            catch (Exception)
            {
                throw new ParcelDeliveryException("The sender of the parcel was not found.\n");
            }
        }

        public void UpdateParcelDeliveryToCustomer(int droneId)
        {
            try
            {
                DroneToList droneToList = BlDrones.Find(indexOfDroneToList => indexOfDroneToList.Id == droneId);//finding drone with given id
                IDAL.DO.Parcel parcel = dal.GetAllParcels().First(item => item.Id == droneToList.ParcelNumInTransfer);//finding parcel that is assigned to this drone
                Drone drone = GetDrone(droneId);//finding drone with given id
                if (drone.ParcelInTransfer.ParcelState == true)//if parcel is delivered
                {
                    //finding distance between drones collecion location and the delivery destination location
                    int distance = (int)Distance.Haversine
                    (drone.ParcelInTransfer.CollectionLocation.Longitude, drone.ParcelInTransfer.CollectionLocation.Latitude,
                    drone.ParcelInTransfer.DeliveryDestination.Longitude, drone.ParcelInTransfer.DeliveryDestination.Latitude);
                    //battery is measured by the distance the drone did and the amount of battery that goes down according to the parcel weight
                    droneToList.Battery -= (int)(distance * Weight(drone.Weight));
                    droneToList.CurrentLocation = drone.ParcelInTransfer.DeliveryDestination;//updating location to destination location
                    droneToList.DroneStatus = (DroneStatuses)1;//updating status of drone to be available
                    int indexOfDroneToList = BlDrones.FindIndex(indexOfDroneToList => indexOfDroneToList.Id == droneId);
                    BlDrones[indexOfDroneToList] = droneToList;//placing updated droneToList into the list of BlDRones
                    dal.UpdateParcelDeliveryToCustomer(droneId);//sending to update in dal
                }
                else
                    throw new ParcelDeliveryException("Drone could not deliver parcel.\n");
            }
            catch (ArgumentNullException ex)
            {
                throw new FailedToCollectParcelException(ex.ToString()); ;
            }
            catch (ParcelDeliveryException ex)
            {
                throw new ParcelDeliveryException(ex.ToString());
            }

        }

        public void UpdateParcelCollectionByDrone(int droneId)
        {
            try
            {
                DroneToList droneToList = BlDrones.Find(indexOfDroneToList => indexOfDroneToList.Id == droneId);//finding drone with given id
                if(droneToList==null)
                {}
                IDAL.DO.Parcel parcel = dal.GetAllParcels().First(item => item.Id == droneToList.ParcelNumInTransfer);//finding parcel that is assigned to this drone
                IDAL.DO.Customer sender = dal.GetAllCustomers().First(item => item.Id == parcel.SenderId);//finding sender that sended this parcel
                if (parcel.Scheduled != DateTime.MinValue && parcel.PickedUp == DateTime.MinValue)
                {
                    //finding distance between original location of drone to the location of its destination
                    int distance = (int)Distance.Haversine
                        (droneToList.CurrentLocation.Longitude, droneToList.CurrentLocation.Latitude, sender.Longitude, sender.Latitude);
                    droneToList.Battery -= (int)(distance * Weight(droneToList.Weight));//updating battery according to distance and weight of the parcel
                    droneToList.CurrentLocation.Longitude = sender.Longitude;//updating location to sender location
                    droneToList.CurrentLocation.Latitude = sender.Latitude;
                    int indexOfDroneToList = BlDrones.FindIndex(indexOfDroneToList => indexOfDroneToList.Id == droneId);
                    BlDrones[indexOfDroneToList] = droneToList;//placing updated droneToList into the list of BlDRones
                    dal.UpdateParcelCollectionByDrone(parcel.Id);//sending to update in dal
                }
                else
                    throw new FailedToCollectParcelException("The drone must meet the condition that it is associated but has not yet been collected.\n");
            }

            catch (InvalidOperationException ex)
            {
                throw new FailedToCollectParcelException("Does not exist.\n", ex);
            }
        }
    }
}
