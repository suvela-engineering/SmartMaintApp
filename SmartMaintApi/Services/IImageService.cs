using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartMaintApi.Services
{
    public interface IImageService
    {
        Task UploadImageAsync(Stream fileStream, string fileName);
        Task<IEnumerable<ImageFile>> GetImagesAsync();
        Task<string> GetImageUrlAsync(string fileId);
        Task<IEnumerable<string>> ListAllFilesAndFoldersAsync();
        Task<Stream> DownloadImageAsync(string fileId);
        Task DeleteImageAsync(string fileId);
        Task<byte[]> DownloadImageBytesAsync(string fileId, int width, int height);
        Task<string> GetFolderIdAsync(string folderName);
        Task<IEnumerable<string>> SearchFilesAsync(string? type = null, string? fileName = null, DateTimeOffset? startDate = null, DateTimeOffset? endDate = null);
    }
}
