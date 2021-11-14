using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PWSWeatherUploader
{
    public class TempestRapidWindObservation
    {
        public TempestRapidWindObservation(string observation)
        {
            string[] obs = observation.Split(',');

            if (obs.Count() == 3)
            {
                TimeEpoch = int.Parse(obs[0]);
                WindSpeed = Double.Parse(obs[1]);
                WindDirection = int.Parse(obs[2]);

            }
        }

        [Description("Total time in seconds/UNIX time")]
        public int TimeEpoch { get; private set; }

        [Description("Wind speed in m/s")]
        public double WindSpeed { get; private set; }

        [Description("Wind direction in degrees")]
        public int WindDirection { get; private set; }
    }
}
