﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        public enum WeightCategories { Easy, Medium, Heavy };
        public enum Priorities { Normal, Fast, Emergency };
        public enum DroneStatuses { Available, Maintenance, Delivery };
        public enum MainSwitchFunctions { Add, Update, Display, ListView, Exit };
        public enum AddingFunction { AddStation, AddDrone, AddCustomer, AddParcel };
        public enum UpdateingFunction { AssignParcelToDrone, ParcelCollectionByDrone, ParcelDeliveryToCustomer, SendDroneToChargingStation, DroneReleaseFromChargingStation};
        public enum DisplayingFunction { Station, Drone, Customer, Parcel };
        public enum ListViewFunction { Stations, Drones, Customers, Parcels, ParcelsWithNoDrone, StationWithAvailableChargingStation };
    }
}