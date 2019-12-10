using DataAnnotationsExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QienUrenMVC.Models
{
    public class ClientModel
    {
        [Required]
        public int ClientId { get; set; }

        //public int AccountId { get; set; }
        [Required(ErrorMessage = "Een bedrijfsnaam is verplicht")]
        [DisplayName("Bedrijfsnaam")]
        public string CompanyName { get; set; }


        [Required(ErrorMessage = "Een contactpersoon naam is verplicht")]
        [DisplayName("Contactperson naam")]
        public string ClientName1 { get; set; }


        [DisplayName("Contactperson naam")]
        public string ClientName2 { get; set; }


        [Required(ErrorMessage = "Een E-mailadres is verplicht")]
        [DisplayName("E-mailadres")]
        [EmailAddress(ErrorMessage = "Ongeldig Email Address")]
        public string ClientEmail1 { get; set; }


        [DisplayName("E-mailadres")]
        [EmailAddress(ErrorMessage = "Ongeldig Email Address")]
        public string ClientEmail2 { get; set; }
    }
}

