using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PWSWeatherUploader
{
    // StationInfo myDeserializedClass = JsonConvert.DeserializeObject<StationInfo>(myJsonResponse); 
    public class StationObservationModel
    {
        public class Status
        {
            [JsonProperty("status_code")]
            public int StatusCode { get; set; }

            [JsonProperty("status_message")]
            public string StatusMessage { get; set; }
        }

        public class StationUnits
        {
            [JsonProperty("units_temp")]
            public string UnitsTemp { get; set; }

            [JsonProperty("units_wind")]
            public string UnitsWind { get; set; }

            [JsonProperty("units_precip")]
            public string UnitsPrecip { get; set; }

            [JsonProperty("units_pressure")]
            public string UnitsPressure { get; set; }

            [JsonProperty("units_distance")]
            public string UnitsDistance { get; set; }

            [JsonProperty("units_direction")]
            public string UnitsDirection { get; set; }

            [JsonProperty("units_other")]
            public string UnitsOther { get; set; }
        }

        public class Ob
        {
            [JsonProperty("timestamp")]
            public int Timestamp { get; set; }

            [JsonProperty("air_temperature")]
            public double AirTemperature { get; set; }

            [JsonProperty("barometric_pressure")]
            public double BarometricPressure { get; set; }

            [JsonProperty("station_pressure")]
            public double StationPressure { get; set; }

            [JsonProperty("sea_level_pressure")]
            public double SeaLevelPressure { get; set; }

            [JsonProperty("relative_humidity")]
            public int RelativeHumidity { get; set; }

            [JsonProperty("precip")]
            public double Precip { get; set; }

            [JsonProperty("precip_accum_last_1hr")]
            public double PrecipAccumLast1hr { get; set; }

            [JsonProperty("precip_accum_local_day")]
            public double PrecipAccumLocalDay { get; set; }

            [JsonProperty("precip_minutes_local_day")]
            public int PrecipMinutesLocalDay { get; set; }

            [JsonProperty("wind_avg")]
            public double WindAvg { get; set; }

            [JsonProperty("wind_direction")]
            public int WindDirection { get; set; }

            [JsonProperty("wind_gust")]
            public double WindGust { get; set; }

            [JsonProperty("wind_lull")]
            public double WindLull { get; set; }

            [JsonProperty("solar_radiation")]
            public int SolarRadiation { get; set; }

            [JsonProperty("uv")]
            public double Uv { get; set; }

            [JsonProperty("brightness")]
            public int Brightness { get; set; }

            [JsonProperty("lightning_strike_count")]
            public int LightningStrikeCount { get; set; }

            [JsonProperty("lightning_strike_count_last_1hr")]
            public int LightningStrikeCountLast1hr { get; set; }

            [JsonProperty("lightning_strike_count_last_3hr")]
            public int LightningStrikeCountLast3hr { get; set; }

            [JsonProperty("feels_like")]
            public double FeelsLike { get; set; }

            [JsonProperty("heat_index")]
            public double HeatIndex { get; set; }

            [JsonProperty("wind_chill")]
            public double WindChill { get; set; }

            [JsonProperty("dew_point")]
            public double DewPoint { get; set; }

            [JsonProperty("wet_bulb_temperature")]
            public double WetBulbTemperature { get; set; }

            [JsonProperty("delta_t")]
            public double DeltaT { get; set; }

            [JsonProperty("air_density")]
            public double AirDensity { get; set; }

            [JsonProperty("pressure_trend")]
            public string PressureTrend { get; set; }
        }

        public class StationInfo
        {
            [JsonProperty("station_id")]
            public int StationId { get; set; }

            [JsonProperty("station_name")]
            public string StationName { get; set; }

            [JsonProperty("public_name")]
            public string PublicName { get; set; }

            [JsonProperty("latitude")]
            public double Latitude { get; set; }

            [JsonProperty("longitude")]
            public double Longitude { get; set; }

            [JsonProperty("timezone")]
            public string Timezone { get; set; }

            [JsonProperty("elevation")]
            public double Elevation { get; set; }

            [JsonProperty("is_public")]
            public bool IsPublic { get; set; }

            [JsonProperty("status")]
            public Status Status { get; set; }

            [JsonProperty("station_units")]
            public StationUnits StationUnits { get; set; }

            [JsonProperty("outdoor_keys")]
            public List<string> OutdoorKeys { get; set; }

            [JsonProperty("obs")]
            public List<Ob> Obs { get; set; }
        }


    }
}
