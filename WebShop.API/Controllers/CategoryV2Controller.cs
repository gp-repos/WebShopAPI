using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using WebShop.API.Models;
using WebShop.API.Models.V2.Category;
using WebShop.Core.DataAccess.Interfaces;
using WebShop.Core.Domain.Entities;
using WebShop.Core.Models;

namespace WebShop.API.Controllers
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/{version:apiVersion}/Category")]
    public class CategoryV2Controller : ControllerBase
    {
        private readonly IGenericRepository<Category> _categoryRepository;
        private readonly ILogger<CategoryController> _logger;
        private readonly IMapper _mapper;

        public CategoryV2Controller(IGenericRepository<Category> categoryRepository, ILogger<CategoryController> logger,
            IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCategories([FromQuery] PageRequestParams pageRequestParams)
        {
            var categories = await _categoryRepository.GetPagedList(pageRequestParams);
            var results = _mapper.Map<IList<CategoryV2DTO>>(categories);
            var pagedResults = ApiPagedList<CategoryV2DTO>.FromList(results, categories.GetMetaData());
            return Ok(pagedResults);
        }

    }
}
