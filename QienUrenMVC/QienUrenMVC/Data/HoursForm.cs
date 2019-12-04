using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace QienUrenMVC.Data
{
    [Table("HoursForm")]
    public class HoursForm
    {
        [Key]
        public int FormId { get; set; }
        [ForeignKey("AspNetUsers")]
        [Required]
        public string AccountId { get; set; }
        public DateTime? DateSend { get; set; }
        public DateTime? DateDue { get; set; }
        public int TotalHours { get; set; }

        [Required]
        public string ProjectMonth { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        public int IsAcceptedClient { get; set; }

        public string commentClient { get; set; }
        public string commentAdmin { get; set; }
        public bool IsLocked { get; set; }
        public Guid Verification_code { get; set; }
    }
}
