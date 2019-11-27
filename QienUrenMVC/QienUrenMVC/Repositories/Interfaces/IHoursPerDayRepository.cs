using System.Collections.Generic;
using System.Threading.Tasks;
using QienUrenMVC.Models;

namespace QienUrenMVC.Repositories
{
    public interface IHoursPerDayRepository
    {

        Task<List<HoursPerDayModel>> Update(List<HoursPerDayModel> daychange);
        Task<List<HoursPerDayModel>> GetAllDaysForForm(int formId);
    }
}