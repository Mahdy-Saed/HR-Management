using HR_Carrer.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace HR_Carrer.Data.Repositery
{
    public interface IRequestRepo
    {
        Task AddAsync(Requests request);

        Task<IEnumerable<Requests>> GetAllAsync();

        Task<IEnumerable<Requests>> GetAllWithQuery(int? id=null, string? name=null);

        Task<Requests?> GetByIdAsync(int id);

        Task<Requests?> GetByIdWithEMployeeAsync(Guid id);


        Task UpdateAsync(Requests request);

        Task DeleteAsync(int id);

        Task DeleteAllAsync();

    }

    public class RequestRepo : IRequestRepo
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

        public async Task<IEnumerable<Requests>> GetAllWithQuery(int? id = null, string? name = null)
        {
            var query =   _context.Requests.AsQueryable();
            
            query = id.HasValue? query.Where(r => r.Id == id) : query;

            query = !string.IsNullOrEmpty(name) ? query.Where(r => r.Title == name) : query;

             return await query.ToListAsync();

        }

        public async Task<Requests?> GetByIdAsync(int id)
        {
           return await _context.Requests.FindAsync(id);
        }

        public async Task<Requests?> GetByIdWithEMployeeAsync(Guid id)
        {
            return await _context.Requests.FirstOrDefaultAsync(r => r.EmployeeId == id);
        }

        public async Task UpdateAsync(Requests request)
        {
            _context.Requests.Update(request);

            await _context.SaveChangesAsync();
        }
    }

}
