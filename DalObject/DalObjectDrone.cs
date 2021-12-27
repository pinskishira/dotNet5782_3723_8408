using System;
using System.Collections.Generic;
using System.Linq;
using DO;

namespace Dal
{
    partial class DalObject
    {
        public void AddDrone(Drone newDrone)
        {
            if (DataSource.Drones.Exists(item => item.Id == newDrone.Id))//checks if drone exists
                throw new ItemExistsException("The drone already exists.\n");
            DataSource.Drones.Add(newDrone);
        }

        public Drone FindDrone(int id)
        {
            try
            {
                return DataSource.Drones.First(item => item.Id == id);//checks if drone exists
            }
            catch (InvalidOperationException)
            {
                throw new ItemExistsException("The drone does not exist.\n");
            }
        }

        public IEnumerable<Drone> GetAllDrones(Predicate<Drone> predicate = null)
        {
            return from itemDrone in DataSource.Drones
                   where predicate == null ? true : predicate(itemDrone)
                   select itemDrone;
        }

        public void UpdateAssignParcelToDrone(int idParcel, int idDrone)
        {
            if (!DataSource.Drones.Exists(item => item.Id == idDrone))//checks if drone exists
                throw new ItemDoesNotExistException("The drone does not exist.\n");
            int indexAssign = DataSource.Parcels.FindIndex(item => item.Id == idParcel);//finding parcel
            if(indexAssign==-1)
                throw new ItemDoesNotExistException("The parcel does not exist.\n");
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
