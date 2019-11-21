using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeatherAlerter.Models
{
    class FiveDayForecast
    {
        public double Day1Max { get; set; }
        public double Day2Max { get; set; }
        public double Day3Max { get; set; }
        public double Day4Max { get; set; }
        public double Day5Max { get; set; }
        public double Day1Min { get; set; }
        public double Day2Min { get; set; }
        public double Day3Min { get; set; }
        public double Day4Min { get; set; }
        public double Day5Min { get; set; }
        public DateTime Day1Date { get; set; }
        public DateTime Day2Date { get; set; }
        public DateTime Day3Date { get; set; }
        public DateTime Day4Date { get; set; }
        public DateTime Day5Date { get; set; }
        public string Day1Desc { get; set; }
        public string Day2Desc { get; set; }
        public string Day3Desc { get; set; }
        public string Day4Desc { get; set; }
        public string Day5Desc { get; set; }
        public string ForecastString { get; set; }

        public void GetForecast()
        {
            FiveDay forecast = OpenWeatherAPIRequests.GetFiveDayForecast().Result;

            List<DateTime> fiveDays = forecast.list.Select(x => DateTimeOffset.FromUnixTimeSeconds(x.dt).Date).Distinct().OrderBy(y => y.Date).ToList();

            //get the max values for that date
            Day1Date = fiveDays[0];//today. - they are ordered, otherwise we could do fiveDays.Min();
            Day2Date = fiveDays[1];
            Day3Date = fiveDays[2];
            Day4Date = fiveDays[3];
            Day5Date = fiveDays[4];

            var day1 = forecast.list.Where(x => DateTimeOffset.FromUnixTimeSeconds(x.dt).Date == Day1Date).ToList();
            var day2 = forecast.list.Where(x => DateTimeOffset.FromUnixTimeSeconds(x.dt).Date == Day2Date).ToList();
            var day3 = forecast.list.Where(x => DateTimeOffset.FromUnixTimeSeconds(x.dt).Date == Day3Date).ToList();
            var day4 = forecast.list.Where(x => DateTimeOffset.FromUnixTimeSeconds(x.dt).Date == Day4Date).ToList();
            var day5 = forecast.list.Where(x => DateTimeOffset.FromUnixTimeSeconds(x.dt).Date == Day5Date).ToList();

            Day1Max = HelperMethods.KelvinToFahrenheit(day1.Max(x => x.main.temp));
            Day1Min = HelperMethods.KelvinToFahrenheit(day1.Min(x => x.main.temp));
            Day2Max = HelperMethods.KelvinToFahrenheit(day2.Max(x => x.main.temp));
            Day2Min = HelperMethods.KelvinToFahrenheit(day2.Min(x => x.main.temp));
            Day3Max = HelperMethods.KelvinToFahrenheit(day3.Max(x => x.main.temp));
            Day3Min = HelperMethods.KelvinToFahrenheit(day3.Min(x => x.main.temp));
            Day4Max = HelperMethods.KelvinToFahrenheit(day4.Max(x => x.main.temp));
            Day4Min = HelperMethods.KelvinToFahrenheit(day4.Min(x => x.main.temp));
            Day5Max = HelperMethods.KelvinToFahrenheit(day5.Max(x => x.main.temp));
            Day5Min = HelperMethods.KelvinToFahrenheit(day5.Min(x => x.main.temp));

            //https://stackoverflow.com/questions/1202981/select-multiple-fields-from-list-in-linq
            //anonymus types
            string day1title = "Forecast for " + Day1Date.Date.ToString("dd MMM yyyy") + "\n";
            string day1text = "";

            foreach (List threehourperiod in day1)
            {
                day1text += "\n" + Convert.ToDateTime(threehourperiod.dt_txt).ToString("HH:mm"); ; //dt.ToString("HH:mm"); // 15:14
                day1text += "\n" + HelperMethods.KelvinToFahrenheit(threehourperiod.main.temp) + " F"; //todo trim this
                foreach (Weather w in threehourperiod.weather)
                {
                    day1text += "\n" + w.description;
                }
                day1text += "\n";
            }

        }
    }
}
