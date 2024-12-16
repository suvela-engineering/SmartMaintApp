using Microsoft.AspNetCore.Mvc;
using SmartMaintApi.Services;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace SmartMaintApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImageController : ControllerBase
    {
        private readonly ImageService _imageService;

        public ImageController(ImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpGet("list-all")]
        public async Task<IActionResult> ListAllFilesAndFolders()
        {
            var items = await _imageService.ListAllFilesAndFoldersAsync();
            return Ok(items);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var result = await _imageService.UploadImageAsync(file);
            if (result)
                return Ok();
            
            return StatusCode(500, "Error uploading file.");
        }

        [HttpGet("download/{id}")]
        public async Task<IActionResult> DownloadImage(string id)
        {
            var stream = await _imageService.DownloadImageAsync(id);
            if (stream == null)
                return NotFound();

            return File(stream, "application/octet-stream", id);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchFiles([FromQuery] string type, [FromQuery] string fileName, [FromQuery] DateTimeOffset? startDate, [FromQuery] DateTimeOffset? endDate)
        {
            var items = await _imageService.SearchFilesAsync(type, fileName, startDate, endDate);
            return Ok(items);
        }
    }
}
