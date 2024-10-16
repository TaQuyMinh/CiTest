using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWP391.EventFlowerExchange.Application;
using SWP391.EventFlowerExchange.Domain.Entities;
using SWP391.EventFlowerExchange.Domain.ObjectValues;
using SWP391.EventFlowerExchange.Infrastructure;

namespace SWP391.EventFlowerExchange.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private INotificationService _service;

        public NotificationController(INotificationService service)
        {
            _service = service;
        }

        [HttpGet("ViewAllNotification")]
        [Authorize(Roles = ApplicationRoles.Manager + " , " + ApplicationRoles.Staff )]
        public async Task<IActionResult> ViewAllNotification()
        {
            try
            {
                return Ok(await _service.ViewAllNotificationFromApiAsync());
            }
            catch
            {
                return Ok("Not found!");
            }
        }

        [HttpGet("ViewNotificationByUserId/{id}")]
        [Authorize(Roles = ApplicationRoles.Seller + " , " + ApplicationRoles.Buyer)]
        public async Task<IActionResult> ViewNotificationByUserId(string id)
        {
            Account acc = new Account();
            acc.Id = id;

            var result = await _service.ViewAllNotificationByUserIdFromApiAsync(acc);
            if (result != null)
            {
                return Ok(result);
            }

            return Ok("Not found!");
        }

        [HttpGet("ViewNotificationById/{id}")]
        [Authorize(Roles = ApplicationRoles.Staff + " , " + ApplicationRoles.Manager)]
        public async Task<IActionResult> ViewNotificationById(int id)
        {
            Notification acc = new Notification();
            acc.NotificationId = id;

            var result = await _service.ViewNotificationByIdFromApiAsync(acc);
            if (result != null)
            {
                return Ok(result);
            }

            return Ok("Not found!");
        }

        [HttpPost("CreateNotification")]
        [Authorize]
        public async Task<ActionResult<bool>> CreateNotification(CreateNotification createNotificationDto)
        {
            try
            {
                var result = await _service.CreateNotificationFromApiAsync(createNotificationDto);
                if (result.Succeeded)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        [HttpGet("ViewAllShopNotification")]
        [Authorize(Roles = ApplicationRoles.Manager + " , " + ApplicationRoles.Staff)]
        public async Task<IActionResult> ViewAllShopNotification()
        {
            try
            {
                return Ok(await _service.ViewAllShopNotificationFromApiAsync());
            }
            catch
            {
                return Ok("Not found!");
            }
        }

        [HttpGet("ViewShopNotificationByUserId/{id}")]
        [Authorize(Roles = ApplicationRoles.Seller)]
        public async Task<IActionResult> ViewShopNotificationByUserId(string id)
        {
            Account acc = new Account();
            acc.Id = id;

            var result = await _service.ViewAllShopNotificationByUserIdFromApiAsync(acc);
            if (result != null)
            {
                return Ok(result);
            }

            return Ok("Not found!");
        }

        [HttpGet("ViewShopNotificationById/{id}")]
        [Authorize(Roles = ApplicationRoles.Staff + " , " + ApplicationRoles.Manager)]
        public async Task<IActionResult> ViewShopNotificationById(int id)
        {
            ShopNotification acc = new ShopNotification();
            acc.ShopNotificationId = id;

            var result = await _service.ViewShopNotificationByIdFromApiAsync(acc);
            if (result != null)
            {
                return Ok(result);
            }

            return Ok("Not found!");
        }

        [HttpPost("CreateShopNotification")]
        [Authorize]
        public async Task<ActionResult<bool>> CreateShopNotification(CreateShopNotification createNotificationDto)
        {
            try
            {
                var result = await _service.CreateShopNotificationFromApiAsync(createNotificationDto);
                if (result.Succeeded)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
