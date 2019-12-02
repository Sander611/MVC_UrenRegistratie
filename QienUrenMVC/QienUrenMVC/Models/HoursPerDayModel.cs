using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace QienUrenMVC.Models
{
    public class HoursPerDayModel
    {
        public int HoursPerDayId { get; set; }

        public int FormId { get; set; }

        [DisplayName("Datum")]
        public int Day { get; set; }

        [DisplayName("Maand")]
        public string Month { get; set; }

        [DisplayName("Opdracht")]
        public int Hours { get; set; }

        [DisplayName("Overwerk")]
        public int OverTimeHours { get; set; }

        [DisplayName("Training")]
        public int Training { get; set; }

        [DisplayName("Verlof")]
        public int IsLeave { get; set; }

        [DisplayName("Overig")]
        public int Other { get; set; }

        [DisplayName("Verklaring m.b.t. overig")]
        public string Reasoning { get; set; }

        public int? ClientId { get; set; }

        public IEnumerable<SelectListItem> ClientNames { get; set; }
        
        [DisplayName("Ziek")]
        public int IsSick { get; set; }
    }
}
