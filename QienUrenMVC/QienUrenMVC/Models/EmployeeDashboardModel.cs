using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace QienUrenMVC.Models
{
    public class EmployeeDashboardModel
    {
        public AccountModel Account { get; set; }
        public List<HoursFormModel> Forms { get; set; }
        //public int formId { get; set; }
        //public int accountId { get; set; }

        public string Info { get; set; }

        //public string Month { get; set; }

        //public int Year { get; set; }

        //public DateTime? Deadline { get; set; }

        //public string FirstName { get; set; }

        //public string LastName { get; set; }

        //public string Address { get; set; }

        //public string ZIP { get; set; }
        //public string City { get; set; }

    }
}
