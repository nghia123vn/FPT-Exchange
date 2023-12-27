using FPT_Exchange_Data.DTO.Filters;
using FPT_Exchange_Data.DTO.Request.Post;
using FPT_Exchange_Data.DTO.Request.Put;
using Microsoft.AspNetCore.Mvc;

namespace FPT_Exchange_Service.Categories
{
    public interface ICategoryService
    {
        Task<IActionResult> GetCategory(Guid id);
        Task<IActionResult> GetCategories(CategoryFilterModel filter);
        Task<IActionResult> CreateCategory(CreateCategoryRequest request);
        Task<IActionResult> UpdateCategory(Guid Id, UpdateCategoryRequest request);
        Task<IActionResult> DeleteCategory(Guid Id);
    }
}
