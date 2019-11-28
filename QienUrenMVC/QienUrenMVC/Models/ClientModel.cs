using DataAnnotationsExtensions;
using System;
using System.Collections.Generic;
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

        public string AccountId { get; set; }

        public string CompanyName { get; set; }

        public string ClientName1 { get; set; }
        public string ClientName2 { get; set; }

        public string ClientEmail1 { get; set; }
        public string ClientEmail2 { get; set; }
    }
}

