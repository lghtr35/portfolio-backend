using Portfolio.Backend.Common.Data.Responses.Common;

namespace Portfolio.Backend.Common.Data.Responses.Project
{
    public class ProjectFileResponse : BaseControllerResponse
    {
        public int ProjectId { get; set; }
        public byte[] ProjectData { get; set; }
        public string ProjectExtension { get; set; }

        public ProjectFileResponse(Entities.Project project)
        {
            ProjectId = project.ProjectId;
            ProjectData = File.ReadAllBytes(project.PayloadPath);
            ProjectExtension = Path.GetExtension(project.PayloadPath).Replace(".", "");
        }
    }
}

