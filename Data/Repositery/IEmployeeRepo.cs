using HR_Carrer.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace HR_Carrer.Data.Repositery
{
    public interface IEmployeeRepo
    {
        Task AddAsync(Employee emplyee);

        Task<IEnumerable<Employee>> GetAllAsync();

        Task<Employee?> GetByIdAsync(Guid id);

        Task<Employee?> GetEmployeeWIthRequest(Guid id);

        Task UpdateAsync(Employee employee);

        Task DeleteAsync(Guid id);

        Task DeleteAllAsync();

    }


    public class EmployeeRepo : IEmployeeRepo
    {
        private readonly ApplicationDbContext _context;

        public EmployeeRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Employee emplyee)
        {
            await _context.Employees.AddAsync(emplyee); 
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAllAsync()
        {
            _context.Employees.RemoveRange(_context.Employees);
            await _context.SaveChangesAsync();

        }

        public async Task DeleteAsync(Guid id)
        {


            var Employee = await GetByIdAsync(id);
            if (Employee != null)
            {
                _context.Employees.Remove(Employee);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _context.Employees.ToListAsync();
        }

        public async Task<Employee?> GetByIdAsync(Guid id)
        {
            return await  _context.Employees.Include(e=>e.User).FirstOrDefaultAsync(em => em.UserId == id);
        }

        public async Task<Employee?> GetEmployeeWIthRequest(Guid id)
        {
            return await _context.Employees
                .Include(e => e.Requests) // Include the Requests navigation property
                .FirstOrDefaultAsync(em => em.UserId == id);
        }

        public async Task UpdateAsync(Employee employee)
        {
           _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
        }

    }

}
