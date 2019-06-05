using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using static Marketplace.Infrastructure.RequestHandler;

namespace Marketplace.UserProfile
{
    [Route("/profile")]
    public class UserProfileCommandsApi : Controller
    {
        private readonly UserProfileApplicationService _applicationService;
        private static readonly ILogger Log = Serilog.Log.ForContext<UserProfileCommandsApi>();

        public UserProfileCommandsApi(UserProfileApplicationService applicationService) 
            => _applicationService = applicationService;

        [HttpPost]
        public Task<IActionResult> Post(Contracts.V1.RegisterUser request)
            => HandleCommand(request, _applicationService.Handle, Log);
    
        [Route("fullname")]
        [HttpPut]
        public Task<IActionResult> Put(Contracts.V1.UpdateUserFullName request)
            => HandleCommand(request, _applicationService.Handle, Log);
    
        [Route("displayname")]
        [HttpPut]
        public Task<IActionResult> Put(Contracts.V1.UpdateUserDisplayName request)
            => HandleCommand(request, _applicationService.Handle, Log);
    
        [Route("photo")]
        [HttpPut]
        public Task<IActionResult> Put(Contracts.V1.UpdateUserProfilePhoto request)
            => HandleCommand(request, _applicationService.Handle, Log);
    }
}
