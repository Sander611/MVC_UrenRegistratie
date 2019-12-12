using QienUrenMVC.Models;
using QienUrenMVC.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest_QienUren.FakeRepositories
{
    class FakeHoursFormRepository : IHoursFormRepository
    {
        public Task ChangeState(int state, int id, string textAdmin, string textClient)
        {
            throw new NotImplementedException();
        }

        public Task<HoursFormModel> CheckIfExists(string id, string month, int year)
        {
            throw new NotImplementedException();
        }

        public Task<HoursFormModel> CreateNewForm(HoursFormModel hoursFormModel, int ClientId)
        {
            throw new NotImplementedException();
        }

        public Task<HoursFormModel> EditForm(HoursFormModel editform)
        {
            throw new NotImplementedException();
        }

        public Task<List<AdminTaskModel>> GetAllClientAcceptedForms()
        {
            throw new NotImplementedException();
        }

        public Task<List<int>> GetAllExistingYears(int year)
        {
            throw new NotImplementedException();
        }

        public Task<List<HoursFormModel>> getAllFormPerAccount(string accountId)
        {
            throw new NotImplementedException();
        }

        public Task<List<HoursFormModel>> GetAllFormsForAccountForYear(int year, string id)
        {
            throw new NotImplementedException();
        }

        public Task<List<HoursFormModel>> GetAllHoursForms()
        {
            throw new NotImplementedException();
        }

        public Task<List<AllHoursYearModel>> GetAllHoursYear(int currYear)
        {
            throw new NotImplementedException();
        }

        public Task<List<int>> GetAllYearsForUser(string id)
        {
            throw new NotImplementedException();
        }

        public Task<HoursFormModel> GetFormById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<HoursFormModel> GetFormsById(int formid)
        {
            throw new NotImplementedException();
        }

        public Task<List<FormsForMonthModel>> GetFormsForYearAndMonth(int year, string month)
        {
            throw new NotImplementedException();
        }

        public Task<List<HoursFormModel>> GetSingleAccountForms(string id, int year)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetYearOfForm(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<YearOverviewModel>> GetYearOverviews(int year, List<string> Traineeids, List<string> Employeeids, List<string> SoftDevids)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAllFormPerAccount(string accountId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateTotalHoursForm(int id, int totalHours, int totalSick, int totalOver, int totalLeave, int totalOther, int TotalTraining)
        {
            throw new NotImplementedException();
        }
    }
}
