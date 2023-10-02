namespace Portfolio.Backend.Common.Data.Responses.Common
{
    public class PageResponse<T> : BaseControllerResponse
    {
        public T[] Content { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int TotalRecords { get; set; }
    }
}

