using System.Collections.Generic;
using X.PagedList;

namespace WebShop.API.Models
{
    public class ApiPagedList<T> where T : class
    {
        public IList<T> Items { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }

        public static ApiPagedList<T> FromList(IList<T> results, PagedListMetaData metaData)
        {
            return new ApiPagedList<T>
            {
                Items = results,
                PageNumber = metaData.PageNumber,
                PageSize = metaData.PageSize,
                TotalItems = metaData.TotalItemCount,
                TotalPages = metaData.PageCount
            };
        }
    }
}