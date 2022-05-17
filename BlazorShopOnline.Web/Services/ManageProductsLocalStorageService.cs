using Blazored.LocalStorage;
using BlazorShopOnline.Models.Dtos;
using BlazorShopOnline.Web.Services.Contracts;

namespace BlazorShopOnline.Web.Services
{
    public class ManageProductsLocalStorageService : IManageProductsLocalStorageService
    {
        private readonly ILocalStorageService _localStorageService;
        private readonly IProductService _productService;

        private readonly string _collectionKey = "ProductCollection";

        public ManageProductsLocalStorageService(ILocalStorageService localStorageService, IProductService productService)
        {
            _localStorageService = localStorageService;
            _productService = productService;
        }

        public async Task<IEnumerable<ProductDto>> GetCollection()
        {
            return await _localStorageService.GetItemAsync<IEnumerable<ProductDto>>(_collectionKey) ?? await AddCollection();
        }

        public async Task RemoveCollection()
        {
            await _localStorageService.RemoveItemAsync(_collectionKey);
        }

        private async Task<IEnumerable<ProductDto>> AddCollection()
        {
            var productCollection = await _productService.GetProducts();

            if (productCollection != null)
            {
                await _localStorageService.SetItemAsync(_collectionKey, productCollection);

                return productCollection;
            }

            return Enumerable.Empty<ProductDto>();
        }
    }
}
