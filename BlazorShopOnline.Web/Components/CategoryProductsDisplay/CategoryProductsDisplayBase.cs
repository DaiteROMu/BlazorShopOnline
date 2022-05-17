using BlazorShopOnline.Models.Dtos;
using BlazorShopOnline.Web.Services.Contracts;
using Microsoft.AspNetCore.Components;

namespace BlazorShopOnline.Web.Components.CategoryProductsDisplay
{
    public class CategoryProductsDisplayBase : ComponentBase
    {
        [Parameter]
        public int CategoryId { get; set; }

        [Inject]
        public IProductService? ProductService { get; set; }

        [Inject]
        public IManageProductsLocalStorageService? ManageProductsLocalStorageService { get; set; }

        public IEnumerable<ProductDto> Products { get; set; } = Enumerable.Empty<ProductDto>();

        public string CategoryName { get; set; } = string.Empty;

        public string ErrorMessage { get; set; } = string.Empty;

        protected override async Task OnParametersSetAsync()
        {
            try
            {
                if (ProductService == null)
                {
                    throw new Exception("ProductService is null");
                }

                Products = await GetProductsByCategoryId(CategoryId);

                if (Products?.Count() > 0)
                {
                    var productDto = Products.FirstOrDefault(p => p.CategoryId == CategoryId);

                    if (productDto != null)
                    {
                        CategoryName = productDto.CategoryName;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        private async Task<IEnumerable<ProductDto>> GetProductsByCategoryId(int categoryId)
        {
            if (ManageProductsLocalStorageService != null)
            {
                var products = await ManageProductsLocalStorageService.GetCollection();

                return products.Where(p => p.CategoryId == categoryId);
            }

            if (ProductService != null)
            {
                return await ProductService.GetProductsByCategoryId(categoryId);
            }

            return Enumerable.Empty<ProductDto>();
        }
    }
}
