using HR_Carrer.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace HR_Carrer.Data.Repositery
{
    public interface ISkillRepo
    {
        // add , get, update, delete              //task mean return nothing but if write it like this Task<T> mean return this type of T
        Task AddAsync(Skills skill);

        Task<IEnumerable<Skills>> GetAllAsync();

        Task<Skills?> GetByIdAsync(int id);

        Task<Skills?> GetByNameAsync(string Name);

            Task<IEnumerable<Skills>> GetAllWithQueryAsync(int? id=null, string? name = null, string? Email = null);


        Task UpdateAsync(Skills skill);

        Task DeleteAsync(int id);

        Task DeleteAllAsync();




    }

    public class SkillRepo : ISkillRepo
    {
        private readonly ApplicationDbContext _context;
        public SkillRepo(ApplicationDbContext context)
        
        {
           _context = context;      
        
        }

        public async Task AddAsync(Skills skill)
        {
            await _context.Skills.AddAsync(skill);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAllAsync()
        {
            _context.Skills.RemoveRange(_context.Skills);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var skill = await GetByIdAsync(id);
            if (skill is not null)
            {
                _context.Skills.Remove(skill);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Skills>> GetAllAsync()
        {
            return await _context.Skills.ToListAsync();
        }

        public async Task<IEnumerable<Skills>> GetAllWithQueryAsync(int? id = null, string? name = null, string? Email = null)
        {
            var query = _context.Skills.AsQueryable();

            query=id.HasValue?query.Where(s => s.Id == id.Value) : query;
            query = !string.IsNullOrEmpty(name) ? query.Where(s => s.Name!.Contains(name)) : query;

            return await query.ToListAsync();


        }

        public async Task<Skills?> GetByIdAsync(int id)
        {
            return await _context.Skills.FindAsync(id);

        }

        public async Task<Skills?> GetByNameAsync(string Name)
        {
            return await _context.Skills.FirstOrDefaultAsync(s => s.Name == Name);
        }

        public async Task UpdateAsync(Skills skill)
        {
            _context.Skills.Update(skill);
            await _context.SaveChangesAsync();
        }
    }



}
