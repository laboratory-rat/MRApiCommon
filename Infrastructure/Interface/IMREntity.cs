using MRApiCommon.Infrastructure.Enum;
using System;

namespace MRApiCommon.Infrastructure.Interface
{
    /// <summary>
    /// Basic entity type
    /// </summary>
    /// <typeparam name="T">Type for id</typeparam>
    public interface IMREntity<T>
    {
        /// <summary>
        /// Typed id
        /// </summary>
        T Id { get; set; }

        /// <summary>
        /// Entity created time
        /// </summary>
        DateTime CreateTime { get; set; }

        /// <summary>
        /// Entity updated time
        /// </summary>
        DateTime? UpdateTime { get; set; }

        /// <summary>
        /// Entity current state
        /// [Active / Deleted]
        /// </summary>
        MREntityState State { get; set; }

        /// <summary>
        /// Generate new key for database
        /// </summary>
        void GenerateKey();
    }

    /// <summary>
    /// String realization of IMREntity
    /// </summary>
    public interface IMREntity : IMREntity<string> { }
}
