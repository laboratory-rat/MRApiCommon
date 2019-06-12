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
        /// <summary>
        /// Take count
        /// </summary>
        public int Take { get; set; }
        
        /// <summary>
        /// Skip entity count
        /// </summary>
        public int Skip { get; set; }
        
        /// <summary>
        /// Total count
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// Total pages
        /// </summary>
        public int TotalPages => Take == 0 ? 0 : (int)(Math.Ceiling((decimal)Total / (decimal)Take));

        /// <summary>
        /// Current page
        /// </summary>
        public int CurrentPage => Take == 0 ? 0 : (int)(Math.Ceiling((decimal)Skip / (decimal)Take) + 1);

        /// <summary>
        /// Results list
        /// </summary>
        public List<T> List { get; set; }

        /// <summary>
        /// Empty constructor
        /// </summary>
        public PaginationApiResponse() { }

        /// <summary>
        /// Full constructor
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="total"></param>
        /// <param name="list"></param>
        public PaginationApiResponse(int skip, int take, int total, List<T> list)
        {
            Take = Math.Max(take, 1);
            Skip = Math.Max(skip, 0);
            Total = Math.Max(total, 0);
            List = list ?? new List<T>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="list"></param>
        public PaginationApiResponse(int skip, int take, List<T> list) : this(skip, take, list?.Count ?? 0, list) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        public PaginationApiResponse(List<T> list) : this(0, list?.Count ?? 0, list?.Count ?? 0, list) { }
    }
}
