using IBL.BO;
using System.Collections.Generic;
using static IBL.BO.Enum;

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
        void UpdateCustomer(int idCustomer, string newName, string customerPhone);
        void UpdateDrone(int idDrone, string model);
        void UpdateStation(int idStation, string newName, int chargeSlots);
        public int BatteryConsumption(DroneToList droneToList, IDAL.DO.Parcel parcel);
        public double Weight(WeightCategories maxWeight);
        public IDAL.DO.Station smallestDistance(double longitude, double latitude);
        public IDAL.DO.Station smallestDistanceFromDrone(Location CurrentLocation);
        public void DroneReleaseFromChargingStation(int idDron, int timeInCharginge);
        public void SendDroneToChargingStation(int idDrone);
        public void UpdateParcelDeliveryToCustomer(int droneId);
        public void UpdateParcelCollectionByDrone(int droneId);
        public void UpdateAssignParcelToDrone(int droneId);
        public Location CopyLocation(double longitude, double latitude);

    }
}