using System.ComponentModel.DataAnnotations;

namespace Portfolio.Backend.Common.Data.Requests.Auth
{
    public class LoginRequest
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}

