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

        public async Task<Request> GetPrevRequest(int id, int role)
        {
            //if role == 0 role is client if role ==1 then role is Handyman
            if (role == 0)
            {
                var request = await _context.Requests.Where(c => c.Client_ID == id && c.Request_Status == 2 && c.Handy_Rate==null && c.Request_Order_Date < DateTime.Now && c.Request_Date > DateTime.Now.AddDays(1)).OrderByDescending(a=>a.Request_Order_Date).FirstOrDefaultAsync();
                return request;
            }
            else
            {
                var request = await _context.Requests.Where(c => c.Handyman_SSN == id && c.Request_Status == 2 && c.Client_Rate == null && c.Request_Order_Date < DateTime.Now && c.Request_Date > DateTime.Now.AddDays(1)).OrderByDescending(a => a.Request_Order_Date).FirstOrDefaultAsync();
                return request;
            }
        }


        public async void CreateRequest(Request request)
        {
            
            await _context.Requests.AddAsync(request);
        }

        public async void CreatePaymentByRequestId(int id)
        {
            var request = await _context.Requests.FindAsync(id);
            Payment requestPayment = new Payment();
            requestPayment.Request_ID=request.Request_ID;
            var handyman = _context.Handymen.Find(request.Handyman_SSN);
            var fixedRate = handyman.Handyman_Fixed_Rate;
            var clientBalance = _context.Clients.Find(request.Client_ID).Balance;
            requestPayment.Payment_Amount = (int)(fixedRate - clientBalance);
            handyman.Balance+=requestPayment.Payment_Amount;
            _context.SaveChanges();
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




        ///Handyman Functions ///

        public async Task<IEnumerable<Request>> GetRequestsByHandymanSsnAsync(int handymanSsn)
        {
            return await _context.Requests.Where(c => c.Handyman_SSN == handymanSsn).ToListAsync();
        }

        public async Task<IEnumerable<Request>> GetActiveRequestsByHandymanSsnAsync(int handymanSsn)
        {
            return await _context.Requests.Where(c => c.Handyman_SSN == handymanSsn && c.Request_Status == 1).ToListAsync();
        }






        //Client Functions


        public async Task<IEnumerable<Request>> GetRequestsByClientIdAsync(int id)
        {
            var requests = await _context.Requests.Where(c => c.Client_ID == id).ToListAsync();
            foreach (var requ in requests)
            {
                if (requ.Request_Status == 1 && requ.Request_Order_Date.Minute < requ.Request_Order_Date.AddMinutes(1).Minute)
                {
                    requ.Request_Status = 4;
                    EditRequest(requ);

                }
            }
            await SaveAllAsync();

            return requests;
        }



        public async Task<bool> CheckRequestsByClienttoHandyman(int clientid, int handymanSsn)
        {
            var check = await _context.Requests.Where(c => c.Client_ID == clientid && c.Handyman_SSN == handymanSsn && c.Request_Status == 1).ToListAsync();
            if (check.Count == 0)
                return true;
            return false;
        }

        //get client from request not to break validation of ClientController
        public async Task<Client> GetClientFromRequestByIdAsync(int id)
        {
            return await _context.Clients.FindAsync(id);
        }
        
        
        //get Handyman from request not to break validation of ClientController
        public async Task<Handyman> GetHandymanFromRequestByIdAsync(int id)
        {
            return await _context.Handymen.FindAsync(id);
        }


    }
}
