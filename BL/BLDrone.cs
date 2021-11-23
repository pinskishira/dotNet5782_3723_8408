﻿using System;
using IDAL;
using System.Collections;
using System.Collections.Generic;
using IBL.BO;
using IBL;
using static IBL.BO.Enum;
using DalObject;
using BL.IBL.BO;
using System.Linq;

namespace BL
{
    public partial class BL
    {
        /// <summary>
        /// Performing logical tests on the recieved new drone and the station its located in and coverting the 
        /// drone fields in the dalObject to the drone fields in the BL.
        /// </summary>
        /// <param name="newDrone">The new drone</param>
        /// <param name="stationNumber">Station where drone is located</param>
        public void AddDrone(Drone newDrone, int stationNumber)
        {
            if ((Math.Round(Math.Floor(Math.Log10(newDrone.Id))) + 1) != 5)//if the ID number of the drone is not 5 digits
                throw new InvalidInputException("The identification number should be 5 digits long\n");
            if (newDrone.Model.Length > 6)//if model name is less than 6 digits
                throw new InvalidInputException("The model number should be 6 digits long\n");
            if (newDrone.Weight != (WeightCategories)1 && newDrone.Weight != (WeightCategories)2 && newDrone.Weight != (WeightCategories)3)//if 1,2 or 3 werent inputted
                throw new InvalidInputException("You need to select 1- for Easy 2- for Medium 3- for Heavy\n");
            if ((Math.Round(Math.Floor(Math.Log10(stationNumber))) + 1) != 4)//if station id isnt 4 digits long
                throw new InvalidInputException("The identification number should be 4 digits long\n");
            newDrone.Battery = rand.Next(20, 41);//battery status between 20 and 40
            newDrone.DroneStatus = (DroneStatuses)2;//drone status -> maintanace
            IDAL.DO.Station newStation = dal.FindStation(stationNumber);//finds the station by the ID number the user entered
            newDrone.CurrentLocation = new();
            newDrone.CurrentLocation.Longitude = newStation.Longitude;//updates the longitude according to the longitude of the station
            newDrone.CurrentLocation.Latitude = newStation.Latitude;//updates the latitude according to the latitude of the station
            IDAL.DO.DroneCharge tempDroneCharge = new();//updates that the drone is charging
            tempDroneCharge.DroneId = newDrone.Id;
            tempDroneCharge.StationId = stationNumber;
            DroneToList newDroneToList = new();//adding information to droneToLists
            newDroneToList.Id = newDrone.Id;
            newDroneToList.Model = newDrone.Model;
            newDroneToList.Weight = newDrone.Weight;
            newDroneToList.Battery = newDrone.Battery;
            newDroneToList.DroneStatus = newDrone.DroneStatus;
            newDroneToList.CurrentLocation = newDrone.CurrentLocation;
            if (newDrone.ParcelInTransfer == null)
                newDroneToList.ParcelNumInTransfer = 0;
            else
                newDroneToList.ParcelNumInTransfer = newDrone.ParcelInTransfer.Id;
            BlDrones.Add(newDroneToList);
            try
            {
                dal.AddDroneCharge(tempDroneCharge);//sends to add to the drones in charging
                //converting BL drone to dal
                IDAL.DO.Drone tempDrone = new();
                object obj = tempDrone;
                newDrone.CopyPropertiesTo(obj);
                tempDrone = (IDAL.DO.Drone)obj;
                newStation.CopyPropertiesTo(tempDrone);
                dal.AddDrone(tempDrone);//sends to add to drones
            }
            catch (IDAL.DO.ItemExistsException ex)
            {
                throw new FailedToAddException("The drone already exists.\n", ex);
            }
            catch (IDAL.DO.ItemDoesNotExistException ex)
            {
                throw new FailedToAddException("The drone in charge does not exist.\n", ex);
            }
        }

