using System.Threading.Tasks;
using AuthenticationService.Domain.Common;
using AuthenticationService.Domain.Dtos;
using AuthenticationService.Domain.Models;
using AuthenticationService.Business.Interfaces;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AuthenticationService.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly SignInManager<User> _signInManager;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;

        public AccountsController(
            IAccountService accountService,
            SignInManager<User> signInManager,
            IIdentityServerInteractionService interaction,
            IClientStore clientStore)
        {
            _accountService = accountService;
            _signInManager = signInManager;
            _interaction = interaction;
            _clientStore = clientStore;
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400, Type = typeof(ValidationErrorDetailsDto))]
        [SwaggerOperation(OperationId = "createAccount")]
        public async Task<IActionResult> CreateAccount([FromBody] AccountDto account)
        {
            var result = await _accountService.CreateAccountAsync(account);

            if (result.IsSuccessful)
            {
                return NoContent();
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("default", error);
            }

            return BadRequest(new ValidationErrorDetailsDto { Errors = new ValidationErrorDictionary(ModelState) });
        }

        [HttpPost("login")]
        [ProducesResponseType(302)]
        [ProducesResponseType(400, Type = typeof(ValidationErrorDetailsDto))]
        [SwaggerOperation(OperationId = "login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto, string returnUrl)
        {
            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);

            var result = await _signInManager.PasswordSignInAsync(loginDto.Username, loginDto.Password, loginDto.RememberMe, lockoutOnFailure: true);

            if (result.Succeeded)
            {
                if (context != null)
                {
                    return NoContent();
                }
                else
                {
                    return BadRequest();
                }
            }

            if (result.IsLockedOut)
                ModelState.AddModelError("default", "Your account is locked out");
            ModelState.AddModelError("default", "Wrong username or password");

            return BadRequest(new ValidationErrorDetailsDto { Errors = new ValidationErrorDictionary(ModelState) });
        }
    }
}
