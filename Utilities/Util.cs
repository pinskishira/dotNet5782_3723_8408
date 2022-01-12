namespace Utilities
{
    /// <summary>
    /// View based on 60s of coordinate values.
    /// </summary>
    public static class Util
    {
        public static string SexagesimalCoordinate(double lon, double lat)
        {
            string latitudeAndLongitude = "";
            double latDegreesWithFraction = lat;
            char lonDirection;
            char latdirection;
            if (lat < 0)
                latdirection = 'W';
            else
                latdirection = 'E';
            double lonDegreesWithFraction = lon;//example: 48.858222
            if (lon < 0)
                lonDirection = 'S';
            else
                lonDirection = 'N';//example: =N

            int latDegrees = (int)latDegreesWithFraction; // Converts the degrees to an integer
            int lonDegrees = (int)lonDegreesWithFraction; //example:  = 48

            double latFractionalDegrees = latDegreesWithFraction - latDegrees; // Finds the minutes by finding the fraction within the initial number he received and then multiplying by 60
            double lonFractionalDegrees = lonDegreesWithFraction - lonDegrees; //example:  = .858222

            double latMinutesWithFraction = 60 * latFractionalDegrees; //multiplying the fraction by 60
            double lonMinutesWithFraction = 60 * lonFractionalDegrees; //example:  = 51.49332

            int latMinutes = (int)latMinutesWithFraction; // Converts the minutes to an integer
            int lonMinutes = (int)lonMinutesWithFraction; //example:  = 51

            double latFractionalMinutes = latMinutesWithFraction - latMinutes; // Finds the seconds by finding the fraction within the initial number he received and then multiplying by 60
            double lonFractionalMinutes = lonMinutesWithFraction - lonMinutes; //example:  = .49332

            double latSecondsWithFraction = 60 * latFractionalMinutes; // multiplying the fraction by 60
            double lonSecondsWithFraction = 60 * lonFractionalMinutes; //example:  = 29.6

            float latSeconds = (float)latSecondsWithFraction; // Convert the seconds to a float
            float lonSeconds = (float)lonSecondsWithFraction; //example:  = 30

            latitudeAndLongitude += lonDegrees + "°" + lonMinutes + "’" + lonSeconds + "’’" + lonDirection + "\n" +
                   latDegrees + "°" + latMinutes + "’" + latSeconds + "’’" + latdirection;

            return latitudeAndLongitude;
        }
    }
}