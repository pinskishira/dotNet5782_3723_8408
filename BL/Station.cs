
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IBL.BO.Enum;
using IBL.BO;


namespace IBL
{
    namespace BO
    {
        public class Station
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public Location StationLocation { get; set; }
            public int AvailableChargeSlots { get; set; }
            public List<DroneInCharging> DronesInCharging { get; set; }
            public override string ToString()
            {
                String result = "";
                result += $"ID is {Id} \n";
                result += $"Name is {Name} \n";
                result += $"Location is {StationLocation} \n";
                result += $"Available Charge Slots is {AvailableChargeSlots} \n";
                result += $"Drones In Charging is {DronesInCharging} \n";
                return result;
            }
        }
    }
}
