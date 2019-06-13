using System.Collections.Generic;
using Marketplace.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Marketplace.ClassifiedAd
{
    [Route("/ad")]
    public class ClassifiedAdsQueryApi : Controller
    {
        private static readonly ILogger _log = Log.ForContext<ClassifiedAdsQueryApi>();

        private readonly IEnumerable<ReadModels.ClassifiedAdDetails> _items;

        public ClassifiedAdsQueryApi(
            IEnumerable<ReadModels.ClassifiedAdDetails> items) =>
            _items = items;

        [HttpGet]
        public IActionResult Get(
            QueryModels.GetPublishedClassifiedAd request) =>
            RequestHandler.HandleQuery(() => _items.Query(request),
                _log);
    }
}