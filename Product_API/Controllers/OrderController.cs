using FPT_Exchange_Data.DTO.Filters;
using FPT_Exchange_Service.ProductActivities;
using FPT_Exchange_Utility.Constants;
using FPT_Exchange_Utility.Extensions;
using FPT_Exchange_Utility.Helpers;
using FPT_Exchange_Utility.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Security.Claims;

namespace Product_API.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IProductActivityService _productActivityService;
        private readonly AppSetting _appSettings;
        public OrderController(IProductActivityService productActivityService, IOptions<AppSetting> appSettings)
        {
            _productActivityService = productActivityService;
            _appSettings = appSettings.Value;
        }

        [HttpGet]
        [Route("success")]
        public async Task<IActionResult> GetSuccessOrder()
        {
            var filter = new ProductActivityFilterModel
            {
                Status = Guid.Parse(ProductStatus.Confirmed.GetEnumMember())
            };
            var result = await _productActivityService.GetProductActivities(filter);
            if (result is JsonResult jsonResult)
            {
                return Ok(jsonResult.Value);
            }
            return BadRequest();

        }

        [HttpGet]
        public async Task<IActionResult> GetProductActivitiesStatusProcessing()
        {
            var filter = new ProductActivityFilterModel
            {
                Status = Guid.Parse(ProductStatus.Processing.GetEnumMember())
            };
            var result = await _productActivityService.GetProductActivities(filter);
            if (result is JsonResult jsonResult)
            {
                return Ok(jsonResult.Value);
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("customer/processing")]
        public async Task<IActionResult> GetProcessingOrdersOfCurrentCustomer()
        {
            Request.Headers.TryGetValue("Authorization", out var authHeader);
            var accessToken = authHeader.FirstOrDefault().Substring("Bearer ".Length);

            ClaimsPrincipal user = JwtHelper.DecodeJwtToken(accessToken, _appSettings.Secret);
            Guid userId = Guid.Parse(user.FindFirstValue("userId"));

            //Guid userId = Guid.Parse("dd300ad0-3ccd-4781-81ab-68a44f4670be");
            try
            {
                var statusId = Guid.Parse(ProductStatus.Processing.GetEnumMember());
                var result = await _productActivityService.GetOrderByCustomerId(userId, statusId);
                if (result is JsonResult jsonResult)
                {
                    return Ok(jsonResult.Value);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




        [HttpPost]
        [Route("{id}")]
        public async Task<IActionResult> CreateOrder([FromRoute] Guid id)
        {
            try
            {
                Request.Headers.TryGetValue("Authorization", out var authHeader);
                var accessToken = authHeader.FirstOrDefault().Substring("Bearer ".Length);

                ClaimsPrincipal user = JwtHelper.DecodeJwtToken(accessToken, _appSettings.Secret);
                Guid customerId = Guid.Parse(user.FindFirstValue("userId"));


                var redis = ConnectionMultiplexer.Connect(_appSettings.RedisConnectionString);
                //if (redis.IsConnected)
                //{
                //    Console.WriteLine("Redis server connection successful!");
                //}
                //else
                //{
                //    Console.WriteLine("Redis server connection failed!");
                //}

                var database = redis.GetDatabase();
                string lockKey = $"product_lock:{id}";

                //Set if Not Exists
                // Try to set the lock key with SETNX command (expiration of 10 seconds)
                bool isLockAcquired = database.StringSet(lockKey, id.ToString(), TimeSpan.FromSeconds(10), When.NotExists);
                if (!isLockAcquired)
                {
                    // Lock could not be acquired, which means someone else is already ordering the same item.
                    // You can handle this situation as per your requirement (e.g., return a conflict response).
                    return StatusCode(StatusCodes.Status409Conflict, "Someone else is already ordering this item. Please try again later.");
                }
                try
                {
                    // Now that we have the lock, proceed with the order creation logic
                    var result = await _productActivityService.CreateOrder(customerId, id);

                    if (result is JsonResult jsonResult)
                    {
                        return StatusCode(StatusCodes.Status201Created, jsonResult.Value);
                    }
                    else if (result is StatusCodeResult status)
                    {
                        if (status.StatusCode == StatusCodes.Status409Conflict)
                        {
                            return StatusCode(StatusCodes.Status409Conflict, "Not enough score.");
                        }
                        if (status.StatusCode == StatusCodes.Status400BadRequest)
                        {
                            return StatusCode(StatusCodes.Status400BadRequest, "Somethings wrong when order this product please check again.");
                        }
                    }

                    return Unauthorized();
                }
                finally
                {
                    // Release the lock after completing the order creation or encountering an exception
                    //database.LockRelease(lockKey, "lock_value");
                    database.KeyDelete(lockKey);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPut]
        [Route("{id}")]
        //[Authorize(UserRole.Staff)]
        public async Task<IActionResult> ConfirmOrder([FromRoute] Guid id)
        {
            try
            {
                Request.Headers.TryGetValue("Authorization", out var authHeader);
                var accessToken = authHeader.FirstOrDefault().Substring("Bearer ".Length);

                ClaimsPrincipal user = JwtHelper.DecodeJwtToken(accessToken, _appSettings.Secret);
                Guid staffId = Guid.Parse(user.FindFirstValue("userId"));
                //var user = (AuthModel)HttpContext.Items["User"]!;
                if (user != null)
                {
                    var result = await _productActivityService.ConfirmOrder(staffId, id);
                    if (result is JsonResult jsonResult)
                    {
                        return StatusCode(StatusCodes.Status201Created, jsonResult.Value);
                    }
                    if (result is StatusCodeResult statusCodeResult)
                    {
                        if (statusCodeResult.StatusCode == StatusCodes.Status404NotFound)
                        {
                            return StatusCode(StatusCodes.Status404NotFound, "Not found this request order.");
                        }
                        if (statusCodeResult.StatusCode == StatusCodes.Status409Conflict)
                        {
                            return StatusCode(StatusCodes.Status409Conflict, "This user not enough score.");
                        }
                    }
                    return BadRequest("Somethings wrong when savechange to database...");
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("reject/{id}")]
        public async Task<IActionResult> RejectOrder([FromRoute] Guid id)
        {
            try
            {
                Request.Headers.TryGetValue("Authorization", out var authHeader);
                var accessToken = authHeader.FirstOrDefault().Substring("Bearer ".Length);

                ClaimsPrincipal user = JwtHelper.DecodeJwtToken(accessToken, _appSettings.Secret);
                Guid staffId = Guid.Parse(user.FindFirstValue("userId"));
                //var user = (AuthModel)HttpContext.Items["User"]!;
                if (user != null)
                {
                    var result = await _productActivityService.RejectOrder(staffId, id);
                    if (result is JsonResult jsonResult)
                    {
                        return StatusCode(StatusCodes.Status201Created, jsonResult.Value);
                    }
                    if (result is StatusCodeResult statusCodeResult)
                    {
                        if (statusCodeResult.StatusCode == StatusCodes.Status404NotFound)
                        {
                            return StatusCode(StatusCodes.Status404NotFound, "Not found this request order.");
                        }
                        if (statusCodeResult.StatusCode == StatusCodes.Status400BadRequest)
                        {
                            return StatusCode(StatusCodes.Status400BadRequest, "Error when save to database.");
                        }
                    }
                    return BadRequest("Somethings wrong...");
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("cancel/{id}")]
        public async Task<IActionResult> CancelOrder([FromRoute] Guid id)
        {
            try
            {
                Request.Headers.TryGetValue("Authorization", out var authHeader);
                var accessToken = authHeader.FirstOrDefault().Substring("Bearer ".Length);

                ClaimsPrincipal user = JwtHelper.DecodeJwtToken(accessToken, _appSettings.Secret);
                Guid customerId = Guid.Parse(user.FindFirstValue("userId"));

                //Guid userId = Guid.Parse("dd300ad0-3ccd-4781-81ab-68a44f4670be");
                var result = await _productActivityService.CancelOrder(id, customerId);
                if (result is JsonResult jsonResult)
                {
                    return StatusCode(StatusCodes.Status201Created, jsonResult.Value);
                }
                else if (result is StatusCodeResult status)
                {
                    if (status.StatusCode == StatusCodes.Status400BadRequest)
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, "Error when savechange to database.");
                    }
                    if (status.StatusCode == StatusCodes.Status404NotFound)
                    {
                        return StatusCode(StatusCodes.Status404NotFound, "Not found this request order.");
                    }
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
