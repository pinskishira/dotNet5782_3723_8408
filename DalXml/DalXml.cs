using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using DalApi;
using DO;

namespace Dal
{
    public class DalXml : IDal
    {
        private static string DroneXml = @"DroneXml.xml";
        private static string StationXml = @"StationXml.xml";
        private static string CustomerXml = @"CustomerXml.xml";
        private static string ParcelXml = @"ParcelXml.xml";
        private static string DroneChargeXml = @"DroneChargeXml.xml";



        internal static DalXml Instance { get { return instance.Value; } }
        private static readonly Lazy<DalXml> instance = new Lazy<DalXml>(() => new DalXml());
        static DalXml() { }//static ctor to ensure instance init is done just before first usage
        private DalXml() { }

        public double[] electricityUse()
        {
            XElement p = XMLTools.LoadListFromXMLElement(@"config.xml");
            double[] elecUse = new double[5];
            elecUse[0] = double.Parse(p.Element("BatteryConsumptionPowerUsageEmpty").Value);
            elecUse[1] = double.Parse(p.Element("BatteryConsumptionLightWeight").Value);
            elecUse[2] = double.Parse(p.Element("BatteryConsumptionMediumWeight").Value);
            elecUse[3] = double.Parse(p.Element("BatteryConsumptionHeavyWeight").Value);
            elecUse[4] = double.Parse(p.Element("DroneChargingRatePH").Value);
            return elecUse;
        }
        #region Customers
        public void AddCustomer(Customer newCustomer)
        {

            XElement customerXml = XMLTools.LoadListFromXMLElement(CustomerXml);

            XElement customer = (from cus in customerXml.Elements()
                                 where cus.Element("Id").Value == newCustomer.Id.ToString()
                                 select cus).FirstOrDefault();
            if (customer != null)
            {
                throw new ItemExistsException("The customer already exists.\n");//checks if customer exists
            }

            XElement CustomerElem = new XElement("Customer",
                                 new XElement("Id", newCustomer.Id),
                                 new XElement("Name", newCustomer.Name),
                                 new XElement("Phone", newCustomer.Phone),
                                 new XElement("Longitude", newCustomer.Longitude),
                                 new XElement("Latitude", newCustomer.Latitude));

            customerXml.Add(CustomerElem);
            XMLTools.SaveListToXMLElement(customerXml, CustomerXml);
        }

        public Customer FindCustomer(int id)
        {
            XElement customerXml = XMLTools.LoadListFromXMLElement(CustomerXml);

            Customer customer = (from cus in customerXml.Elements()
                                 where cus.Element("Id").Value == id.ToString()
                                 select new Customer()
                                 {
                                     Id = int.Parse(cus.Element("Id").Value),
                                     Name = cus.Element("Name").Value,
                                     Phone = cus.Element("Phone").Value,
                                     Longitude = double.Parse(cus.Element("Longitude").Value),
                                     Latitude = double.Parse(cus.Element("Latitude").Value)
                                 }
                        ).FirstOrDefault();

            if (customer.Id != 0)
            {
                return customer;
            }
            else
            {
                throw new ItemExistsException("The customer does not exist.\n");

            }
        }

        public IEnumerable<Customer> GetAllCustomers(Predicate<Customer> predicate = null)
        {
            XElement customerXml = XMLTools.LoadListFromXMLElement(CustomerXml);
            IEnumerable<Customer> customer = from cus in customerXml.Elements()
                                             select new Customer()
                                             {
                                                 Id = int.Parse(cus.Element("Id").Value),
                                                 Name = cus.Element("Name").Value,
                                                 Phone = cus.Element("Phone").Value,
                                                 Longitude = double.Parse(cus.Element("Longitude").Value),
                                                 Latitude = double.Parse(cus.Element("Latitude").Value)
                                             };
            return customer.Select(item => item);
        }

