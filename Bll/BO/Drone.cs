using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static BO.Enum;

namespace BO
{
    public class Drone
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public WeightCategories Weight { get; set; }
        public int Battery { get; set; }
        public DroneStatuses DroneStatus { get; set; }
        public Location CurrentLocation { get; set; }
        public ParcelInTransfer ParcelInTransfer { get; set; }
        public override string ToString()
        {
            String result = "";
            result += $"Id is {Id} \n";
            result += $"Model is {Model} \n";
            result += $"Weight is {Weight} \n";
            result += $"Battery is {Battery} \n";
            result += $"Drone Status is {DroneStatus} \n";
            result += $"Parcel in transfer is {ParcelInTransfer} \n";
            result += $"Current location is {CurrentLocation} \n";
            return result;
        }
    }
}