        /// <summary>
        /// Displays a specific drone, by converting drone to BL an filling the missing fields.
        /// </summary>
        /// <param name="droneId">Id of drone</param>
        /// <returns>Drone</returns>
        public Drone DisplayDrone(int droneId)
        {
            DroneToList tempDroneToList = BlDrones.Find(item => item.Id == droneId);//searches the list of drones by ID number of the drone
            if (tempDroneToList == default)
                throw new FailedDisplayException("The ID number does not exist\n");
            Drone dalDrone = new();
            dalDrone.CurrentLocation = new();
            tempDroneToList.CopyPropertiesTo(dalDrone);//converting the drone in the list to a regular drone
            dalDrone.CurrentLocation = CopyLocation(tempDroneToList.CurrentLocation.Longitude, tempDroneToList.CurrentLocation.Latitude);
            if (tempDroneToList.ParcelNumInTransfer == 0)//parcel wasnt assigned by drone
                dalDrone.ParcelInTransfer = default;
            else//was assigned by drone
            {
                Parcel tempParcel = DisplayParcel(tempDroneToList.ParcelNumInTransfer);//searches for the parcel by ID number
                ParcelInTransfer tempParcelInTransfer = new();
                tempParcel.CopyPropertiesTo(tempParcelInTransfer);//converting from parcel to 
                Customer Sender = DisplayCustomer(tempParcelInTransfer.Sender.Id);//finding sender
                Customer Target = DisplayCustomer(tempParcelInTransfer.Target.Id);//finding target 
                //updating location with sender location and target location
                tempParcelInTransfer.CollectionLocation = Sender.CustomerLocation;
                tempParcelInTransfer.DeliveryDestination = Target.CustomerLocation;
                if (tempParcel.PickedUp == DateTime.MinValue)//if package is waiting for pickup
                    tempParcelInTransfer.ParcelState = false;
                else
                    tempParcelInTransfer.ParcelState = true;
                //finding distance between sender and target
                tempParcelInTransfer.TransportDistance = Distance.Haversine
                (Sender.CustomerLocation.Latitude, Sender.CustomerLocation.Longitude, Target.CustomerLocation.Latitude, Target.CustomerLocation.Longitude);
                dalDrone.ParcelInTransfer = tempParcelInTransfer;
            }
            return dalDrone;
        }

        /// <summary>
        /// Sending list of drones.
        /// </summary>
        /// <returns>List of drones</returns>
        public IEnumerable<DroneToList> ListViewDrones()
        {
            return BlDrones;
        }

        /// <summary>
        /// Finding drone, changing model name and sending to update in dal.
        /// </summary>
        /// <param name="idDrone">Id of drone</param>
        /// <param name="model">New model name</param>
        public void UpdateDrone(int idDrone, string model)
        {
            dal.GetAllDrones().First(item => item.Id == idDrone);
            IDAL.DO.Drone drone = dal.GetAllDrones().First(indexDrones => indexDrones.Id == idDrone);//finding drone
            drone.Model = model;//changing model name
            dal.UpdateDrone(drone);//sending to update in drone
        }

