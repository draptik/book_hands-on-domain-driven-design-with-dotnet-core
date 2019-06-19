using System;

namespace Marketplace.ClassifiedAd
{
    public static class ReadModels
    {
        public class ClassifiedAdDetails
        {
            public Guid ClassifiedAdId { get; set; }
            public string Title { get; set; }
            public decimal Price { get; set; }
            public string CurrencyCode { get; set; }
            public string Description { get; set; }
            public string SellersDisplayName { get; set; }
            public string[] PhotoUrls { get; set; }
            public Guid SellerId { get; set; }
            public string SellersPhotoUrl { get; set; }
        }

        public class UserDetails
        {
            public Guid UserId { get; set; }
            public string DisplayName { get; set; }
            public string PhotoUrl { get; set; }
        }
    }
}