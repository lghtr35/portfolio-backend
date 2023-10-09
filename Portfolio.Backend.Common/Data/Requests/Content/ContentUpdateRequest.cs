using System;
using System.ComponentModel.DataAnnotations;

namespace Portfolio.Backend.Common.Data.Requests.Content
{
    public class ContentUpdateRequest
    {
        [Required]
        public int? ContentId { get; set; }
        public string? Place { get; set; }
        public string? Location { get; set; }
        public string? Payload { get; set; }
    }
}

