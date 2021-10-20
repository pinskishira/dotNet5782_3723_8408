using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        public struct Customer
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public double Longitude { get; set; }
            public double Latitude { get; set; }
            public override string ToString()
            {
                String result = " ";
                result += $"ID is {Id}, \n";
                result += $"Name is {Name}, \n";
                result += $"Telephone is {Phone.Substring(0, 3) + '-' + Phone.Substring(3)}, \n";
                result += $"Latitude is {Latitude}, \n";
                result += $"longitude is {Longitude}, \n";
                return result;
            }
        }
    }
}

