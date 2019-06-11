using System.Data.Common;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using static Marketplace.Infrastructure.RequestHandler;

namespace Marketplace.ClassifiedAd
{
    [Route("/ad")]
    public class ClassifiedAdsQueryApi : Controller
    {
        private static readonly ILogger Log = Serilog.Log.ForContext<ClassifiedAdsQueryApi>();
        
        // RavenDb session
//        private readonly IAsyncDocumentSession _session;
        
        // RavenDb session
//        public ClassifiedAdsQueryApi(IAsyncDocumentSession session)
//            => _session = session;

        // EF-Core session
        private readonly DbConnection _connection;

        // EF-Core session
        public ClassifiedAdsQueryApi(DbConnection connection) => _connection = connection;

        [Route("list")]
        [HttpGet]
        public Task<IActionResult> Get(QueryModels.GetPublishedClassifiedAds request)
            => HandleQuery(() => _connection.Query(request), Log);

        [Route("myads")]
        [HttpGet]
        public Task<IActionResult> Get(QueryModels.GetOwnersClassifiedAd request)
            => HandleQuery(() => _connection.Query(request), Log);

        [HttpGet]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public Task<IActionResult> Get(QueryModels.GetPublishedClassifiedAd request)
            => HandleQuery(() => _connection.Query(request), Log);
    }
}