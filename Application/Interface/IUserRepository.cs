using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> UserExistsByEmailAsync(string email);
        Task<User> AddUserAsync(User user);
        Task<User?> GetUserByEmailAsync(string email, string phone);
        Task<User?> GetUserByEmailAsync(string email);
        Task<bool> UpdateUserNameAsync(Guid id, string newName);
        Task UpdateUserAsync(User user);
        Task<bool> UpdateUserRoleAsync(Guid id, string role);
        Task<int> CountAdminsAsync();
        Task<User?> GetUserByIdAsync(Guid id);
    }
}
