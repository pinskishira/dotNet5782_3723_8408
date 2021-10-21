using System;
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
        public enum MainSwitchFunctions { Add, Update, Display, ListView };
        public enum AddingFunction { AddStation, AddDrone, AddCustomer, AddParcel };

    }
}