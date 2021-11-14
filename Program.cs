using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Configuration;

namespace PWSWeatherUploader
{
    public class Program
    {
        public static WebSocketWeatherFlow ws;

        static void Main(string[] args)
        {
            // Console Title
            Console.Title = $"PWSWeather Uploader {Assembly.GetExecutingAssembly().GetName().Version.ToString()}";

            // Write the last saved observation to the screen
            Console.WriteLine($"Last successful observation saved: {Properties.Settings.Default.LastObsEpoch}");
            Console.WriteLine();

            //Instantiate the Weather Processor and start getting data
            WeatherProcessor wp = new WeatherProcessor();
            wp.Start();

            Console.ReadKey();

        }

    }
}
