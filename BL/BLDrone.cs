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
        public void AddDrone(Drone newDrone, int stationNumber)
        {
            if ((Math.Round(Math.Floor(Math.Log10(newDrone.Id))) + 1) != 5)//if the ID number of the drone is not 5 digits
                throw new InvalidInputException("The identification number should be 5 digits long\n");
            if (newDrone.Model == null)
                throw new InvalidInputException("The drone model is incorrect");
            if (newDrone.Weight != WeightCategories.Easy && newDrone.Weight != WeightCategories.Medium && newDrone.Weight != WeightCategories.Heavy)//if 1,2 or 3 werent inputted
                throw new InvalidInputException("You need to select 1- for Easy 2- for Medium 3- for Heavy\n");
            if ((Math.Round(Math.Floor(Math.Log10(stationNumber))) + 1) != 4)//if station id isnt 4 digits long
                throw new InvalidInputException("The identification number should be 4 digits long\n");
            newDrone.Battery = rand.Next(20, 41);//battery status between 20 and 40
            newDrone.DroneStatus = DroneStatuses.Maintenance;//drone status -> maintanace
            lock (dal)
            {
                DO.Station newStation = dal.FindStation(stationNumber);//finds the station by the ID number the user entered
                newDrone.CurrentLocation = new Location();
                newDrone.CurrentLocation.Longitude = newStation.Longitude;//updates the longitude according to the longitude of the station
                newDrone.CurrentLocation.Latitude = newStation.Latitude;//updates the latitude according to the latitude of the station
                DO.DroneCharge tempDroneCharge = new DO.DroneCharge();//updates that the drone is charging
                tempDroneCharge.DroneId = newDrone.Id;
                tempDroneCharge.StationId = stationNumber;
                DroneToList newDroneToList = new DroneToList();//adding information to droneToLists
                newDroneToList.CurrentLocation = new Location();
                newDrone.CopyPropertiesTo(newDroneToList);
                newDroneToList.CurrentLocation = newDrone.CurrentLocation;
                if (newDrone.ParcelInTransfer == null)
                    newDroneToList.ParcelIdInTransfer = 0;
                else
                    newDroneToList.ParcelIdInTransfer = newDrone.ParcelInTransfer.Id;
                BlDrones.Add(newDroneToList);
                try
                {
                    dal.AddDroneCharge(tempDroneCharge);//sends to add to the drones in charging
                                                        //converting BL drone to dal
                    DO.Drone tempDrone = new DO.Drone();
                    object obj = tempDrone;
                    newDrone.CopyPropertiesTo(obj);
                    tempDrone = (DO.Drone)obj;
                    newStation.CopyPropertiesTo(tempDrone);
                    dal.AddDrone(tempDrone);//sends to add to drones
                }
                catch (DO.ItemExistsException ex)
                {
                    throw new FailedToAddException("ERROR.\n", ex);
                }
                catch (DO.ItemDoesNotExistException ex)
                {
                    throw new FailedToAddException("ERROR.\n", ex);
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Drone GetDrone(int droneId)
        {
            lock (dal)
            {
                try
                {
                    DroneToList tempDroneToList = BlDrones.First(item => item.Id == droneId);//searches the list of drones by ID number of the drone
                    Drone blDrone = new Drone();
                    blDrone.CurrentLocation = new Location();
                    tempDroneToList.CopyPropertiesTo(blDrone);//converting the drone in the list to a regular drone
                    blDrone.CurrentLocation = CopyLocation(tempDroneToList.CurrentLocation.Longitude, tempDroneToList.CurrentLocation.Latitude);
                    if (tempDroneToList.ParcelIdInTransfer == 0)//parcel wasnt assigned by drone
                        blDrone.ParcelInTransfer = default;
                    else//was assigned by drone
                    {
                        Parcel tempParcel = GetParcel(tempDroneToList.ParcelIdInTransfer);//searches for the parcel by ID number
                        ParcelInTransfer tempParcelInTransfer = new ParcelInTransfer();
                        tempParcelInTransfer.Sender = new CustomerInParcel();
                        tempParcelInTransfer.Target = new CustomerInParcel();
                        tempParcelInTransfer.CollectionLocation = new Location();
                        tempParcelInTransfer.DeliveryDestination = new Location();
                        tempParcel.CopyPropertiesTo(tempParcelInTransfer);//converting from parcel to 
                        Customer Sender = GetCustomer(tempParcel.Sender.Id);//finding sender
                        Customer Target = GetCustomer(tempParcel.Target.Id);//finding target 
                        Sender.CopyPropertiesTo(tempParcelInTransfer.Sender);
                        Target.CopyPropertiesTo(tempParcelInTransfer.Target);
                        //updating location with sender location and target location
                        tempParcelInTransfer.CollectionLocation = Sender.CustomerLocation;
                        tempParcelInTransfer.DeliveryDestination = Target.CustomerLocation;
                        if (tempParcel.PickedUp == null)//if package is waiting for pickup
                            tempParcelInTransfer.ParcelState = false;
                        else
                            tempParcelInTransfer.ParcelState = true;
                        //finding distance between sender and target
                        tempParcelInTransfer.TransportDistance = Distance.Haversine
                        (Sender.CustomerLocation.Longitude, Sender.CustomerLocation.Latitude, Target.CustomerLocation.Longitude, Target.CustomerLocation.Latitude);
                        blDrone.ParcelInTransfer = tempParcelInTransfer;
                    }
                    return blDrone;
                }
                catch (ArgumentNullException)
                {
                    throw new FailedGetException("The ID number does not exist\n");
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DroneToList> GetAllDrones(Predicate<DroneToList> predicate = null)
        {
            lock (dal)
            {
                return BlDrones.FindAll(item => (predicate == null ? true : predicate(item)) && !item.DeletedDrone);
            }
        }

        public void UpdateDrone(int idDrone, string model)
        {
            lock (dal)
            {
                try
                {
                    DO.Drone dalDrone = dal.GetAllDrones().First(indexDrones => indexDrones.Id == idDrone);//finding drone
                    if (!BlDrones.Exists(indexOfDroneToList => indexOfDroneToList.Id == idDrone))
                        throw new ItemDoesNotExistException("The drone does not exist.\n");
                    dalDrone.Model = model;//changing model name
                    dal.UpdateDrone(dalDrone);//sending to update in drone
                    DroneToList droneToList = BlDrones.First(indexOfDroneToList => indexOfDroneToList.Id == idDrone);
                    droneToList.Model = model;
                }
                catch (InvalidOperationException)
                {
                    throw new ItemDoesNotExistException("The drone does not exist.\n");
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void SendDroneToChargingStation(int idDrone)
        {
            lock (dal)
            {
                try
                {
                    Drone drone = GetDrone(idDrone);//finding drone using inputted id from user
                    if (drone.DroneStatus != DroneStatuses.Available)//checking if drone is available
                        throw new DroneMaintananceException("The drone is not available");
                    //finding smallest distance of drone from closest station
                    DO.Station station = smallestDistanceFromDrone(drone.CurrentLocation);
                    if (station.Id == -1)
                        throw new DroneMaintananceException("There is no station with available charging stations");
                    //finds amount of battery used during the drone travel from its current location to the closest station to him
                    int batteryConsumption = (int)Math.Ceiling(Distance.Haversine
                        (drone.CurrentLocation.Longitude, drone.CurrentLocation.Latitude, station.Longitude, station.Latitude) * PowerUsageEmpty);
                    if (batteryConsumption > drone.Battery)
                        throw new DroneMaintananceException("The drone does not have enough battery to go to the station");
                    //battery is decreases by amount of battery used during travel times the percentage of battery used with no parcel
                    drone.Battery -= (int)(batteryConsumption * PowerUsageEmpty);
                    drone.CurrentLocation.Longitude = station.Longitude;//upating location
                    drone.CurrentLocation.Latitude = station.Latitude;
                    drone.DroneStatus = DroneStatuses.Maintenance;
                    DroneToList droneToList = BlDrones.First(indexOfDroneToList => indexOfDroneToList.Id == idDrone);//finding drone using inputted id
                    drone.CopyPropertiesTo(droneToList);//converting drone -> droneToList
                    droneToList.CurrentLocation = CopyLocation(drone.CurrentLocation.Longitude, drone.CurrentLocation.Latitude);
                    if (drone.ParcelInTransfer != null)
                        droneToList.ParcelIdInTransfer = drone.ParcelInTransfer.Id;
                    else
                        droneToList.ParcelIdInTransfer = 0;
                    dal.UpdateSendDroneToChargingStation(drone.Id, station.Name);
                }
                catch (DO.ItemExistsException ex)
                {
                    throw new DroneMaintananceException("ERROR\n", ex);
                }
                catch (DO.ItemDoesNotExistException ex)
                {
                    throw new DroneMaintananceException("ERROR\n", ex);
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DroneReleaseFromChargingStation(int idDrone)
        {
            lock (dal)
            {
                try
                {
                    DroneToList droneToList = BlDrones.First(indexOfDroneToList => indexOfDroneToList.Id == idDrone);//finding drone using inputted id
                    if (droneToList.DroneStatus != DroneStatuses.Maintenance)//checking if drone is in maintanace
                        throw new DroneMaintananceException("The drone is not Maintenance");
                    //battery decreases by amount of time in charging times its charing rate per hour
                    TimeSpan timeInCharging = DateTime.Now - dal.GetDroneCharge(droneToList.Id).TimeDroneInCharging;
                    int batteryCharge = (int)(timeInCharging.TotalHours * DroneChargingRatePH);
                    if (batteryCharge + droneToList.Battery > 100)
                        droneToList.Battery = 100;
                    else
                        droneToList.Battery += batteryCharge;
                    droneToList.DroneStatus = DroneStatuses.Available;//drone is now available
                    dal.DroneReleaseFromChargingStation(idDrone);//sending to update in dal
                }
                catch (InvalidOperationException)
                {
                    throw new DroneMaintananceException("The drone does not exist.\n");
                }
            }
        }

        public DO.Station smallestDistanceFromDrone(Location CurrentLocation)
        {
            lock (dal)
            {
                double minDistance = double.PositiveInfinity;//starting with an unlimited value
                DO.Station station = new DO.Station();
                station.Id = -1;
                double tempDistance = -1;
                List<DO.Station> stationList = dal.GetAllStations().ToList();
                foreach (var indexOfStations in stationList)//goes through all the stations 
                {
                    //calculating the distance between the sender and the station
                    tempDistance = Distance.Haversine(indexOfStations.Longitude, indexOfStations.Latitude, CurrentLocation.Longitude, CurrentLocation.Latitude);
                    if (tempDistance < minDistance && indexOfStations.AvailableChargeSlots > 0)//compares which distance is smaller
                    {
                        minDistance = tempDistance;
                        station = indexOfStations;
                    }
                }
                return station;//returns closest station to sender
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteDrone(int idDrone)
        {
            lock (dal)
            {
                try
                {
                    DroneToList droneToList = BlDrones.Find(item => item.Id == idDrone);
                    droneToList.DeletedDrone = true;
                    if (droneToList.DroneStatus == DroneStatuses.Maintenance)
                        dal.DeleteDroneCharge(droneToList.Id);
                    dal.DeleteDrone(idDrone);//delete parcel
                }
                catch (DO.ItemDoesNotExistException ex)
                {
                    throw new ItemDoesNotExistException("ERROR.\n", ex);
                }
            }
        }
    }
}
