using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QienUrenMVC.Data
{
    [Table("Account")]
    public class Account
    {
        [Key]
        public int AccountId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }
        public DateTime? DateOfBirth { get; set; }

        [Required]
        public string HashedPassword { get; set; }
        public string Address { get; set; }
        public string ZIP { get; set; }
        public string MobilePhone { get; set; }
        public string City { get; set; }
        public string IBAN { get; set; }
        public DateTime? CreationDate { get; set; }
        public string ProfileImage { get; set; }

        [Required]
        public bool IsAdmin { get; set; }
        [Required]
        public bool IsTrainee { get; set; }
        [Required]
        public bool IsQienEmployee { get; set; }
        [Required]
        public bool IsSeniorDeveloper { get; set; }
        [Required]
        public bool IsActive { get; set; }
    }
}
