using DO;
using System;
using System.Collections.Generic;


namespace DalApi
{
    public interface IDal
    {
        /// <summary>
        /// Adding a new customer to the list of customers
        /// </summary>
        /// <param name="newCustomer">Adding a new customer variable to the list of customers</param>
        void AddCustomer(Customer newCustomer);

        /// <summary>
        /// Adding a new  drone to the list of drones
        /// </summary>
        /// <param name="newDrone">Adding a new drone variable to the list of drones</param>
        /// <exception cref="IDAL.DO.ItemExistsException"></exception>
        void AddDrone(Drone newDrone);

        /// <summary>
        /// Adding a new drone charge to the list of drone charges
        /// </summary>
        /// <param name="newDroneCharge">Adding A new drone charge variable to list of drone charges</param>
        /// <exception cref="IDAL.DO.ItemExistsException"></exception>
        void AddDroneCharge(DroneCharge newDroneCharge);

        /// <summary>
        /// Adding a new parcel to the list of parcels
        /// </summary>
        /// <param name="newParcel">Adding a new parcel variable to list OF parcels</param>
        /// <exception cref="IDAL.DO.ItemExistsException"></exception>
        void AddParcel(Parcel newParcel);

        /// <summary>
        /// Adding a new station to the list of stations
        /// </summary>
        /// <param name="newStation">Adding a new station to the list of stations</param>
        /// <exception cref="IDAL.DO.ItemExistsException"></exception>
        void AddStation(Station newStation);

        /// <summary>
        /// Releasing a Drone from a charging station
        /// </summary>
        /// <param name="idDrone">Drone released from charging</param>
        /// <exception cref="IDAL.DO.ItemDoesNotExistException"></exception>
        void DroneReleaseFromChargingStation(int idDrone);

        /// <summary>
        /// Finding requested custmer according to its ID name
        /// </summary>
        /// <param name="id">Wanted customer</param>
        /// <returns>if found - the customer is returned, if not - there is exception</returns>
        /// <exception cref="IDAL.DO.ItemDoesNotExistException"></exception>
        Customer FindCustomer(int id);

        /// <summary>
        /// Finding requested drone according to its ID name
        /// </summary>
        /// <param name="id">Wanted drone</param>
        /// <returns>if found - the drone is returned, if not - there is exception</returns>
        /// <exception cref="IDAL.DO.ItemDoesNotExistException"></exception>
        Drone FindDrone(int id);

        /// <summary>
        /// Finding requested parcel according to its ID name
        /// </summary>
        /// <param name="id">Wanted parcel</param>
        /// <returns>if found - the parcel is returned, if not - there is exception</returns>
        /// <exception cref="IDAL.DO.ItemDoesNotExistException"></exception>
        Parcel FindParcel(int id);

        /// <summary>
        /// Finding requested station according to its ID name
        /// </summary>
        /// <returns>if found - the station is returned, if not - there is exception</returns>
        /// <exception cref="IDAL.DO.ItemDoesNotExistException"></exception>
        Station FindStation(int id);

        /// <summary>
        /// Gives a view of the list of customers
        /// </summary>
        /// <returns>List of customers</returns>
        IEnumerable<Customer> GetAllCustomers(Predicate<Customer> predicate = null);

        /// <summary>
        /// Gives a view of the list of drones
        /// </summary>
        /// <returns>List of drones</returns>
        IEnumerable<Drone> GetAllDrones(Predicate<Drone> predicate = null);
        IEnumerable<Drone> GetAllDronesToBlDrones(Predicate<Drone> predicate = null);

        /// <summary>
        /// Gives a view of the list of parcels
        /// </summary>
        /// <returns>List of parcels</returns>
        IEnumerable<Parcel> GetAllParcels(Predicate<Parcel> predicate = null);

        /// <summary>
        /// Gives a view of the list of stations
        /// </summary>
        /// <returns>List of stations</returns>
        IEnumerable<Station> GetAllStations(Predicate<Station> predicate = null);

        /// <summary>
        /// Returns list of all the drones in charges
        /// </summary>
        /// <returns>List of drones in charging</returns>
        IEnumerable<DroneCharge> GetAllDroneCharges(Predicate<DroneCharge> predicate = null);

        /// <summary>
        /// Assigning a parcel to a drone
        /// </summary>
        /// <param name="idParcel">Parcel to assign to drone</param>
        /// <param name="idDrone">Drone which will be assigned a parcel</param>
        /// <exception cref="IDAL.DO.ItemDoesNotExistException"></exception>
        void UpdateAssignParcelToDrone(int idParcel, int idDrone);

        /// <summary>
        /// Updating when parcel is collected by drone
        /// </summary>
        /// <param name="idParcel">Parcel who's collected by drone</param>
        /// <exception cref="IDAL.DO.ItemDoesNotExistException"></exception>
        void UpdateParcelCollectionByDrone(int idParcel);

        /// <summary>
        /// Updating when parcel is delivered to customer
        /// </summary>
        /// <param name="idParcel">Parcel delivered to customer</param>
        /// <exception cref="IDAL.DO.ItemDoesNotExistException"></exception>
        void UpdateParcelDeliveryToCustomer(int idParcel);

        /// <summary>
        /// Sending drone to be charged in an available charging station
        /// </summary>
        /// <param name="idDrone">Drone that needs charging</param>
        /// <param name="nameStation">Station with available charging stations</param>
        /// <exception cref="IDAL.DO.ItemDoesNotExistException"></exception>
        void UpdateSendDroneToChargingStation(int idDrone, string nameStation);

        /// <summary>
        /// Defines an array that holds the data of the amount of battery used per km.
        /// </summary>
        /// <returns>Array of battery consumption data</returns>
        double[] electricityUse();

        /// <summary>
        /// Placing updated drone into drone list
        /// </summary>
        /// <param name="drone">Drone to update</param>
        void UpdateDrone(Drone drone);

        /// <summary>
        /// Updates station name and charge slots.
        /// </summary>
        /// <param name="idStation">Id of station</param>
        /// <param name="newName">Stations new name</param>
        /// <param name="chargeSlots">amount of charge slots in station</param>
        void UpdateStation(int idStation, string newName, int chargingSlots);

        /// <summary>
        /// Updates customers name and phone number
        /// </summary>
        /// <param name="idCustomer">Id of customer to update</param>
        /// <param name="newName">Customers new name</param>
        /// <param name="customerPhone">Customers new phone</param>
        void UpdateCustomer(int idCustomer, string newName, string customerPhone);

        /// <summary>
        /// Returns a drone charge accorging to a specific id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DroneCharge GetDroneCharge(int id);

        /// <summary>
        /// Deletes a parcel
        /// </summary>
        /// <param name="id">The id of the parcel to delete</param>
        void DeleteParcel(int id);

        /// <summary>
        /// Deletes a customer
        /// </summary>
        /// <param name="id">The id of the customer to delete</param>
        void DeleteCustomer(int id);

        /// <summary>
        /// Deletes a drone
        /// </summary>
        /// <param name="id">The id of the drone to delete</param>
        void DeleteDrone(int id);

        /// <summary>
        /// Deletes a station
        /// </summary>
        /// <param name="id">The id of the station to delete</param>
        void DeleteStation(int id);
                /// <summary>
        /// Delete Drone Charge
        /// </summary>
        /// <param name="id">The id of the drone</param>
        void DeleteDroneCharge(int id);

        /// <summary>
        /// checks if customer is deleted
        /// </summary>
        /// <param name="id">The id of customer</param>
        /// <returns>bool</returns>
        bool IsActive(int id);

    }
}