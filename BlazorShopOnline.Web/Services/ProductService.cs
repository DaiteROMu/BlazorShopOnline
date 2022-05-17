using BlazorShopOnline.Models.Dtos;
using BlazorShopOnline.Web.Services.Contracts;
using System.Net.Http.Json;

namespace BlazorShopOnline.Web.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;

        public ProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ProductDto?> GetProduct(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/Product/{id}");

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default;
                    }

                    return await response.Content.ReadFromJsonAsync<ProductDto>();
                }

                var errorMessage = await response.Content.ReadAsStringAsync();

                throw new Exception(errorMessage);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ProductCategoryDto>> GetProductCategories()
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/Product/GetProductCategories");

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<ProductCategoryDto>();
                    }

                    var productCategories = await response.Content.ReadFromJsonAsync<IEnumerable<ProductCategoryDto>>();

                    if (productCategories == null)
                    {
                        return Enumerable.Empty<ProductCategoryDto>();
                    }

                    return productCategories;
                }

                var errorMessage = await response.Content.ReadAsStringAsync();

                throw new Exception(errorMessage);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/Product");

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<ProductDto>();
                    }

                    var products = await response.Content.ReadFromJsonAsync<IEnumerable<ProductDto>>();

                    if (products == null)
                    {
                        return Enumerable.Empty<ProductDto>();
                    }

                    return products;
                }

                var errorMessage = await response.Content.ReadAsStringAsync();

                throw new Exception(errorMessage);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ProductDto>> GetProductsByCategoryId(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/Product/{id}/GetProductsByCategory");

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<ProductDto>();
                    }

                    var products = await response.Content.ReadFromJsonAsync<IEnumerable<ProductDto>>();

                    if (products == null)
                    {
                        return Enumerable.Empty<ProductDto>();
                    }

                    return products;
                }

                var errorMessage = await response.Content.ReadAsStringAsync();

                throw new Exception(errorMessage);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
