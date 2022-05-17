using BlazorShopOnline.Models.Dtos;
using Microsoft.AspNetCore.Components;

namespace BlazorShopOnline.Web.Components.ProductCard
{
    public class ProductCardBase : ComponentBase
    {
        [Parameter]
        public ProductDto? Product { get; set; }
    }
}
