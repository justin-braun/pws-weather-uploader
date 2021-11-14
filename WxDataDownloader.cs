using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PWSWeatherUploader
{
    public class WxDataDownloader
    {
        private readonly string stationObservationJsonUrl = "https://swd.weatherflow.com/swd/rest/observations/station/{1}?token={2}";
        
        public WxDataDownloader(string stationId, string apiToken)
        {
            stationObservationJsonUrl = stationObservationJsonUrl.Replace("{1}", stationId).Replace("{2}", apiToken);
        }

        public string GetCurrentObservation()
        {
            // TODO: Add exception handling

            string json = DownloadJson();

            return json;
        }

        private string DownloadJson()
        {
            // TODO: Add exception handling

            WebClient wc = new WebClient();
            string json = wc.DownloadString(stationObservationJsonUrl);

            return json;

        }
    }
}
