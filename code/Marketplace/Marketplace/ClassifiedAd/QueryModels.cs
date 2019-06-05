using System;

namespace Marketplace.ClassifiedAd
{
    public static class QueryModels
    {
        public class GetPublishedClassifiedAds
        {
            /// <summary>
            /// Page number: zero-based
            /// </summary>
            public int Page { get; set; }
            public int PageSize { get; set; }
        }

        public class GetOwnersClassifiedAd
        {
            public Guid OwnerId { get; set; }
            
            /// <summary>
            /// Page number: zero-based
            /// </summary>
            public int Page { get; set; }
            public int PageSize { get; set; }
        }

        public class GetPublishedClassifiedAd
        {
            public Guid ClassifiedAdId { get; set; }
        }
    }
}