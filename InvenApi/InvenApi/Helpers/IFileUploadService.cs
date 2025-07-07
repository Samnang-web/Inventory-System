namespace InvenApi.Helpers
{
    public interface IFileUploadService
    {
        Task<string> UploadImageAsync(IFormFile file, string folderName);
    }
}
