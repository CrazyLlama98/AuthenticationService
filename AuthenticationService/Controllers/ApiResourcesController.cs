using System.Threading.Tasks;
using AuthenticationService.Business.Interfaces;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationService.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class ApiResourcesController : Controller
    {
        private readonly IApiResourceService _apiResourceService;

        public ApiResourcesController(IApiResourceService apiResourceService)
        {
            _apiResourceService = apiResourceService;
        }

        [HttpGet]
        public async Task<IActionResult> GetApiResources(int page = 1, int pageSize = 10)
        {
            var apiResources = await _apiResourceService.GetAsync(page, pageSize);

            return Ok(apiResources);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateApiResource([FromBody] ApiResource apiResource)
        {
            var result = await _apiResourceService.AddAsync(apiResource);

            return result == null || !result.IsSuccessful ? BadRequest() : (IActionResult)Ok(result.Data);
        }
    }
}
