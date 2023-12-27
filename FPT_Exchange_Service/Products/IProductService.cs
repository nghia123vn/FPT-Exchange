using FPT_Exchange_Data.DTO.Filters;
using FPT_Exchange_Data.DTO.Request.Post;
using FPT_Exchange_Data.DTO.Request.Put;
using FPT_Exchange_Data.DTO.View;
using Microsoft.AspNetCore.Mvc;

namespace FPT_Exchange_Service.Products
{
    public interface IProductService
    {
        public Task<IActionResult> GetProduct(Guid id);
        public Task<IEnumerable<ProductViewModel>> GetProducts(ProductFilterModel filter, int skip, int pageSize);
        public Task<IActionResult> CreateProduct(Guid UserId, CreateProductRequest request);
        public Task<IActionResult> UpdateProduct(Guid productID, UpdateProductRequest productRequest);
        public Task<IActionResult> RemoveProduct(Guid idProduct);
        public Task<IActionResult> GetProductImage(Guid idProduct);
        public Task<IActionResult> GetAllProductsSeller();
        public Task<IActionResult> GetAllProductsSellerInMonth(DateTime startDate, DateTime endDate);
        public Task<IActionResult> GetAllProductsInventory();
        public Task<IActionResult> GetProductInventoryInMonth(DateTime startDate, DateTime endDate);
        public Task<IActionResult> GetProductByer(Guid byerId);
        public Task<IActionResult> GetProductSeller(Guid sellerId);
    }
}
