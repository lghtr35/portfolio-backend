using System.ComponentModel.DataAnnotations;

namespace Portfolio.Backend.Common.Data.Requests.Project
{
    public class ProjectDeleteImageRequest
    {
        [Required]
        public int ProjectId { get; set; }
        [Required, MinLength(1)]
        public int[] ImageIds { get; set; }
    }
}

