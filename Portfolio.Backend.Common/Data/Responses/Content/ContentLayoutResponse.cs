using System;
namespace Portfolio.Backend.Common.Data.Responses.Content
{
    public class ContentLayoutResponse
    {
        public string Place { get; set; }
        public IDictionary<string, ContentResponse> Contents { get; set; }
    }
}

