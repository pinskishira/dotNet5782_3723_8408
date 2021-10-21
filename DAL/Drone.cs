using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        public struct Drone
        {
            public int Id { get; set; }//ID
            public string Model { get; set; }//The drone model
            public WeightCategories MaxWeight { get; set; }//Weight category
            public DroneStatuses Status { get; set; }//Drone mode
            public double Battery { get; set; }//Battery status
            public override string ToString()
            {
                String result = " ";
                result += $"ID is {Id}, \n";
                result += $"Model is {Model}, \n";
                result += $"MaxWeight is {MaxWeight}, \n";
                result += $"Status is {Status}, \n";
                result += $"Battery is {Battery}, \n";
                return result;
            }
        }
    }
}
