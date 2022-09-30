namespace portfolio_backend.Data.Entities
{
    public class Project
    {
        public int ProjectId { get; set; }
        public string Header { get; set; }
        public string Message { get; set; }
#nullable enable
        public string? Link { get; set; }
        public string? PayloadPath { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public virtual Image[]? Images { get; set; }
    }
}
