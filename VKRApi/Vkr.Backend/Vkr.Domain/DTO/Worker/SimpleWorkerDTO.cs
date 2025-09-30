namespace Vkr.Domain.DTO.Worker;

public class SimpleWorkerDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string SecondName { get; set; } = string.Empty;
    public string? ThirdName { get; set; }
    public string Email { get; set; } = string.Empty;
}