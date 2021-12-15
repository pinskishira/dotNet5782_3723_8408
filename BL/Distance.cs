using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    class Distance
    {
        //function receives coordinates 
        // computes half a versine of the angle
        public static double Half(double radian)
        {
            return Math.Sin(radian / 2) * Math.Sin(radian / 2);
        }

        //returns an angle in radians
        public static double Radians(double degree)
        {
            return degree * Math.PI / 180;
        }

        //receiving 2 points the haversine formula returns the distance (in km) between the 2
        public static double Haversine(double lon1, double lat1, double lon2, double lat2)
        {
            const int RADIUS = 6371;//earths radius in KM

            double radLon = Radians(lon2 - lon1);//converts differance btween the points to radians
            double radLat = Radians(lat2 - lat1);
            double havd = Half(radLat) + (Math.Cos(Radians(lat2)) * Math.Cos(Radians(lat1)) * Half(radLon));//haversine formula determines the spherical distance between the two points using given versine
            double distance = 2 * RADIUS * Math.Asin(havd);
            return distance;
        }
    }
}
