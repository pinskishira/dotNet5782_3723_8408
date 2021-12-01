using System.Collections.Generic;
using System;

using IBL.BO;
using static IBL.BO.Enum;

namespace IBL
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
        /// Converting BL list to dal and updating the parcel state, and amount of packages sent and delivered
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
        /// Converting BL list to dal and updating the parcel state, then adding to parcelToList.
        /// </summary>
        /// <returns>List of parcels</returns>
        IEnumerable<ParcelToList> GetAllParcels(Predicate<ParcelToList> predicate = null);

        /// <summary>
        /// Converting BL list to dal and updating amount of unavailable charge slots, then adding to stationToList.
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
        /// Calculates the battery usage used during delivery by calculating the distance between the target, its closest
        /// station and the sender, and according to the weight of the parcel and the amount of battery it uses per km.
        /// </summary>
        /// <param name="droneToList">The drone performing delivery</param>
        /// <param name="parcel">Parcel drone is carrying</param>
        /// <returns>Amount of battery used during delivery</returns>
        public int BatteryConsumption(DroneToList droneToList, IDAL.DO.Parcel parcel);

        /// <summary>
        /// Returns the index to place in the elecUse array, that finds the amount of battery used per km
        /// according to the weight of the parcel.
        /// </summary>
        /// <param name="maxWeight">Weight of parcel</param>
        /// <returns>Index, in elecUse array</returns>
        public double Weight(WeightCategories maxWeight);

        /// <summary>
        /// Finds the smallest distance between the given location and the closest station.
        /// </summary>
        /// <param name="longitude">Longitude in location</param>
        /// <param name="latitude">Lattitude in location</param>
        /// <returns>Closest station to sender</returns>
        public IDAL.DO.Station smallestDistance(double longitude, double latitude);

        /// <summary>
        /// Finds the smallest distance between sent location and closest station.
        /// </summary>
        /// <param name="CurrentLocation">Current location</param>
        /// <returns>Returns smallest distance between drone and closest station</returns>
        public IDAL.DO.Station smallestDistanceFromDrone(Location CurrentLocation);

        /// <summary>
        /// Releasing drone from a charging station and updating the drone released, and charging station
        /// and then sending to update in dal.
        /// </summary>
        /// <param name="idDrone">Id of drone</param>
        /// <param name="timeInCharginge">Time drone was in charging</param>
        /// <exception cref="IBL.BO.DroneMaintananceException"></exception>
        public void DroneReleaseFromChargingStation(int idDron, int timeInCharginge);

        /// <summary>
        /// Sending drone to a charging station, updating drone, and sending to update in dal.
        /// </summary>
        /// <param name="idDrone">Id of drone</param>
        /// <exception cref="IBL.BO.DroneMaintananceException"></exception>
        public void SendDroneToChargingStation(int idDrone);

        /// <summary>
        /// Updating all the fields of drone that delivered the parcel, and the parcel that was delivered.
        /// </summary>
        /// <param name="droneId">Drone that delivered parcel</param>
        /// <exception cref="IBL.BO.ParcelDeliveryException"></exception>
        public void UpdateParcelDeliveryToCustomer(int droneId);

        /// <summary>
        /// Updating all the fields of the drone that collected the parcel, and the parcel that was collected by drone.
        /// </summary>
        /// <param name="droneId">Drone that collected parcel</param>
        /// <exception cref="IBL.BO.FailedToCollectParcelException"></exception>
        public void UpdateParcelCollectionByDrone(int droneId);

        /// <summary>
        /// Assigns a parcel to a drone.
        /// </summary>
        /// <param name="droneId">Drone to assign to parcel</param>
        /// <exception cref="IBL.BO.ParcelDeliveryException"></exception>
        public void UpdateAssignParcelToDrone(int droneId);

        /// <summary>
        /// Function that converts longitude and latitude into Location. 
        /// </summary>
        /// <param name="longitude">Longitude</param>
        /// <param name="latitude">Lattitude</param>
        /// <returns>Current Location</returns>
        public Location CopyLocation(double longitude, double latitude);

    }
}