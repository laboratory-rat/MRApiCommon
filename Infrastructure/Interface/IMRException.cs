using System;
using System.Collections.Generic;

namespace MRApiCommon.Infrastructure.Interface
{
    /// <summary>
    /// Common MR Exception interface
    /// </summary>
    public interface IMRException
    {
        /// <summary>
        /// Code of error
        /// </summary>
        int Code { get; set; }

        /// <summary>
        /// Code desciprion (addition to message)
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Exception created time
        /// </summary>
        DateTime CreatedTime { get; set; }

        /// <summary>
        /// Transform exception to Api Response
        /// </summary>
        /// <returns></returns>
        Dictionary<string, dynamic> ToDictionaryShort();


        /// <summary>
        /// For logs
        /// </summary>
        /// <returns></returns>
        Dictionary<string, dynamic> ToDictionary();

    }
}
