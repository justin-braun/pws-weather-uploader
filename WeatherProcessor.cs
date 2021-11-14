using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Configuration;

namespace PWSWeatherUploader
{   
    public class WeatherProcessor
    {
        private readonly Timer _timer;
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
            _timer = new Timer(_timerInterval * 60000) { AutoReset = true };
            _timer.Elapsed += Timer_Elapsed;

            _uploader = new WxDataUploader(PwsStationId, PwsStationUploadPassword);
            _downloader = new WxDataDownloader(WeatherFlowStationId, WeatherFlowApiToken);

            _timer.Start();
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
            catch (Exception)
            {
                Logger.LogToScreen("Error getting current observation download.");
                throw;
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
                    //throw;
                }

                // Update with last successful upload that we just uploaded
                Logger.LogToScreen($"Observation successfully uploaded for {stationInfo.Obs[0].Timestamp.EpochToDateTimeUtc().ToLocalTime().ToString()}.");

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
            }
        }
    }
}
