using System;
namespace portfolio_backend.Data.DTOs.Project
{
    public class ProjectCreateDTO
    {
        public string Header { get; set; }
        public string Message { get; set; }
        public string? Link { get; set; }
        public string? PayloadPath { get; set; }
    }
}

