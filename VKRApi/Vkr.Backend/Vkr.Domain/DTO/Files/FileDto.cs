using Vkr.Domain.Enums.Files;

namespace Vkr.Domain.DTO.Files;

public class FileDto
{
    public int            Id          { get; set; }
    public FileOwnerType OwnerType   { get; set; }
    public int            OwnerId     { get; set; }
    public string         Name        { get; set; } = null!;
    public string?        Description { get; set; }
    public string         Key         { get; set; } = null!;
    public long           FileSize    { get; set; }
    public DateTime       CreatedAt   { get; set; }
    public int            CreatorId   { get; set; }
    public string?        CreatorName { get; set; }
}