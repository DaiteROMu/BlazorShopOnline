using BlazorShopOnline.Models.Dtos;
using BlazorShopOnline.Web.Services.Contracts;
using Microsoft.AspNetCore.Components;

namespace BlazorShopOnline.Web.Components.ProductDetails
{
    public class ProductDetailsBase : ComponentBase
    {
        [Parameter]
        public int Id { get; set; }

        [Inject]
        public IProductService? ProductService { get; set; }

        [Inject]
        public IShoppingCartService? ShoppingCartService { get; set; }

        [Inject]
        public IManageProductsLocalStorageService? ManageProductsLocalStorageService { get; set; }

        [Inject]
        public IManageCartItemsLocalStorageService? ManageCartItemsLocalStorageService { get; set; }

        [Inject]
        public NavigationManager? NavigationManager { get; set; }

        public ProductDto? Product { get; set; }

        public string ErrorMessage { get; set; } = string.Empty;

        private List<CartItemDto> ShoppingCartItems { get; set; } = new List<CartItemDto>();

        protected override async Task OnInitializedAsync()
        {
            try
            {
                if (ProductService == null)
                {
                    throw new ArgumentNullException(nameof(ProductService));
                }

                if (ManageCartItemsLocalStorageService != null)
                {
                    ShoppingCartItems = await ManageCartItemsLocalStorageService.GetCollection();
                }

                Product = await GetProductById(Id);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        protected async void AddToCart_Click(CartItemToAddDto cartItemToAddDto)
        {
            try
            {
                if (ProductService == null)
                {
                    throw new ArgumentNullException(nameof(ProductService));
                }

                if (ShoppingCartService == null)
                {
                    throw new ArgumentNullException(nameof(ShoppingCartService));
                }

                var cartItemDto = await ShoppingCartService.AddItem(cartItemToAddDto);

                if (NavigationManager == null)
                {
                    throw new ArgumentNullException(nameof(NavigationManager));
                }

                if (cartItemDto != null)
                {
                    ShoppingCartItems.Add(cartItemDto);

                    if (ManageCartItemsLocalStorageService != null)
                    {
                        await ManageCartItemsLocalStorageService.SaveCollection(ShoppingCartItems);
                    }
                }

                NavigationManager.NavigateTo("/ShoppingCart");
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<ProductDto?> GetProductById(int id)
        {
            if (ManageProductsLocalStorageService != null)
            {
                var productDtos = await ManageProductsLocalStorageService.GetCollection();

                if (productDtos != null)
                {
                    var productDto = productDtos.SingleOrDefault(x => x.Id == id);

                    if (productDto != null)
                    {
                        return productDto;
                    }
                }
            }

            if (ProductService != null)
            {
                return await ProductService.GetProduct(Id);
            }

            return default;
        }
    }
}
