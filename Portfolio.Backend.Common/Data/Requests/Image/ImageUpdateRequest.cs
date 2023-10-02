using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Portfolio.Backend.Common.Data.Requests.Image
{
    public class ImageUpdateRequest
    {
        [Required]
        public int ImageId { get; set; }
        public string? ImageName { get; set; }
        public IFormFile? ImageFile { get; set; } 
        public int? ProjectId { get; set; }
    }
}

