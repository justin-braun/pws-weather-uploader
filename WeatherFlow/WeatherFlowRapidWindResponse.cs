using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PWSWeatherUploader
{
    public class WeatherFlowRapidWindResponse
    {
        public class Root
        {
            [JsonProperty("device_id")]
            public int DeviceId;

            [JsonProperty("serial_number")]
            public string SerialNumber;

            [JsonProperty("type")]
            public string Type;

            [JsonProperty("hub_sn")]
            public string HubSn;

            [JsonProperty("ob")]
            public List<double> Ob;
        }

    }
}
