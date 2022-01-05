using System;

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
            String result = "";
            result += $"ID is {Id} \n";
            result += $"Name is {Name} \n";
            result += $"Telephone is {Phone.Substring(0, 3) + '-' + Phone.Substring(3)} \n";
            result += $"Longitude is {string.Format("{0:0.00}", Math.Round(Longitude, 2))} \n";
            result += $"Latitude is {string.Format("{0:0.00}", Math.Round(Latitude, 2))}";
            return result;
        }
    }
}

