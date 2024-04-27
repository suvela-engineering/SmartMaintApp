using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartMaintApi.Models;
using SmartMaintApi.Services;

[ApiController]
[Route("API/[controller]")]
[ProducesResponseType(statusCode: 400, type: typeof(void))] // 400 - Bad request
[ProducesResponseType(statusCode: 401, type: typeof(void))] // 401 - Unauthorized
[ProducesResponseType(statusCode: 403, type: typeof(void))] // 403 - Forbidden
[ProducesResponseType(statusCode: 404, type: typeof(void))] // 404 - Not Found
[ProducesResponseType(statusCode: 500, type: typeof(void))] // 500 - Internal server error
//[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    // **Read User**
    [HttpGet("{userId}", Name = "GetUser")]
    [ProducesResponseType(statusCode: 200, type: typeof(void))] // 200 - Success
    public async Task<IActionResult> GetUser(string userId)
    {
        try
        {
            return await _userService.GetUserAsync(userId);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error get user: {ex.Message}");
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    // **Read All Users**
    [HttpGet(Name = "GetAllUsers")]
    [ProducesResponseType(statusCode: 200, type: typeof(void))] // 200 - Success
    public async Task<IActionResult> GetAllUsers()
    {
        try
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error get all users: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }


    // **Create User** (assuming password is handled separately)
    [HttpPost]
    [ProducesResponseType(statusCode: 201, type: typeof(void))] // 201 - Created at route result
    public async Task<IActionResult> CreateUser([FromBody] User user)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.SelectMany(v => v.Errors));

            return await _userService.CreateUserAsync(user);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error create user: {ex.Message}");
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    // **Update User**
    [ProducesResponseType(statusCode: 204, type: typeof(void))] // 204 - No Content: Ok
    [HttpPut("{userId}")]
    public async Task<IActionResult> UpdateUser(string userId, [FromBody] User userUpdate)
    {
        try
        {
            return await _userService.UpdateUserAsync(userId, userUpdate);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error update user: {ex.Message}");
            return StatusCode(500, "An error occurred while updating the user.");
        }
    }
    // **Delete User** soft delete with audit trail (EntityInfo)
    [HttpDelete("{userId}")]
    // [ProducesResponseType(statusCode: 204, type: typeof(void))]
    [ProducesResponseType(statusCode: 200, type: typeof(void))] // 204 - No Content: Ok
    [ProducesResponseType(statusCode: 410, type: typeof(void))] // 410 - Client Error (Resource Gone)
    public async Task<IActionResult> DeleteUser(string userId)
    {
        try
        {
            // TO DO: Implement user who made the query/logged. Same to Interceptor
            string loggedUser = HttpContext?.User?.Identity?.Name ?? "Unknown User";
            return await _userService.DeleteUserAsync(userId, loggedUser);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error delete user: {ex.Message}");
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        }
    }
}