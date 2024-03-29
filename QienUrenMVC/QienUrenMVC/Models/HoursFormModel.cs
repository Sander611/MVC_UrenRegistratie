﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace QienUrenMVC.Models
{
    public class HoursFormModel
    {

        public int FormId { get; set; }
        public string AccountId { get; set; }

        [DisplayName("Ingeleverd op")]
        public DateTime? DateSend { get; set; }

        [DisplayName("Deadline")]
        public DateTime? DateDue { get; set; }

        [DisplayName("Uren")]
        public int TotalHours { get; set; }
        [DisplayName("Ziek")]
        public int TotalSick { get; set; }

        [DisplayName("Overuren")]
        public int TotalOver { get; set; }

        [DisplayName("Training")]
        public int TotalTraining { get; set; }

        [DisplayName("Verlof")]
        public int TotalLeave { get; set; }

        [DisplayName("Overig")]
        public int TotalOther { get; set; }

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
        public Guid Verification_code { get; set; }

    }
}
