using BlazorShopOnline.API.Entities;
using BlazorShopOnline.Models.Dtos;

namespace BlazorShopOnline.API.Repositories.Contracts
{
    public interface IShoppingCartRepository
    {
        Task<CartItem?> AddItem(CartItemToAddDto cartItemToAddDto);

        Task<CartItem?> UpdateQuantity(int id, CartItemQuantityUpdateDto cartItemQuantityUpdateDto);

        Task<CartItem?> DeleteItem(int id);

        Task<CartItem?> GetItem(int id);

        Task<IEnumerable<CartItem>> GetItems(int userId);
    }
}
