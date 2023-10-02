using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Portfolio.Backend.Common.Data.Requests.Project
{
    public class ProjectUpdateRequest
    {
        [Required]
        public int ProjectId { get; set; }
        public string? Header { get; set; }
        public string? Message { get; set; }
        public string? Link { get; set; }
        public IFormFile? ProjectFile { get; set; }
    }
}

