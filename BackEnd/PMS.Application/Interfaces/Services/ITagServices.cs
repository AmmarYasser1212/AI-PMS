using PMS.Application.DTO.NewFolder;
using PMS.Application.DTO.Tag;
using PMS.Application.DTO.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Application.Interfaces.Services
{
    public interface ITagServices
    {

        Task<TagDto> CreateAsync(CreateTagDto dto,int UserId);
        Task<MessageHand> UpdateAsync(UpdateTagDto dto,int TagId,int UserId);
        Task<bool> DeleteAsync(int tagid, int userid);

        Task<TagDto?> GetByIdAsync(int tagId, int userId);

        Task<List<TagDto>> GetByUserAsync(int userId);

        Task <bool>AssignTagsToTask(int taskId, List<int> tagIds, int userid);
        Task <bool>RemoveTagFromTask(int taskId, int tagId,int userid);

        Task<List<TaskDto>?> FilterTasksByTag(int tagId, int userid);
    }
}
