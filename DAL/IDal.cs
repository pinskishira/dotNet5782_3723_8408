using System.Collections.Generic;
using IDAL.DO;


namespace IDAL
{
    public interface IDal
    {
        void AddCustomer(Customer newCustomer);
        void AddDrone(Drone newDrone);
        void AddDroneCharge(DroneCharge newDroneCharge);
        void AddParcel(Parcel newParcel);
        void AddStation(Station newStation);
        void DroneReleaseFromChargingStation(int idDrone);
        Customer FindCustomer(int id);

        /// <summary>
        /// Finding requested drone according to its ID name
        /// </summary>
        /// <param name="id">Wanted drone</param>
        /// <returns>if found - the droune is returned, if not - there is exception</returns>
        /// <exception cref="IDAL.DO.ItemDoesNotExistException"></exception>
        Drone FindDrone(int id);

        Parcel FindParcel(int id);
        
        Station FindStation(int id);
        IEnumerable<Customer> GetAllCustomers();
        IEnumerable<Drone> GetAllDrones();
        IEnumerable<Parcel> GetAllParcels();
        IEnumerable<Station> GetAllStations();
        IEnumerable<DroneCharge> GetAllDroneCharges();
        IEnumerable<Station> GetStationWithFreeSlots();
        IEnumerable<Parcel> ParcelWithNoDrone();
        void UpdateAssignParcelToDrone(int idParcel, int idDrone);
        void UpdateParcelCollectionByDrone(int idParcel);
        void UpdateParcelDeliveryToCustomer(int idParcel);
        void UpdateSendDroneToChargingStation(int idDrone, string nameStation);
        double[] electricityUse();
        void UpdateDrone(Drone drone);
        void UpdateStation(int idStation, string newName, int chargingSlots);
        void UpdateCustomer(int idCustomer, string newName, string customerPhone);
    }
}