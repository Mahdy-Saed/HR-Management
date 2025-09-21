using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Text.RegularExpressions;

namespace HR_Carrer.Services.FileService
{
    public interface IFileService
    {
        Task<string?> SaveImage(IFormFile? file);
        Task<string?> SaveCertificate(IFormFile? file);
        string DeleteFile(string? filePath);



    }
    public class FileService : IFileService
    {
        private readonly string _ImageDirectory;
        private readonly string _CertificateDirectory;

        public FileService(IWebHostEnvironment env)
        {
            var rootPath = env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

            _CertificateDirectory = Path.Combine(rootPath, "Certificates");
            _ImageDirectory = Path.Combine(rootPath, "Images");


            Directory.CreateDirectory(_CertificateDirectory);
            Directory.CreateDirectory(_ImageDirectory);
        }



        public async Task<string?> SaveCertificate(IFormFile? file)
        {
            if (file is null) return null;
            var allowedExtensions = new[] { ".pdf", ".png" };

            return await SaveFile(file, _CertificateDirectory, allowedExtensions);

        }

        public async Task<string?> SaveImage(IFormFile? file)
        {
            if (file is null) return null;
            var allowedExtensions = new[] { ".jpg", ".png" };

            return await SaveFile(file, _ImageDirectory, allowedExtensions);
        }




        public string DeleteFile(string? filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                return "Invalid Path";
            }
            var FileName = Path.GetFileName(filePath);

            if (string.IsNullOrEmpty(FileName)) return "Invalid Path";

            try
            {
                File.Delete(FileName);
                return "Deleted Successfully";

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return "Failed";

            }


        }

        private async Task<string?> SaveFile(IFormFile file, string SaveDirectory, string[] allowedExtensions)
        {
            var extension = Path.GetExtension(file.FileName).ToLower();
            var originalName = Path.GetFileNameWithoutExtension(file.FileName);

            if (!allowedExtensions.Contains(extension)) { return "File type not allowed"; }

            if (file.Length > 5 * 1024 * 1024) { return " File size exceeds 5 MB."; }

            var escapedBase = Regex.Escape(originalName);
            //make the Reqes to check if the file name exists or not
            var patteern = $"^{escapedBase}(?:-(\\d+))?$";
            var req = new Regex(patteern, RegexOptions.IgnoreCase);

            //get all existing file names without extensions in the Directory.
            var existingNames = Directory.EnumerateFiles(SaveDirectory).Select(Path.GetFileNameWithoutExtension);

            int matchCount = existingNames is not null ? existingNames.Count(req.IsMatch!) : 0;


            string newFileName = $"{originalName}-{matchCount}{extension}";

            var filePath = Path.Combine(SaveDirectory, newFileName);

            using var stream = new FileStream(filePath, FileMode.Create);

            await file.CopyToAsync(stream);

            return filePath;

        }

    }

}



