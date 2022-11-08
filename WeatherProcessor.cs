using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Configuration;
using System.Net;
using static PWSWeatherUploader.StationObservationModel;

namespace PWSWeatherUploader
{   
    public class WeatherProcessor
    {
        private readonly Timer _timer;
        private static readonly NLog.Logger Logger = NLog.LogManager.GetLogger("*");

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

        }

        public void Start()
        {

            Logger.Info("PWSWeatherUploaderService initialized.");

            // Execute initial run before starting timer
            Timer_Elapsed(null, null);

            // Start timer
            _timer.Start();
            
        }

        public void Stop()
        {
            // Start timer
            _timer.Stop();
            Logger.Info("PWSWeatherUploaderService timer was stopped.");
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            // Timer Elapsed
            try
            {
                ProcessNewObservation();
            }
            catch (Exception ex)
            {
                Logger.WithProperty("EventId", -1000).Error($"Error processing observation:{Environment.NewLine}{ex.Message + Environment.NewLine + Environment.NewLine + ex.StackTrace}");
            }
        }


        // Process:
        // Download JSON observation
        // Convert to StationInfoModel
        // Upload to PWSWeather

        private void ProcessNewObservation()
        {
            try
            {
                string json = "";
                json = _downloader.GetCurrentObservation();

                if(json != "")
                {
                    ConvertObservation(json);
                }
                else
                {
                    Logger.Warn("Observation JSON is empty.");
                    return;
                }
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        private void ConvertObservation(string json)
        {
           
            // Convert it to something useful
            StationObservationModel.StationInfo stationInfo = JsonConvert.DeserializeObject<StationObservationModel.StationInfo>(json);

            if(stationInfo != null && stationInfo.Obs.Count > 0)
            {
                //Observation exists
                // Check if it is newer than the previous recorded observation
                if (Properties.Settings.Default.LastObsEpoch < stationInfo.Obs[0].Timestamp)
                {
                    // Upload new observation
                    UploadObservationToPWS(stationInfo);
                }
                else
                {
                    Logger.Info("Observation is not newer than the previous uploaded observation.");
                }
            }
            else
            {
                Logger.Warn("StationInfo doesn't contain any observations.");
            }

        }

        private void UploadObservationToPWS(StationInfo stationInfo)
        {
            try
            {
                // Upload data
                _uploader.UploadToPWSWeather(stationInfo);

                // Update with last successful upload that we just uploaded
                Logger.WithProperty("EventId", 1001).Info($"Observation successfully uploaded for {stationInfo.Obs[0].Timestamp.EpochToDateTimeUtc().ToLocalTime().ToString()}.");

                // Save settings
                Properties.Settings.Default.LastObsEpoch = stationInfo.Obs[0].Timestamp;
                Properties.Settings.Default.Save();
            }
            catch (Exception e)
            {
                DataLogger.SaveFailedObservation(stationInfo.Obs[0]);
                throw e;
            }

        }

        private void ProcessWeatherObservationOld()
        {
            string json = "";

            // Download json string from API
            try
            {
                json = _downloader.GetCurrentObservation();

            }
            catch (WebException ex)
            {
                throw ex;
            }

            // Convert it to something useful
            StationObservationModel.StationInfo stationInfo = JsonConvert.DeserializeObject<StationObservationModel.StationInfo>(json);

            // Make sure that the observation is newer than the previous one we stored in var (lastCheckEpoch)
            if (stationInfo != null && Properties.Settings.Default.LastObsEpoch < stationInfo.Obs[0].Timestamp)
            {
                // Record is newer, so upload
                try
                {
                    _uploader.UploadToPWSWeather(stationInfo);

                    // Update with last successful upload that we just uploaded
                    Logger.WithProperty("EventId", 1001).Info($"Observation successfully uploaded for {stationInfo.Obs[0].Timestamp.EpochToDateTimeUtc().ToLocalTime().ToString()}.");

                    // Save settings
                    Properties.Settings.Default.LastObsEpoch = stationInfo.Obs[0].Timestamp;
                    Properties.Settings.Default.Save();

                }
                catch (Exception ex)
                {
                    DataLogger.SaveFailedObservation(stationInfo.Obs[0]);
                    Logger.WithProperty("EventId", -1001).Error($"Error uploading the current observation payload:{Environment.NewLine}{ex.Message + Environment.NewLine + Environment.NewLine + ex.StackTrace}");
                }

            }
            else
            {
                // Record is older from what we can tell, so don't do anything with it.
                Logger.WithProperty("EventId", -1002).Warn("A newer observation hasn't been received.  Retrying in 60 seconds.");
            }
        }

    }
}
