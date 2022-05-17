using BlazorShopOnline.API.Data;
using BlazorShopOnline.API.Entities;
using BlazorShopOnline.API.Repositories.Contracts;
using BlazorShopOnline.Models.Dtos;
using Microsoft.EntityFrameworkCore;

namespace BlazorShopOnline.API.Repositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly BlazorShopOnlineDbContext _blazorShopOnlineDbContext;

        public ShoppingCartRepository(BlazorShopOnlineDbContext blazorShopOnlineDbContext)
        {
            _blazorShopOnlineDbContext = blazorShopOnlineDbContext;
        }

        private async Task<bool> CartItemExists(int cartId, int productId)
        {
            return await _blazorShopOnlineDbContext.CartItems.AnyAsync(x => x.CartId == cartId && x.ProductId == productId);
        }

        public async Task<CartItem?> AddItem(CartItemToAddDto cartItemToAddDto)
        {
            if (!await CartItemExists(cartItemToAddDto.CartId, cartItemToAddDto.ProductId))
            {
                var item = await (from product in _blazorShopOnlineDbContext.Products
                                  where product.Id == cartItemToAddDto.ProductId
                                  select new CartItem
                                  {
                                      CartId = cartItemToAddDto.CartId,
                                      ProductId = cartItemToAddDto.ProductId,
                                      Quantity = cartItemToAddDto.Quantity
                                  }).SingleOrDefaultAsync();

                if (item != null)
                {
                    var result = await _blazorShopOnlineDbContext.CartItems.AddAsync(item);
                    await _blazorShopOnlineDbContext.SaveChangesAsync();

                    return result.Entity;
                }
            }

            return null;
        }

        public async Task<CartItem?> DeleteItem(int id)
        {
            var item = await _blazorShopOnlineDbContext.CartItems.FindAsync(id);

            if (item != null)
            {
                _blazorShopOnlineDbContext.CartItems.Remove(item);
                await _blazorShopOnlineDbContext.SaveChangesAsync();
            }

            return item;
        }

        public async Task<CartItem?> GetItem(int id)
        {
            return await (from cart in _blazorShopOnlineDbContext.Carts
                          join cartItem in _blazorShopOnlineDbContext.CartItems
                          on cart.Id equals cartItem.CartId
                          where cartItem.Id == id
                          select new CartItem
                          {
                              Id = cartItem.Id,
                              CartId = cartItem.CartId,
                              ProductId = cartItem.ProductId,
                              Quantity = cartItem.Quantity
                          }).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<CartItem>> GetItems(int userId)
        {
            return await (from cart in _blazorShopOnlineDbContext.Carts
                          join cartItem in _blazorShopOnlineDbContext.CartItems
                          on cart.Id equals cartItem.CartId
                          where cart.UserId == userId
                          select new CartItem
                          {
                              Id = cartItem.Id,
                              CartId = cartItem.CartId,
                              ProductId = cartItem.ProductId,
                              Quantity = cartItem.Quantity
                          }).ToListAsync();
        }

        public async Task<CartItem?> UpdateQuantity(int id, CartItemQuantityUpdateDto cartItemQuantityUpdateDto)
        {
            var item = await _blazorShopOnlineDbContext.CartItems.FindAsync(id);

            if (item != null)
            {
                item.Quantity = cartItemQuantityUpdateDto.Quantity;

                await _blazorShopOnlineDbContext.SaveChangesAsync();

                return item;
            }

            return null;
        }
    }
}
