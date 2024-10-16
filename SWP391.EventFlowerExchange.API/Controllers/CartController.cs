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
    public class CartController : ControllerBase
    {
        private ICartService _service;
        private IAccountService _accountService;

        public CartController(ICartService service, IAccountService accountService)
        {
            _service = service;
            _accountService = accountService;
        }

        [HttpGet("ViewCartByUserId/{id}")]
        [Authorize(Roles = ApplicationRoles.Buyer)]
        public async Task<IActionResult> ViewCartByUserId(string id)
        {
            Account acc = new Account();
            acc.Id = id;

            var result = await _service.ViewAllCartItemByUserIdFromApiAsync(acc);
            if (result != null)
            {
                return Ok(result);
            }

            return Ok("Not found!");
        }

        [HttpPost("CreateCartItem")]
        [Authorize(Roles = ApplicationRoles.Manager + " , " +ApplicationRoles.Staff)]
        public async Task<ActionResult<bool>> CreateCartItem(CreateCartItem cartItem)
        {
            Account acc = new Account();
            acc.Id = cartItem.BuyerId;
            var deleteAccount = await _accountService.GetUserByIdFromAPIAsync(acc);

            if (deleteAccount != null)
            {
                var result = await _service.CreateCartItemFromApiAsync(cartItem);
                if (result.Succeeded)
                {
                    return true;
                }
                return false;
            }

            return false; 
        }

        [HttpPost("CreateCartByUserId/{id}")]
        [Authorize(Roles = ApplicationRoles.Buyer)]
        public async Task<ActionResult<bool>> CreateCartByUserId(string id)
        {
            Account acc = new Account();
            acc.Id = id;
            var account = await _accountService.GetUserByIdFromAPIAsync(acc);

            if (account != null)
            {
                var result = await _service.CreateCartFromApiAsync(account);
                if (result.Succeeded)
                {
                    return true;
                }
                return false;
            }

            return false;
        }

        [HttpDelete("RemoveCartItem/{id}/{productid}")]
        [Authorize(Roles = ApplicationRoles.Buyer)]
        public async Task<ActionResult<bool>> RemoveCartItem(string id,int productid)
        {
            Account acc = new Account();
            acc.Id = id;
            var deleteAccount = await _accountService.GetUserByIdFromAPIAsync(acc);

            if (deleteAccount != null)
            {
                CartItem cartItem = new CartItem() { BuyerId=id, ProductId=productid };
                var result = await _service.RemoveItemFromCartFromApiAsync(cartItem);
                if (result.Succeeded)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
