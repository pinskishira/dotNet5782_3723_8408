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
            int indexAssign = CheckExistingParcel(idParcel);//finding parcel
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

        public void UpdateSendDroneToChargingStation(int idDrone, string nameStation)
        {
            if (!DataSource.Drones.Exists(item => item.Id == idDrone))//checks if drone exists
                throw new ItemDoesNotExistException("The drone does not exist.\n");
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

        public void DroneReleaseFromChargingStation(int idDrone)
        {
            int indexDC = DataSource.DroneCharges.FindIndex(indexOfDroneCharges => indexOfDroneCharges.DroneId == idDrone);//finds index where drone is
            if (indexDC == -1)//checks if drone exists
                throw new ItemDoesNotExistException("The drone does not exist.\n");
            int indexS = DataSource.Stations.FindIndex(indexOfStations => indexOfStations.Id == DataSource.DroneCharges[indexDC].StationId);//finds index where station is
            Station newStation = DataSource.Stations[indexS];
            newStation.AvailableChargeSlots++;//increasing amount of places left to charge
            DataSource.Stations[indexS] = newStation;
            DataSource.DroneCharges.RemoveAt(indexDC);//removes drone from charging
        }
    }
}
