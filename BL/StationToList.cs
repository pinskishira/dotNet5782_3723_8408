﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class StationToList//תחתנת בסיס לרשימה
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int AvailableChargingSlots { get; set; }
            public int UnavaialbleChargingSlots { get; set; }
            public override string ToString()//Override function
            {
                String result = "";
                result += $"ID is {Id} \n";
                result += $"Name Of Station is {Name} \n";
                result += $"Available Charge Slots is {AvailableChargingSlots} \n";
                result += $"Unavaialble Charging Slots is {UnavaialbleChargingSlots} \n";
                return result;
            }
        }
    }
}
