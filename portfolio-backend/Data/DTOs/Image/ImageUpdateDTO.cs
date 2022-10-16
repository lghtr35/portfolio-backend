using System;
namespace portfolio_backend.Data.DTOs.Image
{
    public class ImageUpdateDTO
    {
        public int ImageId { get; set; }
        public string? ImagePath { get; set; }
        public string? ImageName { get; set; }
        public int? ProjectId { get; set; }
    }
}

