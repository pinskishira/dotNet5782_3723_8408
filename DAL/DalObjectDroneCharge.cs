using System.Collections.Generic;
using System;

using IDAL.DO;

namespace DalObject
{
    public partial class DalObject
    {
        public void AddDroneCharge(DroneCharge newDroneCharge)
        {
            if (DataSource.DroneCharges.Exists(item => item.DroneId == newDroneCharge.DroneId))//checks if drone charge exists
                throw new ItemExistsException("The drone already exists.\n");
            DataSource.DroneCharges.Add(newDroneCharge);
        }

        public void UpdateSendDroneToChargingStation(int idDrone, string nameStation)
        {
            if (!DataSource.Drones.Exists(item => item.Id == idDrone))//checks if drone exists
                throw new ItemDoesNotExistException("The drone does not exist.\n");
            if (!DataSource.Stations.Exists(item => item.Name == nameStation))//checks if station exists
                throw new ItemDoesNotExistException("The station does not exist.\n");
            DroneCharge newDroneCharge = new();//drone with low battery will go be charged here
            int index = DataSource.Stations.FindIndex(indexOfStation => indexOfStation.Name == nameStation);
            newDroneCharge.DroneId = idDrone;//putting id of low battery drone into its charging station
            newDroneCharge.StationId = DataSource.Stations[index].Id;
            AddDroneCharge(newDroneCharge);//updating that a drone is charging
            Station newStation = DataSource.Stations[index];
            newStation.AvailableChargeSlots--;//less available charge slots in station
            DataSource.Stations[index] = newStation;
        }

        public void DroneReleaseFromChargingStation(int idDrone)
        {
            if (!DataSource.DroneCharges.Exists(item => item.DroneId == idDrone))//checks if drone exists
                throw new ItemDoesNotExistException("The drone does not exist.\n");
            int indexDC = DataSource.DroneCharges.FindIndex(indexOfDroneCharges => indexOfDroneCharges.DroneId == idDrone);//finds index where drone is
            int indexS = DataSource.Stations.FindIndex(indexOfStations => indexOfStations.Id == DataSource.DroneCharges[indexDC].StationId);//finds index where station is
            Station newStation = DataSource.Stations[indexS];
            newStation.AvailableChargeSlots++;//increasing amount of places left to charge
            DataSource.Stations[indexS] = newStation;
            DataSource.DroneCharges.RemoveAt(indexDC);//removes drone from charging
        }

        public IEnumerable<DroneCharge> GetAllDroneCharges(Predicate<DroneCharge> predicate = null)
        {
            return DataSource.DroneCharges.FindAll(item => predicate == null ? true : predicate(item));
        }
    }
}
