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

        //When a user logs in this method checks if the account already has a Hoursform for a specific month and year.
        //When there is no Hoursform connected to the account then one is created.
        public async Task<HoursFormModel> CheckIfExists(string id, string month, int year)
        {
            var form = await context.HoursForms.Where(p => p.AccountId == id && p.ProjectMonth == month && p.Year == year).SingleOrDefaultAsync();
            if (form != null) {
                HoursFormModel model = new HoursFormModel
                {
                    FormId = form.FormId,
                    AccountId = form.AccountId,
                    DateSend = form.DateSend,
                    DateDue = form.DateDue,
                    TotalHours = form.TotalHours,
                    TotalLeave = form.TotalLeave,
                    TotalOver = form.TotalOver,
                    TotalTraining = form.TotalTraining,
                    TotalOther = form.TotalOther,
                    TotalSick = form.TotalSick,
                    Year = form.Year,
                    ProjectMonth = form.ProjectMonth,
                    IsAcceptedClient = form.IsAcceptedClient,
                    IsLocked = form.IsLocked,
                    CommentAdmin = form.commentAdmin,
                    CommentClient = form.commentClient

                };
                return model;
            }
            return null;
        }


        //If the user picks a year this method will show all the Hoursforms with the specific year. 
        //Example: If the user picks the year 2018. It will show all the Hoursforms where the year is 2018.
        public async Task<int> GetYearOfForm(int id)
        {
            return await context.HoursForms.Where(p => p.FormId == id).Select(m => m.Year).SingleOrDefaultAsync();
        }


        //If the user or admin wants to see a specific hoursform this method search for the specific Hoursform ID which is selected.
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
                TotalLeave = form.TotalLeave,
                TotalTraining = form.TotalTraining,
                TotalOver = form.TotalOver,
                TotalOther = form.TotalOther,
                TotalSick = form.TotalSick,
                Year = form.Year,
                ProjectMonth = form.ProjectMonth,
                IsAcceptedClient = form.IsAcceptedClient,
                IsLocked = form.IsLocked,
                CommentAdmin = form.commentAdmin,
                CommentClient = form.commentClient,
                Verification_code = form.Verification_code

            };
            return model;
        }

        //The GetAllHoursForms() method returns all Hoursforms from all Accounts (Trainees, Employees, Developers).
        //When the Hoursforms are picked up by the method they get ordered by the ID of the accounts.
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
                    TotalLeave = p.TotalLeave,
                    TotalTraining = p.TotalTraining,
                    TotalOver = p.TotalOver,
                    TotalOther = p.TotalOther,
                    TotalSick = p.TotalSick,
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


        //This method gets all the forms that are available for an account en shows them to the user
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
                });
            }
            return formPerUser;
        }


        //When an account is deleted by the admin this method makes sure all the existing Hourform (and HoursPerDay) that are connected to the Account(ID) are also deleted.
        public async Task RemoveAllFormPerAccount(string accountId)
        {
            var daysforForm = await context.HoursPerDays.Where(p => p.Form.AccountId == accountId).ToListAsync();
            var hourforms = await context.HoursForms.Where(p => p.AccountId == accountId).ToListAsync();

            context.HoursPerDays.RemoveRange(daysforForm);
            context.HoursForms.RemoveRange(hourforms);
            

            await context.SaveChangesAsync();
        }

        //Get all forms of a single account
        public async Task<List<HoursFormModel>> GetSingleAccountForms(string accountId, int year)
        {
            var formsEntities = await context.HoursForms.Where(p => p.AccountId == accountId && p.Year == year || p.AccountId == accountId && year == 0 ).OrderByDescending(m => m.Year).ToListAsync();

            List<HoursFormModel> allFormsForUser = new List<HoursFormModel>();

            foreach (var form in formsEntities)
                allFormsForUser.Add(new HoursFormModel
                {
                    FormId = form.FormId,
                    AccountId = form.AccountId,
                    DateSend = form.DateSend,
                    DateDue = form.DateDue,
                    TotalHours = form.TotalHours,
                    TotalSick = form.TotalSick,
                    TotalOther = form.TotalOther,
                    TotalOver = form.TotalOver,
                    TotalLeave = form.TotalLeave,
                    TotalTraining = form.TotalTraining,
                    Year = form.Year,
                    ProjectMonth = form.ProjectMonth,
                    IsAcceptedClient = form.IsAcceptedClient,
                    IsLocked = form.IsLocked,
                    CommentAdmin = form.commentAdmin,
                    CommentClient = form.commentClient
                });


            return allFormsForUser;

        }

        //When a user edits his or hers Hoursform (and HoursPerDay (for example changes the hour for a day)) this method makes sure the changes are saved.

        public async Task<HoursFormModel> EditForm(HoursFormModel editform)
        {


            HoursForm entity = context.HoursForms.Single(p => p.FormId == editform.FormId);
            entity.FormId = editform.FormId;
            entity.AccountId = editform.AccountId;
            entity.DateSend = editform.DateSend;
            entity.DateDue = editform.DateDue;
            entity.TotalHours = editform.TotalHours;
            entity.TotalSick = editform.TotalSick;
            entity.TotalLeave = editform.TotalLeave;
            entity.TotalTraining = editform.TotalTraining;
            entity.TotalOver = editform.TotalOver;
            entity.TotalOther = editform.TotalOther;
            entity.Year = editform.Year;
            entity.ProjectMonth = editform.ProjectMonth;
            entity.IsAcceptedClient = editform.IsAcceptedClient;
            entity.IsLocked = editform.IsLocked;
            entity.commentAdmin = editform.CommentAdmin;
            entity.commentClient = editform.CommentClient;

            await context.SaveChangesAsync();

            return editform;
        }


        //When the admin creates an account at the same time this method is evoked.
        //The method makes sure an HoursformModel is created with the right information
        //the switch statement looks for the right months and adds the right amount of days to the HourPerDayModel
        public async Task<HoursFormModel> CreateNewForm(HoursFormModel hoursFormModel, int ClientId)
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
                commentClient = hoursFormModel.CommentClient,
                Verification_code = Guid.NewGuid()
                
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
                    ClientId = ClientId,
                    IsSick = 0

                });

                DaysinMonth--;
                await context.SaveChangesAsync();
            }

            return hoursFormModel;
        }


        //This method looks up every hoursform from every Employee en counts up all hours (sick, vacation etc)
        //It distincts every month of every year.
        //It loops trough every month and every year and then loops trough all the trainees, employees and developers.
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

        //This method gets all Hoursform of a specific month. 
        //The year can be chosen in the view if you click on the link you get every Hourforms of the month you choose.
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
                    TotalSick = form.TotalSick,
                    TotalOver = form.TotalOver,
                    TotalTraining = form.TotalTraining,
                    TotalLeave = form.TotalLeave,
                    TotalOther = form.TotalOther,
                    Year = form.Year,
                    ProjectMonth = form.ProjectMonth,
                    IsAcceptedClient = form.IsAcceptedClient,
                    IsLocked = form.IsLocked,
                    CommentAdmin = form.commentAdmin,
                    CommentClient = form.commentClient
                }) ;
            return formsForYearandMonth;
        }

        //With this method you can choose one Hoursform by its ID
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

        //This method changes the state of a Hoursform
        //For Example: If an employee saves and sends his of hers Hoursform, the state changes and the employee can no longer change the Hoursform
        // The same goes for CLient, if he or she accepts the hoursform the state changes so the admin can accept or reject the hoursform
        public async Task ChangeState(int state, int id, string textClient, string textAdmin = null)
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

        //When someone changes his or hers Hoursform the totalhours gets updated.
        public async Task UpdateTotalHoursForm(int id, int totalHours, int totalSick, int totalOver, int totalLeave, int totalOther, int TotalTraining)
        {
            HoursForm entity = await context.HoursForms.SingleAsync(p => p.FormId == id);
            entity.TotalHours = totalHours;
            entity.TotalSick = totalSick;
            entity.TotalOver = totalOver;
            entity.TotalLeave = totalLeave;
            entity.TotalTraining = TotalTraining;
            entity.TotalOther = totalOther;
            await context.SaveChangesAsync();
        }

        //Get all Hoursform from one account of a specific year.
        public async Task<List<HoursFormModel>> GetAllFormsForAccountForYear(int year, string id)
        {
            List<HoursForm> formsEntities = await context.HoursForms.Where(p => p.Year == year && p.AccountId == id).ToListAsync();
            List<HoursFormModel> formsForYearandMonth = new List<HoursFormModel>();
            foreach (var form in formsEntities)
                formsForYearandMonth.Add(new HoursFormModel
                {
                    FormId = form.FormId,
                    AccountId = form.AccountId,
                    DateSend = form.DateSend,
                    DateDue = form.DateDue,
                    TotalHours = form.TotalHours,
                    TotalSick = form.TotalSick,
                    TotalOver = form.TotalOver,
                    TotalTraining = form.TotalTraining,
                    TotalLeave = form.TotalLeave,
                    TotalOther = form.TotalOther,
                    Year = form.Year,
                    ProjectMonth = form.ProjectMonth,
                    IsAcceptedClient = form.IsAcceptedClient,
                    IsLocked = form.IsLocked,
                    CommentAdmin = form.commentAdmin,
                    CommentClient = form.commentClient
                });
            return formsForYearandMonth;
        }


        //With this method the admin can get all hourforms of one user/account.
        public async Task<List<int>> GetAllYearsForUser(string id)
        {
            List<int> Years = await context.HoursForms.Where(p => p.AccountId == id).Select(m => m.Year).Distinct().OrderBy(x => x).ToListAsync();
            return Years;
        }

        //This method is for the dropdown menu. If a year has an hoursform it can be selected in the dropdownmenu (it becomes an option)
        public async Task<List<int>> GetAllExistingYears(int year)
        {
            List<int> Years = await context.HoursForms.Select(m => m.Year).Distinct().OrderBy(x => x).ToListAsync();
            return Years;
        }

        //This method is for the Admin Dashboard. There is a panel where you can see all the total hours of the current year.
        //It sums up every totalhours of every form and shows the data on the dashboard per Month (and current year) 
        public async Task<List<AllHoursYearModel>> GetAllHoursYear(int currYear)
        {
            List<HoursForm> hoursForms = await context.HoursForms.Where(p => p.Year == currYear).ToListAsync();
            List<string> months = new List<string>() { "januari", "februari", "maart", "april", "mei", "juni", "juli", "augustus", "september", "oktober", "november", "december" };
            List<AllHoursYearModel> models = new List<AllHoursYearModel>();

            foreach(string month in months)
            {
                AllHoursYearModel model = new AllHoursYearModel() { };
                model.Month = month;

                int totalHours = 0;
                int totalSick = 0;
                int totalOvertime = 0;
                int totalTraining = 0;
                int totalLeave = 0;
                int totalOther = 0;

                foreach(var hf in hoursForms)
                {
                    if (hf.ProjectMonth == month)
                    {
                        totalHours += hf.TotalHours;
                        totalSick += hf.TotalSick;
                        totalLeave += hf.TotalLeave;
                        totalOvertime += hf.TotalOver;
                        totalOther += hf.TotalOther;
                        totalTraining += hf.TotalTraining;
                    }

                }

                model.TotalHours = totalHours;
                model.TotalSick = totalSick;
                model.TotalOvertime = totalOvertime;
                model.TotalLeave = totalLeave;
                model.TotalOther = totalOther;
                model.TotalTraining = totalTraining;

                models.Add(model);


            }

            return (models);

    }
}
}
