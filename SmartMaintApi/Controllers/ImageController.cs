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
        private readonly ILogger<ImageService> _logger;

        public ImageController(ImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpGet("list-all")] // For debugging
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
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            try
            {
                using (var stream = file.OpenReadStream())
                {
                    await _imageService.UploadImageAsync(stream, file.FileName);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading image");
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpGet("download/{id}")]
        public async Task<IActionResult> DownloadImage(string id)
        {
            var stream = await _imageService.DownloadImageAsync(id);
            if (stream == null)
                return NotFound();

            return File(stream, "application/octet-stream", id);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImage(string id)
        {
            try
            {
                await _imageService.DeleteImageAsync(id);
                return NoContent(); // HTTP 204: No Content
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
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
