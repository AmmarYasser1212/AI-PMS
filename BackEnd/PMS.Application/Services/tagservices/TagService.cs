using PMS.Application.DTO.NewFolder;
using PMS.Application.DTO.Tag;
using PMS.Application.DTO.Task;
using PMS.Application.Interfaces.Repositories;
using PMS.Application.Interfaces.Services;
using PMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace PMS.Application.Services.tagservices
{
    public class TagService : ITagServices 
    {
        private readonly Irepsitory<Tag> _tagRepo;
        private readonly Irepsitory<TaskTag> _taskTagRepo;
        private readonly Irepsitory<TaskItem> _taskRepo;
        private readonly IunitOfWork _uow;

        public TagService(
            Irepsitory<Tag> tagRepo,
            Irepsitory<TaskTag> taskTagRepo,
            Irepsitory<TaskItem> taskRepo,
            IunitOfWork uow)
        {
            _tagRepo = tagRepo;
            _taskTagRepo = taskTagRepo;
            _taskRepo = taskRepo;
            _uow = uow;
        }

        public async Task<TagDto> CreateAsync(CreateTagDto dto, int UserId)
        {
            
            var exists = await _tagRepo.ExistsAsync(
                t => t.UserId == UserId && t.Name == dto.Name);

            if (exists)
                return new TagDto
                {
                   existAlready = true,
                };

            var tag = new Tag
            {
                Name = dto.Name,
                UserId = UserId
            };

            await _tagRepo.AddAsync(tag);
            await _uow.SaveChangesAsync();

            return new TagDto
            {
                Id = tag.Id,
                Name = tag.Name
            };
        }

        public async Task<MessageHand> UpdateAsync(UpdateTagDto dto, int TagId, int UserId)
        {
            var tag = await _tagRepo.FindOneAsync(t=>t.Id==TagId&&t.UserId==UserId);
            if (tag == null) return new MessageHand { Message="notFound"};
///////////
            var exists = await _tagRepo.ExistsAsync(
              t => t.UserId == UserId && t.Name == dto.Name&&t.Id!=TagId);

            if (exists)
                return new MessageHand { Message = "Already Exist" };
            /////////////////

            if (dto.Name != null)
                tag.Name = dto.Name;

            await _tagRepo.UpdateAsync(tag);
            await _uow.SaveChangesAsync();

            return new MessageHand { Message = "Updated" };
        }

        public async Task<bool> DeleteAsync(int tagid,int userid)
        {
            var tag = await _tagRepo.FindOneAsync(t => t.Id == tagid && t.UserId == userid);
            if (tag == null) return false;

             _tagRepo.Delete(tag);
            await _uow.SaveChangesAsync();

            return true;
        }

        public async Task<List<TagDto>> GetByUserAsync(int userId)
        {
            return await _tagRepo.FindAsyncAdvanced(
                t => t.UserId == userId,
                t => new TagDto
                {
                    Id = t.Id,
                    Name = t.Name
                });
        }
        public async Task<bool> RemoveTagFromTask(int taskId, int tagId,int userid)
        {
           var deletedCount = await _taskTagRepo.DeleteWhereAsync(
                tt => tt.TaskId == taskId && tt.TagId == tagId&&tt.Task.UserId==userid&&tt.Tag.UserId==userid);

            return deletedCount > 0;

        }

        public async Task<List<TaskDto>> FilterTasksByTag(int tagId,int userid)
        {



            return await _taskRepo.FindAsyncAdvanced(
                       t=>t.UserId==userid                        
                        &&t.TaskTags.Any(tt => tt.TagId == tagId),
                       
                       t => new TaskDto
                       {
                           Id= t.Id,
                           Title = t.Title,
                       });

        }

        public async Task<bool> AssignTagsToTask(int taskId, List<int> tagIds,int userid)
        {

            if (tagIds == null || !tagIds.Any())
                return false;

            // check task exists and belongs to user
            var taskExists = await _taskRepo.ExistsAsync(
                t => t.Id == taskId && t.UserId == userid);

            if (!taskExists)
                return false;

            var taskTags = await _taskTagRepo.FindAsync(
      t => t.TaskId == taskId &&
           t.Task.UserId == userid &&
           t.Tag.UserId == userid);

            var existing = taskTags
                .Select(t => t.TagId)
                .ToHashSet();

            var validTagIds = (await _tagRepo.FindAsync(t =>
                    tagIds.Contains(t.Id) && t.UserId == userid))
                .Select(t => t.Id)
                .ToHashSet();


            if (!validTagIds.Any())
                return false;

            var newtags = validTagIds
                .Where(id => !existing.Contains(id))
                .Select(id => new TaskTag
                {
                    TaskId = taskId,
                    TagId = id,
                });

            if (!newtags.Any())
                return false;


            foreach (var tag in newtags)
                await _taskTagRepo.AddAsync(tag);

            await _uow.SaveChangesAsync();
            return true;
        }

        public async Task<TagDto?> GetByIdAsync(int tagId, int userId)
        {
            var tag=await _tagRepo.FindOneAsync(t=>t.UserId == userId&&t.Id==tagId);
            if(tag == null) return null;
            return new TagDto { Id = tag.Id, Name = tag.Name };
            
               
        }
    }
}
