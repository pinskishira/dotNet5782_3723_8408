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
            public int Id { get; set; }//ID
            public string NameOfStation { get; set; }//The station name
            public Location StationLocation { get; set; }//Longitude
            public int AvailableChargeSlots { get; set; }//Number of charging stations
            public List<DroneInCharging> DronesInCharging { get; set; }
            public override string ToString()//Override function
            {
                String result = "";
                result += $"ID is {Id} \n";
                result += $"Name is {NameOfStation} \n";
                result += $"Location is {StationLocation} \n";
                result += $"Available Charge Slots is {AvailableChargeSlots} \n";
                result += $"Drones In Charging is {DronesInCharging} \n";
                return result;
            }
        }
    }
}
