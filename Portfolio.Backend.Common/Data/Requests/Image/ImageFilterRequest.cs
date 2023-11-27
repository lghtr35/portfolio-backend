using System.ComponentModel.DataAnnotations;

namespace Portfolio.Backend.Common.Data.Requests.Image
{
    public class ImageFilterRequest
    {
        [Required]
        public int Size { get; set; }
        [Required]
        public int Page { get; set; }
        public List<int>? IdList { get; set; }
        public List<string>? PathList { get; set; }
        public string? ImageNameSearchString { get; set; }
    }
}

