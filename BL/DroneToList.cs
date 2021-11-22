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
        public class DroneToList
        {
            public int Id { get; set; }
            public string Model { get; set; }
            public WeightCategories Weight { get; set; }
            public int Battery { get; set; }
            public DroneStatuses DroneStatus { get; set; }
            public Location CurrentLocation { get; set; }
            public int ParcelNumInTransfer { get; set; }
            public override string ToString()
            {
                String result = "";
                result += $"Id is {Id} \n";
                result += $"Model is {Model} \n";
                result += $"MaxWeight is {Weight} \n";
                result += $"Battery is {Battery} \n";
                result += $"Drone Status is {DroneStatus} \n";
                result += $"Current Location is {CurrentLocation} \n";
                result += $"Parcel Number In Transfer is {ParcelNumInTransfer} \n";
                return result;
            }
        }
    }
}
