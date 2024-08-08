using JobBoard.Application.Service.Abstraction;

namespace JobBoard.Application.Service
{
    public class FileConfiguration
    {
        public string DefaultFileName { get; set; }
        public string CompanyProfilePath { get; init; }
    }

    public class FileService : IFileService
    {
        private readonly FileConfiguration _config;

        public FileService(FileConfiguration config)
        {
            _config = config;
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

        public Task<FileStream> GetCompanyDefaultPictureAsync()
            => Task.FromResult(File.OpenRead(Path.Combine(_config.CompanyProfilePath, _config.DefaultFileName)));

        public Task<FileStream> GetCompanyProfilePictureAsync(string fileName)
            => Task.FromResult(File.OpenRead(Path.Combine(_config.CompanyProfilePath, fileName)));
            
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
    }
}
