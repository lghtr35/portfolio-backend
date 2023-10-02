using System.ComponentModel.DataAnnotations;

namespace Portfolio.Backend.Common.Data.Requests.Project
{
    public class ProjectFilterRequest
    {
        [Required]
        public int Page { get; set; }
        [Required]
        public int Size { get; set; }
        public IEnumerable<int>? IdList { get; set; }
        public IEnumerable<string>? PathList { get; set; }
        public string? HeaderSearchString { get; set; }
        public string? MessageSearchString { get; set; }
        public string? CreatedAtSearchString { get; set; }
        public string? UpdatedAtSearchString { get; set; }
    }
}

