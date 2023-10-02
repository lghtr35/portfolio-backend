using System.ComponentModel.DataAnnotations.Schema;

namespace Portfolio.Backend.Common.Data.Entities
{
    public class Image
    {
        public int ImageId { get; set; }
        public string PayloadPath { get; set; }
        public string ImageName { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Project? Project { get; set; }
        public int? ProjectId { get; set; }
    }
}
