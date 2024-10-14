using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class FilesController : ControllerBase
{
    private readonly string _imagePath;

    public FilesController(IWebHostEnvironment webHost)
    {
        _imagePath = Path.Combine(webHost.WebRootPath, "Images");
    }

    [HttpPost]
    public async Task<IActionResult> SaveFile(IFormFile file)
    {
        if (file == null || !file.ContentType.StartsWith("image/"))
        {
            return BadRequest("Неверный тип файла. Пожалуйста, загрузите изображение.");
        }

        Console.WriteLine($"Имя файла: {file.FileName}, Тип: {file.ContentType}");

        var fileName = Path.GetRandomFileName() + Path.GetExtension(file.FileName);
        var filePath = Path.Combine(_imagePath, fileName);

        try
        {
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            var host = HttpContext.Request.Host;
            var fileUrl = $"https://{host}/Images/{fileName}";

            return Ok(new { Url = fileUrl });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при загрузке файла: {ex.Message}");
            return StatusCode(500, "Ошибка при загрузке файла.");
        }
    }

    [HttpDelete]
    public IActionResult DeleteFile(string fileName)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            return BadRequest("Имя файла не передано");
        }

        var filePath = Path.Combine(_imagePath, fileName);
        var fileInfo = new FileInfo(filePath);

        if (!fileInfo.Exists)
        {
            return NotFound("Файл не найден");
        }

        fileInfo.Delete();
        return Ok("Файл удален");
    }
}
