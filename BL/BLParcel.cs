using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using BO;
using static BO.Enum;

namespace BL
{
    partial class BL
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddParcel(Parcel newParcel)
        {
            if ((Math.Round(Math.Floor(Math.Log10(newParcel.Sender.Id))) + 1) != 9)//if id inputted is not 9 digits long
                throw new InvalidInputException("The identification number of sender should be 9 digits long\n");
            if ((Math.Round(Math.Floor(Math.Log10(newParcel.Target.Id))) + 1) != 9)//if id inputted is not 9 digits long
                throw new InvalidInputException("The identification number of target should be 9 digits long\n");
            if (newParcel.Weight != WeightCategories.Easy && newParcel.Weight != WeightCategories.Medium && newParcel.Weight != WeightCategories.Heavy)//if 1,2 or 3 werent inputted
                throw new InvalidInputException("You need to select 1- for Easy 2- for Medium 3- for Heavy\n");
            if (newParcel.Priority != Priorities.Normal && newParcel.Priority != Priorities.Fast && newParcel.Priority != Priorities.Emergency)//if 1,2 or 3 were inputted
                throw new InvalidInputException("You need to select 1- for Normal 2- for Fast 3- for Emergency\n");
            lock (dal)
            {
                //updating times
                newParcel.Requested = DateTime.Now;
                newParcel.Scheduled = null;
                newParcel.PickedUp = null;
                newParcel.Delivered = null;
                try
                {
                    //converting BL parcel to dal
                    DO.Parcel tempParcel = new DO.Parcel();
                    object obj = tempParcel;
                    newParcel.CopyPropertiesTo(obj);
                    tempParcel = (DO.Parcel)obj;
                    newParcel.CopyPropertiesTo(tempParcel);
                    tempParcel.SenderId = newParcel.Sender.Id;
                    tempParcel.TargetId = newParcel.Target.Id;
                    dal.AddParcel(tempParcel);//adding to station list in dal
                }
                catch (DO.ItemExistsException ex)
                {
                    throw new FailedToAddException("ERROR.\n", ex);
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Parcel GetParcel(int parcelId)
        {
            lock (dal)
            {
                Parcel blParcel = new Parcel();
                try
                {
                    DO.Parcel dalParcel = dal.FindParcel(parcelId);//finding parcel
                    blParcel.Sender = new CustomerInParcel();
                    blParcel.Target = new CustomerInParcel();
                    blParcel.DroneParcel = new DroneInParcel();
                    dalParcel.CopyPropertiesTo(blParcel);//converting to BL
                    Customer target = GetCustomer(dalParcel.TargetId);//finding the target who will recieve parcel
                    target.CopyPropertiesTo(blParcel.Target);//converting to BL
                    Customer sender = GetCustomer(dalParcel.SenderId);//finding the sender who sends the parcel
                    sender.CopyPropertiesTo(blParcel.Sender);//converting to BL
                    if (dalParcel.DroneId == 0)//if parcel isnt assigned to a drone
                        blParcel.DroneParcel = default;
                    else
                    {
                        DroneToList droneToList = BlDrones.First(indexOfDroneToList => indexOfDroneToList.Id == dalParcel.DroneId);
                        blParcel.DroneParcel.CurrentLocation = new Location();
                        blParcel.DroneParcel.Id = dalParcel.DroneId;
                        blParcel.DroneParcel.Battery = droneToList.Battery;
                        blParcel.DroneParcel.CurrentLocation = droneToList.CurrentLocation;
                    }
                }
                catch (DO.ItemDoesNotExistException ex)
                {
                    throw new FailedGetException("ERROR.\n", ex);
                }
                catch (InvalidOperationException)
                {
                    throw new FailedGetException("The drone does not exist.\n");
                }
                return blParcel;
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<ParcelToList> GetAllParcels(Predicate<ParcelToList> predicate = null)
        {
            lock (dal)
            {
                return (from item in dal.GetAllParcels()
                        select new ParcelToList
                        {
                            Id = item.Id,
                            SenderName = dal.FindCustomer(item.SenderId).Name,
                            TargetName = dal.FindCustomer(item.TargetId).Name,
                            Weight = (WeightCategories)item.Weight,
                            Priority = (Priorities)item.Priority,
                            StateOfParcel = item.Delivered != null ? ParcelState.Provided :
                              (item.PickedUp != null ? ParcelState.PickedUp :
                              (item.Scheduled != null ? ParcelState.Paired : ParcelState.Created))
                        }).Where(item => predicate == null ? true : predicate(item));
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateAssignParcelToDrone(int droneId)
        {
            lock (dal)
            {
                try
                {
                    DroneToList drone = BlDrones.First(indexOfDroneToList => indexOfDroneToList.Id == droneId);//Looking for the drone you want to associate
                    if (drone.DroneStatus == DroneStatuses.Available)//Check that the drone is free
                    {

                        DO.Parcel parcel = default;
                        parcel.Id = 0;
                        parcel = dal.GetAllParcels(p => p.Scheduled == null
                                               && (int)p.Weight <= (int)drone.Weight
                                               && BatteryConsumption(drone.Id, p) < drone.Battery)
                                               .OrderByDescending(x => x.Priority)
                                               .ThenByDescending(x => x.Weight)
                                               .FirstOrDefault();
                        if (parcel.Id != 0)
                        {
                            BlDrones.Find(item => item.Id == drone.Id).DroneStatus = DroneStatuses.Delivery;
                            dal.UpdateAssignParcelToDrone(parcel.Id, drone.Id);//Updating the parcel
                            drone.ParcelIdInTransfer = parcel.Id;
                        }
                        else
                            throw new FailedToCollectParcelException("The drone must meet the condition that it is associated but has not yet been collected.\n");
                    }
                }
                catch (ParcelDeliveryException)
                {
                    throw new ParcelDeliveryException("There is no parcel that can belong to this drone.\n");
                }
                catch (InvalidOperationException)
                {
                    throw new ParcelDeliveryException("Does not exist.\n");
                }
                catch (DO.ItemDoesNotExistException ex)
                {
                    throw new ParcelDeliveryException("ERROR.\n", ex);
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateParcelCollectionByDrone(int droneId)
        {
            lock (dal)
            {
                try
                {
                    DroneToList droneToList = BlDrones.First(indexOfDroneToList => indexOfDroneToList.Id == droneId);//finding drone with given id
                    DO.Parcel parcel = dal.GetAllParcels().First(item => item.Id == droneToList.ParcelIdInTransfer);//finding parcel that is assigned to this drone
                    DO.Customer sender = dal.GetAllCustomers().First(item => item.Id == parcel.SenderId);//finding sender that sended this parcel
                    if (parcel.Scheduled != null && parcel.PickedUp == null)
                    {
                        //finding distance between original location of drone to the location of its destination
                        int distance = (int)Distance.Haversine
                            (droneToList.CurrentLocation.Longitude, droneToList.CurrentLocation.Latitude, sender.Longitude, sender.Latitude);
                        droneToList.Battery -= (int)(distance * PowerUsageEmpty);//updating battery according to distance and weight of the parcel
                        droneToList.CurrentLocation = new Location();
                        droneToList.CurrentLocation.Longitude = sender.Longitude;//updating location to sender location
                        droneToList.CurrentLocation.Latitude = sender.Latitude;
                        dal.UpdateParcelCollectionByDrone(parcel.Id);//sending to update in dal
                    }
                    else
                        throw new FailedToCollectParcelException("The drone must meet the condition that it is associated but has not yet been collected.\n");
                }
                catch (InvalidOperationException)
                {
                    throw new FailedToCollectParcelException("Could not find parcel.\n");
                }
                catch (DO.ItemDoesNotExistException ex)
                {
                    throw new FailedToCollectParcelException("ERROR.\n", ex);
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateParcelDeliveryToCustomer(int droneId)
        {
            lock (dal)
            {
                try
                {
                    DroneToList droneToList = BlDrones.First(indexOfDroneToList => indexOfDroneToList.Id == droneId);//finding drone with given id
                    DO.Parcel parcel = dal.GetAllParcels().First(item => item.Id == droneToList.ParcelIdInTransfer);//finding parcel that is assigned to this drone
                    Drone drone = GetDrone(droneId);//finding drone with given id
                    if (drone.ParcelInTransfer.ParcelState == true)//if parcel is delivered
                    {
                        //finding distance between drones collecion location and the delivery destination location
                        int distance = (int)Distance.Haversine
                        (drone.ParcelInTransfer.CollectionLocation.Longitude, drone.ParcelInTransfer.CollectionLocation.Latitude,
                        drone.ParcelInTransfer.DeliveryDestination.Longitude, drone.ParcelInTransfer.DeliveryDestination.Latitude);
                        //battery is measured by the distance the drone did and the amount of battery that goes down according to the parcel weight
                        droneToList.Battery -= (int)(distance * Weight((WeightCategories)parcel.Weight));
                        droneToList.CurrentLocation = drone.ParcelInTransfer.DeliveryDestination;//updating location to destination location
                        droneToList.DroneStatus = DroneStatuses.Available;//updating status of drone to be available
                        droneToList.ParcelIdInTransfer = 0;
                        dal.UpdateParcelDeliveryToCustomer(parcel.Id);//sending to update in dal
                    }
                    else
                        throw new ParcelDeliveryException("Drone could not deliver parcel.\n");
                }
                catch (InvalidOperationException)
                {
                    throw new ParcelDeliveryException("Parcel does not exist.\n");
                }
                catch (DO.ItemDoesNotExistException ex)
                {
                    throw new ParcelDeliveryException("ERROR.\n", ex);
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteParcel(int idParcel)
        {
            lock (dal)
            {
                try
                {
                    dal.DeleteParcel(idParcel);//delete parcel
                }
                catch (DO.ItemDoesNotExistException ex)
                {
                    throw new ItemDoesNotExistException("ERROR.\n", ex);
                }
            }
        }
    }
}
