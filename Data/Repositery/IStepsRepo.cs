using HR_Carrer.Entity;
using Microsoft.EntityFrameworkCore;

namespace HR_Carrer.Data.Repositery
{
    public interface IStepsRepo
    {

        Task AddAsync(Steps step);

        Task<IEnumerable<Steps>> GetAllAsync();

        Task<Steps?> GetByIdAsync(int id);

        Task UpdateAsync(Steps step);

        Task DeleteAsync(int id);

        Task DeleteAllAsync();

    }

    public class StepsRepo : IStepsRepo
    {
        private readonly ApplicationDbContext _context;
        public StepsRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Steps step)
        {
            await _context.Steps.AddAsync(step);
            await _context.SaveChangesAsync();

        }

        public async Task DeleteAllAsync()
        {
            _context.Steps.RemoveRange(_context.Steps);
            await _context.SaveChangesAsync();

        }

        public async Task DeleteAsync(int id)
        {
            var step =  GetByIdAsync(id);
            if(step is not null)
            {
                _context.Steps.Remove(step.Result);
               await  _context.SaveChangesAsync();
            }

        }

        public async Task<IEnumerable<Steps>> GetAllAsync()
        {
            return await _context.Steps.ToListAsync();
        }

        public async Task<Steps?> GetByIdAsync(int id)
        {
             return await _context.Steps.FirstOrDefaultAsync(s => s.Id == id);

        }

        public async Task UpdateAsync(Steps step)
        {
            _context.Steps.Update(step);
            await _context.SaveChangesAsync();

        }
    }



}