        public void UpdateCustomer(int idCustomer, string newName, string customerPhone)
        {
            XElement customerXml = XMLTools.LoadListFromXMLElement(CustomerXml);

            XElement customer = (from cus in customerXml.Elements()
                                 where cus.Element("Id").Value == idCustomer.ToString()
                                 select cus).FirstOrDefault();
            if (customer == null)
                throw new ItemDoesNotExistException("The customer does not exist.\n");

            customer.Element("Id").Value = idCustomer.ToString();
            customer.Element("Name").Value = newName;
            customer.Element("PhoneNumber").Value = customerPhone;
            customer.Element("Longitude").Value = customer.Element("Longitude").ToString();
            customer.Element("Latitude").Value = customer.Element("Latitude").ToString();

            XMLTools.SaveListToXMLElement(customerXml, CustomerXml);
        }
        #endregion Customers

        #region Drones
        public void AddDrone(Drone newDrone)
        {
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml);
            if (drones.Exists(item => item.Id == newDrone.Id))//checks if drone exists
                throw new ItemExistsException("The drone already exists.\n");
            drones.Add(newDrone);
            XMLTools.SaveListToXMLSerializer(drones, DroneXml);
        }

        public Drone FindDrone(int id)
        {
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml);
            try
            {
                return drones.First(item => item.Id == id);//checks if drone exists
            }
            catch (InvalidOperationException)
            {
                throw new ItemExistsException("The drone does not exist.\n");
            }
        }

    public IEnumerable<Drone> GetAllDrones(Predicate<Drone> predicate = null)
    {
        List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml);
        return from itemDrone in drones
               where predicate == null ? true : predicate(itemDrone)
               select itemDrone;
    }

        public void UpdateAssignParcelToDrone(int idParcel, int idDrone)
        {
            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml);
            if (!drones.Exists(item => item.Id == idDrone))//checks if drone exists
                throw new ItemDoesNotExistException("The drone does not exist.\n");
            int indexAssign = parcels.FindIndex(parcel => parcel.Id == idParcel);
            if (indexAssign == -1)
                throw new ItemDoesNotExistException("No parcel found with this id");
            if (parcels[indexAssign].DeletedParcel)
                throw new ItemDoesNotExistException("This parcel is deleted");
            Parcel newParcel = parcels[indexAssign];
            newParcel.DroneId = idDrone;//giving parcel available drones' id
            newParcel.Scheduled = DateTime.Now;//updating date and time
            parcels[indexAssign] = newParcel;
            XMLTools.SaveListToXMLSerializer(parcels, ParcelXml);
        }

    public void UpdateParcelCollectionByDrone(int idParcel)
    {
        List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);
        int indexParcel = parcels.FindIndex(parcel => parcel.Id == idParcel);//finding parcel that was collected by drone
        if (indexParcel == -1)
            throw new ItemDoesNotExistException("No parcel found with this id");
        if (parcels[indexParcel].DeletedParcel)
            throw new ItemDoesNotExistException("This parcel is deleted");
        Parcel newParcel = parcels[indexParcel];
        newParcel.PickedUp = DateTime.Now;
        parcels[indexParcel] = newParcel;//updating date and time
        XMLTools.SaveListToXMLSerializer(parcels, ParcelXml);
    }

        public void UpdateSendDroneToChargingStation(int idDrone, string nameStation)
        {
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml);
            List<Station> stations = XMLTools.LoadListFromXMLSerializer<Station>(StationXml);
            if (!drones.Exists(item => item.Id == idDrone))//checks if drone exists
                throw new ItemDoesNotExistException("The drone does not exist.\n");
            DroneCharge newDroneCharge = new DroneCharge();
            //drone with low battery will go be charged here
            int indexOfStation = stations.FindIndex(indexOfStation => indexOfStation.Name == nameStation);
            if (indexOfStation == -1)//checks if station exists
                throw new ItemDoesNotExistException("The station does not exist.\n");
            newDroneCharge.DroneId = idDrone;//putting id of low battery drone into its charging station
            newDroneCharge.StationId = stations[indexOfStation].Id;
            newDroneCharge.TimeDroneInCharging = DateTime.Now;
            AddDroneCharge(newDroneCharge);//updating that a drone is charging
            Station newStation = stations[indexOfStation];
            newStation.AvailableChargeSlots--;//less available charge slots in station
            stations[indexOfStation] = newStation;
            XMLTools.SaveListToXMLSerializer(stations, StationXml);
        }

        public void DroneReleaseFromChargingStation(int idDrone)
        {
            List<DroneCharge> droneCharges = XMLTools.LoadListFromXMLSerializer<DroneCharge>(DroneChargeXml);
            List<Station> stations = XMLTools.LoadListFromXMLSerializer<Station>(StationXml);
            int indexDC = droneCharges.FindIndex(indexOfDroneCharges => indexOfDroneCharges.DroneId == idDrone);//finds index where drone is
            if (indexDC == -1)//checks if drone exists
                throw new ItemDoesNotExistException("The drone does not exist.\n");
            int indexS = stations.FindIndex(indexOfStations => indexOfStations.Id == droneCharges[indexDC].StationId);//finds index where station is
            Station newStation = stations[indexS];
            newStation.AvailableChargeSlots++;//increasing amount of places left to charge
            stations[indexS] = newStation;
            droneCharges.Remove(droneCharges[indexDC]);
            XMLTools.SaveListToXMLSerializer(stations, StationXml);
            XMLTools.SaveListToXMLSerializer(droneCharges, DroneChargeXml);
        }

        public void UpdateDrone(Drone drone)
        {
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml);
            int indexOfDrone = drones.FindIndex(index => index.Id == drone.Id);//finding index
            drones[indexOfDrone] = drone;//placing updated drone in place of index
            XMLTools.SaveListToXMLSerializer(drones, DroneXml);
        }
        #endregion Drones

        #region Stations
        public void AddStation(Station newStation)
        {
            List<Station> stations = XMLTools.LoadListFromXMLSerializer<Station>(StationXml);
            if (stations.Exists(item => item.Id == newStation.Id))//checks if station exists
                throw new ItemExistsException("The station already exists.\n");
            stations.Add(newStation);
            XMLTools.SaveListToXMLSerializer(stations, StationXml);
        }

        public Station FindStation(int id)
        {
            List<Station> stations = XMLTools.LoadListFromXMLSerializer<Station>(StationXml);
            try
            {
                return stations.First(item => item.Id == id);//checks if station exists
            }
            catch (InvalidOperationException)
            {
                throw new ItemExistsException("The station does not exist.\n");
            }
        }

        public IEnumerable<Station> GetAllStations(Predicate<Station> predicate = null)
        {
            List<Station> stations = XMLTools.LoadListFromXMLSerializer<Station>(StationXml);
            return from itemStation in stations
                   where predicate == null ? true : predicate(itemStation)
                   select itemStation;
        }

        public void UpdateStation(int idStation, string newName, int chargingSlots)
        {
            List<Station> stations = XMLTools.LoadListFromXMLSerializer<Station>(StationXml);
            XElement DroneCharge = XMLTools.LoadListFromXMLElement(DroneChargeXml);
            List<DroneCharge> droneCharges = (List<DroneCharge>)DroneCharge.Elements(DroneChargeXml);
            int chargeSlotsInUse = 0;
            foreach (var indexOfDroneCharge in droneCharges)//goes through list of drones in charging
                if (indexOfDroneCharge.StationId == idStation)//If the loaded drone ID number is equal to the station ID number
                    chargeSlotsInUse++;
            int indexOfStation = stations.FindIndex(item => item.Id == idStation);
            if (indexOfStation == -1)//checks if station exists
                throw new ItemDoesNotExistException("The station does not exist.\n");
            Station station = stations[indexOfStation];
            if (newName != "")//if enter wasnt inputted
                station.Name = newName;
            if (chargingSlots != 0)//if 0 wasnt inputted
                station.AvailableChargeSlots = chargingSlots - chargeSlotsInUse;
            stations[indexOfStation] = station;//placing updated station in list of stations
            XMLTools.SaveListToXMLSerializer(stations, StationXml);
        }
        #endregion Stations

        #region Parcels
        public void AddParcel(Parcel newParcel)
        {
            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);
            if (parcels.Exists(item => item.Id == newParcel.Id && !newParcel.DeletedParcel))//checks if parcel exists
                throw new ItemExistsException("The parcel already exists.\n");
            XElement runNumber = XMLTools.LoadListFromXMLElement(@"config.xml");
            newParcel.Id = 1 + int.Parse(runNumber.Element("runNum").Value);
            runNumber.Element("runNum").Value = newParcel.Id.ToString();
            parcels.Add(newParcel);
            XMLTools.SaveListToXMLSerializer(parcels, ParcelXml);
            XMLTools.SaveListToXMLElement(runNumber, "config.xml");
        }

        public Parcel FindParcel(int id)
        {
            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);
            int index = parcels.FindIndex(parcel => parcel.Id == id);
            if (index == -1)
                throw new ItemDoesNotExistException("No parcel found with this id");
            if (parcels[index].DeletedParcel)
                throw new ItemDoesNotExistException("This parcel is deleted");
            return parcels[index];//finding parcel
        }

        public IEnumerable<Parcel> GetAllParcels(Predicate<Parcel> predicate = null)
        {
            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);
            return from itemParcel in parcels
                   where predicate == null ? true : predicate(itemParcel)
                   where !itemParcel.DeletedParcel
                   select itemParcel;
        }

        public void UpdateParcelDeliveryToCustomer(int idParcel)
        {
            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);
            int indexParcel = parcels.FindIndex(parcel => parcel.Id == idParcel);//finding parcel that was collected by drone
            if (indexParcel == -1)
                throw new ItemDoesNotExistException("No parcel found with this id");
            if (parcels[indexParcel].DeletedParcel)
                throw new ItemDoesNotExistException("This parcel is deleted");
            Parcel newParcel = parcels[indexParcel];
            newParcel.Delivered = DateTime.Now;
            newParcel.DroneId = 0;//not assigned to drone anymore
            parcels[indexParcel] = newParcel;
            //DataSource.Config.NextParcelNumber--;//updating that theres one less parcel to deliver
            XMLTools.SaveListToXMLSerializer(parcels, ParcelXml);
        }

        public void DeleteParcel(int id)
        {
            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);
            int indexParcel = parcels.FindIndex(parcel => parcel.Id == id);//finding parcel that was collected by drone
            if (indexParcel == -1)
                throw new ItemDoesNotExistException("No parcel found with this id");
            if (parcels[indexParcel].DeletedParcel)
                throw new ItemDoesNotExistException("This parcel is deleted");
            Parcel parcel = parcels[indexParcel];
            parcel.DeletedParcel = true;
            parcels[indexParcel] = parcel;
            XMLTools.SaveListToXMLSerializer(parcels, ParcelXml);

        }
        #endregion Parcels

        #region DroneCharge
        public void AddDroneCharge(DroneCharge droneCharge)
        {
            List<DroneCharge> droneChargeRoot = XMLTools.LoadListFromXMLSerializer<DroneCharge>(DroneChargeXml);
            if (droneChargeRoot.Exists(item => item.DroneId == droneCharge.DroneId))//chcks if the charger already exists
            {
                throw new ItemExistsException("The Drone Charge already exists.\n");
            }
            droneChargeRoot.Add(droneCharge);
            XMLTools.SaveListToXMLSerializer(droneChargeRoot, DroneChargeXml);
        }

        public IEnumerable<DroneCharge> GetAllDroneCharges(Predicate<DroneCharge> predicate = null)
        {
            List<DroneCharge> droneCharges = XMLTools.LoadListFromXMLSerializer<DroneCharge>(DroneChargeXml);
            return from item in droneCharges
                   where predicate == null ? true : predicate(item)
                   select item;
        }

        public DroneCharge GetDroneCharge(int id)
        {
            try
            {
                List<DroneCharge> droneCharges = XMLTools.LoadListFromXMLSerializer<DroneCharge>(DroneChargeXml);
                return droneCharges.First(x => x.DroneId == id);

            }
            catch (InvalidOperationException)
            {
                throw new ItemDoesNotExistException("The drone does not exists.\n");

            }
        }
        #endregion DroneCharge
    }
}


