using BlazorShopOnline.Models.Dtos;
using BlazorShopOnline.Web.Services.Contracts;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorShopOnline.Web.Components.ShoppingCart
{
    public class ShoppingCartBase : ComponentBase
    {
        [Inject]
        public IJSRuntime? Js { get; set; }

        [Inject]
        public IShoppingCartService? ShoppingCartService { get; set; }

        [Inject]
        public IManageCartItemsLocalStorageService? ManageCartItemsLocalStorageService { get; set; }

        public List<CartItemDto> ShoppingCartItems { get; set; } = new List<CartItemDto>();

        public string ErrorMessage { get; set; } = string.Empty;

        protected decimal TotalPrice { get; set; }

        protected int TotalQuantity { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                if (ShoppingCartService == null)
                {
                    throw new InvalidOperationException("ShoppingCartService is null");
                }

                await GetShoppingCartItems();

                ShoppingCartChanged();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        protected async Task DeleteCartItem_Click(int id)
        {
            try
            {
                if (ShoppingCartService == null)
                {
                    throw new InvalidOperationException("ShoppingCartService is null");
                }

                var cartItemDto = await ShoppingCartService.DeleteItem(id);

                if (cartItemDto == null)
                {
                    return;
                }

                await RemoveCartItem(cartItemDto.Id);

                ShoppingCartChanged();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        private CartItemDto? GetCartItemDto(int id)
        {
            return ShoppingCartItems.FirstOrDefault(x => x.Id == id);
        }

        private async Task RemoveCartItem(int id)
        {
            var cartItemDto = GetCartItemDto(id);

            if (cartItemDto == null)
            {
                return;
            }

            ShoppingCartItems.Remove(cartItemDto);

            await UpdateLocalStorage();
        }

        protected async Task UpdateQuantityCartItem_Click(int id, int quantity)
        {
            try
            {
                if (ShoppingCartService == null)
                {
                    throw new InvalidOperationException("ShoppingCartService is null");
                }

                if (quantity > 0)
                {
                    var updateItemDto = new CartItemQuantityUpdateDto
                    {
                        CartItemId = id,
                        Quantity = quantity
                    };

                    var returnedUpdateItemDto = await ShoppingCartService.UpdateQuantity(updateItemDto);

                    if (returnedUpdateItemDto == null)
                    {
                        return;
                    }

                    await UpdateItemTotalPrice(returnedUpdateItemDto);

                    ShoppingCartChanged();

                    if (Js != null)
                    {
                        await Js.InvokeVoidAsync("makeUpdateQuantityBtnVisible", id, false);
                    }
                }
                else
                {
                    var item = ShoppingCartItems.FirstOrDefault(x => x.Id == id);

                    if (item != null)
                    {
                        item.Quantity = 1;
                        item.TotalPrice = item.Price;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        private async Task UpdateItemTotalPrice(CartItemDto cartItemDto)
        {
            var item = GetCartItemDto(cartItemDto.Id);

            if (item != null)
            {
                item.TotalPrice = cartItemDto.Price * cartItemDto.Quantity;
            }

            await UpdateLocalStorage();
        }

        private void CalculateCartSummaryTotals()
        {
            SetTotalPrice();
            SetTotalQuantity();
        }

        private void SetTotalPrice()
        {
            TotalPrice = ShoppingCartItems.Sum(p => p.TotalPrice);
        }

        private void SetTotalQuantity()
        {
            TotalQuantity = ShoppingCartItems.Sum(p => p.Quantity);
        }

        protected async Task UpdateQuantity_Input(int id)
        {
            if (Js != null)
            {
                await Js.InvokeVoidAsync("makeUpdateQuantityBtnVisible", id, true);
            }
        }

        private void ShoppingCartChanged()
        {
            if (ShoppingCartService == null)
            {
                throw new ArgumentNullException("ShoppingCartService is null");
            }

            CalculateCartSummaryTotals();

            ShoppingCartService.RaiseEventOnShoppingCartChanged(TotalQuantity);
        }

        private async Task GetShoppingCartItems()
        {
            if (ManageCartItemsLocalStorageService != null)
            {
                ShoppingCartItems = await ManageCartItemsLocalStorageService.GetCollection();
            }

            if (ShoppingCartService != null)
            {
                ShoppingCartItems = await ShoppingCartService.GetItems(HardCoded.USERID);
            }
        }

        private async Task UpdateLocalStorage()
        {
            if (ManageCartItemsLocalStorageService != null)
            {
                await ManageCartItemsLocalStorageService.SaveCollection(ShoppingCartItems);
            }
        }
    }
}
