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
        public class DroneInPackage//רחפן בחבילה
        {
            public int Id { get; set; }
            public int Battery { get; set; }
            public Location CurrentLocation { get; set; }
            public override string ToString()
            {
                String result = "";
                result += $"Id is {Id} \n";
                result += $"Battery is {Battery} \n";
                result += $"CurrentLocation is {CurrentLocation} \n";
                return result;
            }
        }
    }
}
