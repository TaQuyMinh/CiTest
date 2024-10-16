using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWP391.EventFlowerExchange.Application;
using SWP391.EventFlowerExchange.Domain.Entities;
using SWP391.EventFlowerExchange.Domain.ObjectValues;
using SWP391.EventFlowerExchange.Infrastructure;

namespace SWP391.EventFlowerExchange.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FollowController : ControllerBase
    {
        private IFollowService _service;
        private IAccountService _accountService;

        public FollowController(IFollowService service, IAccountService accountService)
        {
            _service = service;
            _accountService = accountService;
        }

        [HttpGet("ViewFollowerByUserId/{id}")]
        [Authorize(Roles = ApplicationRoles.Buyer)]
        public async Task<IActionResult> ViewFollowerByUserId(string id)
        {
            Account acc = new Account();
            acc.Id = id;

            var result = await _service.GetFollowerListFromApiAsync(acc);
            if (result != null)
            {
                return Ok(result);
            }

            return Ok("Not found!");
        }

        [HttpPost("CreateFollow")]
        [Authorize(Roles = ApplicationRoles.Buyer)]
        public async Task<ActionResult<bool>> CreateFollow(CreateFollower follower)
        {
            Account acc = new Account();
            acc.Id = follower.FollowerId;
            var check1 = await _accountService.GetUserByIdFromAPIAsync(acc);

            Account acc2 = new Account();
            acc.Id = follower.SellerId;
            var check2 = await _accountService.GetUserByIdFromAPIAsync(acc2);

            if (check1 != null && check2 !=null)
            {
                var result = await _service.AddNewFollowerFromApiAsync(follower);
                if (result.Succeeded)
                {
                    return true;
                }
                return false;
            }

            return false;
        }

        [HttpDelete("RemoveFollower/{followerId}/{sellerId}")]
        [Authorize(Roles = ApplicationRoles.Buyer)]
        public async Task<ActionResult<bool>> RemoveAccount(string followerId, string sellerId)
        {
            Account acc = new Account();
            acc.Id = followerId;
            var check1 = await _accountService.GetUserByIdFromAPIAsync(acc);

            Account acc2 = new Account();
            acc.Id = sellerId;
            var check2 = await _accountService.GetUserByIdFromAPIAsync(acc2);

            if (check1 != null && check2 != null)
            {
                ShopNotification cartItem = new ShopNotification() { FollowerId = followerId, SellerId = sellerId };
                var result = await _service.RemoveFollowerFromApiAsync(cartItem);
                if (result.Succeeded)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
