﻿using Microsoft.AspNetCore.Authorization;
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
        private IAccountRepository _accountService;

        public CartController(ICartService service, IAccountRepository accountService)
        {
            _service = service;
            _accountService = accountService;
        }

        /*[HttpGet("ViewCartByUserEmail/{email}")]
        [Authorize(Roles = ApplicationRoles.Buyer)]
        public async Task<IActionResult> ViewCartByUserEmail(string email)
        {
            Account acc = new Account();
            acc.Email = email;

            var check = await _accountService.GetUserByEmailFromAPIAsync(acc);
            if (check != null)
            {
                var result = await _service.ViewAllCartItemByUserIdFromApiAsync(check);
                if (result != null)
                {
                    return Ok(result);
                }
            }

            return Ok("Not found!");
        }*/

        [HttpPost("CreateCartItem")]
        //[Authorize(Roles = ApplicationRoles.Buyer)]
        public async Task<ActionResult<bool>> CreateCartItem(CreateCartItem cartItem)
        {
            Account acc = new Account() { Email = cartItem.BuyerEmail };
            Account account = await _accountService.GetUserByEmailAsync(acc);

            if (account != null)
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

        [HttpPost("CreateCartByUserEmail/{email}")]
        //[Authorize(Roles = ApplicationRoles.Buyer)]
        public async Task<ActionResult<bool>> CreateCartByUserEmail(string email)
        {
            Account acc = new Account();
            acc.Email = email;

            var account = await _accountService.GetUserByEmailAsync(acc);
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

        /*[HttpDelete("RemoveCartItem/{userEmail}/{productid}")]
        [Authorize(Roles = ApplicationRoles.Buyer)]
        public async Task<ActionResult<bool>> RemoveCartItem(string userEmail, int productId)
        {
            Account acc = new Account();
            acc.Email = userEmail;

            var deleteAccount = await _accountService.GetUserByEmailFromAPIAsync(acc);
            if (deleteAccount != null)
            {
                CartItem cartItem = new CartItem() { BuyerId = deleteAccount.Id, ProductId = productId };
                var result = await _service.RemoveItemFromCartFromApiAsync(cartItem);
                if (result.Succeeded)
                {
                    return true;
                }
            }

            return false;
        }*/
    }
}
