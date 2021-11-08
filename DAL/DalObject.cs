﻿using System;
using IDAL.DO;
using System.Collections.Generic;
using DAL.IDAL.DO;
/// <summary>
/// DalObject defines arrays for the stations, drones, customers, parcels and drone charges and then updates them and fills them with data.
/// It also includes adding functions for all arrays as well as searching functions.
/// </summary>
namespace DalObject
{
    /// <summary>
    /// Adding, searching and updating
    /// </summary>
    public class DalObject : IDal
    {
        public DalObject()
        {
            DataSource.Initialize();
        }
        /// <summary>
        /// Adding a new station to the list of stations
        /// </summary>
        /// <param name="newStation">The new station</param>
        public void AddStation(Station newStation)
        {
            if (DataSource.Stations.Contains(newStation) == false)
                throw new DataExceptions("The station already exists.\n");
            DataSource.Stations.Add(newStation);
        }

        /// <summary>
        /// Adding a new  drone to the array of drones
        /// </summary>
        /// <param name="newDrone">The new drone</param>
        public void AddDrone(Drone newDrone)
        {
            if (DataSource.Drones.Contains(newDrone) == false)
                throw new DataExceptions("The drone already exists.\n");
            DataSource.Drones.Add(newDrone);
        }

        /// <summary>
        /// Adding a new customer to the array of customers
        /// </summary>
        /// <param name="newCustomer">The new customer</param>
        public void AddCustomer(Customer newCustomer)
        {
            if (DataSource.Customers.Contains(newCustomer) == false)
                throw new DataExceptions("The customer already exists.\n");
            DataSource.Customers.Add(newCustomer);
        }

        /// <summary>
        /// Adding a new parcel to the array of parcels
        /// </summary>
        /// <param name="newParcel">The new parcel</param>
        public void AddParcel(Parcel newParcel)
        {
            if (DataSource.Parcels.Contains(newParcel) == false)
                throw new DataExceptions("The parcel already exists.\n");
            newParcel.Id = DataSource.Config.NextParcelNumber++;
            DataSource.Parcels.Add(newParcel);
        }

        /// <summary>
        /// Adding a new drone charge to the array of drone charges
        /// </summary>
        /// <param name="newDroneCharge">The new drone charge</param>
        public void AddDroneCharge(DroneCharge newDroneCharge)
        {
            if (DataSource.DroneCharges.Contains(newDroneCharge) == false)
                throw new DataExceptions("The parcel already exists.\n");
            DataSource.DroneCharges.Add(newDroneCharge);
        }

