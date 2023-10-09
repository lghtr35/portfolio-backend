using System;
using System.ComponentModel.DataAnnotations;

namespace Portfolio.Backend.Common.Data.Requests.Content
{
    public class ContentCreateRequest
    {
        [Required]
        public string? Place { get; set; }
        [Required]
        public string? Location { get; set; }
        [Required]
        public string? Payload { get; set; }
    }
}

