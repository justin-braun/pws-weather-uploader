using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Configuration;
using PWSWeatherUploader.Helpers;

namespace PWSWeatherUploader
{   
    public class WeatherProcessor
    {
        private readonly Timer _timer;
        private Helpers.EventLogger eventLogger;

        private int _lastCheckEpoch = 0;
        private readonly double _timerInterval = 1; // minutes

        private readonly WxDataUploader _uploader; 
        private readonly WxDataDownloader _downloader;

        private readonly string PwsStationId = ConfigurationManager.AppSettings["pwsStationId"];
        private readonly string PwsStationUploadPassword = ConfigurationManager.AppSettings["pwsStationUploadPassword"];
        private readonly string WeatherFlowStationId = ConfigurationManager.AppSettings["weatherFlowStationId"];
        private readonly string WeatherFlowApiToken = ConfigurationManager.AppSettings["weatherFlowApiToken"];

        public WeatherProcessor()
        {
            // Setup timer and events
            _timer = new Timer(_timerInterval * 60000) { AutoReset = true };
            _timer.Elapsed += Timer_Elapsed;

            // Instantiate uploader and downloader
            _uploader = new WxDataUploader(PwsStationId, PwsStationUploadPassword);
            _downloader = new WxDataDownloader(WeatherFlowStationId, WeatherFlowApiToken);

            // Get event log ready
            eventLogger = new EventLogger("PWSWeatherUploader");

        }

        public void Start()
        {
            // Start timer
            _timer.Start();
            eventLogger.WriteEvent(System.Diagnostics.EventLogEntryType.Information, 1, "PWSWeatherUploader initialized.");
        }

        public void Stop()
        {
            // Start timer
            _timer.Stop();
            eventLogger.WriteEvent(System.Diagnostics.EventLogEntryType.Information, 1, "PWSWeatherUploader timer stopped.");
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            // Timer Elapsed
            try
            {
                ProcessWeatherObservation();
            }
            catch (Exception ex)
            {
                Logger.LogToScreen(ex.Message);
                eventLogger.WriteEvent(System.Diagnostics.EventLogEntryType.Error, -1, ex.Message);
            }
        }

        private void ProcessWeatherObservation()
        {
            string json;

            // Download json string from API
            try
            {
                json = _downloader.GetCurrentObservation();
            }
            catch (Exception ex)
            {
                Logger.LogToScreen("Error getting current observation download.");
                eventLogger.WriteEvent(System.Diagnostics.EventLogEntryType.Error, -1000, $"Error getting current observation download:{Environment.NewLine}{ex.Message}");
            }

            // Convert it to something useful
            StationObservationModel.StationInfo stationInfo = JsonConvert.DeserializeObject<StationObservationModel.StationInfo>(json);

            // Make sure that the observation is newer than the previous one we stored in var (lastCheckEpoch)
            if (_lastCheckEpoch < stationInfo.Obs[0].Timestamp)
            {
                // Record is newer, so upload
                try
                {
                    //_uploader.UploadToPWSWeather(stationInfo);
                }
                catch (Exception ex)
                {
                    Logger.LogToScreen($"Error uploading the current observation payload.  Error: {ex.Message}");
                    DataLogger.SaveFailedObservation(stationInfo.Obs[0]);
                    eventLogger.WriteEvent(System.Diagnostics.EventLogEntryType.Error, -1001, $"Error uploading the current observation payload:{Environment.NewLine}{ex.Message}");
                }

                // Update with last successful upload that we just uploaded
                Logger.LogToScreen($"Observation successfully uploaded for {stationInfo.Obs[0].Timestamp.EpochToDateTimeUtc().ToLocalTime().ToString()}.");
                eventLogger.WriteEvent(System.Diagnostics.EventLogEntryType.Information, 1001, $"Observation successfully uploaded for {stationInfo.Obs[0].Timestamp.EpochToDateTimeUtc().ToLocalTime().ToString()}.");

                // Save to the settings
                Properties.Settings.Default.LastObsEpoch = stationInfo.Obs[0].Timestamp;
                Properties.Settings.Default.Save();


                // Update variable
                _lastCheckEpoch = Properties.Settings.Default.LastObsEpoch;

            }
            else
            {
                // Record is older from what we can tell, so don't do anything with it.
                Logger.LogToScreen("A newer observation hasn't been received.  Retrying in 60 seconds...");
                eventLogger.WriteEvent(System.Diagnostics.EventLogEntryType.Warning, -1002, "A newer observation hasn't been received.  Retrying in 60 seconds.");
            }
        }
    }
}
