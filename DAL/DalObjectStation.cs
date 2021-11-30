using System.Collections.Generic;
using System;

using IDAL.DO;

namespace DalObject
{
    public partial class DalObject
    {
        public void AddStation(Station newStation)
        {
            if (DataSource.Stations.Exists(item => item.Id == newStation.Id))//checks if station exists
                throw new ItemExistsException("The station already exists.\n");
            DataSource.Stations.Add(newStation);
        }

        public Station FindStation(int id)
        {
            if (!DataSource.Stations.Exists(item => item.Id == id))//checks if station exists
                throw new ItemDoesNotExistException("The station does not exist.\n");
            return DataSource.Stations[DataSource.Stations.FindIndex(item => item.Id == id)];//Going through stations list
        }

        //public IEnumerable<Station> GetStationWithFreeSlots(Predicate<Station> predicate = null)
        //{
        //    //List<Station> freeSlotsStation = new();
        //    //foreach (var indexSlots in DataSource.Stations)//goes through stations list
        //    //{
        //    //    if (indexSlots.AvailableChargeSlots > 0)//if he has available charging slots
        //    //        freeSlotsStation.Add(indexSlots);
        //    //}
        //    //return freeSlotsStation;
        //}

        public IEnumerable<Station> GetAllStations(Predicate<Station> predicate = null)
        {
            //List<Station> tempStations = new List<Station>();
            //foreach (var indexOfStations in DataSource.Stations)//goes through stations list
            //{
            //    tempStations.Add(indexOfStations);//adds to list
            //}
            //return tempStations;
            return DataSource.Stations.FindAll(item => predicate == null ? true : predicate(item));

        }

        /// <summary>
        /// Counts how many charge slots are in use.
        /// </summary>
        /// <param name="idStation">Id of stat</param>
        /// <returns>charge Slots In Use</returns>
        private int ChargeSlotsInUse(int idStation)
        {
            int chargeSlotsInUse = 0;
            foreach (var indexOfDroneCharge in DataSource.DroneCharges)//goes through list of drones in charging
                if (indexOfDroneCharge.StationId == idStation)//If the loaded drone ID number is equal to the station ID number
                    chargeSlotsInUse++;
            return chargeSlotsInUse;
        }

        public void UpdateStation(int idStation, string newName, int chargeSlots)
        {
            if (!DataSource.Stations.Exists(item => item.Id == idStation))//checks if station exists
                throw new ItemDoesNotExistException("The station does not exist.\n");
            int indexOfStation = DataSource.Stations.FindIndex(item => item.Id == idStation);
            Station station = DataSource.Stations[indexOfStation];
            if (newName != "")//if enter wasnt inputted
                station.Name = newName;
            if (chargeSlots != 0)//if 0 wasnt inputted
                station.AvailableChargeSlots = chargeSlots - ChargeSlotsInUse(idStation);
            DataSource.Stations[indexOfStation] = station;//placing updated station in list of stations
        }
    }
}
