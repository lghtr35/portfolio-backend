using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Portfolio.Backend.Common.Data.Requests.Project
{
    public class ProjectCreateRequest
    {
        [Required]
        public string? Header { get; set; }
        [Required]
        public string? Message { get; set; }
        [Required]
        public bool IsDownloadable { get; set; }
        [Required]
        public IFormFile? ProjectFile { get; set; }
        public List<IFormFile>? ProjectImages { get; set; }
        public string? Link { get; set; }
    }
}

