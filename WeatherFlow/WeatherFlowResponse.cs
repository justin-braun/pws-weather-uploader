using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PWSWeatherUploader
{
    public class WeatherFlowResponse
    {
        
        public class Status
        {
            [JsonProperty("status_code")]
            public int StatusCode;

            [JsonProperty("status_message")]
            public string StatusMessage;
        }

        public class Summary
        {
            [JsonProperty("pressure_trend")]
            public string PressureTrend;

            [JsonProperty("strike_count_1h")]
            public int StrikeCount1h;

            [JsonProperty("strike_count_3h")]
            public int StrikeCount3h;

            [JsonProperty("precip_total_1h")]
            public double PrecipTotal1h;

            [JsonProperty("strike_last_dist")]
            public int StrikeLastDist;

            [JsonProperty("strike_last_epoch")]
            public int StrikeLastEpoch;

            [JsonProperty("precip_accum_local_yesterday")]
            public double PrecipAccumLocalYesterday;

            [JsonProperty("precip_accum_local_yesterday_final")]
            public double PrecipAccumLocalYesterdayFinal;

            [JsonProperty("precip_analysis_type_yesterday")]
            public int PrecipAnalysisTypeYesterday;

            [JsonProperty("feels_like")]
            public double FeelsLike;

            [JsonProperty("heat_index")]
            public double HeatIndex;

            [JsonProperty("wind_chill")]
            public double WindChill;

            [JsonProperty("raining_minutes")]
            public List<int> RainingMinutes;

            [JsonProperty("dew_point")]
            public double DewPoint;

            [JsonProperty("wet_bulb_temperature")]
            public double WetBulbTemperature;

            [JsonProperty("air_density")]
            public double AirDensity;

            [JsonProperty("delta_t")]
            public double DeltaT;

            [JsonProperty("precip_minutes_local_day")]
            public int PrecipMinutesLocalDay;

            [JsonProperty("precip_minutes_local_yesterday")]
            public int PrecipMinutesLocalYesterday;
        }

        public class Root
        {
            [JsonProperty("type")]
            public string Type;

            [JsonProperty("id")]
            public string Id;

            [JsonProperty("status")]
            public Status Status;

            [JsonProperty("device_id")]
            public int DeviceId;

            [JsonProperty("source")]
            public string Source;

            [JsonProperty("summary")]
            public Summary Summary;

            [JsonProperty("obs")]
            public List<List<string>> Obs;
        }


    }
}
