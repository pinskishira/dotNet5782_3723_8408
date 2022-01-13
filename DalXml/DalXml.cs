using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using DalApi;
using DO;
using System.Runtime.CompilerServices;

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

        [MethodImpl(MethodImplOptions.Synchronized)]
        public double[] electricityUse()
        {
            XElement p = XMLTools.LoadListFromXMLElement(@"config.xml");
            double[] elecUse = new double[5];
            elecUse[0] = int.Parse(p.Element("BatteryConsumptionPowerUsageEmpty").Value);
            elecUse[1] = int.Parse(p.Element("BatteryConsumptionLightWeight").Value);
            elecUse[2] = int.Parse(p.Element("BatteryConsumptionMediumWeight").Value);
            elecUse[3] = int.Parse(p.Element("BatteryConsumptionHeavyWeight").Value);
            elecUse[4] = int.Parse(p.Element("DroneChargingRatePH").Value);
            return elecUse;
        }

        #region Customers

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddCustomer(Customer newCustomer)
        {

            XElement customerXml = XMLTools.LoadListFromXMLElement(CustomerXml);

            XElement customer = (from cus in customerXml.Elements()
                                 where cus.Element("Id").Value == newCustomer.Id.ToString() && cus.Element("DeletedCustomer").Value == false.ToString()
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
                                 new XElement("Latitude", newCustomer.Latitude),
                                 new XElement("DeletedCustomer", newCustomer.DeletedCustomer));
            customerXml.Add(CustomerElem);
            XMLTools.SaveListToXMLElement(customerXml, CustomerXml);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
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
                                     Latitude = double.Parse(cus.Element("Latitude").Value),
                                     DeletedCustomer = bool.Parse(cus.Element("DeletedCustomer").Value)
                                 }
                        ).FirstOrDefault();

            if (customer.Id != 0)
            {
                return customer;
            }
            else
            {
                throw new ItemExistsException("The customer does not exist or it has been deleted\n");

            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
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
                                                 Latitude = double.Parse(cus.Element("Latitude").Value),
                                                 DeletedCustomer = bool.Parse(cus.Element("DeletedCustomer").Value)
                                             };
            return customer.Where(item => !item.DeletedCustomer);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
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
            customer.Element("Phone").Value = customerPhone;

            XMLTools.SaveListToXMLElement(customerXml, CustomerXml);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteCustomer(int id)
        {

            XElement customerXml = XMLTools.LoadListFromXMLElement(CustomerXml);

            XElement customer = (from cus in customerXml.Elements()
                                 where cus.Element("Id").Value == id.ToString()
                                 select cus).FirstOrDefault();
            if (customer == null)
                throw new ItemDoesNotExistException("The customer does not exist or it has been deleted.\n");

            customer.Element("Id").Value = id.ToString();
            customer.Element("DeletedCustomer").Value = true.ToString();

            XMLTools.SaveListToXMLElement(customerXml, CustomerXml);
        }

        public bool IsActive(int id)
        {
            XElement customerXml = XMLTools.LoadListFromXMLElement(CustomerXml);
            XElement customer = (from cus in customerXml.Elements()
                                 where cus.Element("Id").Value == id.ToString()
                                 select cus).FirstOrDefault();
            if (customer.Element("DeletedCustomer").Value == "True")
                return false;
            return true;
        }

        #endregion Customers

        #region Drones

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddDrone(Drone newDrone)
        {
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml);
            if (drones.Exists(item => item.Id == newDrone.Id) && (!newDrone.DeletedDrone))//checks if drone exists
                throw new ItemExistsException("The drone already exists.\n");
            drones.Add(newDrone);
            XMLTools.SaveListToXMLSerializer(drones, DroneXml);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Drone FindDrone(int id)
        {
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml);
            int index = drones.FindIndex(parcel => parcel.Id == id);
            if (index == -1)
                throw new ItemDoesNotExistException("No drone found with this id");
            return drones[index];//finding drone
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Drone> GetAllDrones(Predicate<Drone> predicate = null)
        {
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml);
            return from itemDrone in drones
                   where (predicate == null ? true : predicate(itemDrone)) && !itemDrone.DeletedDrone
                   select itemDrone;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Drone> GetAllDronesToBlDrones(Predicate<Drone> predicate = null)
        {
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml);
            return from itemDrone in drones
                   where predicate == null ? true : predicate(itemDrone)
                   select itemDrone;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateAssignParcelToDrone(int idParcel, int idDrone)
        {
            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml);
            int index = drones.FindIndex(drone => drone.Id == idDrone);
            if ((!drones.Exists(item => item.Id == idDrone)) && drones[index].DeletedDrone)//checks if drone exists
                throw new ItemDoesNotExistException("The drone does not exist or it is deleted.\n");
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

        [MethodImpl(MethodImplOptions.Synchronized)]
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

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateSendDroneToChargingStation(int idDrone, string nameStation)
        {
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml);
            List<Station> stations = XMLTools.LoadListFromXMLSerializer<Station>(StationXml);
            int index = drones.FindIndex(drone => drone.Id == idDrone);
            if ((!drones.Exists(item => item.Id == idDrone)) && drones[index].DeletedDrone)//checks if drone exists
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

        [MethodImpl(MethodImplOptions.Synchronized)]
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

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateDrone(Drone drone)
        {
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml);
            int indexOfDrone = drones.FindIndex(index => index.Id == drone.Id);//finding index
            drones[indexOfDrone] = drone;//placing updated drone in place of index
            XMLTools.SaveListToXMLSerializer(drones, DroneXml);
        }

        public void DeleteDrone(int id)
        {
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml);
            int indexDrone = drones.FindIndex(drone => drone.Id == id);//finding parcel that was collected by drone
            if (indexDrone == -1)
                throw new ItemDoesNotExistException("No parcel found with this id");
            Drone drone = drones[indexDrone];
            drone.DeletedDrone = true;
            drones[indexDrone] = drone;
            XMLTools.SaveListToXMLSerializer(drones, DroneXml);
        }
        #endregion Drones

        #region Stations
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddStation(Station newStation)
        {
            List<Station> stations = XMLTools.LoadListFromXMLSerializer<Station>(StationXml);
            if (stations.Exists(item => (item.Id == newStation.Id) && (!item.DeletedStation)))//checks if station exists
                throw new ItemExistsException("The station already exists.\n");
            stations.Add(newStation);
            XMLTools.SaveListToXMLSerializer(stations, StationXml);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
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

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Station> GetAllStations(Predicate<Station> predicate = null)
        {
            List<Station> stations = XMLTools.LoadListFromXMLSerializer<Station>(StationXml);
            return from itemStation in stations
                   where (predicate == null ? true : predicate(itemStation)) && !itemStation.DeletedStation
                   select itemStation;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateStation(int idStation, string newName, int chargingSlots)
        {
            List<Station> stations = XMLTools.LoadListFromXMLSerializer<Station>(StationXml);
            List<DroneCharge> droneCharges = XMLTools.LoadListFromXMLSerializer<DroneCharge>(DroneChargeXml);
            int chargeSlotsInUse = 0;
            foreach (var indexOfDroneCharge in droneCharges)//goes through list of drones in charging
                if (indexOfDroneCharge.StationId == idStation)//If the loaded drone ID number is equal to the station ID number
                    chargeSlotsInUse++;
            int indexOfStation = stations.FindIndex(item => item.Id == idStation);
            if (indexOfStation == -1)//checks if station exists
                throw new ItemDoesNotExistException("The station does not exist.\n");
            if (stations[indexOfStation].DeletedStation)
                throw new ItemDoesNotExistException("This delete is deleted");
            Station station = stations[indexOfStation];
            if (newName != "")//if enter wasnt inputted
                station.Name = newName;
            if (chargingSlots != 0)//if 0 wasnt inputted
                station.AvailableChargeSlots = chargingSlots - chargeSlotsInUse;
            stations[indexOfStation] = station;//placing updated station in list of stations
            XMLTools.SaveListToXMLSerializer(stations, StationXml);
        }


        public void DeleteStation(int id)
        {
            List<Station> stations = XMLTools.LoadListFromXMLSerializer<Station>(StationXml);
            int indexStation = stations.FindIndex(station => station.Id == id);//finding parcel that was collected by drone
            if (indexStation == -1)
                throw new ItemDoesNotExistException("No station found with this id");
            Station station = stations[indexStation];
            station.DeletedStation = true;
            stations[indexStation] = station;
            XMLTools.SaveListToXMLSerializer(stations, StationXml);
        }
        #endregion Stations

        #region Parcels
        [MethodImpl(MethodImplOptions.Synchronized)]
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

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Parcel FindParcel(int id)
        {
            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);
            int index = parcels.FindIndex(parcel => parcel.Id == id);
            if (index == -1)
                throw new ItemDoesNotExistException("No parcel found with this id");
            return parcels[index];//finding parcel
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Parcel> GetAllParcels(Predicate<Parcel> predicate = null)
        {
            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);
            return from itemParcel in parcels
                   where (predicate == null ? true : predicate(itemParcel)) && !itemParcel.DeletedParcel
                   select itemParcel;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
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
            XMLTools.SaveListToXMLSerializer(parcels, ParcelXml);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
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
        [MethodImpl(MethodImplOptions.Synchronized)]
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

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DroneCharge> GetAllDroneCharges(Predicate<DroneCharge> predicate = null)
        {
            List<DroneCharge> droneCharges = XMLTools.LoadListFromXMLSerializer<DroneCharge>(DroneChargeXml);
            return from item in droneCharges
                   where predicate == null ? true : predicate(item)
                   select item;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
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

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteDroneCharge(int id)
        {
            List<DroneCharge> droneCharges = XMLTools.LoadListFromXMLSerializer<DroneCharge>(DroneChargeXml);
            if (!droneCharges.Exists(drone => drone.DroneId == id))//chcks if the charger exists
            {
                throw new ItemExistsException("No Drone Charge found with this id.\n");
            }
            droneCharges.RemoveAll(drone => drone.DroneId == id);
            XMLTools.SaveListToXMLSerializer(droneCharges, DroneChargeXml);
        }
        #endregion DroneCharge      
    }
}