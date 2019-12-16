using Microsoft.AspNetCore.Mvc.Rendering;
using QienUrenMVC.Models;
using QienUrenMVC.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest_QienUren.FakeRepositories
{

    public class FakeHoursPerDayRepository : IHoursPerDayRepository
    {
        private List<HoursPerDayModel> hoursPerDays = new List<HoursPerDayModel>
        {

            new HoursPerDayModel
            {
                ClientId = 1,
                Day = 1,
                FormId = 1,
                Hours = 0,
                HoursPerDayId = 1,
                IsLeave = 0,
                IsSick = 0,
                Month = "november",
                Other = 0,
                OverTimeHours = 0,
                Reasoning = "",
                Training = 0,
            },

             new HoursPerDayModel
            {
                ClientId = 1,
                Day = 2,
                FormId = 1,
                Hours = 0,
                HoursPerDayId = 2,
                IsLeave = 0,
                IsSick = 0,
                Month = "november",
                Other = 0,
                OverTimeHours = 0,
                Reasoning = "",
                Training = 0,
            },

                new HoursPerDayModel
            {
                ClientId = 1,
                Day = 3,
                FormId = 1,
                Hours = 0,
                HoursPerDayId = 3,
                IsLeave = 0,
                IsSick = 0,
                Month = "november",
                Other = 0,
                OverTimeHours = 0,
                Reasoning = "",
                Training = 0,
            },


        };

        public async Task<List<HoursPerDayModel>> GetAllDaysForForm(int formId)
        {
            return hoursPerDays;
        }


        public Task<HoursPerDayModel> GetAllFormsByClientId(int id)
        {
            throw new NotImplementedException();
        }

        public  IEnumerable<SelectListItem> GetClientList()
        {
            throw new NotImplementedException();
        }

        public Task<List<HoursPerDayModel>> Update(List<HoursPerDayModel> daychange)
        {
            throw new NotImplementedException();
        }
    }
}
