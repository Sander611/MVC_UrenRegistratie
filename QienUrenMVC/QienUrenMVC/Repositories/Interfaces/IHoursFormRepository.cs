using System.Collections.Generic;
using System.Threading.Tasks;
using QienUrenMVC.Models;

namespace QienUrenMVC.Repositories
{
    public interface IHoursFormRepository
    {
        Task<HoursFormModel> EditForm(HoursFormModel editform);
        Task<List<HoursFormModel>> GetAllHoursForms();

        Task<List<HoursFormModel>> GetSingleAccountForms(string id, int year);

        Task<HoursFormModel> CreateNewForm(HoursFormModel hoursFormModel, int ClientId);
        Task<List<AdminTaskModel>> GetAllClientAcceptedForms();
        Task<List<HoursFormModel>> getAllFormPerAccount(string accountId);

        Task RemoveAllFormPerAccount(string accountId);

        Task<List<YearOverviewModel>> GetYearOverviews(int year, List<string> Traineeids, List<string> Employeeids, List<string> SoftDevids);

        Task<HoursFormModel> GetFormsById(int formid);
        Task<List<FormsForMonthModel>> GetFormsForYearAndMonth(int year, string month);

        Task ChangeState(int state, int id, string textAdmin, string textClient);

        Task UpdateTotalHoursForm(int id, int totalHours, int totalSick, int totalOver, int totalLeave, int totalOther, int TotalTraining);

        Task<int> GetYearOfForm(int id);

        Task<List<int>> GetAllYearsForUser(string id);

        Task<List<int>> GetAllExistingYears(int year);

        Task<HoursFormModel> GetFormById(int id);

        Task<HoursFormModel> CheckIfExists(string id, string month, int year);

        Task<List<HoursFormModel>> GetAllFormsForAccountForYear(int year, string id);
    }
}