using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CodePulse.API.Models.DTO;
using CodePulse.API.Models.Domain;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IRepository<Category> _categoryRepository;
        public CategoriesController(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        [HttpPost]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> CreateCategory(CreateCategoryRequestDto request)
        {
            var category = new Category
            {
                Name = request.Name,
                UrlHandle = request.UrlHandle
            };
            await _categoryRepository.CreateAsync(category);

            var response = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle
            };
            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoryRepository.GetAllAsync();
            var response = categories.Select(category => new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle
            });
            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetCategoriesById(Guid id)
        {
            var entity = await _categoryRepository.GetAsyncById(id);
            if (entity == null)
            {
                return NotFound();
            }
            var response = new CategoryDto
            {
                Id = entity.Id,
                Name = entity.Name,
                UrlHandle = entity.UrlHandle
            };
            return Ok(response);
        }
        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> UpdateCategory(Guid id, UpdateCategoryDto request)
        {
            var category = new Category
            {
                Id = id,
                Name = request.Name,
                UrlHandle = request.UrlHandle
            };
            category = await _categoryRepository.UpdateAsync(category);
            if (category == null)
            {
                return NotFound();
            }
            var response = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle
            };
            return Ok(response);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            var category = await _categoryRepository.DeleteAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            var response = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle
            };

            return Ok(response);
        }
    }
}
