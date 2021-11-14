using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PWSWeatherUploader
{
    public class TempestObservation
    {
        public TempestObservation(string observation)
        {
            string[] obs = observation.Split(',');

            if (obs.Count() == 22)
            {
                TimeEpoch = int.Parse(obs[0]);
                WindLull = Double.Parse(obs[1]);
                WindAvg = Double.Parse(obs[2]);
                WindGust = Double.Parse(obs[3]);
                WindDirection = int.Parse(obs[4]);
                WindSampleInterval = int.Parse(obs[5]);
                StationPressure = Double.Parse(obs[6]);
                AirTemperature = Double.Parse(obs[7]);
                RelativeHumidity = int.Parse(obs[8]);
                Illuminance = int.Parse(obs[9]);
                UV = Double.Parse(obs[10]);
                SolarRadiation = Double.Parse(obs[11]);
                RainAccumulated = Double.Parse(obs[12]);

                PrecipitationTypes PrecipitationTypeResult = PrecipitationTypes.Unknown;
                if(Enum.TryParse<PrecipitationTypes>(obs[13], out PrecipitationTypeResult))
                {
                    PrecipitationType = PrecipitationTypeResult;
                }
                else
                {
                    PrecipitationType = PrecipitationTypes.Unknown;
                }

                //PrecipitationType = (PrecipitationTypes)Enum.Parse(typeof(PrecipitationTypes), obs[13]);


                LightningStrikeAvgDistance = Double.Parse(obs[14]);
                LightningStrikeCount = int.Parse(obs[15]);
                Battery = Double.Parse(obs[16]);
                ReportInterval = int.Parse(obs[17]);
                LocalDailyRainAccumulation = Double.Parse(obs[18]);

                double? _rainCheckAccumulatedFinal;
                if (Double.TryParse(obs[19], out double result))
                {
                    _rainCheckAccumulatedFinal = result;
                }
                else
                {
                    _rainCheckAccumulatedFinal = null;
                }
                RainCheckAccumulatedFinal = _rainCheckAccumulatedFinal;

                double? _rainCheckDailyAccumulatedFinal;
                if (Double.TryParse(obs[20], out double result2))
                {
                    _rainCheckDailyAccumulatedFinal = result2;
                }
                else
                {
                    _rainCheckDailyAccumulatedFinal = null;
                }
                RainCheckDailyAccumulatedFinal = _rainCheckDailyAccumulatedFinal;

                PrecipitationAnalysisTypes PrecipitationAnalysisTypeResult = PrecipitationAnalysisTypes.Unknown;
                if(Enum.TryParse< PrecipitationAnalysisTypes>(obs[21], out PrecipitationAnalysisTypeResult))
                {
                    PrecipitationAnalysisType = PrecipitationAnalysisTypeResult;
                }
                else
                {
                    PrecipitationAnalysisType = PrecipitationAnalysisTypes.Unknown;
                }
                
                //PrecipitationAnalysisType = (PrecipitationAnalysisTypes)Enum.Parse(typeof(PrecipitationAnalysisTypes), obs[21]);
                IsValid = true;
            }
        }

        public enum PrecipitationTypes
        {
            None = 0,
            Rain = 1,
            Hail = 2,
            Unknown = 99
        }

        public enum PrecipitationAnalysisTypes
        {
            None = 0,
            RainCheckWithUserDisplayOn = 1,
            RainCheckWithUserDisplayOff = 2,
            Unknown = 99
        }

        [Description("Set to true when all observation values are successfully loaded")]
        public bool IsValid { get; private set; } = false;

        [Description("Total time in seconds/UNIX time")]
        public int TimeEpoch { get; private set; }
        
        [Description("Minimum 3 second sample in m/s")]
        public double WindLull{ get; private set; }
        
        [Description("average over report interval in m/s")] 
        public double WindAvg { get; private set; }

        [Description("Maximum 3 second sample in m/s")]
        public double WindGust { get; private set; }

        [Description("Wind direction in degrees")] 
        public int WindDirection { get; private set; }

        [Description("Wind sample interval in seconds")] 
        public int WindSampleInterval { get; private set; }

        [Description("Station pressure in MB")] 
        public double StationPressure { get; private set; }

        [Description("Air temperature in Celsius")] 
        public double AirTemperature { get; private set; }

        [Description("Relative humidity percentage")] 
        public int RelativeHumidity { get; private set; }

        [Description("Illuminance in Lux")] 
        public int Illuminance { get; private set; }

        [Description("UV index reading")] 
        public double UV { get; private set; }

        [Description("Solar radition in W/m^2")] 
        public double SolarRadiation { get; private set; }

        [Description("Rain accumulated in mm")] 
        public double RainAccumulated { get; private set; }
        public PrecipitationTypes PrecipitationType { get; private set; }

        [Description("Lightning strike avg. distance in km")] 
        public double LightningStrikeAvgDistance { get; private set; }

        [Description("Lightning strike count")] 
        public int LightningStrikeCount { get; private set; }

        [Description("Battery reading in volts")] 
        public double Battery { get; private set; }

        [Description("Station report interval in minutes")] 
        public int ReportInterval { get; private set; }

        [Description("Accumulation in mm")] 
        public double LocalDailyRainAccumulation { get; private set; }

        [Description("Accumulation in mm")] 
        public double? RainCheckAccumulatedFinal { get; private set; }
        public double? RainCheckDailyAccumulatedFinal { get; private set; }
        public PrecipitationAnalysisTypes PrecipitationAnalysisType { get; private set; }


    }
}
