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
        /// Adding a new  drone to the list of drones
        /// </summary>
        /// <param name="newDrone">The new drone</param>
        public void AddDrone(Drone newDrone)
        {
            if (DataSource.Drones.Exists(item => item.Id == newDrone.Id))
                throw new ItemExistsException("The drone already exists.\n");
            DataSource.Drones.Add(newDrone);
        }

        public Drone FindDrone(int id)
        {
            if (!DataSource.Drones.Exists(item => item.Id == id))
                throw new ItemDoesNotExistException("The drone does not exist.\n");
            int indexFindDrone = DataSource.Drones.FindIndex(item => item.Id == id);//finding drone;
            return DataSource.Drones[indexFindDrone];
        }

        /// <summary>
        /// Gives a view of the list of drones
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
            int indexAssign = DataSource.Parcels.FindIndex(item => item.Id == idParcel);//finding parcel
            Parcel newParcel = DataSource.Parcels[indexAssign];
            newParcel.DroneId = idDrone;//giving parcel available drones' id
            newParcel.Scheduled = DateTime.Now;//updating date and time
            DataSource.Parcels[indexAssign] = newParcel;
        }

        /// <summary>
        /// Placing updated drone into drone list
        /// </summary>
        /// <param name="drone">Drone to update</param>
        public void UpdateDrone(Drone drone)
        {
            int indexOfDrone = DataSource.Drones.FindIndex(index => index.Id == drone.Id);//finding index
            DataSource.Drones[indexOfDrone] = drone;//placing updated drone in place of index
        }

    }
}
