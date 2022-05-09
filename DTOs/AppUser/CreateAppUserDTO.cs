using System.ComponentModel.DataAnnotations;

namespace BankOfAfricaAPI.DTOs.AppUser
{
    public class CreateAppUserDTO
    {
        public int CustomerId { get; set; }
        [Required]
        public string AccountNumber { get; set; }
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
