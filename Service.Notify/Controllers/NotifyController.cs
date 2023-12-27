

using FPT_Exchange_Data.DTO.Request.Post;
using FPT_Exchange_Service.Notify;
using FPT_Exchange_Utility.Helpers;
using FPT_Exchange_Utility.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Service.Notify.Controllers
{
    [Route("api/notifications")]
    [ApiController]
    public class NotifyController : ControllerBase
    {
        private readonly INotifyService _notificationsService;
        private readonly SemaphoreSlim _notificationsLock = new SemaphoreSlim(5, 10);
        private readonly AppSetting _appSettings;

        public NotifyController(INotifyService notificationsService, IOptions<AppSetting> appSettings)
        {
            _notificationsService = notificationsService;
            _appSettings = appSettings.Value;
        }

        [HttpPost]
        public async Task<IActionResult> CreateNotification([FromBody] CreateNotifyRequest request)
        {
            try
            {
                var result = await _notificationsService.CreateNotification(request);
                if (result)
                {
                    return StatusCode(StatusCodes.Status201Created, request);
                }
                return BadRequest("Some things wrong when save to database!!!");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("subscribes")]
        public async Task<IActionResult> GetNotifications([FromRoute] CancellationToken cancellationToken)
        {
            if (Request.Headers.TryGetValue("Authorization", out var authHeader))
            {
                var accessToken = authHeader.FirstOrDefault().Substring("Bearer ".Length);

                ClaimsPrincipal user = JwtHelper.DecodeJwtToken(accessToken, _appSettings.Secret);
                string id = user.FindFirstValue("userId");
                if (id == null) return Unauthorized();


                Response.Headers.Add("Content-Type", "text/event-stream");
                var b = ASCIIEncoding.ASCII.GetBytes($"data:Ping chilling\n\n");
                await Response.Body.WriteAsync(b, 0, b.Length, cancellationToken);
                await Response.Body.FlushAsync(cancellationToken);
                int count = 0;
                try
                {
                    count += _notificationsService.GetNotificationsFromSendTo(id).Count;
                    ConcurrentQueue<FPT_Exchange_Data.Entities.Notify> notifications = _notificationsService.GetNotificationsFromSendTo(id, count);
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        var eventData = string.Empty;
                        await _notificationsLock.WaitAsync(cancellationToken);
                        try
                        {
                            if (notifications.TryDequeue(out var notification))
                            {
                                var jsonNotification = JsonSerializer.Serialize(notification.Description);
                                eventData = $"data:{jsonNotification}\n\n";
                            }
                            else
                            {
                                notifications = _notificationsService.GetNotificationsFromSendTo(id, count);
                                count += notifications.Count;
                                continue;
                            }

                        }
                        finally
                        {
                            _notificationsLock.Release();
                        }
                        if (!string.IsNullOrEmpty(eventData))
                        {
                            var bytes = ASCIIEncoding.ASCII.GetBytes(eventData);
                            await Response.Body.WriteAsync(bytes, 0, bytes.Length, cancellationToken);
                            await Response.Body.FlushAsync(cancellationToken);
                        }

                        await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
                    }
                }
                catch (OperationCanceledException ex)
                {
                    return StatusCode(500, ex.Message);
                }
            }
            else
            {
                return Unauthorized();
            }
            return Ok();

        }
    }
}
