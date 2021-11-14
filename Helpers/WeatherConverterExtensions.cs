using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PWSWeatherUploader
{
    public static class WeatherConverterExtensions
    {
        // Uses string extension methods
        // myString.TempCtoF
        public static double TempCtoF(this double tempC)
        {
            // Converts a temperature in Celsius to Fahrenheit
            var tempF = 9.0 / 5.0 * tempC + 32;
            return tempF;
        }

        public static double MmToIn(this double mm)
        {
            // Convert MM to Inches
            return mm / 25.4;
        }

        public static double MbToIn(this double Mb)
        {
            // Convert MB to InHg
            return Mb * 0.02953;
        }

        public static double MsToMph(this double Ms)
        {
            // Convert meters/sec to Miles/per hour
            return Ms * 2.237;
        }

        public static string WindDegToDirectionString(this int deg)
        {
            deg *= 10;

            string[] cardinals = { "N", "NNE", "NE", "ENE", "E", "ESE", "SE", "SSE", "S", "SSW", "SW", "WSW", "W", "WNW", "NW", "NNW", "N" };
            return cardinals[(int)Math.Round(((double)deg % 3600) / 225)];

        }


    }
}

// https://stackoverflow.com/questions/7490660/converting-wind-direction-in-angles-to-text-words #7
//public static string DegreesToCardinal(double degrees)
//{
//    string[] caridnals = { "N", "NE", "E", "SE", "S", "SW", "W", "NW", "N" };
//    return caridnals[(int)Math.Round(((double)degrees % 360) / 45)];
//}

//public static string DegreesToCardinalDetailed(double degrees)
//{
//    degrees *= 10;

//    string[] caridnals = { "N", "NNE", "NE", "ENE", "E", "ESE", "SE", "SSE", "S", "SSW", "SW", "WSW", "W", "WNW", "NW", "NNW", "N" };
//    return caridnals[(int)Math.Round(((double)degrees % 3600) / 225)];
//}