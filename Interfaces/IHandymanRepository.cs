using HandyMan.Models;

namespace HandyMan.Interfaces
{
    public interface IHandymanRepository
    {
        Task<IEnumerable<Handyman>> GetHandyManAsync();
        Task<Handyman> GetHandymanByIdAsync(int id);
        void CreateHandyman(Handyman handyman);
        void EditHandyman(Handyman handyman);
        void DeleteHandymanById(int id);
        Task<bool> SaveAllAsync();

    }
}
