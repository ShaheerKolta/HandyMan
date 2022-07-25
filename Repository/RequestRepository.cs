using AutoMapper;
using HandyMan.Data;
using HandyMan.Dtos;
using HandyMan.Interfaces;
using HandyMan.Models;
using Microsoft.EntityFrameworkCore;

namespace HandyMan.Repository
{
    public class RequestRepository : IRequestRepository
    {
        private readonly Handyman_DBContext _context;

        public RequestRepository(Handyman_DBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Request>> GetRequestsAsync()
        {
            return await _context.Requests.ToListAsync();
        }

        public async Task<Request> GetRequestByIdAsync(int id)
        {
            return await _context.Requests.FindAsync(id);
        }

        public async void CreateRequest(Request request)
        {
            await _context.Requests.AddAsync(request);
        }
        public void EditRequest(Request request)
        {
            _context.Entry(request).State = EntityState.Modified;
        }

        public void DeleteRequestById(int id)
        {
            _context.Requests.Remove(_context.Requests.Find(id));
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
