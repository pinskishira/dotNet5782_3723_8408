using System;
using System.Collections.Generic;

using IDAL.DO;

namespace DalObject
{
    public partial class DalObject
    {
        public void AddDrone(Drone newDrone)
        {
            if (DataSource.Drones.Exists(item => item.Id == newDrone.Id))//checks if drone exists
                throw new ItemExistsException("The drone already exists.\n");
            DataSource.Drones.Add(newDrone);
        }

        public Drone FindDrone(int id)
        {
            if (!DataSource.Drones.Exists(item => item.Id == id))//checks if drone exists
                throw new ItemDoesNotExistException("The drone does not exist.\n");
            return DataSource.Drones[DataSource.Drones.FindIndex(item => item.Id == id)];//finding drone
        }

        public IEnumerable<Drone> GetAllDrones(Predicate<Drone> predicate = null)
        {
            //List<Drone> tempDrones = new();
            //foreach (var indexOfDrones in DataSource.Drones)//going through drones list
            //{
            //    tempDrones.Add(indexOfDrones);
            //}
            //return tempDrones;
            return DataSource.Drones.FindAll(item => predicate == null ? true : predicate(item));
        }

        public void UpdateAssignParcelToDrone(int idParcel, int idDrone)
        {
            if (!DataSource.Parcels.Exists(item => item.Id == idParcel))//checks if parcel exists
                throw new ItemDoesNotExistException("The parcel does not exist.\n");
            if (!DataSource.Drones.Exists(item => item.Id == idDrone))//checks if drone exists
                throw new ItemDoesNotExistException("The drone does not exist.\n");
            int indexAssign = DataSource.Parcels.FindIndex(item => item.Id == idParcel);//finding parcel
            Parcel newParcel = DataSource.Parcels[indexAssign];
            newParcel.DroneId = idDrone;//giving parcel available drones' id
            newParcel.Scheduled = DateTime.Now;//updating date and time
            DataSource.Parcels[indexAssign] = newParcel;
        }
        public void UpdateDrone(Drone drone)
        {
            int indexOfDrone = DataSource.Drones.FindIndex(index => index.Id == drone.Id);//finding index
            DataSource.Drones[indexOfDrone] = drone;//placing updated drone in place of index
        }
    }
}
