using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebShop.Core.DataAccess.Interfaces;
using WebShop.Core.Domain.Entities;

namespace WebShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IGenericRepository<Category> _categoryRepository;

        public CategoryController(IGenericRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategoryies()
        {
            var categories = await _categoryRepository.GetAll();
            return Ok(categories);
        }

        [HttpGet("{id:int}", Name = "GetCategory")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await _categoryRepository.Get(q => q.Id == id, include: q => q.Include(x => x.Products));
            if (category == null)
                return NotFound();

            return Ok(category);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            if (id < 1)
            {
                return BadRequest();
            }

            await _categoryRepository.Delete(id);
            await _categoryRepository.Save();

            return NoContent();
        }
    }
}
