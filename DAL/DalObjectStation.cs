using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
//using DAL.IDAL.DO;
using IDAL;

namespace DalObject
{
    public partial class DalObject
    {
        /// <summary>
        /// Adding a new station to the list of stations
        /// </summary>
        /// <param name="newStation">The new station</param>
        public void AddStation(Station newStation)
        {
            if (!DataSource.Stations.Exists(item => item.Id == newStation.Id))
                throw new ItemExistsException("The station already exists.\n");
            DataSource.Stations.Add(newStation);
        }

        /// <summary>
        /// Finding requested station according to its ID name
        /// </summary>
        /// <param name="id">Wanted station</param>
        /// <returns></returns>
        public Station FindStation(int id)
        {
            if (!DataSource.Stations.Exists(item => item.Id == id))
                throw new ItemDoesNotExistException("The station does not exist.\n");
            int indexFindStation = DataSource.Stations.FindIndex(item => item.Id == id);//Going through stations array
            return DataSource.Stations[indexFindStation];
        }

        /// <summary>
        /// A function that returns a list of stations whose load position is greater than 0
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Station> GetStationWithFreeSlots()
        {
            List<Station> freeSlotsStation = new();
            foreach (var indexSlots in DataSource.Stations)
            {
                if (indexSlots.AvailableChargeSlots > 0)
                    freeSlotsStation.Add(indexSlots);
            }
            return freeSlotsStation;
        }

        /// <summary>
        /// Gives a view of the list of stations
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Station> GetAllStations()
        {
            List<Station> tempStations = new List<Station>();
            foreach (var indexOfStations in DataSource.Stations)
            {
                tempStations.Add(indexOfStations);
            }
            return tempStations;
        }

        /// <summary>
        /// Counts how many charge slots are in use.
        /// </summary>
        /// <param name="idStation">Id of stat</param>
        /// <returns></returns>
        public int ChargeSlotsInUse(int idStation)//לשנות את השם של הפונקציה
        {
            int chargeSlotsInUse = 0;
            foreach (var indexOfDroneCharge in DataSource.DroneCharges)//goes through list of drones in charging
                if (indexOfDroneCharge.StationId == idStation)//If the loaded drone ID number is equal to the station ID number
                    chargeSlotsInUse++; 
            return chargeSlotsInUse;
        }

        /// <summary>
        /// Updates station name and charge slots.
        /// </summary>
        /// <param name="idStation"></param>
        /// <param name="newName"></param>
        /// <param name="chargeSlots"></param>
        public void UpdateStation(int idStation, string newName, int chargeSlots)
        {
            int indexOfStation = 0;
            Station station = new();
            foreach (var indexStations in DataSource.Stations)//goes through stations
            {
                if(indexStations.Id == idStation)
                {
                    station = indexStations;
                    if (newName != "\n")//if enter wasnt inputted
                        station.Name = newName;
                    if (chargeSlots != 0)//if 0 wasnt inputted
                        station.AvailableChargeSlots = chargeSlots - ChargeSlotsInUse(idStation);
                    break;
                }
                indexOfStation++;
            }
            DataSource.Stations[indexOfStation] = station;//placing updated station in list of stations
        }
    }
}
