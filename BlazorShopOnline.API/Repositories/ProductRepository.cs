using BlazorShopOnline.API.Data;
using BlazorShopOnline.API.Entities;
using BlazorShopOnline.API.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace BlazorShopOnline.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly BlazorShopOnlineDbContext _dbContext;

        public ProductRepository(BlazorShopOnlineDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<ProductCategory>> GetProductCategories()
        {
            return await _dbContext.ProductCategories.ToListAsync();
        }

        public async Task<Product?> GetProduct(int id)
        {
            return await _dbContext.Products.Include(p => p.ProductCategory).SingleOrDefaultAsync(p => p.Id == id);
        }

        public async Task<ProductCategory?> GetProductCategory(int id)
        {
            return await _dbContext.ProductCategories.FindAsync(id);
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _dbContext.Products.Include(p => p.ProductCategory).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByProductCategoryId(int id)
        {
            return await _dbContext.Products.Include(p => p.ProductCategory).Where(p => p.CategoryId == id).ToListAsync();
        }
    }
}
