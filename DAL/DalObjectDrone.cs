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
        /// Adding a new  drone to the array of drones
        /// </summary>
        /// <param name="newDrone">The new drone</param>
        public void AddDrone(Drone newDrone)
        {
            if (!DataSource.Drones.Exists(item => item.Id == newDrone.Id))
                throw new ItemExistsException("The drone already exists.\n");
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
                throw new ItemDoesNotExistException("The drone does not exist.\n");
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
        /// <summary>
        /// Assigning a parcel to a drone
        /// </summary>
        /// <param name="idParcel">Parcel to assign to drone</param>
        /// <param name="idDrone">Drone which will be assigned a parcel</param>
        public void UpdateAssignParcelToDrone(int idParcel, int idDrone)
        {
            if (!DataSource.Parcels.Exists(item => item.Id == idParcel))
                throw new ItemDoesNotExistException("The parcel does not exist.\n");
            if (!DataSource.Drones.Exists(item => item.Id == idDrone))
                throw new ItemDoesNotExistException("The drone does not exist.\n");
            int indexAssign = 0;
            Parcel newParcel = new();
            //Drone newDrone = new();
            while (DataSource.Parcels[indexAssign].Id != idParcel)//finds the placement of the next parcel
                indexAssign++;
            newParcel = DataSource.Parcels[indexAssign];
            newParcel.DroneId = idDrone;//giving parcel available drones' id
            newParcel.Scheduled = DateTime.Now;//updating date and time
            DataSource.Parcels[indexAssign] = newParcel;
            indexAssign = 0;
            //while (DataSource.Drones[indexAssign].Id != idDrone)
            //    indexAssign++;
            //DataSource.Drones[indexAssign].Status = DroneStatuses.Delivery;//updating that drone is busy
        }

        public void UpdateDrone(int idDrone, string newModel)
        {
            int indexOfDrone = 0;
            Drone drone = new();
            foreach (var indexDrones in DataSource.Drones)
            {
                if (indexDrones.Id == idDrone)
                {
                    drone = indexDrones;
                    drone.Model = newModel;
                    break;
                }
                indexOfDrone++;
            }
            DataSource.Drones[indexOfDrone] = drone;
        }
    }
}
