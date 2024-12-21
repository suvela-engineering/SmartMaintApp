using Microsoft.AspNetCore.Mvc;
using SmartMaintApi.Services;
using System.Collections.Generic;
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

        [HttpGet]
        public async Task<IActionResult> GetImages()
        {
            var images = await _imageService.GetImagesAsync();
            return Ok(images);
        }

        [HttpGet("get-image-url/{id}")]
        public async Task<IActionResult> GetImageUrl(string id)
        {
            var url = await _imageService.GetImageUrlAsync(id);
            return Content(url, "text/plain");
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

        [HttpGet("download/resized/{id}")]
        public async Task<IActionResult> DownloadResizedImage(string id, [FromQuery] int width, [FromQuery] int height)
        {
            var imageBytes = await _imageService.DownloadImageBytesAsync(id, width, height);
            if (imageBytes == null || imageBytes.Length == 0)
            {
                return NotFound();
            }

            return File(imageBytes, "image/jpeg");
        }
    }
}
