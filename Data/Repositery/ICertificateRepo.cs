using HR_Carrer.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace HR_Carrer.Data.Repositery
{
    public interface ICertificateRepo
    {

        Task AddAsync(Certificates certificat);

        Task<IEnumerable<Certificates>> GetAllAsync();

        Task<Certificates?> GetByIdAsync(int id);

        Task UpdateAsync(Certificates certificate);

        Task DeleteAsync(int id);

        Task DeleteAllAsync();

    }

    public class CertificateRepo : ICertificateRepo
    {
        private readonly ApplicationDbContext _context;

        public CertificateRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Certificates certificat)
        {
            await _context.Certificates.AddAsync(certificat);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAllAsync()
        {

            _context.Certificates.RemoveRange(_context.Certificates);
            await _context.SaveChangesAsync();

        }

        public async Task DeleteAsync(int id)
        {
            var certificate = await GetByIdAsync(id);
            if (certificate is not null)
            {
                _context.Certificates.Remove(certificate);
                await _context.SaveChangesAsync();

            }
        }

        public async Task<IEnumerable<Certificates>> GetAllAsync()
        {
            return await _context.Certificates.ToListAsync();
        }

        public async Task<Certificates?> GetByIdAsync(int id)
        {
            return await  _context.Certificates.FindAsync(id);

        }

        public async Task UpdateAsync(Certificates certificate)
        {
            _context.Certificates.Update(certificate);
            await _context.SaveChangesAsync();

        }
    }




}
