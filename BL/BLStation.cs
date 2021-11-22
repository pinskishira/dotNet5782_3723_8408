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
            if ((Math.Round(Math.Floor(Math.Log10(newStation.Id))) + 1) != 4)//if id inputted is not 4 digits long
                throw new InvalidInputException("The identification number should be 4 digits long\n");
            if (newStation.Name == "\n")//if nothing was inputted as name for station
                throw new InvalidInputException("You have to enter a valid name, with letters\n");
            //if longitude isnt between -180 and 180 and latitude isnt between -90 and 90
            if (newStation.StationLocation.Longitude < -180 || newStation.StationLocation.Longitude > 180)
                throw new InvalidInputException("The longitude is not valid, enter a longitude point between -180 and 180\n");
            if (newStation.StationLocation.Latitude < -90 || newStation.StationLocation.Latitude > 90)
                throw new InvalidInputException("The Latitude is not valid, enter a Latitude point between -90 and 90\n");
            if (newStation.AvailableChargeSlots < 0)
                throw new InvalidInputException("The number of charging stations of the station is less than 0\n");
            try
            {
                //converting BL station to dal
                IDAL.DO.Station tempStation = new();
                newStation.CopyPropertiesTo(tempStation);
                dal.AddStation(tempStation);//adding to station array in dal

            }
            catch (IDAL.DO.ItemExistsException ex)
            {
                throw new FailedToAddException("The station already exists.\n", ex);
            }
        }

        /// <summary>
        /// Displays a specific station, by converting station to BL an filling the missing fields.
        /// </summary>
        /// <param name="stationId">Id of station</param>
        /// <returns>Parcel</returns>
        public Station DisplayStation(int stationId) 
        {
            Station blStation = new();
            try
            {
                IDAL.DO.Station dalStation = dal.FindStation(stationId);//finding station
                dalStation.CopyPropertiesTo(blStation);//converting to BL
                foreach (var indexOfDroneCharges in dal.GetAllDroneCharges())//going through drone charges
                {
                    if (indexOfDroneCharges.StationId == stationId)//if station id's match
                    {
                        DroneInCharging tempDroneInCharging = new();
                        tempDroneInCharging.Id = indexOfDroneCharges.DroneId;//id's will be equal
                        DroneToList tempDroneToList = BlDrones.Find(indexDroneToList => indexDroneToList.Id == indexOfDroneCharges.DroneId);
                        if (tempDroneToList == default)
                            throw new FailedDisplayException("The Id number does not exist. \n");
                        tempDroneInCharging.Battery = tempDroneToList.Battery;//battery's will be equal
                        blStation.DronesInCharging.Add(tempDroneInCharging);//adding to drones in charging
                    }
                }
            }
            catch (IDAL.DO.ItemDoesNotExistException ex)
            {
                throw new FailedDisplayException("The Id does not exist.\n", ex);
            }
            return blStation;
        }

        /// <summary>
        /// Converting BL list to dal and updating amount of unavailable charge slots, then adding to stationToList.
        /// </summary>
        /// <returns>List of stations</returns>
        public IEnumerable<StationToList> ListViewStations()
        {
            Station tempStation = new();
            StationToList tempStationToList = new();
            List<StationToList> stationToList = new List<StationToList>();
            foreach (var indexOfStations in dal.GetAllStations())//going through stations
            {
                tempStation = DisplayStation(indexOfStations.Id);//getting station with inputted index
                tempStation.CopyPropertiesTo(tempStationToList);//converting to StationToList
                tempStationToList.UnavaialbleChargingSlots = tempStation.DronesInCharging.Count;//checks how many drones are in charging and counts them 
                stationToList.Add(tempStationToList);//ading to StationToList
            }
            return stationToList;
        }

        /// <summary>
        /// Returns list of stations with free charging slots
        /// </summary>
        /// <returns>List of stations</returns>
        public IEnumerable<StationToList> GetStationWithFreeSlots()
        {
            List<StationToList> stationToList = new();
            foreach (var indexOfStation in ListViewStations())
            {
                if (indexOfStation.AvailableChargingSlots > 0)//if station has available charging slots
                    stationToList.Add(indexOfStation);//add to list
            }
            return stationToList;
        }

        /// <summary>
        /// Finds station and sends to update in dal.
        /// </summary>
        /// <param name="idStation">Id of station</param>
        /// <param name="newName">New name of station</param>
        /// <param name="chargeSlots">Amount of charge slots in station</param>
        public void UpdateStation(int idStation, string newName, int chargeSlots)
        {
            if (chargeSlots < 0)//if invalid number was inpitted
                throw new InvalidInputException("The inputted number of empty charges is incorrect. \n");
            dal.GetAllStations().First(item => item.Id == idStation);//finds station
            dal.UpdateStation(idStation, newName, chargeSlots);//sends to update in dal
        }
    }
}
