using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BO
{
    public class Station
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Location StationLocation { get; set; }
        public int AvailableChargeSlots { get; set; }
        public IEnumerable<DroneInCharging> DronesInCharging { get; set; }
        public override string ToString()
        {
            String result = "";
            result += $"ID is {Id} \n";
            result += $"Name is {Name} \n";
            result += $"Location is {StationLocation} \n";
            result += $"Available charge slots is {AvailableChargeSlots} \n";
            if (DronesInCharging != null)
            {
                result += $"Drones in charging is:\n";
                foreach (var indexOfDronesInCharging in DronesInCharging)
                {
                    result += $"{indexOfDronesInCharging} \n";
                }
            }
            return result;
        }
    }
}
