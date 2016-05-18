using System;
namespace JwtCore
{
    public static class JwtCoreExtentions
    {
        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// Convert from .NET DateTime to UnixTimeStamp.
        /// (FYI in NET4.6 -> https://msdn.microsoft.com/en-us/library/system.datetimeoffset.tounixtimeseconds.aspx)
        /// </summary>
        /// <param name="dateTimeUtc">DateTimeUtc</param>
        /// <returns>UnixTimeStamp</returns>
        public static long ToUnixTimeSeconds(this DateTime dateTimeUtc)
        {
            return (long)Math.Round((dateTimeUtc.ToUniversalTime() - UnixEpoch).TotalSeconds);
        }

        /// <summary>
        /// Convert from UnixTimeStamp to .NET DateTime.
        /// (FYI in NET4.6 -> https://msdn.microsoft.com/en-us/library/system.datetimeoffset.tounixtimeseconds.aspx)
        /// </summary>
        /// <param name="unixTimeStamp">UnixTimeStamp</param>
        /// <returns>DateTime by Utc</returns>
        public static DateTime ToDateTiemUtc(this long unixTimeStamp)
        {
            return UnixEpoch.AddSeconds(unixTimeStamp);
        }
    }
}
