using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherAlerter
{
    public static class HelperMethods
    {
        public static double KelvinToFahrenheit(double k)
        {
            return Math.Round(((k - 273.15) * 9 / 5 + 32), 0, MidpointRounding.AwayFromZero);
        }

        public static double KelvinToCelcius(double k)
        {
            return (k - 273.15);
        }
    }
}
