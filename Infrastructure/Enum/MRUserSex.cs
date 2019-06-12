using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MRApiCommon.Infrastructure.Enum
{
    /// <summary>
    /// 
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum MRUserSex
    {
        /// <summary>
        /// 
        /// </summary>
        UNDEFINED,

        /// <summary>
        /// 
        /// </summary>
        MALE,

        /// <summary>
        /// 
        /// </summary>
        FEMALE
    }
}
