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
                    HoursPerDayId = day.HoursPerDayId
                });

            return allDaysForFormId;
        }

        public async Task RemoveAllDaysForForm(int formId)
        {
            var allDaysForFormId = await context.HoursPerDays.Where(p => p.FormId == formId).ToListAsync();

            foreach (var day in allDaysForFormId)
            {
                context.HoursPerDays.Remove(day);
            }

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
