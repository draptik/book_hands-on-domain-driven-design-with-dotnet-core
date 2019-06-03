using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents.Session;
using Serilog;
using static Marketplace.Infrastructure.RequestHandler;

namespace Marketplace.ClassifiedAd
{
    [Route("/ad")]
    public class ClassifiedAdsQueryApi : Controller
    {
        private static readonly ILogger Log = Serilog.Log.ForContext<ClassifiedAdsQueryApi>();
        
        private readonly IAsyncDocumentSession _session;
        
        public ClassifiedAdsQueryApi(IAsyncDocumentSession session)
            => _session = session;

        [Route("list")]
        [HttpGet]
        public Task<IActionResult> Get(QueryModels.GetPublishedClassifiedAds request)
            => HandleQuery(() => _session.Query(request), Log);

        [Route("myads")]
        [HttpGet]
        public Task<IActionResult> Get(QueryModels.GetOwnersClassifiedAd request)
            => HandleQuery(() => _session.Query(request), Log);

        [HttpGet]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public Task<IActionResult> Get(QueryModels.GetPublishedClassifiedAd request)
            => HandleQuery(() => _session.Query(request), Log);
    }
}