namespace Vkr.Domain.DTO.Tag;


public class AddTagsDTO
{
    public List<int> ExistingTagIds { get; set; } = new List<int>();
    public List<CreateTagDTO> NewTags { get; set; } = new List<CreateTagDTO>();
}