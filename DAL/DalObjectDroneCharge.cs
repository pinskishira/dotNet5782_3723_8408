using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
//using DAL.IDAL.DO;
using IDAL;
using DalObject;

namespace DalObject
{
    public partial class DalObject
    {
        /// <summary>
        /// Adding a new drone charge to the array of drone charges
        /// </summary>
        /// <param name="newDroneCharge">The new drone charge</param>
        public void AddDroneCharge(DroneCharge newDroneCharge)
        {
            if (DataSource.DroneCharges.Exists(item => item.DroneId == newDroneCharge.DroneId));
                throw new ItemDoesNotExistException("The parcel already exists.\n");
            DataSource.DroneCharges.Add(newDroneCharge);
        }
        /// <summary>
        /// Sending drone to be charged in an available charging station
        /// </summary>
        /// <param name="idDrone">Drone that needs charging</param>
        /// <param name="nameStation">Station with available charging stations</param>
        public void UpdateSendDroneToChargingStation(int idDrone, string nameStation)
        {
            if (!DataSource.Drones.Exists(item => item.Id == idDrone))
                throw new ItemDoesNotExistException("The drone does not exist.\n");
            if (!DataSource.Stations.Exists(item => item.Name == nameStation))
                throw new ItemDoesNotExistException("The station does not exist.\n");
            Station newStation = new();
            DroneCharge newDroneCharge = new();//drone with low battery will go be charged here
            int index = 0;
            //while (DataSource.Drones[index].Id != idDrone)
            //    index++;
            //DataSource.Drones[index].Status = DroneStatuses.Maintenance;//saying that the drone is in maintanance and unavailable to deliver
            newDroneCharge.DroneId = idDrone;//putting id of low battery drone into its charging station
            while (DataSource.Stations[index].Name != nameStation)
                index++;
            newDroneCharge.StationId = DataSource.Stations[index].Id;
            AddDroneCharge(newDroneCharge);//updating that a drone is charging
            newStation = DataSource.Stations[index];
            newStation.ChargeSlots--;
            DataSource.Stations[index] = newStation;
        }
        /// <summary>
        /// Releasing a Drone from a charging station
        /// </summary>
        /// <param name="idDrone">Drone released from charging</param>
        public void DroneReleaseFromChargingStation(int idDrone)
        {
            if (!DataSource.DroneCharges.Exists(item => item.DroneId == idDrone))
                throw new ItemDoesNotExistException("The drone does not exist.\n");
            Station newStation = new();
            DroneCharge newDroneCharge = new();
            int indexDC = 0, indexS = 0, indexD = 0;
            while (DataSource.DroneCharges[indexDC].DroneId != idDrone)
                indexDC++;
            while (DataSource.Stations[indexS].Id != DataSource.DroneCharges[indexDC].StationId)
                indexS++;
            while (DataSource.Drones[indexD].Id != idDrone)
                indexD++;
            newStation = DataSource.Stations[indexS];
            newStation.ChargeSlots++;//increasing amount of places left to charge
            DataSource.Stations[indexS] = newStation;
            newDroneCharge = DataSource.DroneCharges[indexDC];
            newDroneCharge.DroneId = 0;
            newDroneCharge.StationId = 0;
            DataSource.DroneCharges[indexDC] = newDroneCharge;
            //DataSource.Drones[indexD].Battery = 100;
            //DataSource.Drones[indexD].Status = DroneStatuses.Available;
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
