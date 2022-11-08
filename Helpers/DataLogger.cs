using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PWSWeatherUploader
{
    public static class DataLogger
    {

        private const string MISSED_OBS_FILE = "missed_observations.csv";

        // Naming for log files
        //private static string MissedObsLogFileName
        //{
        //    get { return $"missed_observations_{DateTime.Now.Year.ToString()}{DateTime.Now.Month.ToString("d2")}{DateTime.Now.Day.ToString("d2")}.csv"; ; }
        //}
        private static string windLogFileName
        {
            get { return $"wind_{DateTime.Now.Year.ToString()}{DateTime.Now.Month.ToString("d2")}{DateTime.Now.Day.ToString("d2")}.csv"; ; }
        }

        // Save failed observation so we can play it back later.
        public static void SaveFailedObservation(StationObservationModel.Ob observation)
        {
            // If log file doesn't exist, create it and write the header row
            if (!File.Exists(MISSED_OBS_FILE))
            {
                PrepareMissedObsLogFile();
            }

            string[] obs = new string[]
            {

                observation.Timestamp.EpochToDateTimeUtc().ToString("yyyy-MM-dd HH:mm:ss"),
                observation.WindDirection.ToString(),
                observation.WindAvg.MsToMph().ToString("0.0"),
                observation.WindGust.MsToMph().ToString("0.0"),
                observation.AirTemperature.TempCtoF().ToString("0.0"),
                observation.Precip.MmToIn().ToString("0.00"),
                observation.PrecipAccumLocalDay.MmToIn().ToString("0.00"),
                observation.BarometricPressure.MbToIn().ToString("0.00"),
                observation.DewPoint.TempCtoF().ToString("0.0"),
                observation.RelativeHumidity.ToString(),
                observation.SolarRadiation.ToString(),
                observation.Uv.ToString("0.0")
            };

            // Write observation to file
            File.AppendAllText(MISSED_OBS_FILE, String.Join(",", obs) + Environment.NewLine);

        }
        private static void PrepareMissedObsLogFile()
        {
            string[] headerRow = new string[] { 
                "dateutc", 
                "winddir", 
                "windspeedmph", 
                "windgustmph", 
                "tempf", 
                "rainin", 
                "dailyrainin", 
                "baromin", 
                "dewptf", 
                "humidity", 
                "solarradiation", 
                "uv" 
            };

            // Create/Append to log file
            File.AppendAllText(MISSED_OBS_FILE, String.Join(",", headerRow) + Environment.NewLine);

        }

        // Used with Web Sockets if we wanted to save specifics on wind readings
        private static void PrepareWindLogFile()
        {
            string[] headerRow = new string[] {
                "epoch",
                "dateutc",
                "datelocal",
                "winddir",
                "windspeedmph",
                "windspeedms",
            };

            // Create/Append to log file
            File.AppendAllText(windLogFileName, String.Join(",", headerRow) + Environment.NewLine);

        }


        // Used with Web Sockets if we wanted to save specifics on wind readings
        public static void SaveWindReading(string observation)
        {
            // If log file doesn't exist, create it and write the header row
            if (!File.Exists(windLogFileName))
            {
                PrepareWindLogFile();
            }

            TempestRapidWindObservation obs = new TempestRapidWindObservation(observation);

            string[] windInfo = new string[]
            {
                obs.TimeEpoch.ToString(),
                obs.TimeEpoch.EpochToDateTimeUtc().ToString("yyyy-MM-dd HH:mm:ss"),
                obs.TimeEpoch.EpochToDateTimeUtc().ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss"),
                obs.WindDirection.ToString(),
                obs.WindSpeed.MsToMph().ToString("0.0"),
                obs.WindSpeed.ToString()
            };

            // Write observation to file
            File.AppendAllText(windLogFileName, String.Join(",", windInfo) + Environment.NewLine);

        }
    }
}
