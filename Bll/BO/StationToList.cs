using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class StationToList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AvailableChargeSlots { get; set; }
        public int OccupiedChargeSlots { get; set; }
        public override string ToString()
        {
            String result = "";
            result += $"ID is {Id} \n";
            result += $"Name Of Station is {Name} \n";
            result += $"Available Charge Slots is {AvailableChargeSlots} \n";
            result += $"Occupied Charge Slots is {OccupiedChargeSlots} \n";
            return result;
        }
    }
}
