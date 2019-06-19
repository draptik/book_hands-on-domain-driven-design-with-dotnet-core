using System;
using System.Threading.Tasks;
using Marketplace.Domain.ClassifiedAd;
using Marketplace.Infrastructure;
using Raven.Client.Documents.Session;

namespace Marketplace.Projections
{
    public class ClassifiedAdDetailsProjection : RavenDbProjection<ReadModels.ClassifiedAdDetails>
    {
        private readonly Func<Guid, Task<string>> _getUserDisplayName;

        public ClassifiedAdDetailsProjection(
            Func<IAsyncDocumentSession> getSession,
            Func<Guid, Task<string>> getUserDisplayName)
            : base(getSession) => _getUserDisplayName = getUserDisplayName;

        // @formatter:off
        public override Task Project(object @event) =>
            @event switch
            {
                Events.ClassifiedAdCreated e =>
                    Create(async () => new ReadModels.ClassifiedAdDetails
                    {
                        Id = e.Id.ToString(),
                        SellerId = e.OwnerId,
                        SellersDisplayName = await _getUserDisplayName(e.OwnerId)
                    }),
                Events.ClassifiedAdTitleChanged e =>
                    UpdateOne(e.Id, ad => ad.Title = e.Title),
                Events.ClassifiedAdTextUpdated e =>
                    UpdateOne(e.Id, ad => ad.Description = e.AdText),
                Events.ClassifiedAdPriceUpdated e =>
                    UpdateOne(e.Id, ad =>
                    {
                        ad.Price = e.Price;
                        ad.CurrencyCode = e.CurrencyCode;
                    }),
                Domain.UserProfile.Events.UserDisplayNameUpdated e =>
                    UpdateWhere(
                        x => x.SellerId == e.UserId,
                        x => x.SellersDisplayName = e.DisplayName),
                ClassifiedAdUpcastedEvents.V1.ClassifiedAdPublished e =>
                    UpdateOne(e.Id, ad => ad.SellersPhotoUrl = e.SellersPhotoUrl),
                _ => Task.CompletedTask
            };
        // @formatter:on
    }
}