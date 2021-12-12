using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class DroneInParcel
    {
        public int Id { get; set; }
        public int Battery { get; set; }
        public Location CurrentLocation { get; set; }
        public override string ToString()
        {
            String result = "";
            result += $"Id is {Id} \n";
            result += $"Battery is {Battery} \n";
            result += $"Current location is {CurrentLocation} \n";
            return result;
        }
    }
}

