using System;
using System.ComponentModel.DataAnnotations;
using static BankOfAfricaAPI.Enums.BankAccountEnums;

namespace BankOfAfricaAPI.Entities
{
    public class BankAccount : BaseEntity
    {
        [Key]
        public int CustomerId { get; set; }
        [Required]
        public string Surname { get; set; }
        public string Middlename { get; set; }
        [Required]
        public string Firstname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public TitleEnum Title { get; set; }
        public MaritalStatusEnum MaritalStatus { get; set; }
        public GenderEnum Gender { get; set; }
        public string Address { get; set; }
        [Required]
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        [Required]
        public string BVN { get; set; }
        [Required]
        public string NIN { get; set; }
        public decimal AccountBal { get; set; } = 5000000;
        public AccountTypeEnum AccountType { get; set; }
        [Required]
        public string AccountNumber { get; set; }
        public string AccountOfficerName { get; set; }
        public string AccountOfficerEmail { get; set; }
        public DateTime DateTimeCreated { get; set; } = DateTime.Now;
    }
}
