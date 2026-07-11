using PMS.Application.DTO.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Application.Interfaces.Services
{
    public interface IUserServices
    {

        Task<int> CreateUserAsync(CreateUserDto dto);
        Task<UserDto?> GetByIdAsync(int id);
        Task<List<UserDto>> GetAllAsync();
        Task<bool> UpdateUserAsync(UpdateUserDto dto,int UserId);
        Task<bool> DeleteUserAsync(int id);



    }
}
