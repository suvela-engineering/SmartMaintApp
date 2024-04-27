using System.Collections;
using Microsoft.AspNetCore.Mvc;
using SmartMaintApi.Models;

namespace SmartMaintApi.Services
{
    public interface IUserService
    {
        Task<IActionResult> GetUserAsync(string userId);
        Task<IEnumerable> GetAllUsersAsync();
        Task<IActionResult> CreateUserAsync(User user);
        Task<IActionResult> UpdateUserAsync(string userId, User user);
        Task<IActionResult> DeleteUserAsync(string userId, string loggedUser);
    }
}