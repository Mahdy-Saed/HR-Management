using HR_Carrer.Entity;
using Microsoft.EntityFrameworkCore;

namespace HR_Carrer.Data.Repositery
{
    public interface IUserRepo
    {
       

            // add , get, update, delete              //task mean return nothing but if write it like this Task<T> mean return this type of T
            Task AddAsync(User user);

            Task<IEnumerable<User>> GetAllAsync();

            Task<User?> GetByIdAsync(Guid id);

            Task UpdateAsync(User user);

            Task DeleteAsync(Guid id);

            Task DeleteAllAsync();


    
    }


    public class UserRepo : IUserRepo
    {
        private readonly ApplicationDbContext _context;
        public UserRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAllAsync()
        {
              _context.Users.RemoveRange(_context.Users);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var user = await GetByIdAsync(id);
            if (user is not null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _context.Users.FindAsync(id);

        }

        public async Task UpdateAsync(User user)
        {
              _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }


       
    }
}
