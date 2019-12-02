using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace QienUrenMVC.Models
{
    public class FormsForMonthModel
    {

        public int FormId { get; set; }
        public string AccountId { get; set; }

        public string fullName { get; set; }

        [DisplayName("Ingeleverd op")]
        public DateTime? DateSend { get; set; }

        [DisplayName("Uiterste inleverdatum")]
        public DateTime? DateDue { get; set; }

        [DisplayName("Totaal aantal uren")]
        public int TotalHours { get; set; }

        [DisplayName("Maand")]
        public string ProjectMonth { get; set; }

        [DisplayName("Jaar")]
        public int Year { get; set; }

        [DisplayName("Keuring opdrachtgever")]
        public int IsAcceptedClient { get; set; }

        public string CommentClient { get; set; }
        public string CommentAdmin { get; set; }

        [DisplayName("Gesloten")]
        public bool IsLocked { get; set; }
        
    }
}
