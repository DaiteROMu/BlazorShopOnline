using BlazorShopOnline.Models.Dtos;
using BlazorShopOnline.Web.Services.Contracts;
using Microsoft.AspNetCore.Components;

namespace BlazorShopOnline.Web.Components.ProductsDisplay
{
    public class ProductsDisplayBase : ComponentBase
    {
        [Inject]
        public IProductService? ProductService { get; set; }

        [Inject]
        public IShoppingCartService? ShoppingCartService { get; set; }

        [Inject]
        public IManageProductsLocalStorageService? ManageProductsLocalStorageService { get; set; }

        [Inject]
        public IManageCartItemsLocalStorageService? ManageCartItemsLocalStorageService { get; set; }

        public IEnumerable<ProductDto>? Products { get; set; }

        public string? ErrorMessage { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                if (ProductService == null)
                {
                    throw new Exception("ProductService is null");
                }

                if (ShoppingCartService == null)
                {
                    throw new Exception("ShoppingCartService is null");
                }

                await ClearLocalStorage();

                await GetProducts();

                var shoppingCartItems = await GetCartItems();

                var totalQuantity = shoppingCartItems.Sum(item => item.Quantity);

                ShoppingCartService.RaiseEventOnShoppingCartChanged(totalQuantity);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        protected IOrderedEnumerable<IGrouping<int, ProductDto>> GetGroupedProductsByCategory()
        {
            return from product in Products
                   group product by product.CategoryId into productsByCategoryGroup
                   orderby productsByCategoryGroup.Key
                   select productsByCategoryGroup;
        }

        protected string GetCategoryName(IGrouping<int, ProductDto> categoryProducts)
        {
            var productDto = categoryProducts.FirstOrDefault(pg => pg.CategoryId == categoryProducts.Key);

            if (productDto == null)
            {
                return string.Empty;
            }

            return productDto.CategoryName;
        }

        private async Task GetProducts()
        {
            if (ManageProductsLocalStorageService != null)
            {
                Products = await ManageProductsLocalStorageService.GetCollection();
                return;
            }

            if (ProductService != null)
            {
                Products = await ProductService.GetProducts();
            }
        }

        private async Task<List<CartItemDto>> GetCartItems()
        {
            if (ManageCartItemsLocalStorageService != null)
            {
                return await ManageCartItemsLocalStorageService.GetCollection();
            }

            if (ShoppingCartService != null)
            {
                return await ShoppingCartService.GetItems(HardCoded.USERID);
            }

            return new List<CartItemDto>();
        }

        private async Task ClearLocalStorage()
        {
            if (ManageCartItemsLocalStorageService != null)
            {
                await ManageCartItemsLocalStorageService.RemoveCollection();
            }

            if (ManageProductsLocalStorageService != null)
            {
                await ManageProductsLocalStorageService.RemoveCollection();
            }
        }
    }
}
