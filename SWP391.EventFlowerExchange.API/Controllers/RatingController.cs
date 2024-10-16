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
    public class RatingController : ControllerBase
    {
        private IRatingService _service;
        private IAccountService _accountService;

        public RatingController(IRatingService service, IAccountService accountService)
        {
            _service = service;
            _accountService = accountService;
        }

        [HttpGet("ViewRatingByUserId/{id}")]
        [Authorize]
        public async Task<IActionResult> ViewRatingByUserId(string id)
        {
            Account acc = new Account();
            acc.Id = id;

            var result = await _service.ViewAllRatingByUserIdFromApiAsync(acc);
            if (result != null)
            {
                return Ok(result);
            }

            return Ok("Not found!");
        }

        [HttpPost("PostRating")]
        [Authorize(Roles = ApplicationRoles.Buyer)]
        public async Task<ActionResult<bool>> PostRating(CreateRating rate)
        {
            Account acc = new Account();
            acc.Id = rate.BuyerId;
            var deleteAccount = await _accountService.GetUserByIdFromAPIAsync(acc);

            if (deleteAccount != null)
            {
                var result = await _service.PostRatingFromApiAsync(rate);
                if (result.Succeeded)
                {
                    return true;
                }
                return false;
            }

            return false;
        }
    }
}
