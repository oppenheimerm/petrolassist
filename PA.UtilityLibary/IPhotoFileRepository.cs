
using Microsoft.AspNetCore.Http;

namespace PA.UtilityLibary
{
    public interface IPhotoFileRepository
    {
        Task<(FileInfo fileInfo, bool Success, string ErrorMessage)> AddPhotoAsync(IFormFile file, string rootDirectory, string path);
    }
}
