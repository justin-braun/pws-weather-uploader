using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace PWSWeatherUploader
{
    public class WxDataUploader
    {
        private readonly string PWSWeatherUploadUrl = @"https://pwsupdate.pwsweather.com/api/v1/submitwx";
        private static readonly HttpClient _client = new HttpClient();
        private string _id = "";
        private string _password = "";

        public WxDataUploader(string stationId, string uploadPassword)
        {
            // setup id and password variables
            _id = stationId;
            _password = uploadPassword;

        }

        public async Task UploadToPWSWeatherAsync(StationObservationModel.StationInfo station)
        {
            // Check for observation data in the download
            if (station.Obs.Count == 0)
            {
                // There is no observations to work with, so throw an exception
                throw new Exception("No observation data found.");
            }

            // There is an observation, so prepare to upload the data
            StationObservationModel.Ob observation = station.Obs[0];

            var readings = new Dictionary<string, string>
            {
                { "ID", _id },
                { "PASSWORD", _password },
                { "dateutc", observation.Timestamp.EpochToDateTimeUtc().ToString("yyyy-MM-dd HH:mm:ss") },
                { "winddir", observation.WindDirection.ToString() },
                { "windspeedmph", observation.WindAvg.MsToMph().ToString("0.0") },
                { "windgustmph", observation.WindGust.MsToMph().ToString("0.0") },
                { "tempf", observation.AirTemperature.TempCtoF().ToString("0.0") },
                { "rainin", observation.Precip.MmToIn().ToString("0.00") },
                { "dailyrainin", observation.PrecipAccumLocalDay.MmToIn().ToString("0.00") },
                { "baromin", observation.BarometricPressure.MbToIn().ToString("0.00") },
                { "dewptf", observation.DewPoint.TempCtoF().ToString("0.0") },
                { "humidity", observation.RelativeHumidity.ToString() },
                { "solarradiation", observation.SolarRadiation.ToString() },
                { "UV", observation.Uv.ToString("0.0") },
                { "softwaretype", "PWSWeatherUploaderForWeatherFlow1.0" },
                { "action", "updateraw" },
            };

            // Use for debug output of dictionary
            //var asString = string.Join(Environment.NewLine, readings);
            //Console.WriteLine(asString);

            // Turn dictionary into url encoded content
            var content = new FormUrlEncodedContent(readings);


            using (var client = new HttpClient())
            {
                // Set client timeout to 30 sec.
                client.Timeout = TimeSpan.FromSeconds(5);

                // Send post request async to API with payload
                var response = await client.PostAsync(PWSWeatherUploadUrl, content);

                // Receive response async 
                var responseString = await response.Content.ReadAsStringAsync();

                // DEBUG: Console.WriteLine(responseString);

                // Check json response for errors
                dynamic jsonResponse = JObject.Parse(responseString);

                // For debugging: dynamic jsonResponse = JObject.Parse("{ \"error\":null,\"success\":true}");
                bool jsonResponseSuccess = jsonResponse.success;
                string jsonResponseErrorMessage = jsonResponse.error;

                //if (jsonResponseSuccess == true)
                //{
                //    Console.WriteLine($"Observation successfully uploaded for {observation.Timestamp.EpochToDateTimeUtc().ToLocalTime().ToString()}.");
                //}
                //else
                //{
                //    Console.WriteLine("Error:");
                //    Console.WriteLine(jsonResponseErrorMessage);
                //} 

                if (jsonResponseSuccess != true)
                {
                    throw new Exception($"An error was received in the JSON response: {jsonResponseErrorMessage}");
                }

            }

        }

        public void UploadToPWSWeather(StationObservationModel.StationInfo station)
        {
            // Check for observation data in the download
            if (station.Obs.Count == 0)
            {
                // There is no observations to work with, so throw an exception
                throw new Exception("No observation data found.");
            }

            // There is an observation, so prepare to upload the data
            StationObservationModel.Ob observation = station.Obs[0];

            var readings = new Dictionary<string, string>
            {
                { "ID", _id },
                { "PASSWORD", _password },
                { "dateutc", observation.Timestamp.EpochToDateTimeUtc().ToString("yyyy-MM-dd HH:mm:ss") },
                { "winddir", observation.WindDirection.ToString() },
                { "windspeedmph", observation.WindAvg.MsToMph().ToString("0.0") },
                { "windgustmph", observation.WindGust.MsToMph().ToString("0.0") },
                { "tempf", observation.AirTemperature.TempCtoF().ToString("0.0") },
                { "rainin", observation.Precip.MmToIn().ToString("0.00") },
                { "dailyrainin", observation.PrecipAccumLocalDay.MmToIn().ToString("0.00") },
                { "baromin", observation.BarometricPressure.MbToIn().ToString("0.00") },
                { "dewptf", observation.DewPoint.TempCtoF().ToString("0.0") },
                { "humidity", observation.RelativeHumidity.ToString() },
                { "solarradiation", observation.SolarRadiation.ToString() },
                { "UV", observation.Uv.ToString("0.0") },
                { "softwaretype", "PWSWeatherUploaderForWeatherFlow1.0" },
                { "action", "updateraw" },
            };

            // DEBUG: Use for debug output of dictionary
            //var asString = string.Join(Environment.NewLine, readings);
            //Console.WriteLine(asString);

            // Turn dictionary into url encoded content
            var content = new FormUrlEncodedContent(readings);


            using (var client = new HttpClient())
            {
                // Set client timeout to 30 sec.
                client.Timeout = TimeSpan.FromSeconds(30);

                // Send post request async to API with payload synchronously
                var response = client.PostAsync(PWSWeatherUploadUrl, content).Result;
                
                if(response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content;

                    // Receive response async 
                    var responseString = responseContent.ReadAsStringAsync().Result;

                    // DEBUG: Console.WriteLine(responseString);

                    // Check json response for errors
                    dynamic jsonResponse = JObject.Parse(responseString);

                    // For debugging: dynamic jsonResponse = JObject.Parse("{ \"error\":null,\"success\":true}");
                    bool jsonResponseSuccess = jsonResponse.success;
                    string jsonResponseErrorMessage = jsonResponse.error;

                    //if (jsonResponseSuccess == true)
                    //{
                    //    Console.WriteLine($"Observation successfully uploaded for {observation.Timestamp.EpochToDateTimeUtc().ToLocalTime().ToString()}.");
                    //}
                    //else
                    //{
                    //    Console.WriteLine("Error:");
                    //    Console.WriteLine(jsonResponseErrorMessage);
                    //}

                    if (jsonResponseSuccess != true)
                    {
                        throw new Exception($"An error was received in the JSON response: {jsonResponseErrorMessage}");
                    }

                }
                else
                {
                    // Console.WriteLine($"Observation upload failed with a non-success response code. ({response.StatusCode.ToString()})");
                    throw new Exception($"Non-success response code received on upload. ({response.StatusCode.ToString()})");
                }
            }

        }
    }
}
