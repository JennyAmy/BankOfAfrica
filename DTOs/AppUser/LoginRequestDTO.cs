using System.ComponentModel.DataAnnotations;

namespace BankOfAfricaAPI.DTOs.AppUser
{
    public class LoginRequestDTO
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
