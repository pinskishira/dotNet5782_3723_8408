using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Enum
    {
        public enum WeightCategories { Easy, Medium, Heavy };
        public enum Priorities { Normal, Fast, Emergency };
        public enum DroneStatuses { Available, Maintenance, Delivery };
        public enum ParcelState { Created, Paired, PickedUp, Provided };
    }
}
