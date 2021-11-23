﻿using System;

namespace IDAL
{
    namespace DO
    {
        public struct Drone
        {
            public int Id { get; set; }//ID
            public string Model { get; set; }//The drone model
            public WeightCategories Weight { get; set; }//Weight category
            public override string ToString()
            {
                String result = "";
                result += $"ID is {Id} \n";
                result += $"Model is {Model} \n";
                result += $"Weight is {Weight} \n";
                return result;
            }
        }
    }
}
