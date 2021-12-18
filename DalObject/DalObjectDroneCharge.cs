using System.Collections.Generic;
using System;

using DO;
using System.Linq;

namespace Dal
{
    partial class DalObject
    {
        public void AddDroneCharge(DroneCharge newDroneCharge)
        {
            if (DataSource.DroneCharges.Any(item => item.DroneId == newDroneCharge.DroneId))//checks if drone charge exists
                throw new ItemExistsException("The drone already exists.\n");
            DataSource.DroneCharges.Add(newDroneCharge);
        }

        public void UpdateSendDroneToChargingStation(int idDrone, string nameStation)
        {
            if (!DataSource.Drones.Any(item => item.Id == idDrone))//checks if drone exists
                throw new ItemDoesNotExistException("The drone does not exist.\n");
            DroneCharge newDroneCharge = new DroneCharge();
            //drone with low battery will go be charged here
            int indexOfStation = DataSource.Stations.FindIndex(indexOfStation => indexOfStation.Name == nameStation);
            if(indexOfStation==-1)//checks if station exists
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
           if(indexDC==-1)//checks if drone exists
                throw new ItemDoesNotExistException("The drone does not exist.\n");
            int indexS = DataSource.Stations.FindIndex(indexOfStations => indexOfStations.Id == DataSource.DroneCharges[indexDC].StationId);//finds index where station is
            Station newStation = DataSource.Stations[indexS];
            newStation.AvailableChargeSlots++;//increasing amount of places left to charge
            DataSource.Stations[indexS] = newStation;
            DataSource.DroneCharges.RemoveAt(indexDC);//removes drone from charging
        }

        public IEnumerable<DroneCharge> GetAllDroneCharges(Predicate<DroneCharge> predicate = null)
        {
            return from itemDroneCharges in DataSource.DroneCharges
                   where predicate == null ? true : predicate(itemDroneCharges)
                   select itemDroneCharges;
        }

        public DroneCharge GetDroneCharge(int id)
        {
            try
            {
                return DataSource.DroneCharges.First(item => item.DroneId == id);
            }
            catch (InvalidOperationException)
            {
                throw new ItemExistsException("The drone does not exists.\n");
            }
            
        }
    }
}
