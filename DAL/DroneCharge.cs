﻿using System;

namespace IDAL
{
    namespace DO
    {
        public struct DroneCharge
        {
            public int DroneId { get; set; }//Drone ID
            public int StationId { get; set; }//Base Station ID
            public override string ToString()
            {
                String result = "";
                result += $"DroneId is {DroneId} \n";
                result += $"Station Id is {StationId} \n";
                return result;
            }
        }
    }
}
