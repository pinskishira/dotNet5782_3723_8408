using System.Collections.Generic;
using System;

using BO;
using static BO.Enum;

namespace BlApi
{
    public interface Ibl
    {

        /// <summary>
        /// Performing logical tests on the recieved customer and coverting the customer fields in the dalObject
        /// to the customer fields in the BL.
        /// </summary>
        /// <param name="newCustomer">The new customer</param>
        /// <exception cref="IBL.BO.InvalidInputException"></exception>
        void AddCustomer(Customer newCustomer);

        /// <summary>
        /// Performing logical tests on the recieved new drone and the station its located in and coverting the 
        /// drone fields in the dalObject to the drone fields in the BL.
        /// </summary>
        /// <param name="newDrone">The new drone</param>
        /// <param name="stationNumber">Station where drone is located</param>
        /// <exception cref="IBL.BO.InvalidInputException"></exception>
        void AddDrone(Drone newDrone, int stationNumber);

        /// <summary>
        /// Performing logical tests on the recieved parcel and coverting the parcel fields in the dalObject
        /// to the parcel fields in the BL, and placing parcel into the dal list of drones.
        /// </summary>
        /// <param name="newParcel">The new parcel</param>
        /// <exception cref="IBL.BO.InvalidInputException"></exception>
        void AddParcel(Parcel newParcel);

        /// <summary>
        /// Performing logical tests on the recieved station and coverting the station fields in the dalObject
        /// to the station fields in the BL.
        /// </summary>
        /// <param name="newStation">The new station</param>
        /// <exception cref="IBL.BO.InvalidInputException"></exception>
        void AddStation(Station newStation);

        /// <summary>
        /// Returns a specific customer, by converting dcustomer to BL an filling the missing fields.
        /// </summary>
        /// <param name="customerId">Id of customer</param>
        /// <returns>Customer</returns>
        /// <exception cref="IBL.BO.FailedGetException"></exception>
        Customer GetCustomer(int customerId);

        /// <summary>
        /// Returns a specific drone, by converting drone to BL an filling the missing fields.
        /// </summary>
        /// <param name="droneId">Id of drone</param>
        /// <returns>Drone</returns>
        ///  <exception cref="IBL.BO.FailedGetException"></exception>
        Drone GetDrone(int droneId);

        /// <summary>
        /// Returns a specific parcel, by converting parcel to BL an filling the missing fields.
        /// </summary>
        /// <param name="parcelId">Id of parcel</param>
        /// <returns>Parcel</returns>
        ///  <exception cref="IBL.BO.FailedGetException"></exception>
        Parcel GetParcel(int parcelId);

        /// <summary>
        /// Returns a specific station, by converting station to BL an filling the missing fields.
        /// </summary>
        /// <param name="stationId">Id of station</param>
        /// <returns>Parcel</returns>
        /// <exception cref="IBL.BO.FailedGetException"></exception>
        Station GetStation(int stationId);

        /// <summary>
        /// Converting dal list to bl and updating the parcel state, and amount of packages sent and delivered
        /// to customer, then adding to custonerToList.
        /// </summary>
        /// <returns>List of customers</returns>
        IEnumerable<CustomerToList> GetAllCustomers(Predicate<CustomerToList> predicate = null);

        /// <summary>
        /// Sending list of drones.
        /// </summary>
        /// <returns>List of drones</returns>
        IEnumerable<DroneToList> GetAllDrones(Predicate<DroneToList> predicate = null);

        /// <summary>
        /// Converting dal list to bl and updating the parcel state, then adding to parcelToList.
        /// </summary>
        /// <returns>List of parcels</returns>
        IEnumerable<ParcelToList> GetAllParcels(Predicate<ParcelToList> predicate = null);

        /// <summary>
        /// Converting dal list to bl and updating amount of unavailable charge slots, then adding to stationToList.
        /// </summary>
        /// <returns>List of stations</returns>
        IEnumerable<StationToList> GetAllStations(Predicate<StationToList> predicate = null);

        /// <summary>
        /// Finds customer and sends to update in dal.
        /// </summary>
        /// <param name="idCustomer">Id of customer</param>
        /// <param name="newName">New name of customer</param>
        /// <param name="customerPhone">New phone of customer</param>
        void UpdateCustomer(int idCustomer, string newName, string customerPhone);

