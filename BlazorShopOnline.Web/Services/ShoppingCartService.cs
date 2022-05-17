using BlazorShopOnline.Models.Dtos;
using BlazorShopOnline.Web.Services.Contracts;
using System.Net.Http.Json;
using System.Text.Json;

namespace BlazorShopOnline.Web.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly HttpClient _httpClient;

        public event Action<int>? OnShoppingCartChanged;

        public ShoppingCartService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CartItemDto?> AddItem(CartItemToAddDto cartItemToAddDto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/ShoppingCart", cartItemToAddDto);

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default;
                    }

                    return await response.Content.ReadFromJsonAsync<CartItemDto>();
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();

                    throw new Exception($"Http status: {response.StatusCode} - {message}");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CartItemDto?> DeleteItem(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/ShoppingCart/{id}");

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default;
                    }

                    return await response.Content.ReadFromJsonAsync<CartItemDto>();
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();

                    throw new Exception($"Http status: {response.StatusCode} - {message}");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<CartItemDto>> GetItems(int userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/ShoppingCart/{userId}/GetItems");

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return new List<CartItemDto>();
                    }

                    var items = await response.Content.ReadFromJsonAsync<List<CartItemDto>>();

                    if (items == null)
                    {
                        return new List<CartItemDto>();
                    }

                    return items;
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();

                    throw new Exception($"Http status: {response.StatusCode} - {message}");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void RaiseEventOnShoppingCartChanged(int totalQuantity)
        {
            if (OnShoppingCartChanged != null)
            {
                OnShoppingCartChanged.Invoke(totalQuantity);
            }
        }

        public async Task<CartItemDto?> UpdateQuantity(CartItemQuantityUpdateDto cartItemQuantityUpdateDto)
        {
            try
            {
                var jsonRequest = JsonSerializer.Serialize(cartItemQuantityUpdateDto);

                var content = new StringContent(jsonRequest, System.Text.Encoding.UTF8, "application/json-patch+json");

                var response = await _httpClient.PatchAsync($"api/ShoppingCart/{cartItemQuantityUpdateDto.CartItemId}", content);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<CartItemDto>();
                }

                return default;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
