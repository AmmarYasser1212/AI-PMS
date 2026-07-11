using Microsoft.AspNetCore.Identity;
using PMS.Application.DTO.User;
using PMS.Application.Interfaces.Repositories;
using PMS.Application.Interfaces.Services;
using PMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Application.Services.userser
{
    public class UserServices :IUserServices
    {
        private readonly Irepsitory<User>_context;
        private readonly Irepsitory<TaskTag> _taskTagRepo;
        private readonly Irepsitory<ScheduleTask> _scheduleTaskRepo;
        private readonly Irepsitory<TaskItem> _taskRepo;
        private readonly Irepsitory<Category> _categoryRepo;
        private readonly Irepsitory<Schedule> _scheduleRepo;
        private readonly Irepsitory<Tag> _tagRepo;
       // private readonly UserManager<AppUser> _userManager;

        private readonly IunitOfWork _uow;

        public UserServices(Irepsitory<User> context, IunitOfWork uow, Irepsitory<TaskTag> taskTagRepo, Irepsitory<ScheduleTask> scheduleTaskRepo, Irepsitory<TaskItem> taskRepo,
           Irepsitory<Category> categoryRepo, Irepsitory<Schedule> scheduleRepo, Irepsitory<Tag> tagRepo) {
        
            _context = context;
            _taskTagRepo = taskTagRepo;
            _scheduleTaskRepo = scheduleTaskRepo;
            _categoryRepo = categoryRepo;
            _scheduleRepo = scheduleRepo;
            _tagRepo = tagRepo;
            _uow = uow;
        }

        public async Task<int> CreateUserAsync(CreateUserDto dto)
        {
           // var exists = await _context.ExistsAsync(u => u.Email == dto.Email);
            //if (exists)
            //    throw new Exception("Email already exists");

            var user = new User
            {
                //Email = dto.Email,
                //PasswordHash = dto.Password,
                Name = dto.Name,
                Avatar = dto.Avatar,
                CreatedAt = DateTime.UtcNow
            };

            await _context.AddAsync(user);
            await _uow.SaveChangesAsync();

            return user.Id;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {

            var user = await _context.FindOneAsync(u => u.Id == id);

            if (user == null)
                return false;

         //   var identityUser = await _userManager.FindByIdAsync(user.IdentityUserId);

            // =========================
            // 1. TaskTags (direct via Tasks)
            // =========================
            var taskTags = await _taskTagRepo.FindAsync(tt =>
                tt.Task.UserId == id);

          await _taskTagRepo.DeleteRangeAsync(taskTags);

            // =========================
            // 2. ScheduleTasks
            // =========================
            var scheduleTasks = await _scheduleTaskRepo.FindAsync(st =>
                st.Schedule.UserId == id);

           await _scheduleTaskRepo.DeleteRangeAsync(scheduleTasks);

            // =========================
            // 3. Tasks
            // =========================
            var tasks = await _taskRepo.FindAsync(t=>
                t.UserId == id);

           await _taskRepo.DeleteRangeAsync(tasks);

            // =========================
            // 4. Schedules
            // =========================
            var schedules = await _scheduleRepo.FindAsync(s =>
                s.UserId == id);

           await _scheduleRepo.DeleteRangeAsync(schedules);

            // =========================
            // 5. Categories
            // =========================
            var categories = await _categoryRepo.FindAsync(c =>
                c.UserId == id);

           await _categoryRepo.DeleteRangeAsync(categories);

            // =========================
            // 6. Tags
            // =========================
            var tags = await _tagRepo.FindAsync(t =>
                t.UserId == id);

           await _tagRepo.DeleteRangeAsync(tags);

            // =========================
            // 7. User
            // =========================
            _context.Delete(user);

            // =========================
            // 8. Identity
            // =========================
            //if (identityUser != null)
            //{
            //    var result = await _userManager.DeleteAsync(identityUser);
            //    if (!result.Succeeded)
            //        return false;
            //}

            await _uow.SaveChangesAsync();

            return true;




            //// we should first delete any categories and tags and tasks'user  cascade with categories and tags so we should remove tasks first

            //var user = await _context.GetByIdAsync(id);

            //if (user == null)
            //    return false;

            // _context.Delete(user);

            //await _uow.SaveChangesAsync();
            //return true;

        }

        public async Task<List<UserDto>> GetAllAsync()
        {
            var users = await _context.GetAllAsync();

            return users.Select(u => new UserDto
            {
                Id = u.Id,
              //  Email = u.Email,
                Name = u.Name,
                Avatar = u.Avatar
            }).ToList();
        }

        public async Task<UserDto?> GetByIdAsync(int id)
        {
            var user = await _context.GetByIdAsync(id);

            if (user == null) return null;

            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                Avatar = user.Avatar
            };
        }

        public async Task <bool> UpdateUserAsync(UpdateUserDto dto, int UserId)
        {
            var user = await _context.GetByIdAsync(UserId);
            if (user == null) return false;

            if (!string.IsNullOrWhiteSpace(dto.Name))
                user.Name = dto.Name.Trim();

            if (!string.IsNullOrWhiteSpace(dto.Avatar))
                user.Avatar = dto.Avatar;

            //  await _context.UpdateAsync(user);  entity من  طالما جبت db 

            await _uow.SaveChangesAsync();

            return true;
        }

    }
}
