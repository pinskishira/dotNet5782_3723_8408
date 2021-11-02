﻿using System;
using IDAL.DO;

/// <summary>
/// DalObject defines arrays for the stations, drones, customers, parcels and drone charges and then updates them and fills them with data.
/// It also includes adding functions for all arrays as well as searching functions.
/// </summary>
namespace DalObject
{
    /// <summary>
    /// Adding, searching and updating
    /// </summary>
    public class DalObject
    {
        /// <summary>
        /// Adding a new station to the array of stations
        /// </summary>
        /// <param name="newStation">The new station</param>
        public static void AddStation(Station newStation)
        {
            DataSource.Stations[DataSource.Config.IndexStation++] = newStation;
        }

        /// <summary>
        /// Adding a new  drone to the array of drones
        /// </summary>
        /// <param name="newDrone">The new drone</param>
        public static void AddDrone(Drone newDrone)
        {
            DataSource.Drones[DataSource.Config.IndexDrone++] = newDrone;
        }

        /// <summary>
        /// Adding a new customer to the array of customers
        /// </summary>
        /// <param name="newCustomer">The new customer</param>
        public static void AddCustomer(Customer newCustomer)
        {
            DataSource.Customers[DataSource.Config.IndexCustomer++] = newCustomer;
        }

        /// <summary>
        /// Adding a new parcel to the array of parcels
        /// </summary>
        /// <param name="newParcel">The new parcel</param>
        public static void AddParcel(Parcel newParcel)
        {
            DataSource.Parcels[DataSource.Config.IndexParcel]= newParcel;
            DataSource.Parcels[DataSource.Config.IndexParcel].Id = DataSource.Config.NextParcelNumber++;
            DataSource.Config.IndexParcel++;//Promoting the index
        }

        /// <summary>
        /// Adding a new drone charge to the array of drone charges
        /// </summary>
        /// <param name="newDroneCharge">The new drone charge</param>
        public static void AddDroneCharge(DroneCharge newDroneCharge)
        {
            DataSource.DroneCharges[DataSource.Config.IndexDroneCharge].DroneId = newDroneCharge.DroneId;
            DataSource.DroneCharges[DataSource.Config.IndexDroneCharge].StationId = newDroneCharge.StationId;
            DataSource.Config.IndexDroneCharge++;//Promoting the index
        }

        /// <summary>
        /// Assigning a parcel to a drone
        /// </summary>
        /// <param name="idParcel">Parcel to assign to drone</param>
        /// <param name="idDrone">Drone which will be assigned a parcel</param>
        public static void UpdateAssignParcelToDrone(int idParcel, int idDrone)
        {
            int indexAssign = 0;
            while (DataSource.Parcels[indexAssign].Id != idParcel)//finds the placement of the next parcel
                indexAssign++;
            DataSource.Parcels[indexAssign].DroneId = idDrone;//giving parcel available drones' id
            DataSource.Parcels[indexAssign].Scheduled = DateTime.Now;//updating date and time
            while (DataSource.Drones[indexAssign].Id != idDrone)
                indexAssign++;
            DataSource.Drones[indexAssign].Status = DroneStatuses.Delivery;//updating that drone is busy
        }

        /// <summary>
        /// Updating when parcel is collected by drone
        /// </summary>
        /// <param name="idParcel">Parcel who's collected by drone</param>
        public static void UpdateParcelCollectionByDrone(int idParcel)
        {

            int indexParcel = 0;
            while (DataSource.Parcels[indexParcel].Id != idParcel)//finding parcel that was collected by drone
                indexParcel++;
            DataSource.Parcels[indexParcel].PickedUp = DateTime.Now;//updating date and time
        }

        /// <summary>
        /// Updating when parcel is delivered to customer
        /// </summary>
        /// <param name="idParcel">Parcel delivered to customer</param>
        public static void UpdateParcelDeliveryToCustomer(int idParcel)
        {
            int indexParcel = 0,indexDrone=0;
            while (DataSource.Parcels[indexParcel].Id != idParcel)
                indexParcel++;
            DataSource.Parcels[indexParcel].Delivered = DateTime.Now;//updating date and time
            DataSource.Config.NextParcelNumber--;//updating that theres one less parcel to deliver
            while (DataSource.Drones[indexDrone].Id != DataSource.Parcels[indexParcel].DroneId)
                indexDrone++;
            DataSource.Drones[indexDrone].Status = DroneStatuses.Available;//now drone is availbale
            DataSource.Parcels[indexParcel].DroneId = 0;
        }

        /// <summary>
        /// Sending drone to be charged in an available charging station
        /// </summary>
        /// <param name="idDrone">Drone that needs charging</param>
        /// <param name="nameStation">Station with available charging stations</param>
        public static void UpdateSendDroneToChargingStation(int idDrone, string nameStation)
        {
            DroneCharge newDroneCharge = new();//drone with low battery will go be charged here
            int index = 0;
            while (DataSource.Drones[index].Id != idDrone)
                index++;
            DataSource.Drones[index].Status = DroneStatuses.Maintenance;//saying that the drone is in maintanance and unavailable to deliver
            newDroneCharge.DroneId = idDrone;//putting id of low battery drone into its charging station
            index = 0;
            while (DataSource.Stations[index].Name != nameStation)
                index++;
            newDroneCharge.StationId = DataSource.Stations[index].Id;
            AddDroneCharge(newDroneCharge);//updating that a drone is charging 
            DataSource.Stations[index].ChargeSlots--;
        }

