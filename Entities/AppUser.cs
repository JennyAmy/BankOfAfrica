using System;
using System.ComponentModel.DataAnnotations;

namespace BankOfAfricaAPI.Entities
{
    public class AppUser : BaseEntity
    {
        public int AppUserId { get; set; }
        [Required]
        public int CustomerId { get; set; }
        [Required]
        public string AccountNumber { get; set; }
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Lastname { get; set; }
        public string Email { get; set; }       
        public byte[] Password { get; set; }
        public byte[] PasswordKey { get; set; }
        public bool IsValidated { get; set; }
        public DateTime DateTimeCreated { get; set; } = DateTime.Now;
    }
}
