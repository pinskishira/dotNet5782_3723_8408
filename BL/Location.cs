using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class Location
        {
            public double Longitude { get; set; }
            public double Latitude { get; set; }
            public override string ToString()
            {
                String result = "";
                result += $"Longitude is {string.Format("{0:0.00}", Longitude)} \n";
                result += $"Latitude is {string.Format("{0:0.00}", Latitude)}\n";
                return result;
            }
        }
    }
}
