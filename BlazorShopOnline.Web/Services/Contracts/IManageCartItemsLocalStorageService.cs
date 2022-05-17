using BlazorShopOnline.Models.Dtos;

namespace BlazorShopOnline.Web.Services.Contracts
{
    public interface IManageCartItemsLocalStorageService
    {
        Task<List<CartItemDto>> GetCollection();

        Task SaveCollection(List<CartItemDto> cartItemDtos);

        Task RemoveCollection();
    }
}
