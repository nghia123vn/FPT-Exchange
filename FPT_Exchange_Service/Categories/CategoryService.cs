using AutoMapper;
using AutoMapper.QueryableExtensions;
using FPT_Exchange_Data;
using FPT_Exchange_Data.DTO.Filters;
using FPT_Exchange_Data.DTO.Request.Post;
using FPT_Exchange_Data.DTO.Request.Put;
using FPT_Exchange_Data.DTO.View;
using FPT_Exchange_Data.Entities;
using FPT_Exchange_Data.Repositories.Categories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FPT_Exchange_Service.Categories
{
    public class CategoryService : BaseService, ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _categoryRepository = unitOfWork.Category;
        }

        public async Task<IActionResult> GetCategory(Guid id)
        {
            var category = await _categoryRepository.GetMany(category => category.Id.Equals(id))
                            .ProjectTo<CategoryViewModel>(_mapper.ConfigurationProvider)
                            .FirstOrDefaultAsync();
            if(category != null)
            {
                return new JsonResult(category);
            }
            return new StatusCodeResult(StatusCodes.Status404NotFound);
        }

        public async Task<IActionResult> GetCategories(CategoryFilterModel filter)
        {
            var query = _categoryRepository.GetAll();
            if(filter.Name != null)
            {
                query = query.Where(category => category.Name.Contains(filter.Name));
            }

            var categories = await query.ProjectTo<CategoryViewModel>(_mapper.ConfigurationProvider).ToListAsync();
            return new JsonResult(categories);
        }

        public async Task<IActionResult> CreateCategory(CreateCategoryRequest request)
        {
            if(_categoryRepository.Any(category => category.Name.Equals(request.Name)))
            {
                return new StatusCodeResult(StatusCodes.Status409Conflict);
            }
            var category = new Category
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
            };
            _categoryRepository.Add(category);

            var result = await _unitOfWork.SaveChanges();
            if(result > 0)
            {
                return await GetCategory(category.Id);
            }
            return new StatusCodeResult(StatusCodes.Status400BadRequest);
        }

        public async Task<IActionResult> UpdateCategory(Guid Id, UpdateCategoryRequest request)
        {
            var category = await _categoryRepository.GetMany(category => category.Id.Equals(Id)).FirstOrDefaultAsync();
            if(category == null)
            {
                return new StatusCodeResult(StatusCodes.Status404NotFound);
            }

            category.Name = request.Name ?? category.Name;

            _categoryRepository.Update(category);
            var result = await _unitOfWork.SaveChanges();
            if(result > 0)
            {
                return await GetCategory(Id);
            }
            return new StatusCodeResult(StatusCodes.Status400BadRequest);
        }

        public async Task<IActionResult> DeleteCategory(Guid Id)
        {
            var category = await _categoryRepository.GetMany(category => category.Id.Equals(Id)).FirstOrDefaultAsync();
            if (category != null)
            {
                _categoryRepository.Remove(category);

                var result = await _unitOfWork.SaveChanges();
                if(result > 0)
                {
                    return new StatusCodeResult(StatusCodes.Status204NoContent);
                }
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }
            return new StatusCodeResult(StatusCodes.Status404NotFound);
        }
    }
}
