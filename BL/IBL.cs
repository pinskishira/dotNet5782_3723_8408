using IBL.BO;
using System.Collections.Generic;

namespace IBL
{
    public interface Ibl
    {
        void AddCustomer(Customer newCustomer);
        void AddDrone(Drone newDrone, int stationNumber);
        void AddParcel(Parcel newParcel);
        void AddStation(Station newStation);
        Customer DisplayCustomer(int customerId);
        Drone DisplayDrone(int droneId);
        Parcel DisplayParcel(int parcelId);
        Station DisplayStation(int stationId);
        IEnumerable<StationToList> GetStationWithFreeSlots();
        IEnumerable<CustomerToList> ListViewCustomers();
        IEnumerable<DroneToList> ListViewDrones();
        IEnumerable<ParcelToList> ListViewParcels();
        IEnumerable<StationToList> ListViewStations();
        IEnumerable<ParcelToList> ParcelWithNoDrone();
        IDAL.DO.Station smallestDistance(int CustomerId);
        void UpdateCustomer(int idCustomer, string newName, string customerPhone);
        void UpdateDrone(int idDrone, string model);
        void UpdateStation(int idStation, string newName, int chargeSlots);
    }
}