using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using WeatherAlerter.Models;
using System.Configuration;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.Generic;

namespace WeatherAlerter
{
    class Program
    {
        //pass in email you wish to send Weather Alert to
        static void Main(string[] args)
        {
            string recipientEmail;
#if DEBUG
            recipientEmail = "xxx@vtext.com";
#endif
            //todo test this
#if !DEBUG
            //this hasnt been tested
            if (args.Length == 0)
            {
                Console.WriteLine("Recepient Email required");
                Console.ReadLine();
                return;
            }
            else
            {
                recipientEmail = args[0];
            }
#endif

            //i did have ot change my email to allow less secure apps and i also click somewhere for gmail to recognize this machine. not sure if both of these are necessary or not.
            try
            {
                //get fiveday forecast, currently does kansas city (can change in app.config, need to check open weather for valid cities)
                //doesn't pull whatever is left of today will need to make second api call for that.
                //second api call won't help, it can only do current so there is no way to know the high for a day in the morning.
                //tempted to add some storage for this adn story min max in the database and can pull it from there. probably easier to find another api.
                //this pulls 40 records no matter what time it is called - 40 records = (24hrs / 3 hrs) * 5 days
                FiveDay forecast = OpenWeatherAPIRequests.GetFiveDayForecast().Result;

                //get the date
                List<DateTime> fiveDays = forecast.list.Select(x => DateTimeOffset.FromUnixTimeSeconds(x.dt).Date).Distinct().OrderBy(y => y.Date).ToList();

                //get the max values for that date
                //currently just sends the 1st day and the forecast for every 3 hours
                // was considering sending the high and lows for the next 5 days
                DateTime day1Date = fiveDays[0];//today. they are ordered, otherwise we could do fiveDays.Min();
                //DateTime day2Date = fiveDays[1];
                //DateTime day3Date = fiveDays[2];
                //DateTime day4Date = fiveDays[3];
                //DateTime day5Date = fiveDays[4];

                var day1 = forecast.list.Where(x => DateTimeOffset.FromUnixTimeSeconds(x.dt).Date == day1Date).ToList();
                //var day2 = forecast.list.Where(x => DateTimeOffset.FromUnixTimeSeconds(x.dt).Date == day2Date).ToList();
                //var day3 = forecast.list.Where(x => DateTimeOffset.FromUnixTimeSeconds(x.dt).Date == day3Date).ToList();
                //var day4 = forecast.list.Where(x => DateTimeOffset.FromUnixTimeSeconds(x.dt).Date == day4Date).ToList();
                //var day5 = forecast.list.Where(x => DateTimeOffset.FromUnixTimeSeconds(x.dt).Date == day5Date).ToList();

                //double day1Max = HelperMethods.KelvinToFahrenheit(day1.Max(x => x.main.temp));
                //double day1Min = HelperMethods.KelvinToFahrenheit(day1.Min(x => x.main.temp));
                //double day2Max = HelperMethods.KelvinToFahrenheit(day2.Max(x => x.main.temp));
                //double day2Min = HelperMethods.KelvinToFahrenheit(day2.Min(x => x.main.temp));
                //double day3Max = HelperMethods.KelvinToFahrenheit(day3.Max(x => x.main.temp));
                //double day3Min = HelperMethods.KelvinToFahrenheit(day3.Min(x => x.main.temp));
                //double day4Max = HelperMethods.KelvinToFahrenheit(day4.Max(x => x.main.temp));
                //double day4Min = HelperMethods.KelvinToFahrenheit(day4.Min(x => x.main.temp));
                //double day5Max = HelperMethods.KelvinToFahrenheit(day5.Max(x => x.main.temp));
                //double day5Min = HelperMethods.KelvinToFahrenheit(day5.Min(x => x.main.temp));

                //https://stackoverflow.com/questions/1202981/select-multiple-fields-from-list-in-linq
                //anonymus types
                string day1title = "Forecast for " + day1Date.Date.ToString("dd MMM yyyy") + "\n";
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

                forecast.list.Where(x => x.main.temp == forecast.list.Max(y => y.main.temp));
                //string body = "Forecast for " + forecast.city + ": " + forecast.list[0].main. 

                //https://home.openweathermap.org/api_keys
                //open360weather
                //https://stackoverflow.com/questions/31246531/can-i-send-sms-messages-from-a-c-sharp-application

                string weatherAlerterName = ConfigurationManager.AppSettings.Get("WeatherAlerterName");
                string weatherAlerterEmail = ConfigurationManager.AppSettings.Get("WeatherAlerterEmail");
                Alerter weatherAlerter = new Alerter();

                weatherAlerter.Recipient = new Recipient
                {
                    Email = recipientEmail
                };

                weatherAlerter.Sender = new Sender
                {
                    Name = ConfigurationManager.AppSettings.Get("WeatherAlerterName"),
                    Email = ConfigurationManager.AppSettings.Get("WeatherAlerterEmail"),
                    Password = ConfigurationManager.AppSettings.Get("WeatherAlerterEmailPassword")
                };

                weatherAlerter.Body = day1title + day1text;

                weatherAlerter.SendAlert();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
    }
}
