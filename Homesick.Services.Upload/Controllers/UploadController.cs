using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Homesick.Services.Upload.Controllers
{
    [Route("api/upload")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public UploadController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var uploadsFolder = Path.Combine(_env.WebRootPath, "images/houses");
            Directory.CreateDirectory(uploadsFolder); // Ensure the directory exists
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            await using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            var fileUrl = $"/images/houses/{fileName}";
            return Ok(new { Url = fileUrl });
        }
    }
}
