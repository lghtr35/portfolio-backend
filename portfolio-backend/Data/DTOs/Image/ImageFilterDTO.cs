using System;
namespace portfolio_backend.Data.DTOs.Image
{
    public class ImageFilterDTO
    {
        public int Page { get; set; }
        public int Size { get; set; }
        public IEnumerable<int>? IdList { get; set; }
        public IEnumerable<string>? PathList { get; set; }
        public string? ImageNameSearchString { get; set; }
        public string? CreatedAtSearchString { get; set; }
        public string? UpdatedAtSearchString { get; set; }
    }
}

