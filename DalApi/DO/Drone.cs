using System;
using static DO.Enum;

namespace DO
{
    public struct Drone
    {
        public int Id { get; set; }
        public string Model { get; set; }//The drone model
        public WeightCategories Weight { get; set; }
        public bool DeletedDrone { get; set; }
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
