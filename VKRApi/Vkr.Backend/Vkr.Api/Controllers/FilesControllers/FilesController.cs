using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Vkr.API.Contracts.FilesContracts;
using Vkr.Application.Interfaces.Services.FilesService;
using Vkr.Domain.Enums.Files;
using File = Vkr.Domain.Entities.Files.File;

namespace Vkr.API.Controllers.FilesControllers;

[ApiController]
[Route("api/{ownerType}/{ownerId:int}/files")]
public class FilesController : ControllerBase
{
    private readonly IFilesService _filesService;

    public FilesController(IFilesService filesService)
    {
        _filesService = filesService;
    }

    /// <summary>
    /// Список всех файлов для заданного владельца
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<File>>> List(
        [FromRoute] FileOwnerType ownerType,
        [FromRoute] int ownerId)
    {
        var files = await _filesService.ListByOwnerAsync(ownerType, ownerId);
        return Ok(files);
    }

    /// <summary>
    /// Загрузка нового файла для заданного владельца
    /// (multipart/form-data: file, title, description)
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<File>> Upload(
        [FromRoute] FileOwnerType ownerType,
        [FromRoute] int ownerId,
        [FromForm] FileUploadRequest request)
    {
        var file = request.File;

        var userIdClaim = HttpContext.User.FindFirst("userId")?.Value;
        if (userIdClaim == null)
            return Forbid();

        var userId = int.Parse(userIdClaim);

        var saved = await _filesService.UploadAsync(
            ownerType, ownerId, file,
            request.Title, request.Description,
            userId);

        return CreatedAtAction(nameof(Download),
            new { ownerType, ownerId, fileId = saved.Id },
            saved);
    }

    /// <summary>
    /// Скачивание файла по идентификатору
    /// </summary>
    [HttpGet("{fileId:int}")]
    public async Task<IActionResult> Download(
        [FromRoute] FileOwnerType ownerType,
        [FromRoute] int ownerId,
        [FromRoute] int fileId)
    {
        try
        {
            var (stream, fileName) = await _filesService.DownloadAsync(fileId);
            if (stream == null)
                return NotFound();

            return File(stream, "application/octet-stream", fileName);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Удаление файла по идентификатору
    /// </summary>
    [HttpDelete("{fileId:int}")]
    public async Task<IActionResult> Delete(
        [FromRoute] FileOwnerType ownerType,
        [FromRoute] int ownerId,
        [FromRoute] int fileId)
    {
        var userIdClaim = HttpContext.User.FindFirst("userId")?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized("User ID not found in token.");

        var userId = int.Parse(userIdClaim);
        var removed = await _filesService.DeleteAsync(fileId, userId);
        return removed ? NoContent() : NotFound();
    }
}