using System;
using Portfolio.Backend.Common.Data.Responses.Common;

namespace Portfolio.Backend.Common.Data.Responses.Image
{
	public class ImageResponse : BaseControllerResponse
	{
        public int ImageId { get; set; }
        public byte[] ImageData { get; set; }
        public string ImageExtension { get; set; }
        public string ImageName { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? ProjectId { get; set; }

        public ImageResponse(Entities.Image image)
        {
            ImageId = image.ImageId;
            ImageData = File.ReadAllBytes(image.PayloadPath);
            ImageExtension = Path.GetExtension(image.PayloadPath).Replace(".","");
            ImageName = image.ImageName;
            CreatedAt = image.CreatedAt;
            UpdatedAt = image.UpdatedAt;
            ProjectId = image.ProjectId;
        }
    }
}

