using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class Enum
        {
            public enum WeightCategories { Easy = 1, Medium, Heavy };
            public enum Priorities { Normal = 1, Fast, Emergency };
            public enum DroneStatuses { Available = 1, Maintenance, Delivery };
            public enum ParcelState { Created = 1, Paired, PickedUp, Provided };
        }
    }
}
