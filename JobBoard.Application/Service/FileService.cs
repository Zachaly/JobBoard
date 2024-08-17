using JobBoard.Application.Service.Abstraction;
using Microsoft.Extensions.Options;
using MigraDoc.Rendering;

namespace JobBoard.Application.Service
{
    public class FileConfiguration
    {
        public string DefaultFileName { get; set; }
        public string CompanyProfilePath { get; init; }
        public string ResumeFilePath { get; init; }
        public string EmployeeResumeFilePath { get; init; }
    }

    public class FileService : IFileService
    {
        private readonly FileConfiguration _config;

        public FileService(IOptions<FileConfiguration> config)
        {
            _config = config.Value;
        }

        public Task<string> CopyEmployeeResumeToApplicationAsync(string fileName)
        {
            var newFileName = $"{Guid.NewGuid()}.pdf";

            File.Copy(Path.Combine(_config.EmployeeResumeFilePath, fileName), Path.Combine(_config.ResumeFilePath, newFileName));

            return Task.FromResult(newFileName);
        }

        public Task DeleteCompanyProfilePictureAsync(string? fileName)
        {
            if(fileName is null)
            {
                return Task.CompletedTask;
            }

            var path = Path.Combine(_config.CompanyProfilePath, fileName);

            File.Delete(path);

            return Task.CompletedTask;
        }

        public Task DeleteEmployeeResumeFileAsync(string fileName)
        {
            var path = Path.Combine(_config.EmployeeResumeFilePath, fileName);

            File.Delete(path);

            return Task.CompletedTask;
        }

        public Task DeleteResumeFileAsync(string fileName)
        {
            var path = Path.Combine(_config.ResumeFilePath, fileName);

            File.Delete(path);

            return Task.CompletedTask;
        }

        public Task<FileStream> GetCompanyDefaultPictureAsync()
            => Task.FromResult(File.OpenRead(Path.Combine(_config.CompanyProfilePath, _config.DefaultFileName)));

        public Task<FileStream> GetCompanyProfilePictureAsync(string fileName)
            => Task.FromResult(File.OpenRead(Path.Combine(_config.CompanyProfilePath, fileName)));

        public Task<FileStream> GetEmployeeResumeFileAsync(string fileName)
            => Task.FromResult(File.OpenRead(Path.Combine(_config.EmployeeResumeFilePath, fileName)));

        public Task<FileStream> GetResumeFileAsync(string fileName)
            => Task.FromResult(File.OpenRead(Path.Combine(_config.ResumeFilePath, fileName)));

        public async Task<string> SaveCompanyProfilePictureAsync(Stream stream)
        {
            var fileName = Guid.NewGuid().ToString() + ".png";
            var filePath = Path.Combine(_config.CompanyProfilePath, fileName);

            using(var fileStream = File.Create(filePath))
            {
                await stream.CopyToAsync(fileStream);
            }

            return fileName;
        }

        public Task<string> SaveEmployeeResumeFileAsync(PdfDocumentRenderer renderer)
        {
            Directory.CreateDirectory(_config.EmployeeResumeFilePath);

            var fileName = Guid.NewGuid().ToString() + ".pdf";
            var path = Path.Combine(_config.EmployeeResumeFilePath, fileName);

            renderer.RenderDocument();
            renderer.Save(path);

            return Task.FromResult(fileName);
        }

        public async Task<string> SaveResumeFileAsync(Stream stream)
        {
            Directory.CreateDirectory(_config.ResumeFilePath);

            var fileName = Guid.NewGuid().ToString() + ".pdf";

            var filePath = Path.Combine(_config.ResumeFilePath, fileName);

            using(var fileStream = File.Create(filePath))
            {
                await stream.CopyToAsync(fileStream);
            }

            return fileName;
        }
    }
}
