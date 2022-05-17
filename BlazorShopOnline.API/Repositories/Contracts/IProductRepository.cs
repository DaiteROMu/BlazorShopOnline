using BlazorShopOnline.API.Entities;

namespace BlazorShopOnline.API.Repositories.Contracts
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProducts();

        Task<IEnumerable<Product>> GetProductsByProductCategoryId(int id);

        Task<IEnumerable<ProductCategory>> GetProductCategories();

        Task<Product?> GetProduct(int id);

        Task<ProductCategory?> GetProductCategory(int id);
    }
}
