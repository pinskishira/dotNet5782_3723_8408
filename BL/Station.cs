using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IBL.BO.Enum;
using IBL.BO;

namespace BL
{
    class Station
    {
        public int Id { get; set; }//ID
        public string Name { get; set; }//The station name
        public Location StationLocation { get; set; }//Longitude
        public int AvailableChargeSlots { get; set; }//Number of charging stations
        public List<DroneInCharging> DronesInCharging { get; set; }
    }
}
