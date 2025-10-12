using HR_Carrer.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace HR_Carrer.Data.Repositery
{
    public interface IRoadmapRepo
    {

        Task AddAsync(Roadmap roadmap);

        Task<IEnumerable<Roadmap>> GetAllAsync();

        Task<Roadmap?> GetByIdAsync(int id);

        Task<Roadmap?> GetByEmployeeId(Guid id, string RoamapTitle);


        Task<IEnumerable<Roadmap>> GetAllAsyncWithQuery(int? id=null, string? title=null);
        Task UpdateAsync(Roadmap certificate);

        Task DeleteAsync(int id);

        Task DeleteAllAsync();


    }

    public class RoadmapRepo : IRoadmapRepo
    {
        private readonly ApplicationDbContext _context;
        public RoadmapRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Roadmap roadmap)
        {

            await _context.Roadmaps.AddAsync(roadmap);
            await _context.SaveChangesAsync();

        }

        public async Task DeleteAllAsync()
        {
            _context.Roadmaps.RemoveRange(_context.Roadmaps);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var roadmap = await GetByIdAsync(id);

            if (roadmap is not null)
            {
                _context.Roadmaps.Remove(roadmap);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Roadmap>> GetAllAsync()
        {
            return await _context.Roadmaps.ToListAsync();
        }

        public async Task<IEnumerable<Roadmap>> GetAllAsyncWithQuery(int? id = null, string? title = null)
        {
            var query = _context.Roadmaps.Include(u => u.Steps).AsQueryable();

            query = id.HasValue ? query.Where(u => u.Id == id.Value) : query;

            query = !string.IsNullOrWhiteSpace(title) ? query.Where(u => u.Title == title) : query;


            return await query.ToListAsync();
        }

        public async Task<Roadmap?> GetByEmployeeId(Guid id,string RoamapTitle)
        {
            return await _context.Roadmaps.FirstOrDefaultAsync(s => s.EmployeeId == id &&  s.Title==RoamapTitle);
        }

        public async Task<Roadmap?> GetByIdAsync(int id)
        {
            return await _context.Roadmaps.FindAsync(id);
        }

        public async Task UpdateAsync(Roadmap certificate)
        {
            _context.Roadmaps.Update(certificate);
            await _context.SaveChangesAsync();
        }
    }
}
