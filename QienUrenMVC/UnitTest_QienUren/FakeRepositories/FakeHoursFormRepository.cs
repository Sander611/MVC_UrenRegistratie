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
        

        private List<HoursFormModel> hoursforms = new List<HoursFormModel>
        {

            new HoursFormModel
            {
                AccountId = "1",
                FormId = 1,
                CommentAdmin = "",
                CommentClient = "",
                TotalHours = 0,
                IsAcceptedClient = 0,
                DateSend = new DateTime(2019, 03,23),
                DateDue = new DateTime (2019,11,23),
                IsLocked = false,
                ProjectMonth = "november",
                TotalLeave = 0,
                TotalOther = 0,
                TotalOver = 0,
                TotalSick = 0,
                TotalTraining = 0,
                Year = 2019,
                Verification_code = new Guid ("62FA647C-AD54-4BCC-A860-E5A2664B019D")
            },

            new HoursFormModel
            {
                AccountId = "1",
                FormId = 2,
                CommentAdmin = "",
                CommentClient = "",
                TotalHours = 0,
                IsAcceptedClient = 0,
                DateSend = new DateTime(2019, 03,23),
                DateDue = new DateTime (2019,11,23),
                IsLocked = false,
                ProjectMonth = "november",
                TotalLeave = 0,
                TotalOther = 0,
                TotalOver = 0,
                TotalSick = 0,
                TotalTraining = 0,
                Year = 2019,
                Verification_code = new Guid ("62FA647C-AD54-4BCC-A860-E5A2664B019D")
            }

        };

        private List<FormsForMonthModel> formsForMonths = new List<FormsForMonthModel>
        {
            new FormsForMonthModel
            {
                AccountId = "1",
                FormId = 1,
                Year = 2019,
                CommentAdmin = "",
                CommentClient = "",
                DateDue = new DateTime(2019,03,23),
                DateSend = new DateTime(2019,03,23),
                fullName = "Ron Dijkstra",
                IsAcceptedClient = 0,
                IsLocked = false,
                ProjectMonth = "november",
                TotalHours = 0,
                TotalLeave = 0,
                TotalOther = 0,
                TotalOver = 0,
                TotalSick = 0,
                TotalTraining = 0


            }
        };

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

        public async Task<HoursFormModel> EditForm(HoursFormModel editform)
        {
            return hoursforms[0];
        }

        public Task<List<AdminTaskModel>> GetAllClientAcceptedForms()
        {
            throw new NotImplementedException();
        }

        public Task<List<int>> GetAllExistingYears(int year)
        {
            throw new NotImplementedException();
        }

        public async Task<List<HoursFormModel>> getAllFormPerAccount(string accountId)
        {
            return hoursforms;
        }

        public Task<List<HoursFormModel>> GetAllFormsForAccountForYear(int year, string id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<HoursFormModel>> GetAllHoursForms()
        {
            return hoursforms;
        }

        public Task<List<AllHoursYearModel>> GetAllHoursYear(int currYear)
        {
            throw new NotImplementedException();
        }

        public Task<List<int>> GetAllYearsForUser(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<HoursFormModel> GetFormById(int id)
        {
            return hoursforms[0];
        }

        public async Task<HoursFormModel> GetFormsById(int formid)
        {
            return hoursforms[0];
        }

        public async Task<List<FormsForMonthModel>> GetFormsForYearAndMonth(int year, string month)
        {
            return formsForMonths;
        }

        public Task<List<HoursFormModel>> GetSingleAccountForms(string id, int year)
        {
            throw new NotImplementedException();
        }

        public async Task<int> GetYearOfForm(int id)
        {
            return 2019;
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