        /// <summary>
        /// Assigning a parcel to a drone
        /// </summary>
        /// <param name="idParcel">Parcel to assign to drone</param>
        /// <param name="idDrone">Drone which will be assigned a parcel</param>
        public void UpdateAssignParcelToDrone(int idParcel, int idDrone)
        {
            if (!DataSource.Parcels.Exists(item => item.Id == idParcel))
                throw new DataExceptions("The parcel does not exist.\n");
            if (!DataSource.Drones.Exists(item => item.Id == idDrone))
                throw new DataExceptions("The drone does not exist.\n");
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

        /// <summary>
        /// Updating when parcel is collected by drone
        /// </summary>
        /// <param name="idParcel">Parcel who's collected by drone</param>
        public void UpdateParcelCollectionByDrone(int idParcel)
        {
            if (!DataSource.Parcels.Exists(item => item.Id == idParcel))
                throw new DataExceptions("The parcel does not exist.\n");
            int indexParcel = 0;
            Parcel newParcel = new();
            while (DataSource.Parcels[indexParcel].Id != idParcel)//finding parcel that was collected by drone
                indexParcel++;
            newParcel = DataSource.Parcels[indexParcel];
            newParcel.PickedUp = DateTime.Now;
            DataSource.Parcels[indexParcel] = newParcel;//updating date and time
        }

        /// <summary>
        /// Updating when parcel is delivered to customer
        /// </summary>
        /// <param name="idParcel">Parcel delivered to customer</param>
        public void UpdateParcelDeliveryToCustomer(int idParcel)
        {
            if (!DataSource.Parcels.Exists(item => item.Id == idParcel))
                throw new DataExceptions("The parcel does not exist.\n");
            Parcel newParcel = new();
            int indexParcel = 0/*,indexDrone=0*/;
            while (DataSource.Parcels[indexParcel].Id != idParcel)
                indexParcel++;
            newParcel = DataSource.Parcels[indexParcel];
            newParcel.Delivered = DateTime.Now;
            newParcel.DroneId = 0;
            DataSource.Parcels[indexParcel] = newParcel;
            DataSource.Config.NextParcelNumber--;//updating that theres one less parcel to deliver
            //while (DataSource.Drones[indexDrone].Id != DataSource.Parcels[indexParcel].DroneId)
            //    indexDrone++;
            //DataSource.Drones[indexDrone].Status = DroneStatuses.Available;//now drone is availbale
        }

        /// <summary>
        /// Sending drone to be charged in an available charging station
        /// </summary>
        /// <param name="idDrone">Drone that needs charging</param>
        /// <param name="nameStation">Station with available charging stations</param>
        public void UpdateSendDroneToChargingStation(int idDrone, string nameStation)
        {
            if (!DataSource.Drones.Exists(item => item.Id == idDrone))
                throw new DataExceptions("The drone does not exist.\n");
            if (!DataSource.Stations.Exists(item => item.Name == nameStation))
                throw new DataExceptions("The station does not exist.\n");
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
                throw new DataExceptions("The drone does not exist.\n");
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

        /// <summary>
        /// Finding requested custmer according to its ID name
        /// </summary>
        /// <param name="id">Wanted customer</param>
        /// <returns></returns>
        public Customer FindCustomer(int id)
        {
            if (!DataSource.Customers.Exists(item => item.Id == id))
                throw new DataExceptions("The customer does not exist.\n");
            int indexFindCustomer = 0;
            while (DataSource.Customers[indexFindCustomer].Id != id)//Going through customers array
                indexFindCustomer++;
            return DataSource.Customers[indexFindCustomer];
        }

        /// <summary>
        /// Finding requested station according to its ID name
        /// </summary>
        /// <param name="id">Wanted station</param>
        /// <returns></returns>
        public Station FindStation(int id)
        {
            if (!DataSource.Stations.Exists(item => item.Id == id))
                throw new DataExceptions("The station does not exist.\n");
            int indexFindStation = 0;
            while (DataSource.Stations[indexFindStation].Id != id)//Going through stations array
                indexFindStation++;
            return DataSource.Stations[indexFindStation];
        }

        /// <summary>
        /// Finding requested drone according to its ID name
        /// </summary>
        /// <param name=id">Wanted drone</param>
        /// <returns></returns>
        public Drone FindDrone(int id)
        {
            if (!DataSource.Drones.Exists(item => item.Id == id))
                throw new DataExceptions("The drone does not exist.\n");
            int indexFindDrone = 0;
            while (DataSource.Drones[indexFindDrone].Id != id)//Going through drones array
                indexFindDrone++;
            return DataSource.Drones[indexFindDrone];
        }

        /// <summary>
        /// Finding requested parcel according to its ID name
        /// </summary>
        /// <param name="id">Wanted parcel</param>
        /// <returns></returns>
        public Parcel FindParcel(int id)
        {
            if (!DataSource.Parcels.Exists(item => item.Id == id))
                throw new DataExceptions("The parcel does not exist.\n");
            int indexFindParcel = 0;
            while (DataSource.Parcels[indexFindParcel].Id != id)//Going through parcels array
                indexFindParcel++;
            return DataSource.Parcels[indexFindParcel];
        }

        /// <summary>
        /// Gives a view of the array of stations
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Station> GetAllStations()
        {
            List<Station>tempStations = new List<Station>();
            foreach (var indexOfStations in DataSource.Stations)
            {
                tempStations.Add(indexOfStations);
            }
            return (IEnumerable<Station>)tempStations;
        }

        /// <summary>
        /// Gives a view of the array of parcels
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Parcel> GetAllParcels()
        {
            List<Parcel> tempParcels = new();
            foreach (var indexOfParcels in DataSource.Parcels)
            {
                tempParcels.Add(indexOfParcels);
            }
            return tempParcels;
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
        /// Gives a view of the array of customers
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Customer> GetAllCustomers()
        {
            List<Customer> tempCustomers = new();
            foreach (var indexOfCustomers in DataSource.Customers)
            {
                tempCustomers.Add(indexOfCustomers);
            }
            return tempCustomers;
        }

        /// <summary>
        /// Gives a view of the an array of parcels with no assigned drones
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Parcel> ParcelWithNoDrone()
        {
            List<Parcel> parcelNoDrone = new();
            foreach (var indexNoDrone in DataSource.Parcels)
            {
                if (indexNoDrone.DroneId == 0)
                    parcelNoDrone.Add(indexNoDrone);
            }
            return parcelNoDrone;
        }

        /// <summary>
        /// A function that returns an array of stations whose load position is greater than 0
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Station> GetStationWithFreeSlots()
        {
            List<Station> freeSlotsStation = new();
            foreach (var indexSlots in DataSource.Stations)
            {
                if (indexSlots.ChargeSlots > 0)
                    freeSlotsStation.Add(indexSlots);
            }
            return freeSlotsStation;
        }

        public double[] electricityUse()
        {
            double []elecUse = new double[5];
            elecUse[0] = DataSource.Config.Available;
            elecUse[1] = DataSource.Config.LightWeight;
            elecUse[2] = DataSource.Config.MediumWeight;
            elecUse[3] = DataSource.Config.HeavyWeight;
            elecUse[4] = DataSource.Config.DroneLoadingRate;
            return elecUse;
        }
    }
}