using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QienUrenMVC.Models
{
    public class AllHoursYearModel
    {
        [DisplayName("Maand")]
        public string Month { get; set; }

        [DisplayName("Uren")]
        public int TotalHours { get; set; }

        [DisplayName("Ziek")]
        public int TotalSick { get; set; }

        [DisplayName("Overwerk")]
        public int TotalOvertime { get; set; }

        [DisplayName("Training")]
        public int TotalTraining { get; set; }

        [DisplayName("Verlof")]
        public int TotalLeave { get; set; }

        [DisplayName("Overig")]
        public int TotalOther { get; set; }
    }
}
