using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebShop.Core.DataAccess.Interfaces;
using WebShop.Core.Domain.Entities;

namespace WebShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IGenericRepository<Product> _productRepository;

        public ProductController(IGenericRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducties()
        {
            var products = await _productRepository.GetAll();
            return Ok(products);
        }

        [HttpGet("{id:int}", Name = "GetProduct")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _productRepository.Get(q => q.Id == id, include: q => q.Include(x => x.Category));
            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (id < 1)
            {
                return BadRequest();
            }

            await _productRepository.Delete(id);
            await _productRepository.Save();

            return NoContent();
        }
    }
}
