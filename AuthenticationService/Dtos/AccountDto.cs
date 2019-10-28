using System.ComponentModel.DataAnnotations;

namespace AuthenticationService.Dtos
{
    public class AccountDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
