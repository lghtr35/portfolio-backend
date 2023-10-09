using System;
namespace Portfolio.Backend.Common.Data.Entities
{
	public class Content
	{
        public int ContentId { get; set; }
        public string Place { get; set; }
        public string Location { get; set; }
        public string Payload { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
	}
}

