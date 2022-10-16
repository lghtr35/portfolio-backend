using System;
using System.ComponentModel.DataAnnotations;

namespace portfolio_backend.Data.DTOs.Project
{
    public class ProjectUploadImageDTO
    {
        [Required]
        public int ProjectId { get; set; }
        [Required]
        public IEnumerable<IFormFile> Files { get; set; }
    }
}

