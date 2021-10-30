using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        public struct DroneCharge
        {
            public int DroneId { get; set; }//Drone ID
            public int StationId { get; set; }//Base Station ID
            public override string ToString()
            {
                String result = "";
                result += $"DroneId is {DroneId} \n";
                result += $"StationId is {StationId} \n";
                return result;
            }
        }
    }
}
