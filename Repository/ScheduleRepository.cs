using HandyMan.Data;
using HandyMan.Interfaces;
using HandyMan.Models;
using Microsoft.EntityFrameworkCore;

namespace HandyMan.Repository
{
    public class ScheduleRepository : IScheduleRepository
    {
        private readonly Handyman_DBContext context;

        public ScheduleRepository(Handyman_DBContext context)
        {
            this.context = context;
        }

        public async void CreateSchedule(Schedule schedule)
        {
            await context.Schedules.AddAsync(schedule);
        }

        

        public void DeleteScheduleById(int id, DateTime date, TimeSpan time)
        {
            Schedule schedule = context.Schedules.FirstOrDefault(a => a.Handy_SSN == id && a.Schedule_Date.Equals(date) && a.Time_From.Equals(time));
            context.Remove(schedule);
        }

        public void EditSchedule(Schedule schedule)
        {
            context.Entry(schedule).State = EntityState.Modified;
        }

        public async Task<IEnumerable<Schedule>> GetScheduleAsync()
        {
            return await context.Schedules.ToListAsync();
        }

        public async Task<Schedule> GetScheduleByHandymanSsnAsync(int id, DateTime date, TimeSpan time)
        {
            Schedule schedule = context.Schedules.FirstOrDefault(a => a.Handy_SSN == id && a.Schedule_Date.Equals(date) && a.Time_From.Equals(time));
            return schedule;
        }


        public Task<bool> SaveAllAsync()
        {
            throw new NotImplementedException();
        }
    }
}
