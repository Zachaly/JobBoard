namespace JobBoard.Application.Service.Abstraction
{
    public interface IFileService
    {
        Task<string> SaveCompanyProfilePictureAsync(Stream stream);
        Task<FileStream> GetCompanyProfilePictureAsync(string fileName);
        Task<FileStream> GetCompanyDefaultPictureAsync();
        Task DeleteCompanyProfilePictureAsync(string? fileName);
    }
}
