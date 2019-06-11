using System;
using System.Collections.Generic;

namespace MRApiCommon.Infrastructure.Model.ApiResponse
{
    /// <summary>
    /// Default Pagination api response model 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PaginationApiResponse<T>
        where T : class, new()
    {
        public int Take { get; set; }
        public int Skip { get; set; }
        public int Total { get; set; }
        public int TotalPages => Take == 0 ? 0 : (int)(Math.Ceiling((decimal)Total / (decimal)Take));
        public int CurrentPage => Take == 0 ? 0 : (int)(Math.Ceiling((decimal)Skip / (decimal)Take) + 1);
        public List<T> List { get; set; }

        public PaginationApiResponse() { }

        public PaginationApiResponse(int skip, int take, int total, List<T> list)
        {
            Take = Math.Max(take, 1);
            Skip = Math.Max(skip, 0);
            Total = Math.Max(total, 0);
            List = list ?? new List<T>();
        }

        public PaginationApiResponse(int skip, int take, List<T> list) : this(skip, take, list?.Count ?? 0, list) { }

        public PaginationApiResponse(List<T> list) : this(0, list?.Count ?? 0, list?.Count ?? 0, list) { }
    }
}
