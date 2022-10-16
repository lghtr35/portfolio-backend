namespace portfolio_backend.Data.Entities
{
    public class Image
    {
        public int ImageId { get; set; }
        public string ImagePath { get; set; }
        public string? ImageName { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public virtual Project? Project { get; set; }
        public int? ProjectId { get; set; }

        public Image()
        {
            ImagePath = "";
        }
    }
}
