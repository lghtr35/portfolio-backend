using System;
using System.Collections.Generic;


namespace backend.Models
{
    public class Image
    {
        public int ImageId { get; set; }
        public string Place { get; set; }
        public string ImagePath { get; set; }
#nullable enable
        public string? ImageName { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
