using System;
using Portfolio.Backend.Common.Data.Responses.Common;

namespace Portfolio.Backend.Common.Data.Responses.Content
{
    public class ContentResponse : BaseControllerResponse
    {
        public int ContentId { get; set; }
        public string Place { get; set; }
        public string Location { get; set; }
        public string Payload { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public ContentResponse()
        {
            Place = "";
            Location = "";
            Payload = "";
        }

        public ContentResponse(Entities.Content c)
        {
            ContentId = c.ContentId;
            Place = c.Place;
            Location = c.Location;
            Payload = c.Payload;
            CreatedAt = c.CreatedAt;
            UpdatedAt = c.UpdatedAt;
        }
    }
}

