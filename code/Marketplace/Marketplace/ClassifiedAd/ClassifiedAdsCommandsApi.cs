using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using static Marketplace.Infrastructure.RequestHandler;

namespace Marketplace.ClassifiedAd
{
    [Route("/ad")]
    public class ClassifiedAdsCommandsApi : Controller
    {
        private readonly ClassifiedAdsApplicationService _applicationService;
        private static readonly ILogger Log = Serilog.Log.ForContext<ClassifiedAdsCommandsApi>();
        
        public ClassifiedAdsCommandsApi(ClassifiedAdsApplicationService applicationService) 
            => _applicationService = applicationService;

        [HttpPost]
        public Task<IActionResult> Post(Commands.V1.Create request)
            => HandleRequest(request, _applicationService.Handle, Log);
        
        [Route("name")]
        [HttpPut]
        public Task<IActionResult> Put(Commands.V1.SetTitle request)
            => HandleRequest(request, _applicationService.Handle, Log);
        
        [Route("text")]
        [HttpPut]
        public Task<IActionResult> Put(Commands.V1.UpdateText request)
            => HandleRequest(request, _applicationService.Handle, Log);

        [Route("price")]
        [HttpPut]
        public Task<IActionResult> Put(Commands.V1.UpdatePrice request)
            => HandleRequest(request, _applicationService.Handle, Log);

        [Route("requestpublish")]
        [HttpPut]
        public Task<IActionResult> Put(Commands.V1.RequestToPublish request)
            => HandleRequest(request, _applicationService.Handle, Log);

        [Route("publish")]
        [HttpPut]
        public Task<IActionResult> Put(Commands.V1.Publish request)
            => HandleRequest(request, _applicationService.Handle, Log);
    }
}