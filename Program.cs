using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Configuration;
using Topshelf;

namespace PWSWeatherUploader
{
    public class Program
    {
        public static WebSocketWeatherFlow ws;

        static void Main(string[] args)
        {
            var exitCode = HostFactory.Run(x =>
            {
                x.Service<WeatherProcessor>(s =>
                {
                    s.ConstructUsing(uploadSvc => new WeatherProcessor());
                    s.WhenStarted(uploadSvc => uploadSvc.Start());
                    s.WhenStopped(uploadSvc => uploadSvc.Stop());
                }
                );

                x.RunAsLocalSystem();

                x.SetServiceName("PWSWeatherUploader");
                x.SetDisplayName("PWS Weather Uploader");
                x.SetDescription("This service uploads data from WeatherFlow Tempest weather stations to AerisWeather's PWSWeather service.");
            });

            int exitCodeValue = (int)Convert.ChangeType(exitCode, exitCode.GetTypeCode());
            Environment.ExitCode = exitCodeValue;

        }

    }
}
