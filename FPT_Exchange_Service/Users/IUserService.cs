using FPT_Exchange_Data.DTO.Filters;
using FPT_Exchange_Data.DTO.Internal;
using FPT_Exchange_Data.DTO.Request.Post;
using FPT_Exchange_Data.DTO.Request.Put;
using FPT_Exchange_Data.DTO.View;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FPT_Exchange_Service.Users
{
    public interface IUserService
    {
        Task<AuthModel?> GetAuthUser(Guid id);
        Task<AuthViewModel> AuthenticatedUser(AuthRequest auth);
        Task<IActionResult> GetUser(Guid id);
        Task<IActionResult> GetUsers(UserFilterModel filter);
        Task<IActionResult> RegisterStaff(RegisterStaffRequest request);
        Task<IActionResult> AddAvatar(Guid id, IFormFile file);
        Task<IActionResult> UpdateStaff(Guid id, UpdateStaffRequest request);
        Task<IActionResult> DeleteStaff(Guid id);
    }
}
