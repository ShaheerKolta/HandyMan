using HandyMan.Data;
using HandyMan.Interfaces;
using HandyMan.Models;
using Microsoft.EntityFrameworkCore;

namespace HandyMan.Repository
{
    public class HandymanRepository : IHandymanRepository
    {
        private readonly Handyman_DBContext context;

        public HandymanRepository(Handyman_DBContext _context)
        { 
            context = _context;
        }
        public async void CreateHandyman(Handyman handyman)
        {
            await context.Handymen.AddAsync(handyman);
        }

        public void DeleteHandymanById(int id)
        {
            context.Handymen.Remove(context.Handymen.Find(id));
        }

        public void EditHandyman(Handyman handyman)
        {
            context.Entry(handyman).State = EntityState.Modified;
        }

        public async Task<IEnumerable<Handyman>> GetHandyManAsync()
        {
            return await context.Handymen.ToListAsync();
        }

        public async Task<IEnumerable<Handyman>> GetVerifiedHandyManAsync()
        {
            return await context.Handymen.Where(a=>a.Approved==true).ToListAsync();
        }

        public async Task<Handyman> GetHandymanByIdAsync(int id)
        {
            return await context.Handymen.FindAsync(id);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }
    }
}
