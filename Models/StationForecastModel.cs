using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PWSWeatherUploader
{
    // StationForecastInfo myDeserializedClass = JsonConvert.DeserializeObject<StationForecastInfo>(myJsonResponse); 
    public class StationForecastModel
    {
        public class CurrentConditions
        {
            [JsonProperty("time")]
            public int Time { get; set; }

            [JsonProperty("conditions")]
            public string Conditions { get; set; }

            [JsonProperty("icon")]
            public string Icon { get; set; }

            [JsonProperty("air_temperature")]
            public int AirTemperature { get; set; }

            [JsonProperty("sea_level_pressure")]
            public double SeaLevelPressure { get; set; }

            [JsonProperty("station_pressure")]
            public double StationPressure { get; set; }

            [JsonProperty("pressure_trend")]
            public string PressureTrend { get; set; }

            [JsonProperty("relative_humidity")]
            public int RelativeHumidity { get; set; }

            [JsonProperty("wind_avg")]
            public int WindAvg { get; set; }

            [JsonProperty("wind_direction")]
            public int WindDirection { get; set; }

            [JsonProperty("wind_direction_cardinal")]
            public string WindDirectionCardinal { get; set; }

            [JsonProperty("wind_gust")]
            public int WindGust { get; set; }

            [JsonProperty("solar_radiation")]
            public int SolarRadiation { get; set; }

            [JsonProperty("uv")]
            public int Uv { get; set; }

            [JsonProperty("brightness")]
            public int Brightness { get; set; }

            [JsonProperty("feels_like")]
            public int FeelsLike { get; set; }

            [JsonProperty("dew_point")]
            public int DewPoint { get; set; }

            [JsonProperty("wet_bulb_temperature")]
            public int WetBulbTemperature { get; set; }

            [JsonProperty("delta_t")]
            public int DeltaT { get; set; }

            [JsonProperty("air_density")]
            public double AirDensity { get; set; }

            [JsonProperty("lightning_strike_count_last_1hr")]
            public int LightningStrikeCountLast1hr { get; set; }

            [JsonProperty("lightning_strike_count_last_3hr")]
            public int LightningStrikeCountLast3hr { get; set; }

            [JsonProperty("precip_accum_local_day")]
            public int PrecipAccumLocalDay { get; set; }

            [JsonProperty("precip_minutes_local_day")]
            public int PrecipMinutesLocalDay { get; set; }

            [JsonProperty("is_precip_local_day_rain_check")]
            public bool IsPrecipLocalDayRainCheck { get; set; }
        }

        public class Daily
        {
            [JsonProperty("day_start_local")]
            public int DayStartLocal { get; set; }

            [JsonProperty("day_num")]
            public int DayNum { get; set; }

            [JsonProperty("month_num")]
            public int MonthNum { get; set; }

            [JsonProperty("conditions")]
            public string Conditions { get; set; }

            [JsonProperty("icon")]
            public string Icon { get; set; }

            [JsonProperty("sunrise")]
            public int Sunrise { get; set; }

            [JsonProperty("sunset")]
            public int Sunset { get; set; }

            [JsonProperty("air_temp_high")]
            public int AirTempHigh { get; set; }

            [JsonProperty("air_temp_low")]
            public int AirTempLow { get; set; }

            [JsonProperty("precip_probability")]
            public int PrecipProbability { get; set; }

            [JsonProperty("precip_icon")]
            public string PrecipIcon { get; set; }

            [JsonProperty("precip_type")]
            public string PrecipType { get; set; }
        }

        public class Hourly
        {
            [JsonProperty("time")]
            public int Time { get; set; }

            [JsonProperty("conditions")]
            public string Conditions { get; set; }

            [JsonProperty("icon")]
            public string Icon { get; set; }

            [JsonProperty("air_temperature")]
            public int AirTemperature { get; set; }

            [JsonProperty("sea_level_pressure")]
            public double SeaLevelPressure { get; set; }

            [JsonProperty("relative_humidity")]
            public int RelativeHumidity { get; set; }

            [JsonProperty("precip")]
            public double Precip { get; set; }

            [JsonProperty("precip_probability")]
            public int PrecipProbability { get; set; }

            [JsonProperty("precip_icon")]
            public string PrecipIcon { get; set; }

            [JsonProperty("wind_avg")]
            public int WindAvg { get; set; }

            [JsonProperty("wind_direction")]
            public int WindDirection { get; set; }

            [JsonProperty("wind_direction_cardinal")]
            public string WindDirectionCardinal { get; set; }

            [JsonProperty("wind_gust")]
            public int WindGust { get; set; }

            [JsonProperty("uv")]
            public int Uv { get; set; }

            [JsonProperty("feels_like")]
            public int FeelsLike { get; set; }

            [JsonProperty("local_hour")]
            public int LocalHour { get; set; }

            [JsonProperty("local_day")]
            public int LocalDay { get; set; }

            [JsonProperty("precip_type")]
            public string PrecipType { get; set; }
        }

        public class Forecast
        {
            [JsonProperty("daily")]
            public List<Daily> Daily { get; set; }

            [JsonProperty("hourly")]
            public List<Hourly> Hourly { get; set; }
        }

        public class Status
        {
            [JsonProperty("status_code")]
            public int StatusCode { get; set; }

            [JsonProperty("status_message")]
            public string StatusMessage { get; set; }
        }

        public class Units
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

            [JsonProperty("units_brightness")]
            public string UnitsBrightness { get; set; }

            [JsonProperty("units_solar_radiation")]
            public string UnitsSolarRadiation { get; set; }

            [JsonProperty("units_other")]
            public string UnitsOther { get; set; }

            [JsonProperty("units_air_density")]
            public string UnitsAirDensity { get; set; }
        }

        public class StationForecastInfo
        {
            [JsonProperty("latitude")]
            public double Latitude { get; set; }

            [JsonProperty("longitude")]
            public double Longitude { get; set; }

            [JsonProperty("timezone")]
            public string Timezone { get; set; }

            [JsonProperty("timezone_offset_minutes")]
            public int TimezoneOffsetMinutes { get; set; }

            [JsonProperty("current_conditions")]
            public CurrentConditions CurrentConditions { get; set; }

            [JsonProperty("forecast")]
            public Forecast Forecast { get; set; }

            [JsonProperty("status")]
            public Status Status { get; set; }

            [JsonProperty("units")]
            public Units Units { get; set; }
        }


    }
}
