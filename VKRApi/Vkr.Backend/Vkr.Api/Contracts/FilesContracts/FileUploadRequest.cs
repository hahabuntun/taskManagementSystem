using Microsoft.AspNetCore.Mvc;

namespace Vkr.API.Contracts.FilesContracts;

/// <summary>
/// DTO для загрузки файла через multipart/form-data
/// </summary>
public class FileUploadRequest
{
    /// <summary>Собственно файл</summary>
    [FromForm(Name = "file")]
    public IFormFile File { get; set; } = null!;

    /// <summary>Заголовок (название) файла</summary>
    [FromForm(Name = "title")]
    public string Title { get; set; } = null!;

    /// <summary>Дополнительное описание</summary>
    [FromForm(Name = "description")]
    public string? Description { get; set; }
}