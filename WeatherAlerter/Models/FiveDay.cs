using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherAlerter.Models
{
    //todo this model is api specific, we need a mapping to a model containing the weather information i am interested in.
    public class Main
    {
        /// <summary>
        /// Temperature. Unit Default: Kelvin, Metric: Celsius, Imperial: Fahrenheit.
        /// </summary>
        public double temp { get; set; }

        /// <summary>
        /// Minimum temperature at the moment of calculation. 
        /// This is deviation from 'temp' that is possible for large cities 
        /// and megalopolises geographically expanded (use these parameter 
        /// optionally). Unit Default: Kelvin, Metric: Celsius, Imperial: Fahrenheit.
        /// </summary>
        public double temp_min { get; set; }

        /// <summary>
        /// Maximum temperature at the moment of calculation. 
        /// This is deviation from 'temp' that is possible for large cities
        /// and megalopolises geographically expanded (use these parameter 
        /// optionally). Unit Default: Kelvin, Metric: Celsius, Imperial: Fahrenheit.
        /// </summary>
        public double temp_max { get; set; }

        /// <summary>
        /// Atmospheric pressure on the sea level by default, hPa
        /// </summary>
        public double pressure { get; set; }

        /// <summary>
        /// Atmospheric pressure on the sea level, hPa
        /// </summary>
        public double sea_level { get; set; }

        /// <summary>
        /// Atmospheric pressure on the ground level, hPa
        /// </summary>
        public double grnd_level { get; set; }

        /// <summary>
        /// Humidity, %
        /// </summary>
        public int humidity { get; set; }

        /// <summary>
        /// Internal parameter
        /// </summary>
        public double temp_kf { get; set; }
    }

    /// <summary>
    /// (more info Weather condition codes)
    /// </summary>
    public class Weather
    {
        /// <summary>
        /// Weather condition id
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// Group of weather parameters (Rain, Snow, Extreme etc.)
        /// </summary>
        public string main { get; set; }

        /// <summary>
        /// Weather condition within the group
        /// </summary>
        public string description { get; set; }

        /// <summary>
        /// Weather icon id
        /// </summary>
        public string icon { get; set; }
    }

    public class Clouds
    {
        /// <summary>
        /// Cloudiness, %
        /// </summary>
        public int all { get; set; }
    }

    public class Wind
    {
        /// <summary>
        /// Wind speed. Unit Default: meter/sec, Metric: meter/sec, Imperial: miles/hour.
        /// </summary>
        public double speed { get; set; }

        /// <summary>
        /// Wind direction, degrees (meteorological)
        /// </summary>
        public double deg { get; set; }
    }

    public class Rain
    {
        /// <summary>
        /// Rain volume for last 3 hours, mm
        /// name from api 3h
        /// </summary>
        public double __invalid_name__3h { get; set; }
    }

    public class Sys
    {
        public string pod { get; set; }
    }

    public class List
    {
        /// <summary>
        /// Time of data forecasted, unix, UTC
        /// </summary>
        public int dt { get; set; }

        public Main main { get; set; }

        //not sure why weather is a list, i guess you can have different types over a 3 hour period, though in practice i have only seen this populated with one.
        public List<Weather> weather { get; set; }

        public Clouds clouds { get; set; }

        public Wind wind { get; set; }

        public Rain rain { get; set; }

        public Sys sys { get; set; }

        /// <summary>
        /// Data/time of calculation, UTC
        /// </summary>
        public string dt_txt { get; set; }
    }

    public class Coord
    {
        /// <summary>
        /// City geo location, latitude
        /// </summary>
        public double lat { get; set; }

        /// <summary>
        /// City geo location, longitude
        /// </summary>
        public double lon { get; set; } 
    }

    public class City
    {
        public int id { get; set; }

        public string name { get; set; }

        public Coord coord { get; set; }

        /// <summary>
        ///Country code (GB, JP etc.)
        ///</summary>
        public string country { get; set; } 

        public int population { get; set; }

        /// <summary>
        /// Shift in seconds from UTC
        /// </summary>
        public int timezone { get; set; } 

        public int sunrise { get; set; }

        public int sunset { get; set; }

    }

    /// <summary>
    /// Five Day Forecast
    /// </summary>
    public class FiveDay
    {
        /// <summary>
        /// interal
        /// </summary>
        public string cod { get; set; } 

        /// <summary>
        /// internal
        /// </summary>
        public double message { get; set; }

        /// <summary>
        /// Number of lines returned by this API call
        /// </summary>
        public int cnt { get; set; }

        public List<List> list { get; set; }

        public City city { get; set; }
    }
}
