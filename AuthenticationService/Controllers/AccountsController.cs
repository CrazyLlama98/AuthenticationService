using System.Threading.Tasks;
using AuthenticationService.Dtos;
using AuthenticationService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationService.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount([FromBody] AccountDto account)
        {
            var result = await _accountService.CreateAccountAsync(account);

            return result.IsSuccessful ? (IActionResult)NoContent() : BadRequest(new { result.Errors });
        }
    }
}
