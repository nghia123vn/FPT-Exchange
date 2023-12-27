using FPT_Exchange_Data.DTO.Filters;
using FPT_Exchange_Data.DTO.Request.Post;
using FPT_Exchange_Data.DTO.Request.Put;
using FPT_Exchange_Service.Categories;
using Microsoft.AspNetCore.Mvc;

namespace FPT_Exchange_API.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetCategory([FromRoute] Guid id)
        {
            try
            {
                var result = await _categoryService.GetCategory(id);
                if (result is JsonResult jsonResult)
                {
                    return Ok(jsonResult.Value);
                }
                return NotFound("Not found this category.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories([FromQuery] CategoryFilterModel filter)
        {
            try
            {
                var result = await _categoryService.GetCategories(filter);
                if (result is JsonResult jsonResult)
                {
                    return Ok(jsonResult.Value);
                }
                return BadRequest("Something wrong!!!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequest request)
        {
            try
            {
                var result = await _categoryService.CreateCategory(request);
                if (result is JsonResult jsonResult)
                {
                    return StatusCode(StatusCodes.Status201Created, jsonResult.Value);
                }
                if (result is StatusCodeResult statusCodeResult)
                {
                    if (statusCodeResult.StatusCode == StatusCodes.Status409Conflict)
                    {
                        return StatusCode(StatusCodes.Status409Conflict, "This category name already exists.");
                    }
                }
                return BadRequest("Some things wrong when save to database!!!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateCategory([FromRoute] Guid id,
                                                        [FromBody] UpdateCategoryRequest request)
        {
            try
            {
                var result = await _categoryService.UpdateCategory(id, request);
                if (result is JsonResult jsonResult)
                {
                    return StatusCode(StatusCodes.Status201Created, jsonResult.Value);
                }
                if (result is StatusCodeResult statusCodeResult)
                {
                    if (statusCodeResult.StatusCode == StatusCodes.Status404NotFound)
                    {
                        return StatusCode(StatusCodes.Status404NotFound, "Not found this category");
                    }
                    if (statusCodeResult.StatusCode == StatusCodes.Status400BadRequest)
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, "Somethings wrong when save to database!!!");
                    }
                }
                return BadRequest("Somethings wrong!!!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
        {
            try
            {
                var result = await _categoryService.DeleteCategory(id);
                if (result is StatusCodeResult statusCodeResult)
                {
                    if (statusCodeResult.StatusCode == StatusCodes.Status204NoContent)
                    {
                        return StatusCode(StatusCodes.Status204NoContent);
                    }
                    if (statusCodeResult.StatusCode == StatusCodes.Status400BadRequest)
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, "Somethings wrong when save to database!!!");
                    }
                    if (statusCodeResult.StatusCode == StatusCodes.Status404NotFound)
                    {
                        return StatusCode(StatusCodes.Status404NotFound, "Not found this category!!!");
                    }
                }
                return BadRequest("Somethings wrong!!!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
