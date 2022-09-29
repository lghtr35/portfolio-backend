using System;

namespace backend.Models
{
    public class Post
    {
        public int PostId { get; set; }
        public string Header { get; set; }
        public string Message { get; set; }
        public string Email { get; set; }
#nullable enable
        public string? Name { get; set; }
        public string? Bio { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public virtual Image[]? Images { get; set; }
    }
}
