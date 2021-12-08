using System;

namespace IDAL
{
    namespace DO
    {
        public struct Station
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public double Longitude { get; set; }
            public double Latitude { get; set; }
            public int AvailableChargeSlots { get; set; }//Number of available charging stations
            public override string ToString()
            {
                String result = "";
                result += $"ID is {Id} \n";
                result += $"Name is {Name} \n";
                result += $"Longitude is {string.Format("{0:0.00}", Math.Round(Longitude, 2))} \n";
                result += $"Latitude is {string.Format("{0:0.00}", Math.Round(Latitude, 2))}";
                result += $"ChargeSlots is {AvailableChargeSlots} \n";
                return result;
            }
        }
    }
}
