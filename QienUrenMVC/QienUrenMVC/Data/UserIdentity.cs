using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QienUrenMVC.Data
{
    public class UserIdentity : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Address { get; set; }
        public string ZIP { get; set; }
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
