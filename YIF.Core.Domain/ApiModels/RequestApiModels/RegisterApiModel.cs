using System.ComponentModel.DataAnnotations;

namespace YIF.Core.Domain.ApiModels.RequestApiModels
{
    public class RegisterApiModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
    }
}
