using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PWSWeatherUploader
{
    public static class Extensions
    {
        public static DateTime EpochToDateTimeUtc(this int epoch)
        {
            DateTimeOffset dtOffset = DateTimeOffset.FromUnixTimeSeconds(epoch);
            return dtOffset.UtcDateTime;

        }

    }
}
