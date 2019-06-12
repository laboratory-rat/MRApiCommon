namespace MRApiCommon.Infrastructure.Model.ApiResponse
{
    /// <summary>
    /// Api Success / Unsuccess response
    /// </summary>
    public class OkApiResponse
    {
        /// <summary>
        /// Request result
        /// </summary>
        public bool Result { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="result">Response result [false]</param>
        public OkApiResponse(bool result = true)
        {
            Result = result;
        }

        /// <summary>
        /// From bool?
        /// </summary>
        /// <param name="result"></param>
        public static implicit operator OkApiResponse(bool? result)
            => new OkApiResponse(result ?? false);
    }
}
