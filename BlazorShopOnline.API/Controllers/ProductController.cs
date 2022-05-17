using BlazorShopOnline.API.Extenstions;
using BlazorShopOnline.API.Repositories.Contracts;
using BlazorShopOnline.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace BlazorShopOnline.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>?>> GetProducts()
        {
            try
            {
                var products = await _productRepository.GetProducts();

                if (products == null)
                {
                    return NotFound();
                }

                return Ok(products.ConvertToDto());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDto?>> GetProduct(int id)
        {
            try
            {
                var product = await _productRepository.GetProduct(id);

                if (product == null)
                {
                    return NotFound();
                }

                return Ok(product.ConvertToDto());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route(nameof(GetProductCategories))]
        public async Task<ActionResult<IEnumerable<ProductCategoryDto>>> GetProductCategories()
        {
            try
            {
                var productCategories = await _productRepository.GetProductCategories();

                var productCategoryDtos = productCategories.ConvertToDto();

                return Ok(productCategoryDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet()]
        [Route($"{{categoryId}}/{nameof(GetProductsByCategory)}")]
        public async Task<ActionResult<ProductDto>> GetProductsByCategory(int categoryId)
        {
            try
            {
                var products = await _productRepository.GetProductsByProductCategoryId(categoryId);

                if (products == null)
                {
                    return NotFound();
                }

                return Ok(products.ConvertToDto());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
