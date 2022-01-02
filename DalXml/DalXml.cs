using System;
using System.Collections.Generic;
using Dal;
using DalApi;
using DO;

namespace DalXml
{
    public class DalXml: IDal
    {
        private static string DroneXml = "@DroneXml.xml";
        private static string StationXml = "@StationXml.xml";
        private static string CustomerXml = "@CustomerXml.xml";
        private static string ParcelXml = "@ParcelXml.xml";
        private static string DroneChargeXml = "@DroneChargeXml.xml";

        internal static DalXml Instance { get { return instance.Value; } }
        private static readonly Lazy<DalXml> instance = new Lazy<DalXml>(() => new DalXml());
        static DalXml() { }//static ctor to ensure instance init is done just before first usage
        private DalXml()
        {
            //DataSource.Initialize();
        }

        public double[] electricityUse()
        {
            double[] elecUse = new double[5];
            elecUse[0] = DataSource.Config.BatteryConsumptionPowerUsageEmpty;
            elecUse[1] = DataSource.Config.BatteryConsumptionLightWeight;
            elecUse[2] = DataSource.Config.BatteryConsumptionMediumWeight;
            elecUse[3] = DataSource.Config.BatteryConsumptionHeavyWeight;
            elecUse[4] = DataSource.Config.DroneChargingRatePH;
            return elecUse;
        }

        //public void AddCustomer(Customer newCustomer)
        //{
        //    throw new NotImplementedException();
           
        //}
        public void AddCustomer(Customer newCustomer)
        {
            List<Customer> customers = XMLTools.LoadListFromXMLSerializer<Customer>(CustomerXml);
            if (DataSource.Customers.Exists(item => item.Id == newCustomer.Id))//checks if customer exists
                throw new ItemExistsException("The customer already exists.\n");
            customers.Add(newCustomer);
            XMLTools.SaveListToXMLSerializer(customers,CustomerXml)
        }

        public void AddDrone(Drone newDrone)
        {
            throw new NotImplementedException();
        }

        public void AddDroneCharge(DroneCharge newDroneCharge)
        {
            throw new NotImplementedException();
        }

        public void AddParcel(Parcel newParcel)
        {
            throw new NotImplementedException();
        }

        public void AddStation(Station newStation)
        {
            throw new NotImplementedException();
        }

        public void DroneReleaseFromChargingStation(int idDrone)
        {
            throw new NotImplementedException();
        }

        public Customer FindCustomer(int id)
        {
            throw new NotImplementedException();
        }

        public Drone FindDrone(int id)
        {
            throw new NotImplementedException();
        }

        public Parcel FindParcel(int id)
        {
            throw new NotImplementedException();
        }

        public Station FindStation(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Customer> GetAllCustomers(Predicate<Customer> predicate = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Drone> GetAllDrones(Predicate<Drone> predicate = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Parcel> GetAllParcels(Predicate<Parcel> predicate = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Station> GetAllStations(Predicate<Station> predicate = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DroneCharge> GetAllDroneCharges(Predicate<DroneCharge> predicate = null)
        {
            throw new NotImplementedException();
        }

        public void UpdateAssignParcelToDrone(int idParcel, int idDrone)
        {
            throw new NotImplementedException();
        }

        public void UpdateParcelCollectionByDrone(int idParcel)
        {
            throw new NotImplementedException();
        }

        public void UpdateParcelDeliveryToCustomer(int idParcel)
        {
            throw new NotImplementedException();
        }

        public void UpdateSendDroneToChargingStation(int idDrone, string nameStation)
        {
            throw new NotImplementedException();
        }

        public void UpdateDrone(Drone drone)
        {
            throw new NotImplementedException();
        }

        public void UpdateStation(int idStation, string newName, int chargingSlots)
        {
            throw new NotImplementedException();
        }

        public void UpdateCustomer(int idCustomer, string newName, string customerPhone)
        {
            throw new NotImplementedException();
        }

        public DroneCharge GetDroneCharge(int id)
        {
            throw new NotImplementedException();
        }

        public void DeleteParcel(int id)
        {
            throw new NotImplementedException();
        }
    }
}
