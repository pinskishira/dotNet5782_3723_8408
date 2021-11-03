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
            public override string ToString()
            {
                String result = "";
                result += $"ID is {Id} \n";
                result += $"Model is {Model} \n";
                result += $"MaxWeight is {MaxWeight} \n";
                return result;
            }
        }
    }
}
