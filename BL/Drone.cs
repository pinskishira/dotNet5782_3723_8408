﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IBL.BO.Enum;
using IBL.BO;

namespace   IBL
{
    namespace BO
    {
        class Drone
        {
            public int Id { get; set; }//ID
            public string Model { get; set; }//The drone model
            public WeightCategories MaxWeight { get; set; }
            public int Battery { get; set; }
            public State DroneStatus { get; set; }
            public PackageInTranser ParcelInTranser { get; set; }
            public Location CurrentLocation { get; set; }
            public override string ToString()
            {
                String result = "";
                result += $"Id is {Id} \n";
                result += $"Model is {Model} \n";
                result += $"MaxWeight is {MaxWeight} \n";
                result += $"Battery is {Battery} \n";
                result += $"DroneStatus is {DroneStatus} \n";
                result += $"ParcelInTranser is {ParcelInTranser} \n";
                result += $"CurrentLocation is {CurrentLocation} \n";
                return result;
            }
        }
    }
}
