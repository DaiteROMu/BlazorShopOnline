using BlazorShopOnline.Models.Dtos;

namespace BlazorShopOnline.Web.Services.Contracts
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProducts();

        Task<IEnumerable<ProductDto>> GetProductsByCategoryId(int id);

        Task<ProductDto?> GetProduct(int id);

        Task<IEnumerable<ProductCategoryDto>> GetProductCategories();
    }
}
