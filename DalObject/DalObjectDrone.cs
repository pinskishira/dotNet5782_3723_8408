using System;
using System.Collections.Generic;
using System.Linq;
using DO;
using System.Runtime.CompilerServices;

namespace Dal
{
    partial class DalObject
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddDrone(Drone newDrone)
        {
            if (DataSource.Drones.Exists(item => item.Id == newDrone.Id && !newDrone.DeletedDrone))//checks if drone exists
                throw new ItemExistsException("The drone already exists.\n");
            DataSource.Drones.Add(newDrone);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Drone FindDrone(int id)
        {
            int indexDrone = DataSource.Drones.FindIndex(drone => drone.Id == id);//checks if drone exists
            if (indexDrone == -1)
                throw new ItemDoesNotExistException("No drone found with this id");
            return DataSource.Drones[indexDrone];
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Drone> GetAllDrones(Predicate<Drone> predicate = null)
        {
            return from itemDrone in DataSource.Drones
                   where (predicate == null ? true : predicate(itemDrone)) && (!itemDrone.DeletedDrone)
                   select itemDrone;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateAssignParcelToDrone(int idParcel, int idDrone)
        {
            if (!DataSource.Drones.Exists(item => (item.Id == idDrone) && (!item.DeletedDrone)))//checks if drone exists
                throw new ItemDoesNotExistException("The drone does not exist or it had been deleted.\n");
            int indexAssign = CheckExistingParcel(idParcel);//finding parcel
            Parcel newParcel = DataSource.Parcels[indexAssign];
            newParcel.DroneId = idDrone;//giving parcel available drones' id
            newParcel.Scheduled = DateTime.Now;//updating date and time
            DataSource.Parcels[indexAssign] = newParcel;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateDrone(Drone drone)
        {
            int indexOfDrone = CheckExistingDrone(drone.Id);//finding index
            DataSource.Drones[indexOfDrone] = drone;//placing updated drone in place of index
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateSendDroneToChargingStation(int idDrone, string nameStation)
        {

            if (!DataSource.Drones.Exists(item => (item.Id == idDrone) && (!item.DeletedDrone)))//checks if drone exists
                throw new ItemDoesNotExistException("The drone does not exist or it has been deleted.\n");
            DroneCharge newDroneCharge = new DroneCharge();
            //drone with low battery will go be charged here
            int indexOfStation = DataSource.Stations.FindIndex(indexOfStation => indexOfStation.Name == nameStation);
            if (indexOfStation == -1)//checks if station exists
                throw new ItemDoesNotExistException("The station does not exist.\n");
            newDroneCharge.DroneId = idDrone;//putting id of low battery drone into its charging station
            newDroneCharge.StationId = DataSource.Stations[indexOfStation].Id;
            newDroneCharge.TimeDroneInCharging = DateTime.Now;
            AddDroneCharge(newDroneCharge);//updating that a drone is charging
            Station newStation = DataSource.Stations[indexOfStation];
            newStation.AvailableChargeSlots--;//less available charge slots in station
            DataSource.Stations[indexOfStation] = newStation;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DroneReleaseFromChargingStation(int idDrone)
        {
            int indexDC = CheckExistingDrone(idDrone);//finds drone
            int indexS = DataSource.Stations.FindIndex(indexOfStations => indexOfStations.Id == DataSource.DroneCharges[indexDC].StationId);//finds index where station is
            Station newStation = DataSource.Stations[indexS];
            newStation.AvailableChargeSlots++;//increasing amount of places left to charge
            DataSource.Stations[indexS] = newStation;
            DataSource.DroneCharges.RemoveAt(indexDC);//removes drone from charging
        }

        private int CheckExistingDrone(int id)
        {
            int index = DataSource.Drones.FindIndex(drone => drone.Id == id);
            if (index == -1)
                throw new ItemDoesNotExistException("No drone found with this id");
            if (DataSource.Drones[index].DeletedDrone)
                throw new ItemDoesNotExistException("This drone is deleted");
            return index;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteDrone(int id)
        {
            int indexOfDrones = CheckExistingDrone(id);//checks if parcel exists
            Drone drone = DataSource.Drones[indexOfDrones];
            drone.DeletedDrone = true;
            DataSource.Drones[indexOfDrones] = drone;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Drone> GetAllDronesToBlDrones(Predicate<Drone> predicate = null)
        {
            return from itemDrone in DataSource.Drones
                   where predicate == null ? true : predicate(itemDrone)
                   select itemDrone;
        }
    }
}