        /// <summary>
        /// Sending drone to a charging station, updating drone, and sending to update in dal.
        /// </summary>
        /// <param name="idDrone">Id of drone</param>
        public void SendDroneToChargingStation(int idDrone)
        {
            try
            {
                Drone drone = DisplayDrone(idDrone);//finding drone using inputted id from user
                if (drone.DroneStatus != (DroneStatuses)1)//checking if drone is available
                    throw new FailedSendDroneToChargingException("The drone is not available");
                //finding smallest distance of drone from closest station
                IDAL.DO.Station station = smallestDistanceFromDrone(drone.CurrentLocation);
                if (station.Id == -1)
                    throw new FailedSendDroneToChargingException("There is no station with available charging stations");
                //finds amount of battery used during the drone travel from its current location to the closest station to him
                double batteryConsumption = Distance.Haversine
                    (drone.CurrentLocation.Longitude, drone.CurrentLocation.Latitude, station.Longitude, station.Latitude)* PowerUsageEmpty;
                if (batteryConsumption < drone.Battery)
                    throw new FailedSendDroneToChargingException("The drone does not have enough battery to go to the station");
                //battery is decreases by amount of battery used during travel times the percentage of battery used with no parcel
                drone.Battery -= (int)(batteryConsumption * PowerUsageEmpty);
                drone.CurrentLocation.Longitude = station.Longitude;//upating location
                drone.CurrentLocation.Latitude = station.Latitude;
                drone.DroneStatus = (DroneStatuses)2;
                DroneToList droneToList = new();//new drone to list
                drone.CopyPropertiesTo(droneToList);//converting drone -> droneToList
                droneToList.ParcelNumInTransfer = drone.ParcelInTransfer.Id;
                int indexDroneToList = BlDrones.FindIndex(indexOfDroneToList => indexOfDroneToList.Id == droneToList.Id);//finds drone
                BlDrones[indexDroneToList] = droneToList;//updated droneToList
                //updates that the charging slots decreased and also adds the drone to the list of drones in the charge
                dal.UpdateSendDroneToChargingStation(drone.Id, station.Name);
            }
            catch (IDAL.DO.ItemExistsException ex)
            {
                throw new FailedSendDroneToChargingException(ex.ToString(), ex);
            }
            catch (IDAL.DO.ItemDoesNotExistException ex)
            {
                throw new FailedSendDroneToChargingException(ex.ToString(), ex);
            }
        }

        /// <summary>
        /// Releasing drone from a charging station and updating the drone released, and charging station
        /// and then sending to update in dal.
        /// </summary>
        /// <param name="idDrone">Id of drone</param>
        /// <param name="timeInCharginge">Time drone was in charging</param>
        public void DroneReleaseFromChargingStation(int idDrone,int timeInCharging)
        {
            try
            {
                DroneToList droneToList = BlDrones.Find(indexOfDroneToList => indexOfDroneToList.Id == idDrone);//finding drone using inputted id
                if (droneToList.DroneStatus != (DroneStatuses)2)//checking if drone is in maintanace
                    throw new FailedReleaseDroneFromChargingException("The drone is not Maintenance");
                //battery decreases by amount of time in charging times its charing rate per hour
                droneToList.Battery -= (int)(timeInCharging * DroneChargingRatePH);
                droneToList.DroneStatus = (DroneStatuses)1;//drone is now available
                dal.DroneReleaseFromChargingStation(idDrone);//sending to update in dal
                int indexOfDroneToList = BlDrones.FindIndex(indexOfDroneToList => indexOfDroneToList.Id == idDrone);//finding index
                BlDrones[indexOfDroneToList] = droneToList;//inputs updated droneToList
            }
            catch (ArgumentNullException)
            {
                throw new FailedReleaseDroneFromChargingException("The drone does not exist.\n");
            }
        }

        /// <summary>
        /// Finds the smallest distance between sent location and closest station.
        /// </summary>
        /// <param name="CurrentLocation">Current location</param>
        /// <returns></returns>
        public IDAL.DO.Station smallestDistanceFromDrone(Location CurrentLocation)
        {
            double minDistance = double.PositiveInfinity;//starting with an unlimited value
            IDAL.DO.Station station = new();
            station.Id = -1;
            double tempDistance = -1;
            foreach (var indexOfStations in dal.GetAllStations())//goes through all the stations 
            {
                //calculating the distance between the sender and the station
                tempDistance = Distance.Haversine(indexOfStations.Longitude, indexOfStations.Latitude, CurrentLocation.Longitude, CurrentLocation.Latitude) ;
                if (tempDistance < minDistance && indexOfStations.AvailableChargeSlots > 0)//compares which distance is smaller
                {
                    minDistance = tempDistance;
                    station = indexOfStations;
                }
            }
            return station;//returns closest station to sender
        }
    }
}
