using System;
namespace portfolio_backend.Data.DTOs.Common
{
    public class PageDTO<T> : BaseControllerResponse
    {
        public IEnumerable<T> Content { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int TotalRecords { get; set; }

        public PageDTO()
        {
            Content = new List<T>();
        }
    }
}

