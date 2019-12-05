using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace QienUrenMVC.Models
{
    public class UserPersonaliaModel
    {
        public string AccountId { get; set; }

        [DisplayName("Naam")]
        public string FirstName { get; set; }

        [DisplayName("Achternaam")]
        public string LastName { get; set; }

        [DisplayName("E-mailadres")]
        [EmailAddress(ErrorMessage = "Ongeldig Email Address")]
        public string Email { get; set; }


        [DataType(DataType.Date)]
        [DisplayName("Geboortedatum")]
        public DateTime? DateOfBirth { get; set; }

        [DisplayName("Adres")]
        public string Address { get; set; }

        [DisplayName("Postcode")]
        public string ZIP { get; set; }

        [DisplayName("Telefoonnummer")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Geen geldig telefoonnummer")]
        public string MobilePhone { get; set; }

        [DisplayName("Woonplaats")]
        public string City { get; set; }

        [DisplayName("IBAN-nummer")]
        public string IBAN { get; set; }

        public DateTime? CreationDate { get; set; }
        public string ProfileImage { get; set; }

        public bool IsAdmin { get; set; }

        public bool IsTrainee { get; set; }

        public bool IsQienEmployee { get; set; }

        public bool IsSeniorDeveloper { get; set; }

        [DisplayName("Actief")]
        public bool IsActive { get; set; }

        [DisplayName("Info is veranderd")]
        public bool IsChanged { get; set; }

        public UserPersonaliaModel previousPersonalia;

        public UserPersonaliaModel newPersonalia;
    }
}
