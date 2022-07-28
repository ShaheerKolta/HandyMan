using HandyMan.Models;

namespace HandyMan.Interfaces
{
    public interface IHandymanRepository
    {
        Task<IEnumerable<Handyman>> GetHandyManAsync();
        Task<IEnumerable<Handyman>> GetVerifiedHandyManAsync();
        Task<Handyman> GetHandymanByIdAsync(int id);
        void CalculateHandymanRate(Handyman handyman);
        void CreateHandyman(Handyman handyman);
        void EditHandyman(Handyman handyman);
        void DeleteHandymanById(int id);
        bool ApproveHandymanById(Handyman handyman);
        Task<bool> SaveAllAsync();

    }
}
