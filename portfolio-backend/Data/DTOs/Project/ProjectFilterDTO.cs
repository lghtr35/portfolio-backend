using System;
namespace portfolio_backend.Data.DTOs.Project
{
    public class ProjectFilterDTO
    {
        public int Page { get; set; }
        public int Size { get; set; }
        public IEnumerable<int>? IdList { get; set; }
        public IEnumerable<string>? PathList { get; set; }
        public string? HeaderSearchString { get; set; }
        public string? MessageSearchString { get; set; }
        public string? CreatedAtSearchString { get; set; }
        public string? UpdatedAtSearchString { get; set; }
    }
}

