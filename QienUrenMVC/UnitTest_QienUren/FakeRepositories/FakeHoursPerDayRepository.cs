using Microsoft.AspNetCore.Mvc.Rendering;
using QienUrenMVC.Models;
using QienUrenMVC.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest_QienUren.FakeRepositories
{
    class FakeHoursPerDayRepository : IHoursPerDayRepository
    {
        public Task<List<HoursPerDayModel>> GetAllDaysForForm(int formId)
        {
            throw new NotImplementedException();
        }

        public Task<HoursPerDayModel> GetAllFormsByClientId(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SelectListItem> GetClientList()
        {
            throw new NotImplementedException();
        }

        public Task<List<HoursPerDayModel>> Update(List<HoursPerDayModel> daychange)
        {
            throw new NotImplementedException();
        }
    }
}
