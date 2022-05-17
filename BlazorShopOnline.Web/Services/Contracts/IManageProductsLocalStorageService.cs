using BlazorShopOnline.Models.Dtos;

namespace BlazorShopOnline.Web.Services.Contracts
{
    public interface IManageProductsLocalStorageService
    {
        Task<IEnumerable<ProductDto>> GetCollection();

        Task RemoveCollection();
    }
}
