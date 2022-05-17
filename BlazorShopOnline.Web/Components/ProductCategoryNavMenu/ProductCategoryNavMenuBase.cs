using BlazorShopOnline.Models.Dtos;
using BlazorShopOnline.Web.Services.Contracts;
using Microsoft.AspNetCore.Components;

namespace BlazorShopOnline.Web.Components.ProductCategoryNavMenu
{
    public class ProductCategoryNavMenuBase : ComponentBase
    {
        [Inject]
        public IProductService? ProductService { get; set; }

        public IEnumerable<ProductCategoryDto> ProductCategoryDtos { get; set; } = Enumerable.Empty<ProductCategoryDto>();

        public string ErrorMessage { get; set; } = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                if (ProductService == null)
                {
                    throw new Exception("ProductService is null");
                }

                ProductCategoryDtos = await ProductService.GetProductCategories();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }
    }
}
