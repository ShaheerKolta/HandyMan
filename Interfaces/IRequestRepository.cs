using HandyMan.Models;

namespace HandyMan.Interfaces
{
    public interface IRequestRepository
    {
        Task<IEnumerable<Request>> GetRequestsAsync();
        Task<Request> GetRequestByIdAsync(int id);
        void CreateRequest(Request request);
        void EditRequest(Request request);
        void DeleteRequestById(int id);
        Task<bool> SaveAllAsync();
    }
}
