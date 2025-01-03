using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace SmartMaintApi.Services;
public class ImageService
{
    private readonly IConfiguration _configuration;
    private readonly DriveService _driveService;
    public ImageService(IConfiguration configuration)
    {
        _configuration = configuration;

        var serviceAccountKeyFile = _configuration["GDrive:ServiceAccountKeyFile"];
        var userEmail = _configuration["GDrive:UserEmail"];

        var credential = GoogleCredential.FromFile(serviceAccountKeyFile)
            .CreateScoped(DriveService.Scope.Drive)
            .CreateWithUser(userEmail); // Impersonate user

        _driveService = new DriveService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = "MyAppName"
        });
    }

    public async Task UploadImageAsync(Stream fileStream, string fileName)
    {
        var folderId = GetFolderIdAsync("Images");
        var fileMetadata = new Google.Apis.Drive.v3.Data.File
        {
            Name = fileName,
            Parents = new List<string> { await folderId } 
        };

        var request = _driveService.Files.Create(fileMetadata, fileStream, "image/jpeg");
        request.Fields = "id";
        var file = await request.UploadAsync();

        if (file.Status == Google.Apis.Upload.UploadStatus.Failed)
        {
            throw new Exception($"Failed to upload file: {file.Exception.Message}");
        }
    }

    public async Task<IEnumerable<ImageFile>> GetImagesAsync()
    {
        var folderId = await GetFolderIdAsync("Images");

        // Hae kuvat jaetusta kansiosta
        var request = _driveService.Files.List();
        request.Q = $"'{folderId}' in parents and mimeType contains 'image/'";
        request.Fields = "files(id, name, thumbnailLink, webContentLink, createdTime)";

        var result = await request.ExecuteAsync();
        return result.Files.Select(f => new ImageFile
        {
            Id = f.Id,
            Name = f.Name,
            ThumbnailUrl = f.ThumbnailLink,
            Date = f.CreatedTimeDateTimeOffset.HasValue ? f.CreatedTimeDateTimeOffset.Value : (DateTimeOffset?)null,
            FullImageUrl = f.WebContentLink
        }).ToList();
    }


    public async Task<string> GetImageUrlAsync(string fileId)
    {
        var request = _driveService.Files.Get(fileId);
        request.Fields = "webContentLink";
        var file = await request.ExecuteAsync();
        return file.WebContentLink;
    }

    public async Task<IEnumerable<string>> ListAllFilesAndFoldersAsync()
    {
        var request = _driveService.Files.List();
        request.Fields = "files(id, name, mimeType, parents, createdTime, modifiedTime, size, owners(displayName, emailAddress), webViewLink)";

        var result = await request.ExecuteAsync();

        return result.Files.Select(f =>
            $"ID: {f.Id}, Name: {f.Name}, Type: {f.MimeType}, " +
            $"Parents: {string.Join(", ", f.Parents ?? [])}, " +
            $"Created: {f.CreatedTimeDateTimeOffset}, Modified: {f.ModifiedTimeDateTimeOffset}, " +
            $"Size: {f.Size}, Owners: {string.Join(", ", f.Owners.Select(o => $"{o.DisplayName} ({o.EmailAddress})"))}, " +
            $"Web View Link: {f.WebViewLink}"
        ).ToList();
    }

    public async Task<Stream> DownloadImageAsync(string fileId)
    {
        var request = _driveService.Files.Get(fileId);
        var stream = new MemoryStream();
        await request.DownloadAsync(stream);
        stream.Seek(0, SeekOrigin.Begin);
        return stream;
    }
    public async Task<byte[]> DownloadImageBytesAsync(string fileId, int width, int height)
    {
        var request = _driveService.Files.Get(fileId);
        using (var stream = new MemoryStream())
        {
            await request.DownloadAsync(stream);
            stream.Seek(0, SeekOrigin.Begin);

            using (var image = SixLabors.ImageSharp.Image.Load(stream))
            {
                var resizedImage = ResizeImage(image, width, height);
                using (var resizedStream = new MemoryStream())
                {
                    resizedImage.Save(resizedStream, new JpegEncoder());
                    return resizedStream.ToArray();
                }
            }
        }
    }

    private SixLabors.ImageSharp.Image ResizeImage(SixLabors.ImageSharp.Image image, int width, int height)
    {
        var aspectRatio = (float)image.Width / image.Height;
        int newWidth, newHeight;

        if (width / (float)height > aspectRatio)
        {
            newHeight = height;
            newWidth = (int)(height * aspectRatio);
        }
        else
        {
            newWidth = width;
            newHeight = (int)(width / aspectRatio);
        }

        image.Mutate(x => x.Resize(newWidth, newHeight));
        return image;
    }

    public async Task<string> GetFolderIdAsync(string folderName)
    {
        var folderRequest = _driveService.Files.List();
        folderRequest.Q = $"name='{folderName}' and mimeType='application/vnd.google-apps.folder'";
        folderRequest.Fields = "files(id, name)";

        var folderResult = await folderRequest.ExecuteAsync();
        var folderId = folderResult.Files.FirstOrDefault()?.Id;

        if (folderId == null)
        {
            throw new Exception($"{folderName} folder not found.");
        }

        return folderId;
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
