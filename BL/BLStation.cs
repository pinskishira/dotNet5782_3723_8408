﻿using System;
using System.Collections.Generic;
using System.Linq;

using BO;

namespace BL
{
    partial class BL
    {
        public void AddStation(Station newStation)
        {
            if ((Math.Round(Math.Floor(Math.Log10(newStation.Id))) + 1) != 4)//if id inputted is not 4 digits long
                throw new InvalidInputException("The identification number should be 4 digits long\n");
            if (newStation.Name == "\n")//if nothing was inputted as name for station
                throw new InvalidInputException("You have to enter a valid name, with letters\n");//if longitude isnt between 29.3 and 33.5 and latitude isnt between 33.7 and 36.3
            if (newStation.StationLocation.Longitude < 29.3 || newStation.StationLocation.Longitude > 33.5)
                throw new InvalidInputException("The longitude is not valid, enter a longitude point between 29.3 and 33.5\n");
            if (newStation.StationLocation.Latitude < 33.7 || newStation.StationLocation.Latitude > 36.3)
                throw new InvalidInputException("The Latitude is not valid, enter a Latitude point between 33.7 and 36.3\n");
            if (newStation.AvailableChargeSlots < 0)
                throw new InvalidInputException("The number of charging stations of the station is less than 0\n");
            try
            {
                //converting BL station to dal
                DO.Station tempStation = new DO.Station();
                object obj = tempStation;
                newStation.CopyPropertiesTo(obj);
                tempStation = (DO.Station)obj;
                newStation.CopyPropertiesTo(tempStation);
                dal.AddStation(tempStation);//adding to station list in dal
            }
            catch (DO.ItemExistsException ex)
            {
                throw new FailedToAddException("ERROR.\n", ex);
            }
        }

        public Station GetStation(int stationId)
        {
            Station blStation = new Station();
            try
            {
                DO.Station dalStation = dal.FindStation(stationId);//finding station
                dalStation.CopyPropertiesTo(blStation);//converting to BL
                blStation.StationLocation = CopyLocation(dalStation.Longitude, dalStation.Latitude);
                blStation.DronesInCharging = from item in dal.GetAllDroneCharges(item => item.StationId == stationId)
                                             let temp = BlDrones.FirstOrDefault(indexDroneToList => indexDroneToList.Id == item.DroneId)
                                             select new DroneInCharging
                                             {
                                                 Id = item.DroneId,
                                                 Battery = temp != default ? temp.Battery :
                                              throw new FailedGetException("The Id number does not exist. \n")
                                             };


                //List <DO.DroneCharge> DroneChargeList = dal.GetAllDroneCharges(item => item.StationId == stationId).ToList();
                //foreach (var indexOfDroneCharges in DroneChargeList)//going through drone charges
                //{
                //    DroneInCharging tempDroneInCharging = new DroneInCharging();
                //    tempDroneInCharging.Id = indexOfDroneCharges.DroneId;//id's will be equal
                //    DroneToList tempDroneToList = BlDrones.First(indexDroneToList => indexDroneToList.Id == indexOfDroneCharges.DroneId);
                //    if (tempDroneToList == default)
                //        throw new FailedGetException("The Id number does not exist. \n");
                //    tempDroneInCharging.Battery = tempDroneToList.Battery;//battery's will be equal
                //    blStation.DronesInCharging.ToList().Add(tempDroneInCharging);//adding to drones in charging
                //}


            }
            catch (DO.ItemDoesNotExistException ex)
            {
                throw new FailedGetException("ERROR.\n", ex);
            }
            catch (InvalidOperationException)
            {
                throw new FailedGetException("The drone does not exist.\n");
            }
            return blStation;
        }

        public IEnumerable<StationToList> GetAllStations(Predicate<StationToList> predicate = null)
        {
            Station tempStation = new Station();
            List<StationToList> stationToList = new List<StationToList>();
            List<DO.Station> stationList = dal.GetAllStations().ToList();
            foreach (var indexOfStations in stationList)//going through stations
            {
                StationToList tempStationToList = new StationToList();
                tempStation = GetStation(indexOfStations.Id);//getting station with inputted index
                tempStation.CopyPropertiesTo(tempStationToList);//converting to StationToList
                if (tempStation.DronesInCharging == null)
                    tempStationToList.OccupiedChargeSlots = 0;
                else
                    tempStationToList.OccupiedChargeSlots = tempStation.DronesInCharging.Count();//checks how many drones are in charging and counts them 
                stationToList.Add(tempStationToList);//ading to StationToList
            }
            return stationToList.FindAll(item => predicate == null ? true : predicate(item));
        }

        public void UpdateStation(int idStation, string newName, int chargeSlots)
        {
            try
            {
                if (chargeSlots < 0)//if invalid number was inputted
                    throw new InvalidInputException("The inputted number of empty charges is incorrect. \n");
                dal.UpdateStation(idStation, newName, chargeSlots);//sends to update in dal
            }
            catch (DO.ItemDoesNotExistException ex)
            {
                throw new FailedToAddException("ERROR.\n", ex);
            }
        }
    }
}
