using Vkr.Domain.Entities.Worker;
using Vkr.Domain.Enums.Files;

namespace Vkr.Domain.Entities.Files;

public class File
{
    public const int NameMaxLength        = 100;
    public const int DescriptionMaxLength = 500;

    public int              Id          { get; set; }
    public FileOwnerType    OwnerType   { get; set; }
    public int              OwnerId     { get; set; }

    public string           Name        { get; set; } = null!;
    public string?          Description { get; set; }
    public string           Key         { get; set; } = null!;
    public long             FileSize    { get; set; }
    public DateTime         CreatedAt   { get; set; }

    public int              CreatorId   { get; set; }
    // Навигация на пользователя-создателя
    public Workers?         Creator     { get; set; }
}