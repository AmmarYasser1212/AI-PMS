using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PMS.Application.DTO.Tag;
using PMS.Application.Interfaces.Services;
using PMS.Domain.Entities;
using PMS.Helpers;

namespace PMS.Controllers
{
    [Authorize(Roles = "User")]
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly ITagServices _tagService;

        public TagsController(ITagServices tagService)
        {
            _tagService = tagService;
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreateTagDto dto)
        {
            var UserId = User.GetBusinessUserId();
            var result = await _tagService.CreateAsync(dto,UserId);
            if (result.existAlready == true) { 
            
                return Conflict("Tag Already Exist");
            }
            return Ok(result);
        }

        [HttpGet("{tagId}")]

        public async Task<IActionResult> GetById(  int tagId/*,[FromQuery] int userId*/) 
        {
            var UserId = User.GetBusinessUserId();

            if (tagId <= 0)
                return BadRequest("Invalid tag id");

            var tag= await _tagService.GetByIdAsync(tagId, UserId);
            if (tag == null)
                return NotFound();
            return Ok(tag);
        }

        [HttpPut("{tagId}")]
        public async Task<IActionResult> Update(int tagId, [FromBody] UpdateTagDto dto)
        {
            var userId = User.GetBusinessUserId();

            if (tagId <= 0)
                return BadRequest("Invalid tag id");

            var result = await _tagService.UpdateAsync(dto, tagId, userId);

            if (result.Message== "notFound")
                return NotFound();
            else if(result.Message== "Already Exist")
                return Conflict("Tag Already Exist");

            return Ok();
        }


        [HttpDelete("{tagId}")]
        public async Task<IActionResult> Delete(int tagId)
        {
            var UserId = User.GetBusinessUserId();

            if (tagId <= 0)
                return BadRequest("Invalid tag id");

            var result = await _tagService.DeleteAsync(tagId, UserId);
            if (!result)
                return NotFound("Tag not found");

            return Ok("Tag deleted successfully");
        }

       
        [HttpGet]
        public async Task<IActionResult> GetAllTags()
        {
            var UserId = User.GetBusinessUserId();
            var tags = await _tagService.GetByUserAsync(UserId);
            if (tags == null)
            {
                return Ok("No tags found");
            }
            return Ok(tags);
        }


        [HttpPost("assign")]
        public async Task<IActionResult> AssignTagsToTask(int taskId, List<int> tagIds)
        {
            var userId = User.GetBusinessUserId();

            if (taskId <= 0)
                return BadRequest("Invalid task id");

            if (tagIds == null || !tagIds.Any())
                return BadRequest("Tag ids are required.");

            var result = await _tagService.AssignTagsToTask(taskId, tagIds, userId);

            if (!result)
                return BadRequest("Unable to assign tags to task.");

            return Ok("Tags assigned successfully.");
        }


        [HttpDelete("{taskId}/tags/{tagId}")]
        public async Task<IActionResult> RemoveTagFromTask(int taskId, int tagId)
        {
            var userId = User.GetBusinessUserId();

            if (tagId <= 0)
                return BadRequest("Invalid tag id");

            var result = await _tagService.RemoveTagFromTask(taskId, tagId, userId);

            if (!result)
                return NotFound();

            return NoContent();
        }


        [HttpGet("{tagId}/tasks")]
        public async Task<IActionResult> FilterTasksByTagId(int tagId)
        {
            var UserId = User.GetBusinessUserId();

            if (tagId <= 0)
                return BadRequest("Invalid tag id");


            var tasks = await _tagService.FilterTasksByTag(tagId, UserId);
            return Ok(tasks.Select(t => new
            {
               Id= t.Id,
                Title=t.Title
            }));

        }
}
}
