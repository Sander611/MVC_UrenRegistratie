using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using QienUrenMVC.Models;

namespace QienUrenMVC.Repositories
{
    public interface IHoursPerDayRepository
    {
        public IEnumerable<SelectListItem> GetClientList();
        Task<List<HoursPerDayModel>> Update(List<HoursPerDayModel> daychange);
        Task<List<HoursPerDayModel>> GetAllDaysForForm(int formId);

        Task RemoveAllDaysForForm(int formId);
    }
}