        /// <summary>
        /// Releasing a Drone from a charging station
        /// </summary>
        /// <param name="idDrone">Drone released from charging</param>
        public static void DroneReleaseFromChargingStation(int idDrone)
        {
            int indexDC = 0, indexS = 0, indexD = 0;
            while (DataSource.DroneCharges[indexDC].DroneId != idDrone)
                indexDC++;
            while (DataSource.Stations[indexS].Id != DataSource.DroneCharges[indexDC].StationId)
                indexS++;
            DataSource.Stations[indexS].ChargeSlots++;//increasing amount of places left to charge
            DataSource.DroneCharges[indexDC].DroneId = 0;
            DataSource.DroneCharges[indexDC].StationId = 0;
            while (DataSource.Drones[indexD].Id != idDrone)
                indexD++;
            DataSource.Drones[indexD].Battery = 100;
            DataSource.Drones[indexD].Status = DroneStatuses.Available;
        }

        /// <summary>
        /// Finding requested custmer according to its ID name
        /// </summary>
        /// <param name="id">Wanted customer</param>
        /// <returns></returns>
        public static Customer FindCustomer(int id)
        {
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
        public static Station FindStation(int id)
        {
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
        public static Drone FindDrone(int id)
        {

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
        public static Parcel FindParcel(int id)
        {
            int indexFindParcel = 0;
            while (DataSource.Parcels[indexFindParcel].Id != id)//Going through parcels array
                indexFindParcel++;
            return DataSource.Parcels[indexFindParcel];
        }

        /// <summary>
        /// A function that returns an array of stations whose load position is greater than 0
        /// </summary>
        /// <returns></returns>
        public static Station[] GetStationWithFreeSlots()
        {
            Station[] stationChargingSlot = new Station[DataSource.Config.IndexStation];
            int count = 0, indexChargingSlot;
            for (indexChargingSlot = 0; indexChargingSlot < DataSource.Config.IndexStation; indexChargingSlot++)
            {
                if (DataSource.Stations[indexChargingSlot].ChargeSlots > 0)
                    stationChargingSlot[count] = DataSource.Stations[indexChargingSlot];
                count++;
            }
            //StationChargingSlot[count + 1].Id = -1;//Placing -1 in the last place in the array
            return stationChargingSlot;
        }

        /// <summary>
        /// Returns place of index in station array
        /// </summary>
        /// <returns></returns>
        public static int GetIndexStation()
        {
            return DataSource.Config.IndexStation;
        }

        /// <summary>
        /// Gives a view of the array of stations
        /// </summary>
        /// <returns></returns>
        public static Station[] GetAllStations()
        {
            Station[] list = new Station[DataSource.Config.IndexStation];//placing array in another array which will be sent back to main
            for (int i = 0; i < DataSource.Config.IndexStation; i++)
            {
                list[i] = DataSource.Stations[i];
            }
            return list;
        }

        /// <summary>
        /// Gives a view of the array of parcels
        /// </summary>
        /// <returns></returns>
        public static Parcel[] GetAllParcels()
        {
            Parcel[] viewParcel = new Parcel[DataSource.Config.IndexParcel];//placing array in another array which will be sent back to main
            for (int i = 0; i < DataSource.Config.IndexParcel; i++)
            {
                viewParcel[i] = DataSource.Parcels[i];
            }
            return viewParcel;
        }

        /// <summary>
        /// Gives a view of the array of drones
        /// </summary>
        /// <returns></returns>
        public static Drone[] GetAllDrones()
        {
            Drone[] viewDrone = new Drone[DataSource.Config.IndexDrone];
            for (int i = 0; i < DataSource.Config.IndexDrone; i++)
            {
                viewDrone[i] = DataSource.Drones[i];
            }
            return viewDrone;
        }

        /// <summary>
        /// Gives a view of the array of customers
        /// </summary>
        /// <returns></returns>
        public static Customer[] GetAllCustomers()
        {
            Customer[] viewCustomer = new Customer[DataSource.Config.IndexCustomer];
            for (int i = 0; i < DataSource.Config.IndexCustomer; i++)
            {
                viewCustomer[i] = DataSource.Customers[i];
            }
            return viewCustomer;
        }

        /// <summary>
        /// Gives a view of the an array of parcels with no assigned drones
        /// </summary>
        /// <returns></returns>
        public static Parcel[] ParcelWithNoDrone()
        {
            int count = 0;
            for (int i = 0; i < DataSource.Config.IndexParcel; i++)//counts how many parcels dont have an assigned drone
            {
                if (DataSource.Parcels[i].DroneId == 0)//if the drone ID doesnt equal 0
                    count++;
            }
            Parcel[] viewParcel = new Parcel[count];
            count = 0;
            for (int i = 0; i < DataSource.Config.IndexParcel; i++)//places the parcels that dont have an assigned drone into an array
            {
                if (DataSource.Parcels[i].DroneId == 0)
                    viewParcel[count++] = DataSource.Parcels[i];
            }
            return viewParcel;
        }

        /// <summary>
        /// Gives a view of the an array stations with available charging slots
        /// </summary>
        /// <returns></returns>
        public static Station[] AvailableChargingSlots()
        {
            int count = 0;
            for (int i = 0; i < DataSource.Config.IndexStation; i++)
            {
                if (DataSource.Stations[i].ChargeSlots > 0)//if the charge slots if greather than zero
                    count++;
            }
            Station[] viewStation = new Station[count];
            count = 0;
            for (int i = 0; i < DataSource.Config.IndexStation; i++)
            {
                if (DataSource.Stations[i].ChargeSlots > 0)
                    viewStation[count++] = DataSource.Stations[i];
            }
            return viewStation;
        }
    }
}