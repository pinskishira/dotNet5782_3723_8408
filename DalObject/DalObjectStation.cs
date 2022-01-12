using System.Collections.Generic;
using System;

using DO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Dal
{
    partial class DalObject
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddStation(Station newStation)
        {
            if (DataSource.Stations.Exists(item => item.Id == newStation.Id && !newStation.DeletedStation))//checks if station exists
                throw new ItemExistsException("The station already exists.\n");
            DataSource.Stations.Add(newStation);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Station FindStation(int id)
        {
            int indexStation = DataSource.Stations.FindIndex(customer => customer.Id == id);//checks if station exists
            if (indexStation == -1)
                throw new ItemDoesNotExistException("No station found with this id");
            return DataSource.Stations[indexStation];//finding station
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Station> GetAllStations(Predicate<Station> predicate = null)
        {
            return from itemStation in DataSource.Stations
                   where predicate == null ? true : predicate(itemStation) && (!itemStation.DeletedStation)
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
            int indexOfStation = CheckExistingStation(idStation);
            Station station = DataSource.Stations[indexOfStation];
            if (newName != "")//if enter wasnt inputted
                station.Name = newName;
            if (chargeSlots != 0)//if 0 wasnt inputted
                station.AvailableChargeSlots = chargeSlots - ChargeSlotsInUse(idStation);
            DataSource.Stations[indexOfStation] = station;//placing updated station in list of stations
        }

        private int CheckExistingStation(int id)
        {
            int index = DataSource.Stations.FindIndex(customer => customer.Id == id);
            if (index == -1)
                throw new ItemDoesNotExistException("No station found with this id");
            if (DataSource.Stations[index].DeletedStation)
                throw new ItemDoesNotExistException("This station is deleted");
            return index;
        }

        public void DeleteStation(int id)
        {
            int indexOfStations = CheckExistingStation(id);//checks if parcel exists
            Station station = DataSource.Stations[indexOfStations];
            station.DeletedStation = true;
            DataSource.Stations[indexOfStations] = station;
        }
    }
}
