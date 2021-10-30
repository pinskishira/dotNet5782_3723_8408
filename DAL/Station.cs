using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {

        public struct Station
        {
            public int Id { get; set; }//ID
            public string Name { get; set; }//The station name
            public double Longitude { get; set; }//Longitude
            public double Latitude { get; set; }//Latitude
            public int ChargeSlots { get; set; }//Number of charging stations
            public override string ToString()//Override function
            {
                String result = "";
                result += $"ID is {Id} \n";
                result += $"Name is {Name} \n";
                result += $"Latitude is {string.Format("{0:0.00}",Latitude)}\n";
                result += $"Longitude is {string.Format("{0:0.00}",Longitude)} \n";
                result += $"ChargeSlots is {ChargeSlots} \n";
                return result;
            }
        }
    }
}
