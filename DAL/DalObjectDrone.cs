using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
using DAL.IDAL.DO;
using IDAL;

namespace DalObject
{
    public partial class DalObject
    {
        /// <summary>
        /// Adding a new  drone to the array of drones
        /// </summary>
        /// <param name="newDrone">The new drone</param>
        public void AddDrone(Drone newDrone)
        {
            if (!DataSource.Drones.Exists(item => item.Id == newDrone.Id))
                throw new DataExceptions("The drone already exists.\n");
            DataSource.Drones.Add(newDrone);
        }
        /// <summary>
        /// Finding requested drone according to its ID name
        /// </summary>
        /// <param name=id">Wanted drone</param>
        /// <returns></returns>
        public Drone FindDrone(int id)
        {
            if (!DataSource.Drones.Exists(item => item.Id == id))
                throw new DataExceptions("The drone does not exist.\n");
            int indexFindDrone = 0;
            while (DataSource.Drones[indexFindDrone].Id != id)//Going through drones array
                indexFindDrone++;
            return DataSource.Drones[indexFindDrone];
        }
        /// <summary>
        /// Gives a view of the array of drones
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Drone> GetAllDrones()
        {
            List<Drone> tempDrones = new();
            foreach (var indexOfDrones in DataSource.Drones)
            {
                tempDrones.Add(indexOfDrones);
            }
            return tempDrones;
        }
    }
}
