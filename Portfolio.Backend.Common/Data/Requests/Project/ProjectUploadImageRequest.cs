using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Portfolio.Backend.Common.Data.Requests.Project
{
    public class ProjectUploadImageRequest
    {
        [Required]
        public int ProjectId { get; set; }
        [Required]
        public List<Tuple<string,IFormFile>> Images { get; set; }
    }
}

