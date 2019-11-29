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
    }
}