using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace QienUrenMVC.Models
{
    public class YearOverviewModel
    {
        [DisplayName("Maand")]
        public string Month { get; set; }

        [DisplayName("Year")]
        public int Year { get; set; }

        [DisplayName("Totaal Trainees")]
        public int HoursTrainee { get; set; }

        [DisplayName("Totaal Werknemers")]
        public int HoursEmployee { get; set; }

        [DisplayName("Totaal Sr. Software Developers")]
        public int HoursSoftwareDev { get; set; }

        [DisplayName("Totaal alle categorieën")]
        public int TotalHours { get; set; }
    }
}
