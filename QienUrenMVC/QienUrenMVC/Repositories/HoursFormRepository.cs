using Microsoft.EntityFrameworkCore;
using QienUrenMVC.Data;
using QienUrenMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QienUrenMVC.Repositories
{
    public class HoursFormRepository : IHoursFormRepository
    {
        private readonly ApplicationDbContext context;
        public HoursFormRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<int> GetYearOfForm(int id)
        {
            return await context.HoursForms.Where(p => p.FormId == id).Select(m => m.Year).SingleOrDefaultAsync();
        }

        public async Task<HoursFormModel> GetFormById(int id)
        {
            var form = await context.HoursForms.Where(p => p.FormId == id).SingleOrDefaultAsync();

            HoursFormModel model = new HoursFormModel
            {
                FormId = form.FormId,
                AccountId = form.AccountId,
                DateSend = form.DateSend,
                DateDue = form.DateDue,
                TotalHours = form.TotalHours,
                Year = form.Year,
                ProjectMonth = form.ProjectMonth,
                IsAcceptedClient = form.IsAcceptedClient,
                IsLocked = form.IsLocked,
                CommentAdmin = form.commentAdmin,
                CommentClient = form.commentClient

            };

            return model;
        }

        //returning all hoursforms, ordered by account Id
        public async Task<List<HoursFormModel>> GetAllHoursForms()
        {

            var models = context.HoursForms
                .Select(p => new HoursFormModel
                {
                    FormId = p.FormId,
                    AccountId = p.AccountId,
                    DateSend = p.DateSend,
                    DateDue = p.DateDue,
                    TotalHours = p.TotalHours,
                    Year = p.Year,
                    ProjectMonth = p.ProjectMonth,
                    IsAcceptedClient = p.IsAcceptedClient,
                    IsLocked = p.IsLocked,
                    CommentAdmin = p.commentAdmin,
                    CommentClient = p.commentClient

                });
            return await models.OrderBy(m => m.AccountId).ToListAsync();
        }

        //returning all hoursforms where IsAcceptedClient is NOT null
        public async Task<List<AdminTaskModel>> GetAllClientAcceptedForms()
        {
            var formsEntities = await context.HoursForms.Where(p => p.IsAcceptedClient == 1 || p.IsAcceptedClient == 2 ).ToListAsync();

            List<AdminTaskModel> allAdminTasks = new List<AdminTaskModel>();

            foreach (var form in formsEntities) {
                var user = from currentUser in context.UserIdentity.Where(c => c.Id == form.AccountId) select new { Name = currentUser.FirstName + " " + currentUser.LastName };
                
                allAdminTasks.Add(new AdminTaskModel
                {
                    formId = form.FormId,
                    accountId = form.AccountId,
                    FullName = user.First().Name,
                    Info = "Uren registratie " + form.ProjectMonth + " " + form.Year.ToString(),
                    Month = form.ProjectMonth,
                    Year = form.Year,
                    HandInTime = form.DateSend,
                    stateClientCheck = form.IsAcceptedClient,



                });
            }



            return allAdminTasks;
            
        }

        public async Task<List<HoursFormModel>> getAllFormPerAccount(string accountId)
        {
            var formsEntities = await context.HoursForms.Where(p => p.AccountId == accountId && p.IsAcceptedClient != 3).ToListAsync();
            List<HoursFormModel> formPerUser = new List<HoursFormModel>();


            foreach (var form in formsEntities)
            {
                formPerUser.Add(new HoursFormModel
                {
                    FormId = form.FormId,
                    AccountId = form.AccountId,
                    DateDue = form.DateDue,
                    ProjectMonth = form.ProjectMonth,
                    Year = form.Year,
                    IsAcceptedClient = form.IsAcceptedClient
                    
                    
                    //Info = "Uren registratie " + form.ProjectMonth + " " + form.Year.ToString(),
                });
            }
            return formPerUser;
        }

        public async Task RemoveAllFormPerAccount(string accountId)
        {
            var daysforForm = await context.HoursPerDays.Where(p => p.Form.AccountId == accountId).ToListAsync();
            var hourforms = await context.HoursForms.Where(p => p.AccountId == accountId).ToListAsync();

            context.HoursPerDays.RemoveRange(daysforForm);
            context.HoursForms.RemoveRange(hourforms);
            

            await context.SaveChangesAsync();
        }

        //new hoursform
        //public async Task<HoursForm> createHoursForm(HoursFormModel hoursFormModel, int clientId)
        //{
        //    context.HoursForms.Add(hoursFormModel);

        //    await context.SaveChangesAsync();
        //}

        //getting all forms for specific account
        public async Task<List<HoursFormModel>> GetSingleAccountForms(string accountId)
        {
            var formsEntities = await context.HoursForms.Where(p => p.AccountId == accountId).ToListAsync();

            List<HoursFormModel> allFormsForUser = new List<HoursFormModel>();

            foreach (var form in formsEntities)
                allFormsForUser.Add(new HoursFormModel
                {
                    FormId = form.FormId,
                    AccountId = form.AccountId,
                    DateSend = form.DateSend,
                    DateDue = form.DateDue,
                    TotalHours = form.TotalHours,
                    Year = form.Year,
                    ProjectMonth = form.ProjectMonth,
                    IsAcceptedClient = form.IsAcceptedClient,
                    IsLocked = form.IsLocked,
                    CommentAdmin = form.commentAdmin,
                    CommentClient = form.commentClient
                });


            return allFormsForUser;

        }

        //getting a single form
        //public async Task<HoursForm> GetSingleForm(int formId)
        //{
        //    return await context.HoursForms.FindAsync(formId);
        //}

        //edit the form
        public async Task<HoursFormModel> EditForm(HoursFormModel editform)
        {


            HoursForm entity = context.HoursForms.Single(p => p.FormId == editform.FormId);
            entity.FormId = editform.FormId;
            entity.AccountId = editform.AccountId;
            entity.DateSend = editform.DateSend;
            entity.DateDue = editform.DateDue;
            entity.TotalHours = editform.TotalHours;
            entity.Year = editform.Year;
            entity.ProjectMonth = editform.ProjectMonth;
            entity.IsAcceptedClient = editform.IsAcceptedClient;
            entity.IsLocked = editform.IsLocked;
            entity.commentAdmin = editform.CommentAdmin;
            entity.commentClient = editform.CommentClient;

            await context.SaveChangesAsync();

            return editform;
        }

        public async Task<HoursFormModel> CreateNewForm(HoursFormModel hoursFormModel)
        {
            DateTime day = DateTime.Today;
            var firstDay = new DateTime(day.Year, day.Month, 1);
            int days = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            HoursForm hoursForm = new HoursForm()
            {
                AccountId = hoursFormModel.AccountId,
                DateSend = hoursFormModel.DateSend,
                DateDue = firstDay.AddDays(days + 5),
                TotalHours = hoursFormModel.TotalHours,
                ProjectMonth = hoursFormModel.ProjectMonth,
                Year = hoursFormModel.Year,
                IsAcceptedClient = 0,

                IsLocked = hoursFormModel.IsLocked,
                commentAdmin = hoursFormModel.CommentAdmin,
                commentClient = hoursFormModel.CommentClient
                
            };

            context.HoursForms.Add(hoursForm);
            await context.SaveChangesAsync();

            var DaysinMonth = 0;

            switch (hoursForm.ProjectMonth)
            {
                case "januari":
                    DaysinMonth = 31;
                    break;
                case "februari":
                    if (DateTime.IsLeapYear(Convert.ToInt32(hoursForm.Year)) == true)
                    {
                        DaysinMonth = 29;
                    }
                    else
                    {
                        DaysinMonth = 28;
                    }
                    break;
                case "maart":
                    DaysinMonth = 31;
                    break;
                case "april":
                    DaysinMonth = 30;
                    break;
                case "mei":
                    DaysinMonth = 31;
                    break;
                case "juni":
                    DaysinMonth = 30;
                    break;
                case "juli":
                    DaysinMonth = 31;
                    break;
                case "augustus":
                    DaysinMonth = 31;
                    break;
                case "september":
                    DaysinMonth = 30;
                    break;
                case "oktober":
                    DaysinMonth = 31;
                    break;
                case "november":
                    DaysinMonth = 30;
                    break;
                case "december":
                    DaysinMonth = 31;
                    break;
            }

            while (DaysinMonth > 0)
            {

                context.HoursPerDays.Add(new HoursPerDay
                {
                    FormId = hoursForm.FormId,
                    Day = DaysinMonth,
                    Month = hoursForm.ProjectMonth,
                    Hours = 0,
                    OverTimeHours = 0,
                    Training = 0,
                    IsLeave = 0,
                    Other = 0,
                    Reasoning = "",
                    ClientId = null,
                    IsSick = 0

                });

                DaysinMonth--;
                await context.SaveChangesAsync();
            }

            return hoursFormModel;
        }

        public async Task<List<YearOverviewModel>> GetYearOverviews(int year, List<string> Traineeids, List<string> Employeeids, List<string> SoftDevids)
        {
            List<string> months = new List<string>() { "januari", "februari", "maart", "april", "mei", "juni", "juli", "augustus", "september", "oktober", "november", "december" };
            List<YearOverviewModel> yearOverviews = new List<YearOverviewModel>();
            foreach (string month in months)
            {
                YearOverviewModel yom = new YearOverviewModel();
                yom.Month = month;
                yom.Year = year;
                foreach (string id in Traineeids)
                {
                    int? hours = await context.HoursForms.Where(p => p.AccountId == id && p.Year == year && p.ProjectMonth == month).Select(m => m.TotalHours).SingleOrDefaultAsync();
                    if (hours != null)
                    {
                        yom.HoursTrainee += Convert.ToInt32(hours);
                        yom.TotalHours += Convert.ToInt32(hours);
                    }
                }
                foreach (string id in Employeeids)
                {
                    int? hours = await context.HoursForms.Where(p => p.AccountId == id && p.Year == year && p.ProjectMonth == month).Select(m => m.TotalHours).SingleOrDefaultAsync();
                    if (hours != null)
                    {
                        yom.HoursTrainee += Convert.ToInt32(hours);
                        yom.TotalHours += Convert.ToInt32(hours);
                    }
                }
                foreach (string id in SoftDevids)
                {
                    int? hours = await context.HoursForms.Where(p => p.AccountId == id && p.Year == year && p.ProjectMonth == month).Select(m => m.TotalHours).SingleOrDefaultAsync();
                    if (hours != null)
                    {
                        yom.HoursTrainee += Convert.ToInt32(hours);
                        yom.TotalHours += Convert.ToInt32(hours);
                    }
                }
                yearOverviews.Add(yom);
            }
            return yearOverviews;
        }

        public async Task<List<FormsForMonthModel>> GetFormsForYearAndMonth(int year, string month)
        {
            List<HoursForm> formsEntities = await context.HoursForms.Where(p => p.Year == year && p.ProjectMonth == month).ToListAsync();
            List<FormsForMonthModel> formsForYearandMonth = new List<FormsForMonthModel>();
            foreach (var form in formsEntities)
                formsForYearandMonth.Add(new FormsForMonthModel
                {
                    FormId = form.FormId,
                    AccountId = form.AccountId,
                    fullName = await context.UserIdentity.Where(p => p.Id == form.AccountId).Select(m => m.FirstName +" " + m.LastName).SingleOrDefaultAsync(),
                    DateSend = form.DateSend,
                    DateDue = form.DateDue,
                    TotalHours = form.TotalHours,
                    Year = form.Year,
                    ProjectMonth = form.ProjectMonth,
                    IsAcceptedClient = form.IsAcceptedClient,
                    IsLocked = form.IsLocked,
                    CommentAdmin = form.commentAdmin,
                    CommentClient = form.commentClient
                }) ;
            return formsForYearandMonth;
        }
        public async Task<HoursFormModel> GetFormsById(int formid)
        {
            HoursForm hoursForm = await context.HoursForms.SingleAsync(a => a.FormId == formid);


            return new HoursFormModel
            {
                FormId = hoursForm.FormId,
                ProjectMonth = hoursForm.ProjectMonth,
                Year = hoursForm.Year
            };
        }

        public async Task ChangeState(int state, int id, string textAdmin, string textClient)
        {
            HoursForm entity = await context.HoursForms.SingleAsync(p => p.FormId == id);
            entity.IsAcceptedClient = state;
            entity.commentAdmin = textAdmin;
            entity.commentClient = textClient;
            if(state == 3)
            {
                entity.IsLocked = true;
            }

            await context.SaveChangesAsync();
        }

        public async Task UpdateTotalHoursForm(int id, int totalHours)
        {
            HoursForm entity = await context.HoursForms.SingleAsync(p => p.FormId == id);
            entity.TotalHours = totalHours;
            await context.SaveChangesAsync();
        }
    }
}
