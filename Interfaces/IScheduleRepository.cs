using HandyMan.Models;

namespace HandyMan.Interfaces
{
    public interface IScheduleRepository
    {
        Task<IEnumerable<Schedule>> GetScheduleAsync();
        Task<Schedule> GetScheduleByHandymanSsnAsync(int id,DateTime date , TimeSpan time);
        void CreateSchedule(Schedule schedule);
        void EditSchedule(Schedule schedule);
        void DeleteScheduleById(int id, DateTime date, TimeSpan time);
        Task<bool> SaveAllAsync();
    }
}
