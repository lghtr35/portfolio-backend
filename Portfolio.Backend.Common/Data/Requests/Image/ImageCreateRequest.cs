using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Portfolio.Backend.Common.Data.Requests.Image
{
    public class ImageCreateRequest
    {
        [Required]
        public string? ImageName { get; set; }
        [Required]
        public IFormFile? ImageFile { get; set; }
    }
}

