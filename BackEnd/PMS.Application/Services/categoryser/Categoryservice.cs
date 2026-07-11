using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMS.Application.DTO.Category;
using PMS.Application.Interfaces.Repositories;
using PMS.Application.Interfaces.Services;
using PMS.Domain.Entities;

namespace PMS.Application.Services.categoryser
{
    public class Categoryservice : ICategoryService
    {
        private readonly Irepsitory<Category> _repo;
        private readonly Irepsitory<TaskItem> _taskRepo;
        private readonly Irepsitory<ScheduleTask> _scheduleTaskRepo;
        private readonly IunitOfWork _uow;

        public Categoryservice(Irepsitory<Category> repo, IunitOfWork uow, Irepsitory<TaskItem> taskRepo, Irepsitory<ScheduleTask> scheduleTaskRepo)
        {
            _repo = repo;
            _uow = uow;
            _taskRepo = taskRepo;
            _scheduleTaskRepo=scheduleTaskRepo;
        }

        public async Task<int> CreateAsync(CreateCategoryDto dto, int UserId)
        {
            
            var exists = await _repo.ExistsAsync(c =>
                c.UserId == UserId && c.Name == dto.Name);

            if (exists)
                return -1;

            var category = new Category
            {
                Name = dto.Name,
                UserId = UserId
            };

            await _repo.AddAsync(category);
            await _uow.SaveChangesAsync();

            return category.Id;
        }

        public async Task<List<CategoryDto>> GetByUserAsync(int userId)
        {
            var data = await _repo.FindAsyncAdvanced(c => c.UserId == userId, c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name
            });

            return data;
                  
        }

        public async Task<CategoryDto?> GetByIdAsync(int id, int userid)
        {
            var category = await _repo.FindOneAsync(c=>c.Id==id&&c.UserId==userid);

            if (category == null)
                return null;

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name
            };
        }

        public async Task<bool> UpdateAsync(UpdateCategory dto, int id, int userId)
        {
            var category = await _repo.FindOneAsync(c =>
                   c.Id == id &&
                   c.UserId == userId);

            if (category == null)
                return false;

            var exists = await _repo.ExistsAsync(c =>
                c.UserId == userId &&
                c.Name == dto.Name &&
                c.Id != id);

            if (exists)
                return false;

            category.Name = dto.Name;

            await _uow.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id, int userid)
        {


            var category = await _repo.FindOneAsync(c => c.Id == id && c.UserId == userid);
            if (category == null)
                return false;
            /**********************************************************************************
            // 🧨 احذف TaskTags  no because cascade delete automatic
            //  await _taskTagRepo.DeleteWhereAsync(t => t.Task.CategoryId == id);

            // 🧨 احذف ScheduleTasks no because cascade delete automatic
            //  await _scheduleTaskRepo.DeleteWhereAsync(t => t.Task.CategoryId == id);
            */////////////////////////////////////////////////////////////////////////////////////////

            //CheckBeforeDeleteAsync(int categoryId);  call and make desicion before delete task
            // 🧨 احذف Tasks 
            // await _taskRepo.DeleteWhereAsync(t => t.CategoryId == id);

            //so relation setnull

            _repo.Delete(category);  

            await _uow.SaveChangesAsync();
            return true;
        }

        public async Task<CategoryDeleteCheckResult> CheckBeforeDeleteCategoryAsync(int categoryId, int userid)
        {
            var hasTasks = await _taskRepo.ExistsAsync(t => t.CategoryId == categoryId && t.UserId == userid);

            if (!hasTasks)
                return new CategoryDeleteCheckResult
                {
                    CanDeleteDirectly = true
                };

            var hasScheduledTasks = await _scheduleTaskRepo
                .ExistsAsync(st => st.Task.CategoryId == categoryId&&st.Task.UserId == userid);

            if (!hasScheduledTasks)
                return new CategoryDeleteCheckResult
                {
                    CanDeleteDirectly = true
                };

           

            return new CategoryDeleteCheckResult
            {
                CanDeleteDirectly = false,
                HasScheduledTasks = true,
                Message = "This category has tasks linked to schedules",
                Options = new List<string>
        {
            "ReplaceTasks",
            "RebuildSchedule",
            "ClearScheduleSlots",
            "Cancel"
        }
            };
        }

        //no in buissness
        public async Task HandleReplaceTasks(int categoryId, Dictionary<int, int> taskMapping)
        {

            var oldTaskIds = taskMapping.Keys.ToArray();

            var tasks = await _scheduleTaskRepo.FindAsync(
                t => t.Task != null &&
                     t.Task.CategoryId == categoryId &&
                     t.TaskId.HasValue &&
                     oldTaskIds.Contains(t.TaskId.Value)
            );

            foreach (var task in tasks)
            {
                if (task.TaskId is int oldId && taskMapping.TryGetValue(oldId, out var newId))
                {
                    task.TaskId = newId;
                }
            }

            await _uow.SaveChangesAsync();

            //replace tasks are not existed in same category

            // task ممكن ال 

            //await _scheduleTaskRepo.UpdateWhereAsync(
            //    st => st.TaskId == map.Key,
            //    st => st.TaskId = map.Value
            //);
        }

        public async Task ClearScheduleSlots(int categoryId,int userid)
        {
            var items = await _scheduleTaskRepo
                .FindAsync(st => st.Task.CategoryId == categoryId);

            foreach (var item in items)
            {
                item.TaskId = null; 
            }
        }



    }
}
