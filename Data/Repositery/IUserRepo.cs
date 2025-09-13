using HR_Carrer.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace HR_Carrer.Data.Repositery
{

    // return only the User with Role here 
    public interface IUserRepo
    {
       

            // add , get, update, delete              //task mean return nothing but if write it like this Task<T> mean return this type of T
            Task AddAsync(User user);

            Task<IEnumerable<User>> GetAllAsync();

            Task<User?> GetByIdAsync(Guid id);
            Task<User?> GetByEmailAsync(string email);

            Task<User?> GetByRefreshTokenAsync(string refreshToken);


        Task UpdateAsync(User user);

            Task DeleteAsync(Guid id);

            Task<int> DeleteAllAsync();


    
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

        public async Task<int> DeleteAllAsync()
        {
              _context.Users.RemoveRange(_context.Users);
            return  await _context.SaveChangesAsync();
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
            return await _context.Users.Include(u=>u.Role).ToListAsync();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.Include(u=>u.Role).FirstOrDefaultAsync(u=>u.Email== email);


        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == id);

        }

        public async Task<User?> GetByRefreshTokenAsync(string refreshToken)
        {
            return await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
        }

        public async Task UpdateAsync(User user)
        {
              _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }


       
    }
}
