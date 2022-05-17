using Blazored.LocalStorage;
using BlazorShopOnline.Models.Dtos;
using BlazorShopOnline.Web.Services.Contracts;

namespace BlazorShopOnline.Web.Services
{
    public class ManageCartItemsLocalStorageService : IManageCartItemsLocalStorageService
    {
        private readonly ILocalStorageService _localStorageService;
        private readonly IShoppingCartService _shoppingCartService;

        private readonly string _collectionKey = "CartItemCollection";

        public ManageCartItemsLocalStorageService(ILocalStorageService localStorageService, IShoppingCartService shoppingCartService)
        {
            _localStorageService = localStorageService;
            _shoppingCartService = shoppingCartService;
        }

        public async Task<List<CartItemDto>> GetCollection()
        {
            return await _localStorageService.GetItemAsync<List<CartItemDto>>(_collectionKey) ?? await AddCollection();
        }

        public async Task RemoveCollection()
        {
            await _localStorageService.RemoveItemAsync(_collectionKey);
        }

        public async Task SaveCollection(List<CartItemDto> cartItemDtos)
        {
            await _localStorageService.SetItemAsync(_collectionKey, cartItemDtos);
        }

        public async Task<List<CartItemDto>> AddCollection()
        {
            var shoppingCartCollection = await _shoppingCartService.GetItems(HardCoded.USERID);

            if (shoppingCartCollection != null)
            {
                await _localStorageService.SetItemAsync(_collectionKey, shoppingCartCollection);

                return shoppingCartCollection;
            }

            return new List<CartItemDto>();
        }
    }
}
