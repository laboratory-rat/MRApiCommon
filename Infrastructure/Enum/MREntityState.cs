using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace MRApiCommon.Infrastructure.Enum
{
    /// <summary>
    /// MR entity state
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum MREntityState
    {
        Active = 0,
        Archived,
        None
    }
}
