using Microsoft.AspNetCore.Mvc;
using Vkr.Application.Interfaces;
using Vkr.Domain.Entities.Task;

namespace Vkr.Web.Controllers
{
    [Route("api/task-templates/{templateId}/links")]
    [ApiController]
    public class TaskTemplateLinkController : ControllerBase
    {
        private readonly ITaskTemplateLinkService _taskTemplateLinkService;

        public TaskTemplateLinkController(ITaskTemplateLinkService taskTemplateLinkService)
        {
            _taskTemplateLinkService = taskTemplateLinkService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskTemplateLink>>> GetLinks(int templateId)
        {
            try
            {
                var links = await _taskTemplateLinkService.GetLinksByTaskTemplateIdAsync(templateId);
                return Ok(links);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Data);
            }
        }

        [HttpPost]
        public async Task<ActionResult<int>> AddLink(int templateId, [FromBody] AddTaskTemplateLinkRequest request)
        {
            try
            {

                var linkId = await _taskTemplateLinkService.AddLinkAsync(
                    templateId,
                    request.Link,
                    request.Description);

                return CreatedAtAction(nameof(GetLinks), new { templateId }, new { LinkId = linkId });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Внутренняя ошибка сервера.");
            }
        }


        [HttpPut("{linkId}")]
        public async Task<ActionResult> UpdateLink(int templateId, int linkId, [FromBody] UpdateTaskTemplateLinkRequest request)
        {
            try
            {
                var updated = await _taskTemplateLinkService.UpdateLinkAsync(linkId, request.Link, request.Description);
                if (!updated)
                    return NotFound("Ссылка не найдена.");

                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Внутренняя ошибка сервера.");
            }
        }

        [HttpDelete("{linkId}")]
        public async Task<ActionResult> DeleteLink(int templateId, int linkId)
        {
            try
            {
                var deleted = await _taskTemplateLinkService.DeleteLinkAsync(linkId);
                if (!deleted)
                    return NotFound("Ссылка не найдена.");

                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Внутренняя ошибка сервера.");
            }
        }
    }

    public class AddTaskTemplateLinkRequest
    {
        public string Link { get; set; } = null!;
        public string? Description { get; set; }
    }

    public class UpdateTaskTemplateLinkRequest
    {
        public string Link { get; set; } = null!;
        public string? Description { get; set; }
    }
}