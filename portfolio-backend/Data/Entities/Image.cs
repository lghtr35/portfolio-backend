namespace portfolio_backend.Data.Entities
{
    public class Image
    {
        public int ImageId { get; set; }
        public string PlacePath { get; set; }
        public string ImagePath { get; set; }
#nullable enable
        public string? ImageName { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
