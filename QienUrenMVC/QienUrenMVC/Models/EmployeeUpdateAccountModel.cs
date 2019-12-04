using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace QienUrenMVC.Models
{
    public class EmployeeUpdateAccountModel
    {
        public string AccountId { get; set; }

        [Required(ErrorMessage = "Een voornaam is verplicht")]
        [DisplayName("Naam")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Een achternaam is verplicht")]
        [DisplayName("Achternaam")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Een E-mailadres is verplicht")]
        [DisplayName("E-mailadres")]
        [EmailAddress(ErrorMessage = "Ongeldig Email Address")]
        public string Email { get; set; }

        public string HashedPassword { get; set; }

        [Required(ErrorMessage = "Een geboortedatum is verplicht")]
        [DataType(DataType.Date)]
        [DisplayName("Geboortedatum")]
        public DateTime? DateOfBirth { get; set; }

        //[Required]
        //[DataType(DataType.Password)]
        //[DisplayName("Wachtwoord")]
        //public string Password { get; set; }

        //[DataType(DataType.Password)]
        //[DisplayName("Herhaal wachtwoord")]
        //[Compare("Password",
        //        ErrorMessage = "Wachtwoord en herhaling wachtwoord zijn niet hetzelfde.")]
        //public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Een Adres is verplicht")]
        [DisplayName("Adres")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Een postcode is verplicht")]
        [DisplayName("Postcode")]
        public string ZIP { get; set; }

        [Required(ErrorMessage = "Een telefoonnummer is verplicht")]
        [DisplayName("Telefoonnummer")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Geen geldig telefoonnummer")]
        public string MobilePhone { get; set; }

        [Required(ErrorMessage = "Een woonplaats is verplicht")]
        [DisplayName("Woonplaats")]
        public string City { get; set; }

        [Required(ErrorMessage = "Een IBAN is verplicht")]
        [DisplayName("IBAN-nummer")]
        public string IBAN { get; set; }

        public DateTime? CreationDate { get; set; }

        public IFormFile ProfileImage { get; set; }


        public bool IsAdmin { get; set; }

        public bool IsTrainee { get; set; }

        public bool IsQienEmployee { get; set; }

        public bool IsSeniorDeveloper { get; set; }

        [DisplayName("Actief")]
        public bool IsActive { get; set; }
        public int RoleId { get; set; }
        
        [DisplayName("Info is veranderd")]
        public bool IsChanged { get; set; }
    }
}
