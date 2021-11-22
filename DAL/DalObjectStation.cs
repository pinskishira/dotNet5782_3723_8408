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
            int indexFindStation = 0;
            while (DataSource.Stations[indexFindStation].Id != id)//Going through stations array
                indexFindStation++;
            return DataSource.Stations[indexFindStation];
        }

        /// <summary>
        /// A function that returns an array of stations whose load position is greater than 0
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Station> GetStationWithFreeSlots()
        {
            List<Station> freeSlotsStation = new();
            foreach (var indexSlots in DataSource.Stations)
            {
                if (indexSlots.ChargeSlots > 0)
                    freeSlotsStation.Add(indexSlots);
            }
            return freeSlotsStation;
        }

        /// <summary>
        /// Gives a view of the array of stations
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

        public int countDroneCharge(int idStation)//לשנות את השם של הפונקציה
        {
            int countDroneCharge = 0;
            foreach (var indexOfDroneCharge in DataSource.DroneCharges)//עובר על הרשימה של הרחפנים בטעינה
                if (indexOfDroneCharge.StationId == idStation)//אם המסםר מזהה של הרחפן הטעון שווה למספר המזהה של התחנה
                    countDroneCharge++; 
            return countDroneCharge;
        }

        public void UpdateStation(int idStation, string newName, int chargeSlots)
        {
            int indexOfStation = 0;
            Station station = new();
            foreach (var indexStations in DataSource.Stations)
            {
                if(indexStations.Id == idStation)
                {
                    station = indexStations;
                    if (newName != "\n")
                        station.Name = newName;
                    if (chargeSlots != 0)
                        station.ChargeSlots = chargeSlots - countDroneCharge(idStation);
                    break;
                }
                indexOfStation++;
            }
            DataSource.Stations[indexOfStation] = station;
        }
    }
}
