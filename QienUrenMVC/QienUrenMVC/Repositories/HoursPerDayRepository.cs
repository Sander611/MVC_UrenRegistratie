using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QienUrenMVC.Data;
using QienUrenMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QienUrenMVC.Repositories
{
    public class HoursPerDayRepository : IHoursPerDayRepository
    {
        private readonly ApplicationDbContext context;
        public HoursPerDayRepository(ApplicationDbContext _context)
        {
            context = _context;
        }

        private readonly HoursPerDay hoursperday;
        public HoursPerDayRepository(HoursPerDay hoursperday)
        {
            this.hoursperday = hoursperday;
        }
        private readonly Client client;
        public HoursPerDayRepository(Client client)
        {
            this.client = client;
        }

        public async Task<List<HoursPerDayModel>> Update(List<HoursPerDayModel> daychangeList)
        {
            foreach (var daychange in daychangeList)
            {
                var entity = context.HoursPerDays.Single(p => p.HoursPerDayId == daychange.HoursPerDayId);
                entity.HoursPerDayId = daychange.HoursPerDayId;
                entity.FormId = daychange.FormId;
                entity.Day = daychange.Day;
                entity.Hours = daychange.Hours;
                entity.Month = daychange.Month;
                entity.Training = daychange.Training;
                entity.IsLeave = daychange.IsLeave;
                entity.IsSick = daychange.IsSick;
                entity.Other = daychange.Other;
                entity.OverTimeHours = daychange.OverTimeHours;
                entity.Reasoning = daychange.Reasoning;
                entity.ClientId = daychange.ClientId;

                await context.SaveChangesAsync();
            }

            return daychangeList;
        }

        public async Task<List<HoursPerDayModel>> GetAllDaysForForm(int formId)
        {
            var allDaysForFormId = new List<HoursPerDayModel>();

            foreach (var day in await context.HoursPerDays.Where(p => p.FormId == formId).OrderBy(x => x.Day).ToListAsync())
                allDaysForFormId.Add(new HoursPerDayModel
                {
                    FormId = day.FormId,
                    Day = day.Day,
                    Hours = day.Hours,
                    Month = day.Month,
                    Training = day.Training,
                    IsLeave = day.IsLeave,
                    IsSick = day.IsSick,
                    Other = day.Other,
                    OverTimeHours = day.OverTimeHours,
                    ClientId = day.ClientId,
                    HoursPerDayId = day.HoursPerDayId,
                    Reasoning = day.Reasoning
                });

            return allDaysForFormId;
        }

        public  IEnumerable<SelectListItem> GetClientList()
        {
                 List<SelectListItem> clients = context.Clients.AsNoTracking()
                     .OrderBy(n => n.CompanyName)
                     .Select(n =>
                     new SelectListItem
                     {
                         Value = Convert.ToString(n.ClientId),
                         Text = n.CompanyName
                     }).ToList();
                return new SelectList(clients, "Value", "Text");
        }

        public async Task<HoursPerDayModel> GetAllFormsByClientId(int id)
        {
            HoursPerDay entity = await context.HoursPerDays.Where(p => p.ClientId == id).FirstOrDefaultAsync();

            HoursPerDayModel model = new HoursPerDayModel() { };
            if (entity != null) {

                model.FormId = entity.FormId;
                model.Day = entity.Day;
                model.Hours = entity.Hours;
                model.Month = entity.Month;
                model.Training = entity.Training;
                model.IsLeave = entity.IsLeave;
                model.IsSick = entity.IsSick;
                model.Other = entity.Other;
                model.OverTimeHours = entity.OverTimeHours;
                model.ClientId = entity.ClientId;
                model.HoursPerDayId = entity.HoursPerDayId;
                model.Reasoning = entity.Reasoning;

            }
            else
            {
                model = null;
            }


            return (model);
        }































        //public async Task<List<HoursPerDay>> GetAllDaysFromOneForm(int formId)
        //{
        //    var form = await context.HoursPerDays.SingleAsync(p => p.FormId == formId);
        //    return new List<HoursPerDay>
        //    {

        //    };
        //}



        //public async Task<HoursPerDay> SaveADay(HoursPerDay dayedit)
        //{
        //    HoursPerDay newHoursPerDay = new HoursPerDay()
        //    {
        //        HoursPerDayId = dayedit.HoursPerDayId,

        //        ClientId = dayedit.ClientId,
        //        Day = dayedit.Day,
        //        Month = dayedit.Month,
        //        Other = dayedit.Other,
        //        Hours = dayedit.Hours,
        //        FormId = dayedit.FormId,
        //        Training = dayedit.Training,
        //        Year = dayedit.Year,
        //        IsLeave = dayedit.IsLeave,
        //        IsSick = dayedit.IsSick,
        //        ProjectDay = dayedit.ProjectDay,
        //        OverTimeHours = dayedit.OverTimeHours,
        //        Reasoning = dayedit.Reasoning
        //    };

        //    context.HoursPerDays.Add(newHoursPerDay);
        //    await context.SaveChangesAsync();
        //    return dayedit;
        //}
    }
}