        /// <summary>
        /// Finding drone, changing model name and sending to update in dal.
        /// </summary>
        /// <param name="idDrone">Id of drone</param>
        /// <param name="model">New model name</param>
        void UpdateDrone(int idDrone, string model);

        /// <summary>
        /// Finds station and sends to update in dal.
        /// </summary>
        /// <param name="idStation">Id of station</param>
        /// <param name="newName">New name of station</param>
        /// <param name="chargeSlots">Amount of charge slots in station</param>
        /// <exception cref="IBL.BO.InvalidInputException"></exception>
        void UpdateStation(int idStation, string newName, int chargeSlots);

        /// <summary>
        /// Releasing drone from a charging station and updating the drone released, and charging station
        /// and then sending to update in dal.
        /// </summary>
        /// <param name="idDrone">Id of drone</param>
        /// <exception cref="IBL.BO.DroneMaintananceException"></exception>
        void DroneReleaseFromChargingStation(int idDron);

        /// <summary>
        /// Sending drone to a charging station, updating drone, and sending to update in dal.
        /// </summary>
        /// <param name="idDrone">Id of drone</param>
        /// <exception cref="IBL.BO.DroneMaintananceException"></exception>
        void SendDroneToChargingStation(int idDrone);

        /// <summary>
        /// Updating all the fields of drone that delivered the parcel, and the parcel that was delivered.
        /// </summary>
        /// <param name="droneId">Drone that delivered parcel</param>
        /// <exception cref="IBL.BO.ParcelDeliveryException"></exception>
        void UpdateParcelDeliveryToCustomer(int droneId);

        /// <summary>
        /// Updating all the fields of the drone that collected the parcel, and the parcel that was collected by drone.
        /// </summary>
        /// <param name="droneId">Drone that collected parcel</param>
        /// <exception cref="IBL.BO.FailedToCollectParcelException"></exception>
        void UpdateParcelCollectionByDrone(int droneId);

        /// <summary>
        /// Assigns a parcel to a drone.
        /// </summary>
        /// <param name="droneId">Drone to assign to parcel</param>
        /// <exception cref="IBL.BO.ParcelDeliveryException"></exception>
        void UpdateAssignParcelToDrone(int droneId);

        /// <summary>
        /// Delete parcel
        /// </summary>
        /// <param name="idParcel">parcel to delete</param>
        void DeleteParcel(int idParcel);

        /// <summary>
        /// Delete customer
        /// </summary>
        /// <param name="idCustomer">customer to delete</param>
        void DeleteCustomer(int idCustomer);

        /// <summary>
        ///  Delete drone
        /// </summary>
        /// <param name="idDrone">drone to delete</param>
        void DeleteDrone(int idDrone);

        /// <summary>
        ///  Delete station
        /// </summary>
        /// <param name="idStation">station to delete</param>
        void DeleteStation(int idStation);


        /// <summary>
        /// Fution that starts simulation
        /// </summary>
        /// <param name="droneId">drone to start simulation on</param>
        /// <param name="action">delegate to update the display</param>
        /// <param name="stop">delegate to stop simulation</param>
        void StartSimulator(int droneId, Action action, Func<bool> stop);

        /// <summary>
        /// Calculates the battery usage used during delivery by calculating the distance between the target, its closest
        /// station and the sender, and according to the weight of the parcel and the amount of battery it uses per km.
        /// </summary>
        /// <param name="droneToList">The drone performing delivery</param>
        /// <param name="parcel">Parcel drone is carrying</param>
        /// <returns>Amount of battery used during delivery</returns>
        int BatteryConsumption(int droneId, DO.Parcel parcel);

        /// <summary>
        /// Finds the smallest distance between the given location and the closest station.
        /// </summary>
        /// <param name="longitude">Longitude in location</param>
        /// <param name="latitude">Lattitude in location</param>
        /// <returns>Closest station to sender</returns>
        DO.Station smallestDistance(double longitude, double latitude);
        /// <summary>
        ///  Finds the smallest distance between the given location and the closest station.
        /// </summary>
        /// <param name="CurrentLocation">Location of drone</param>
        /// <returns></returns>
        DO.Station smallestDistanceFromDrone(Location CurrentLocation);


        /// <summary>
        /// checks if customer is deleted
        /// </summary>
        /// <param name="id">The id of customer</param>
        /// <returns></returns>
        bool IsActive(int id);
    }
}