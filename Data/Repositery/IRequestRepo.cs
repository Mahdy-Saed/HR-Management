using HR_Carrer.Entity;
using Microsoft.EntityFrameworkCore;

namespace HR_Carrer.Data.Repositery
{
    public interface IRequestRepo
    {
        Task AddAsync(Requests request);

        Task<IEnumerable<Requests>> GetAllAsync();

        Task<Requests?> GetByIdAsync(int id);

        Task UpdateAsync(Requests request);

        Task DeleteAsync(int id);

        Task DeleteAllAsync();

    }

    public class RequestRepo:IRequestRepo
    {
        private readonly ApplicationDbContext _context;
        public RequestRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Requests request)
        {
           await _context.Requests.AddAsync(request);
            await _context.SaveChangesAsync();

        }

        public async Task DeleteAllAsync()
        {
            _context.Requests.RemoveRange(_context.Requests);
            await _context.SaveChangesAsync();

        }

        public async Task DeleteAsync(int id)
        {
            var request = await  GetByIdAsync(id);
            if (request is not null)
            {
                _context.Requests.Remove(request);
               await   _context.SaveChangesAsync();
            }

        }

        public async Task<IEnumerable<Requests>> GetAllAsync()
        {
            return await _context.Requests.ToListAsync();
        }

        public async Task<Requests?> GetByIdAsync(int id)
        {
           return await _context.Requests.FindAsync(id);
        }

        public async Task UpdateAsync(Requests request)
        {
            _context.Requests.Update(request);

            await _context.SaveChangesAsync();
        }
    }

}
