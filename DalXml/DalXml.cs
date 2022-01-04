using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using DalApi;
using DO;

namespace Dal
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
        static DalXml() { XMLTools.SaveListToXMLSerializer(new List<int> { 0 }, "RunParcelNum.xml"); }//static ctor to ensure instance init is done just before first usage
        private DalXml()
        {
            List<DroneCharge> droneCharge = XMLTools.LoadListFromXMLSerializer<DroneCharge>(DroneChargeXml);
            foreach (var item in droneCharge)
            {
                UpdateChargeSlotsAStations(item.StationId);
            }
            droneCharge.Clear();
            XMLTools.SaveListToXMLSerializer(droneCharge, DroneChargeXml);
        }

        private void UpdateChargeSlotsAStations(int stationId)
        {
            List<Station> stations = XMLTools.LoadListFromXMLSerializer<Station>(StationXml);
            //BaseStation update.
            int indexaforBaseStationId = stations.FindIndex(x => x.Id == stationId);
            Station temp = stations[indexaforBaseStationId];
            temp.AvailableChargeSlots++;
            stations[indexaforBaseStationId] = temp;
            XMLTools.SaveListToXMLSerializer(stations, StationXml);
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

        public void AddCustomer(Customer newCustomer)
        {
            List<Customer> customers = XMLTools.LoadListFromXMLSerializer<Customer>(CustomerXml);
            if (DataSource.Customers.Exists(item => item.Id == newCustomer.Id))//checks if customer exists
                throw new ItemExistsException("The customer already exists.\n");
            customers.Add(newCustomer);
            XMLTools.SaveListToXMLSerializer(customers, CustomerXml);
        }

        public void AddDrone(Drone newDrone)
        {
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml);
            if (DataSource.Drones.Exists(item => item.Id == newDrone.Id))//checks if drone exists
                throw new ItemExistsException("The drone already exists.\n");
           drones.Add(newDrone);
            XMLTools.SaveListToXMLSerializer(drones, DroneXml);
        }

        public void AddDroneCharge(DroneCharge droneCharge)
        {
            XElement droneChargeRoot = XMLTools.LoadListFromXMLElement(DroneChargeXml);
            XElement droneId = new XElement("DroneId", droneCharge.DroneId);
            XElement stationId = new XElement("StationId", droneCharge.StationId);
            XElement timeDroneInCharging = new XElement("TimeDroneInCharging", droneCharge.TimeDroneInCharging);
            droneChargeRoot.Add(new XElement("DroneCharge", droneId, stationId, timeDroneInCharging));
            droneChargeRoot.Save(DroneChargeXml);
        }

        public void AddParcel(Parcel newParcel)
        {
            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);
            if (DataSource.Parcels.Exists(item => item.Id == newParcel.Id && !newParcel.DeletedParcel))//checks if parcel exists
                throw new ItemExistsException("The parcel already exists.\n");
            XElement runningNum = XElement.Load(@"RunParcelNum.xml");
            newParcel.Id = 1 +int.Parse(runningNum.Element("Id").Value);
            parcels.Add(newParcel);
            XMLTools.SaveListToXMLSerializer(parcels, ParcelXml);
            XMLTools.SaveListToXMLElement(runningNum, "RunParcelNum.xml");
        }

        public void AddStation(Station newStation)
        {
            List<Station> stations = XMLTools.LoadListFromXMLSerializer<Station>(StationXml);
            if (DataSource.Stations.Exists(item => item.Id == newStation.Id))//checks if station exists
                throw new ItemExistsException("The station already exists.\n");
            stations.Add(newStation);
            XMLTools.SaveListToXMLSerializer(stations, StationXml);
        }

       
        public Customer FindCustomer(int id)
        {
            List<Customer> customers = XMLTools.LoadListFromXMLSerializer<Customer>(CustomerXml);
            try
            {
                return customers.First(item => item.Id == id);//checks if customer exists
            }
            catch (InvalidOperationException)
            {
                throw new ItemExistsException("The customer does not exist.\n");
            }
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

        public IEnumerable<Customer> GetAllCustomers(Predicate<Customer> predicate = null)
        {
            List<Customer> customers = XMLTools.LoadListFromXMLSerializer<Customer>(CustomerXml);
            return from itemCustomer in customers
                   where predicate == null ? true : predicate(itemCustomer)
                   select itemCustomer;
        }

        public IEnumerable<Drone> GetAllDrones(Predicate<Drone> predicate = null)
        {
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml);
            return from itemDrone in drones
                   where predicate == null ? true : predicate(itemDrone)
                   select itemDrone;
        }

        public IEnumerable<Parcel> GetAllParcels(Predicate<Parcel> predicate = null)
        {
            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);
            return from itemParcel in parcels
                   where predicate == null ? true : predicate(itemParcel)
                   where !itemParcel.DeletedParcel
                   select itemParcel;
        }

        public IEnumerable<Station> GetAllStations(Predicate<Station> predicate = null)
        {
            List<Station> stations = XMLTools.LoadListFromXMLSerializer<Station>(StationXml);
            return from itemStation in stations
                   where predicate == null ? true : predicate(itemStation)
                   select itemStation;
        }

        public IEnumerable<DroneCharge> GetAllDroneCharges(Predicate<DroneCharge> predicate = null)
        {
            XElement droneChargeRoot = XMLTools.LoadListFromXMLElement(DroneChargeXml);
            IEnumerable<DroneCharge> droneCharges;
            try
            {
                droneCharges = (from p in droneChargeRoot.Elements()
                                select new DroneCharge()
                                {
                                    DroneId = Convert.ToInt32(p.Element("DroneId").Value),
                                    StationId = Convert.ToInt32(p.Element("StationId").Value),
                                    TimeDroneInCharging = Convert.ToDateTime(p.Element("TimeDroneInCharging").Value),
                                });
            }
            catch
            {
                droneCharges = null;
            }
            return droneCharges;
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

        public void UpdateParcelDeliveryToCustomer(int idParcel)
        {
            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);
            int indexParcel = parcels.FindIndex(parcel => parcel.Id == idParcel);//finding parcel that was collected by drone
            if (indexParcel == -1)
                throw new ItemDoesNotExistException("No parcel found with this id");
            if (parcels[indexParcel].DeletedParcel)
                throw new ItemDoesNotExistException("This parcel is deleted");
            Parcel newParcel = DataSource.Parcels[indexParcel];
            newParcel.Delivered = DateTime.Now;
            newParcel.DroneId = 0;//not assigned to drone anymore
            parcels[indexParcel] = newParcel;
            DataSource.Config.NextParcelNumber--;//updating that theres one less parcel to deliver
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
            XElement DroneCharge = XMLTools.LoadListFromXMLElement(DroneChargeXml);
            List<DroneCharge> droneCharges = (List<DroneCharge>)DroneCharge.Elements(DroneChargeXml);
            List<Station> stations = XMLTools.LoadListFromXMLSerializer<Station>(StationXml);
            int indexDC = droneCharges.FindIndex(indexOfDroneCharges => indexOfDroneCharges.DroneId == idDrone);//finds index where drone is
            if (indexDC == -1)//checks if drone exists
                throw new ItemDoesNotExistException("The drone does not exist.\n");
            int indexS = stations.FindIndex(indexOfStations => indexOfStations.Id == DataSource.DroneCharges[indexDC].StationId);//finds index where station is
            Station newStation = stations[indexS];
            newStation.AvailableChargeSlots++;//increasing amount of places left to charge
            stations[indexS] = newStation;
            droneCharges.RemoveAt(indexDC);//removes drone from charging
            XMLTools.SaveListToXMLSerializer(stations, StationXml);
            XMLTools.SaveListToXMLElement(DroneCharge, DroneChargeXml);
        }

        public void UpdateDrone(Drone drone)
        {
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml);
            int indexOfDrone = drones.FindIndex(index => index.Id == drone.Id);//finding index
            drones[indexOfDrone] = drone;//placing updated drone in place of index
            XMLTools.SaveListToXMLSerializer(drones, DroneXml);
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
        

        public void UpdateCustomer(int idCustomer, string newName, string customerPhone)
        {
            List<Customer> customers = XMLTools.LoadListFromXMLSerializer<Customer>(CustomerXml);
            int indexOfCustomer = customers.FindIndex(item => item.Id == idCustomer);//finds index where customer is
            if (indexOfCustomer == -1)//checks if customer exists
                throw new ItemDoesNotExistException("The customer does not exist.\n");
            Customer customer = customers[indexOfCustomer];
            if (newName != "")//if enter was entered instead of new name
                customer.Name = newName;
            if (customerPhone != "")//if enter was entered instead of new phone
                customer.Phone = customerPhone;
            customers[indexOfCustomer] = customer;//updated customer into list of customers
            XMLTools.SaveListToXMLSerializer(customers, CustomerXml);

        }

        public DroneCharge GetDroneCharge(int id)
        {
            XElement droneChargeRoot = XMLTools.LoadListFromXMLElement(DroneChargeXml);
            DroneCharge droneCharge;
            try
            {
                droneCharge = (from p in droneChargeRoot.Elements()
                               where Convert.ToInt32(p.Element("DroneId").Value) == id
                               select new DroneCharge()
                               {
                                   DroneId = Convert.ToInt32(p.Element("DroneId").Value),
                                   StationId = Convert.ToInt32(p.Element("StationId").Value),
                                   TimeDroneInCharging = Convert.ToDateTime(p.Element("TimeDroneInCharging").Value),
                               }).FirstOrDefault();
            }
            catch
            {
                droneCharge = default;
            }
            return droneCharge;
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
    }
}
