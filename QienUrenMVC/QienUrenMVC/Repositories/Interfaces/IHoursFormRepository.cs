﻿using System.Collections.Generic;
using System.Threading.Tasks;
using QienUrenMVC.Models;

namespace QienUrenMVC.Repositories
{
    public interface IHoursFormRepository
    {
        Task<HoursFormModel> EditForm(HoursFormModel editform);
        Task<List<HoursFormModel>> GetAllHoursForms();

        Task<List<HoursFormModel>> GetSingleAccountForms(string id);

        Task<HoursFormModel> CreateNewForm(HoursFormModel hoursFormModel);
        Task<List<AdminTaskModel>> GetAllClientAcceptedForms();
        Task<List<HoursFormModel>> getAllFormPerAccount(string accountId);

        Task RemoveAllFormPerAccount(string accountId);

        Task<List<YearOverviewModel>> GetYearOverviews(int year, List<string> Traineeids, List<string> Employeeids, List<string> SoftDevids);

        Task<HoursFormModel> GetFormsById(int formid);
        Task<List<FormsForMonthModel>> GetFormsForYearAndMonth(int year, string month);

        Task ChangeState(int state, int id, string textAdmin, string textClient);

        Task UpdateTotalHoursForm(int id, int totalHours);

        Task<int> GetYearOfForm(int id);

        Task<HoursFormModel> GetFormById(int id);
    }
}