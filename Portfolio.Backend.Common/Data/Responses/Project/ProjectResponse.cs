using System;
using Portfolio.Backend.Common.Data.Entities;
using Portfolio.Backend.Common.Data.Responses.Common;
using Portfolio.Backend.Common.Data.Responses.Image;

namespace Portfolio.Backend.Common.Data.Responses.Project
{
    public class ProjectResponse : BaseControllerResponse
    {
        public int ProjectId { get; set; }
        public string ProjectTitle { get; set; }
        public string ProjectDescription { get; set; }
        public string? Link { get; set; }
        public string PayloadPath { get; set; }
        public bool IsDownloadable { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public ImageResponse[]? Images { get; set; }

        public ProjectResponse()
        {
            ProjectTitle = "";
            ProjectDescription = "";
            PayloadPath = "";
        }

        public ProjectResponse(Entities.Project project)
        {
            ProjectId = project.ProjectId;
            ProjectTitle = project.Header;
            ProjectDescription = project.Message;
            Link = project.Link;
            PayloadPath = project.PayloadPath;
            CreatedAt = project.CreatedAt;
            UpdatedAt = project.UpdatedAt;
            Images = project.Images?.Select(i => new ImageResponse(i)).ToArray();
            IsDownloadable = project.IsDownloadable;
        }
    }
}

