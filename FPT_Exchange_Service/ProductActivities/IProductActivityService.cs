using FPT_Exchange_Data.DTO.Filters;
using Microsoft.AspNetCore.Mvc;

namespace FPT_Exchange_Service.ProductActivities
{
    public interface IProductActivityService
    {
        Task<IActionResult> CreateOrder(Guid customerId, Guid productId);
        Task<IActionResult> ConfirmOrder(Guid staffId, Guid productActivityId);
        Task<IActionResult> CancelOrder(Guid productActivityId, Guid customerId);
        Task<IActionResult> RejectOrder(Guid staffId, Guid productActivityId);
        Task<IActionResult> GetProductActivity(Guid id);
        Task<IActionResult> GetProductActivities(ProductActivityFilterModel filter);
        Task<IActionResult> GetOrderByCustomerId(Guid customerId, Guid statusId);
    }
}
