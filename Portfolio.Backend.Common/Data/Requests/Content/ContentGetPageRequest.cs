using System;
using System.ComponentModel.DataAnnotations;

namespace Portfolio.Backend.Common.Data.Requests.Content
{
    public class ContentGetPageRequest
    {
        [Required]
        public string? Place { get; set; }

        public ContentGetPageRequest(string p)
        {
            Place = p;
        }
    }
}

