
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QienUrenMVC.Data
{
    [Table("HoursPerDay")]
    public class HoursPerDay
    {
        [Key]
        public int HoursPerDayId { get; set; }

        [ForeignKey("HoursForm")]
        public int FormId { get; set; }
        public HoursForm Form { get; set; }

        [Required]
        public int Day { get; set; }
        [Required]
        public string Month { get; set; }
       
        public int Hours { get; set; }
        public int OverTimeHours { get; set; }
        public int Training { get; set; }
        public int IsLeave { get; set; }
        public int Other { get; set; }
        public string Reasoning { get; set; }
        public string ProjectDay { get; set; }

        [ForeignKey("Client")]
        public int? ClientId { get; set; }
        public int IsSick { get; set; }

    }
}
