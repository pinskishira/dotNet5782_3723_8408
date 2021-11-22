﻿using System;
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
        public class DroneToList//רחפן לרשימה
        {
            public int Id { get; set; }//ID
            public string Model { get; set; }//The drone model
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
                result += $"DroneStatus is {DroneStatus} \n";
                result += $"CurrentLocation is {CurrentLocation} \n";
                result += $"ParcelNumInTransfer is {ParcelNumInTransfer} \n";
                return result;
            }
        }
    }
}
