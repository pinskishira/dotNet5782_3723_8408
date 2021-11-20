using System;
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
        /// Performing logical tests on the recieved station and coverting the station fields in the dalObject
        /// to the station fields in the BL.
        /// </summary>
        /// <param name="newStation">The new station</param>
        public void AddStation(Station newStation)
        {
            if ((Math.Round(Math.Floor(Math.Log10(newStation.Id))) + 1) != 4)
                throw new InvalidInputException("The identification number should be 4 digits long\n");
            if (newStation.NameOfStation == "\n")
                throw new InvalidInputException("You have to enter a valid name, with letters\n");
            if (newStation.StationLocation.Longitude < -180 || newStation.StationLocation.Longitude > 180)
                throw new InvalidInputException("The longitude is not valid, enter a longitude point between -180 and 180\n");
            if (newStation.StationLocation.Latitude < -90 || newStation.StationLocation.Latitude > 90)
                throw new InvalidInputException("The Latitude is not valid, enter a Latitude point between -90 and 90\n");
            if (newStation.AvailableChargeSlots < 0)
                throw new InvalidInputException("The number of charging stations of the station is less than 0\n");
            try
            {
                IDAL.DO.Station tempStation = new();
                newStation.CopyPropertiesTo(tempStation);
                dal.AddStation(tempStation);
            }
            catch (IDAL.DO.ItemExistsException ex)
            {
                throw new FailedToAddException("The station already exists.\n", ex);
            }
        }

        public Station DisplayStation(int stationId) //תצוגת תחנת-בסיס
        {
            Station blStation = new();
            try
            {
                IDAL.DO.Station dalStation = dal.FindStation(stationId);
                dalStation.CopyPropertiesTo(blStation);
                foreach (var indexOfDroneCharges in dal.GetAllDroneCharges())
                {
                    if (indexOfDroneCharges.StationId == stationId)
                    {
                        DroneInCharging tempDroneInCharging = new();
                        tempDroneInCharging.Id = indexOfDroneCharges.DroneId;
                        DroneToList tempDroneToList = BlDrones.Find(indexDroneToList => indexDroneToList.Id == indexOfDroneCharges.DroneId);
                        if (tempDroneToList == default)
                            throw new FailedDisplayException("The Id number does not exist. \n");
                        tempDroneInCharging.Battery = tempDroneToList.Battery;
                        blStation.DronesInCharging.Add(tempDroneInCharging);
                    }
                }
            }
            catch (IDAL.DO.ItemDoesNotExistException ex)
            {
                throw new FailedDisplayException("The Id does not exist.\n", ex);
            }
            return blStation;
        }

        public IEnumerable<StationToList> ListViewStations()
        {
            Station tempStation = new();
            StationToList tempStationToList = new();
            List<StationToList> stationToList = new List<StationToList>();
            foreach (var indexOfStations in dal.GetAllStations())
            {
                tempStation = DisplayStation(indexOfStations.Id);
                tempStation.CopyPropertiesTo(tempStationToList);//
                tempStationToList.UnavaialbleChargingSlots = tempStation.DronesInCharging.Count;//בודק כמה רחפנים בטעינה וסופר
                stationToList.Add(tempStationToList);
            }
            return stationToList;
        }
        public IEnumerable<StationToList> GetStationWithFreeSlots()
        {
            List<StationToList> stationToList = new();
            foreach (var indexOfStation in ListViewStations())
            {
                if (indexOfStation.AvailableChargingSlots > 0)
                    stationToList.Add(indexOfStation);
            }
            return stationToList;
        }

        public void UpdateStation(int idStation, string newName, int chargeSlots)
        {
            if (chargeSlots < 0)
                throw new InvalidInputException("The inputted number of empty charges is incorrect. \n");
            dal.GetAllStations().First(item => item.Id == idStation);//לברר מה זה
            dal.UpdateStation(idStation, newName, chargeSlots);
        }
    }
}
