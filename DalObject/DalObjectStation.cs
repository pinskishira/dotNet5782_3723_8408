using System.Collections.Generic;
using System;

using DO;
using System.Linq;

namespace Dal
{
    partial class DalObject
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddStation(Station newStation)
        {
            if (DataSource.Stations.Exists(item => item.Id == newStation.Id))//checks if station exists
                throw new ItemExistsException("The station already exists.\n");
            DataSource.Stations.Add(newStation);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Station FindStation(int id)
        {
            try
            {
                return DataSource.Stations.First(item => item.Id == id);//checks if station exists
            }
            catch (InvalidOperationException)
            {
                throw new ItemExistsException("The station does not exist.\n");
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Station> GetAllStations(Predicate<Station> predicate = null)
        {
            return from itemStation in DataSource.Stations
                   where predicate == null ? true : predicate(itemStation)
                   select itemStation;
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

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateStation(int idStation, string newName, int chargeSlots)
        {
            int indexOfStation = DataSource.Stations.FindIndex(item => item.Id == idStation);
            if(indexOfStation==-1)//checks if station exists
                throw new ItemDoesNotExistException("The station does not exist.\n");
            Station station = DataSource.Stations[indexOfStation];
            if (newName != "")//if enter wasnt inputted
                station.Name = newName;
            if (chargeSlots != 0)//if 0 wasnt inputted
                station.AvailableChargeSlots = chargeSlots - ChargeSlotsInUse(idStation);
            DataSource.Stations[indexOfStation] = station;//placing updated station in list of stations
        }
    }
}
