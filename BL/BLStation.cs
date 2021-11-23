using System;
using System.Collections.Generic;
using System.Linq;

using IBL.BO;

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
            //if longitude isnt between 29.3 and 33.5 and latitude isnt between 33.7 and 36.3
            if (newStation.StationLocation.Longitude < 29.3 || newStation.StationLocation.Longitude > 33.5)
                throw new InvalidInputException("The longitude is not valid, enter a longitude point between 29.3 and 33.5\n");
            if (newStation.StationLocation.Latitude < 33.7 || newStation.StationLocation.Latitude > 36.3)
                throw new InvalidInputException("The Latitude is not valid, enter a Latitude point between 33.7 and 36.3\n");
            if (newStation.AvailableChargeSlots < 0)
                throw new InvalidInputException("The number of charging stations of the station is less than 0\n");
            try
            {
                //converting BL station to dal
                IDAL.DO.Station tempStation = new();
                object obj = tempStation;
                newStation.CopyPropertiesTo(obj);
                tempStation = (IDAL.DO.Station)obj;
                newStation.CopyPropertiesTo(tempStation);
                dal.AddStation(tempStation);//adding to station list in dal
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
                blStation.StationLocation = CopyLocation(dalStation.Longitude, dalStation.Latitude);
                blStation.DronesInCharging = new();
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
            List<StationToList> stationToList = new List<StationToList>();
            foreach (var indexOfStations in dal.GetAllStations())//going through stations
            {
                StationToList tempStationToList = new();
                tempStation = DisplayStation(indexOfStations.Id);//getting station with inputted index
                tempStation.CopyPropertiesTo(tempStationToList);//converting to StationToList
                if (tempStation.DronesInCharging == null)
                    tempStationToList.OccupiedChargeSlots = 0;
                else
                    tempStationToList.OccupiedChargeSlots = tempStation.DronesInCharging.Count;//checks how many drones are in charging and counts them 
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
                if (indexOfStation.AvailableChargeSlots > 0)//if station has available charging slots
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
            if (chargeSlots < 0)//if invalid number was inputted
                throw new InvalidInputException("The inputted number of empty charges is incorrect. \n");
            dal.GetAllStations().First(item => item.Id == idStation);//finds station
            dal.UpdateStation(idStation, newName, chargeSlots);//sends to update in dal
        }
    }
}
