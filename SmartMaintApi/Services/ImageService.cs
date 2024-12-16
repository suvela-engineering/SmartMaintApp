using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SmartMaintApi.Services
{
    public class ImageService
    {
        private readonly IConfiguration _configuration;
        private readonly DriveService _driveService;

        public ImageService(IConfiguration configuration)
        {
            _configuration = configuration;

            var credential = GoogleCredential.FromFile(_configuration["GDrive:ServiceAccountKeyFile"]).CreateScoped(DriveService.Scope.Drive);
            _driveService = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "MyAppName",
            });
        }

        public async Task<IEnumerable<string>> ListAllFilesAndFoldersAsync()
        {
            var request = _driveService.Files.List();
            request.Fields = "files(id, name, mimeType, parents)";

            var result = await request.ExecuteAsync();

            return result.Files.Select(f => $"ID: {f.Id}, Name: {f.Name}, Type: {f.MimeType}, Parents: {string.Join(", ", f.Parents ?? [])}").ToList();
        }

        public async Task<bool> UploadImageAsync(IFormFile file)
        {
            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = file.FileName,
            };

            FilesResource.CreateMediaUpload request;
            using (var stream = file.OpenReadStream())
            {
                request = _driveService.Files.Create(fileMetadata, stream, file.ContentType);
                request.Fields = "id";
                await request.UploadAsync();
            }

            var fileResponse = request.ResponseBody;
            return fileResponse != null;
        }

        public async Task<Stream> DownloadImageAsync(string fileId)
        {
            var request = _driveService.Files.Get(fileId);
            var stream = new MemoryStream();
            await request.DownloadAsync(stream);
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }
        public async Task<IEnumerable<string>> SearchFilesAsync(string? type = null, string? fileName = null, DateTimeOffset? startDate = null, DateTimeOffset? endDate = null)
        {
            var request = _driveService.Files.List();
            var queryConditions = new List<string>();

            if (!string.IsNullOrEmpty(type))
            {
                queryConditions.Add($"mimeType='{type}'");
            }

            if (!string.IsNullOrEmpty(fileName))
            {
                queryConditions.Add($"name contains '{fileName}'");
            }

            if (startDate.HasValue)
            {
                queryConditions.Add($"modifiedTime >= '{startDate.Value:yyyy-MM-ddTHH:mm:ssZ}'");
            }

            if (endDate.HasValue)
            {
                queryConditions.Add($"modifiedTime <= '{endDate.Value:yyyy-MM-ddTHH:mm:ssZ}'");
            }

            if (queryConditions.Any())
            {
                request.Q = string.Join(" and ", queryConditions);
            }

            request.Fields = "files(id, name, mimeType, modifiedTime, parents)";

            var result = await request.ExecuteAsync();

            return result.Files.Select(f => $"ID: {f.Id}, Name: {f.Name}, Type: {f.MimeType}, ModifiedByMeTimeRaw: {f.ModifiedByMeTimeRaw}, Parents: {string.Join(", ", f.Parents ?? [])}").ToList();
        }
    }
}
