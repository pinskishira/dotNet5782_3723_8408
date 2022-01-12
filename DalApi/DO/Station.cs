using System;
using Utilities;

namespace DO
{
    public struct Station
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public int AvailableChargeSlots { get; set; }//Number of available charging stations
        public bool DeletedStation { get; set; }

        public override string ToString()
        {
            String result = "";
            result += $"ID is {Id} \n";
            result += $"Name is {Name} \n";
            result += $"Location is {Util.SexagesimalCoordinate(Longitude, Latitude)} \n";
            result += $"ChargeSlots is {AvailableChargeSlots} \n";
            return result;
        }
    }
}
