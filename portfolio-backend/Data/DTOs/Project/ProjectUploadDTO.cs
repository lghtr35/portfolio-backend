using System;
namespace portfolio_backend.Data.DTOs.Project
{
    public class ProjectUploadDTO
    {
        public IFormFile ProjectFile { get; set; }
        public int ProjectId { get; set; }
    }
}

