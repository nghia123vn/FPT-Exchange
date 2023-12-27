using FPT_Exchange_Data.DTO.Filters;
using FPT_Exchange_Data.DTO.Request.Post;
using FPT_Exchange_Data.DTO.Request.Put;
using FPT_Exchange_Service.Products;
using FPT_Exchange_Utility.Helpers;
using FPT_Exchange_Utility.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace FPT_Exchange_API.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly AppSetting _appSettings;

        public ProductController(IProductService productService, IOptions<AppSetting> appSettings)
        {
            _productService = productService;
            _appSettings = appSettings.Value;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts([FromQuery] ProductFilterModel filter, int page = 1)
        {
            int pageSize = 20;
            int skip = (page - 1) * pageSize;

            var result = await _productService.GetProducts(filter, skip, pageSize);

            var totalItems = result.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var response = new
            {
                TotalItems = totalItems,
                TotalPages = totalPages,
                Page = page,
                PageSize = pageSize,
                Products = result
            };

            return Ok(response);
        }


        [HttpGet]
        [Route("{id}")]
        public Task<IActionResult> GetProductByID([FromRoute] Guid id)
        {
            return _productService.GetProduct(id);
        }

        [HttpGet]
        [Route("Buyer")]
        public Task<IActionResult> GetProductByer()
        {
            try
            {
                Request.Headers.TryGetValue("Authorization", out var authHeader);
                var accessToken = authHeader.FirstOrDefault().Substring("Bearer ".Length);

                ClaimsPrincipal user = JwtHelper.DecodeJwtToken(accessToken, _appSettings.Secret);
                Guid userId = Guid.Parse(user.FindFirstValue("userId"));
                if (user != null)
                {
                    return _productService.GetProductByer(userId);
                }
                return Task.FromResult<IActionResult>(Unauthorized());
            }
            catch (Exception ex)
            {
                return Task.FromResult<IActionResult>(BadRequest(ex));
            }
        }

        [HttpGet]
        [Route("selling")]
        public Task<IActionResult> GetProductSeller()
        {
            try
            {
                Request.Headers.TryGetValue("Authorization", out var authHeader);
                var accessToken = authHeader.FirstOrDefault().Substring("Bearer ".Length);

                ClaimsPrincipal user = JwtHelper.DecodeJwtToken(accessToken, _appSettings.Secret);
                Guid userId = Guid.Parse(user.FindFirstValue("userId"));
                if (user != null)
                {
                    return _productService.GetProductSeller(userId);
                }
                return Task.FromResult<IActionResult>(Unauthorized());
            }
            catch (Exception ex)
            {
                return Task.FromResult<IActionResult>(BadRequest(ex));
            }
        }
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateProduct([FromForm] CreateProductRequest product)
        {
            try
            {
                if (product.ImageProducts.Count == 0)
                {
                    return BadRequest("Please input three image");
                }
                //var user = (AuthModel)HttpContext.Items["User"]!;
                Request.Headers.TryGetValue("Authorization", out var authHeader);
                var accessToken = authHeader.FirstOrDefault().Substring("Bearer ".Length);

                ClaimsPrincipal user = JwtHelper.DecodeJwtToken(accessToken, _appSettings.Secret);
                Guid userId = Guid.Parse(user.FindFirstValue("userId"));
                if (user != null)
                {
                    var pro = await _productService.CreateProduct(userId, product);
                    if (pro is JsonResult jsonResult)
                    {
                        if (jsonResult.Value is null) return BadRequest("Do not null");
                        return StatusCode(StatusCodes.Status201Created, jsonResult.Value);
                    }
                    return StatusCode(StatusCodes.Status400BadRequest, "Create Product fail");
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return BadRequest("Can't create product" + ex);
            }
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateProduct([FromRoute] Guid id, [FromForm] UpdateProductRequest product)
        {
            try
            {
                var pro = await _productService.UpdateProduct(id, product);
                if (pro is JsonResult jsonResult)
                {
                    return StatusCode(StatusCodes.Status201Created, jsonResult.Value);
                }
                else if (pro is StatusCodeResult status)
                {
                    if (status.StatusCode == 500) return StatusCode(StatusCodes.Status500InternalServerError);
                    if (status.StatusCode == 400) return StatusCode(StatusCodes.Status400BadRequest, "Please input four image");

                }
                return BadRequest("Can't update product");
            }
            catch (Exception ex)
            {
                return BadRequest("Can't update product, ERROR: " + ex.Message);
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> RemoveProduct([FromRoute] Guid id)
        {
            try
            {
                var pro = await _productService.RemoveProduct(id);
                if (pro is JsonResult jsonResult)
                {
                    if (jsonResult.StatusCode == 500) return BadRequest("Can't remove product");
                    if (jsonResult.Value is null) return BadRequest("Product Id not exist");
                    return StatusCode(StatusCodes.Status204NoContent);
                }
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            catch (Exception ex)
            {
                return BadRequest("Can't remove product, ERROR: " + ex.Message);
            }
        }
        [HttpGet]
        [Route("seller")]
        public Task<IActionResult> GetAllProductsSeller()
        {
            return _productService.GetAllProductsSeller();
        }
        [HttpGet]
        [Route("inventory")]
        public Task<IActionResult> GetAllProductsInventory()
        {
            return _productService.GetAllProductsInventory();
        }
        [HttpGet]
        [Route("inventory/product-in-month")]
        public Task<IActionResult> GetProductInventoryInMonth(DateTime startDate, DateTime endDate)
        {
            return _productService.GetProductInventoryInMonth(startDate, endDate);
        }
        [HttpGet]
        [Route("seller/product-in-month")]
        public Task<IActionResult> GetAllProductsSellerInMonth(DateTime startDate, DateTime endDate)
        {
            return _productService.GetAllProductsSellerInMonth(startDate, endDate);
        }
    }
}
