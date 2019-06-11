namespace MRApiCommon.Infrastructure.Model.ApiResponse
{
    public class OkApiResponse
    {
        public bool Result { get; set; }

        public OkApiResponse(bool result = true)
        {
            Result = result;
        }

        public static implicit operator OkApiResponse(bool? result)
        {
            return new OkApiResponse(result ?? false);
        }
    }
}
