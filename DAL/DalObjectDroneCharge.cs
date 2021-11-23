using System.Collections.Generic;

using IDAL.DO;

namespace DalObject
{
    public partial class DalObject
    {
        public void AddDroneCharge(DroneCharge newDroneCharge)
        {
            if (DataSource.DroneCharges.Exists(item => item.DroneId == newDroneCharge.DroneId))
                throw new ItemExistsException("The drone already exists.\n");
            DataSource.DroneCharges.Add(newDroneCharge);
        }

        public void UpdateSendDroneToChargingStation(int idDrone, string nameStation)
        {
            if (!DataSource.Drones.Exists(item => item.Id == idDrone))
                throw new ItemDoesNotExistException("The drone does not exist.\n");
            if (!DataSource.Stations.Exists(item => item.Name == nameStation))
                throw new ItemDoesNotExistException("The station does not exist.\n");
            Station newStation = new();
            DroneCharge newDroneCharge = new();//drone with low battery will go be charged here
            int index = 0;
            newDroneCharge.DroneId = idDrone;//putting id of low battery drone into its charging station
            while (DataSource.Stations[index].Name != nameStation)
                index++;
            newDroneCharge.StationId = DataSource.Stations[index].Id;
            AddDroneCharge(newDroneCharge);//updating that a drone is charging
            newStation = DataSource.Stations[index];
            newStation.AvailableChargeSlots--;
            DataSource.Stations[index] = newStation;
        }

        public void DroneReleaseFromChargingStation(int idDrone)
        {
            if (!DataSource.DroneCharges.Exists(item => item.DroneId == idDrone))
                throw new ItemDoesNotExistException("The drone does not exist.\n");
            Station newStation = new();
            int indexDC = 0, indexS = 0, indexD = 0;
            while (DataSource.Stations[indexS].Id != DataSource.DroneCharges[indexDC].StationId)
                indexS++;
            while (DataSource.Drones[indexD].Id != idDrone)
                indexD++;
            newStation = DataSource.Stations[indexS];
            newStation.AvailableChargeSlots++;//increasing amount of places left to charge
            DataSource.Stations[indexS] = newStation;
            DataSource.DroneCharges.RemoveAt(indexDC);
        }

        public IEnumerable<DroneCharge> GetAllDroneCharges()
        {
            List<DroneCharge> tempDroneCharges = new();
            foreach (var indexOfDroneCharges in DataSource.DroneCharges)
            {
                tempDroneCharges.Add(indexOfDroneCharges);
            }
            return tempDroneCharges;
        }
    }
}
