using System;

namespace MRApiCommon.Extensions
{
    /// <summary>
    /// DateTime extensions
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// From DateTime to Timestamp
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static long ToUnixTimestamp(this DateTime source)
            => ((DateTimeOffset)source).ToUnixTimeSeconds();

        /// <summary>
        /// From timestamp to DateTime
        /// </summary>
        /// <param name="source"></param>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static DateTime FromUnixTimestamp(this DateTime source, long timestamp)
        {
            source = DateTimeOffset.FromUnixTimeSeconds(timestamp).DateTime;
            return source;
        }
    }
}
