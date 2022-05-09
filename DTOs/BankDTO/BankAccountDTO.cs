using System;
using System.ComponentModel.DataAnnotations;

namespace BankOfAfricaAPI.DTOs.BankDTO
{
    public class BankAccountDTO
    {
        public string Surname { get; set; }
        public string Middlename { get; set; }
        [Required]
        public string Firstname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Title { get; set; }
        public int MaritalStatus { get; set; }
        public int Gender { get; set; }
        public string Address { get; set; }
        [Required]
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        [Required]
        public string BVN { get; set; }
        [Required]
        public string NIN { get; set; }
        public int AccountType { get; set; }
        public int CustomerId { get; set; }
        public string AccountNumber { get; set; }
        public string AccountOfficerName { get; set; }
        public string AccountOfficerEmail { get; set; }
    }
}
