using MigraDoc.Rendering;

namespace JobBoard.Application.Service.Abstraction
{
    public interface IFileService
    {
        Task<string> SaveCompanyProfilePictureAsync(Stream stream);
        Task<FileStream> GetCompanyProfilePictureAsync(string fileName);
        Task<FileStream> GetCompanyDefaultPictureAsync();
        Task DeleteCompanyProfilePictureAsync(string? fileName);

        Task<string> SaveResumeFileAsync(Stream stream);
        Task<FileStream> GetResumeFileAsync(string fileName);
        Task DeleteResumeFileAsync(string fileName);

        Task DeleteEmployeeResumeFileAsync(string fileName);
        Task<string> SaveEmployeeResumeFileAsync(PdfDocumentRenderer renderer);
        Task<FileStream> GetEmployeeResumeFileAsync(string fileName);

        Task<string> CopyEmployeeResumeToApplicationAsync(string fileName);
    }
}
