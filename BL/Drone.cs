using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IBL.BO.Enum;
using IBL.BO;

namespace BL
{
    class Drone
    {
        public int Id { get; set; }//ID
        public string Model { get; set; }//The drone model
        public WeightCategories MaxWeight { get; set; }
        public int Battery { get; set; }
        public State DroneStatus { get; set; }
        public PackageInTranser PackInTranser { get; set; }
        public Location CurrentLocation { get; set; }

    }
}
