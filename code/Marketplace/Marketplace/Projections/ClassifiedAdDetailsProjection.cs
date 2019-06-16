using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Marketplace.ClassifiedAd;
using Marketplace.Domain.ClassifiedAd;
using Marketplace.Framework;

namespace Marketplace.Projections
{
    public class ClassifiedAdDetailsProjection : IProjection
    {
        private readonly List<ReadModels.ClassifiedAdDetails> _items;

        public ClassifiedAdDetailsProjection(List<ReadModels.ClassifiedAdDetails> items)
        {
            _items = items;
        }

        public Task Project(object @event)
        {
            switch (@event)
            {
                case Events.ClassifiedAdCreated e:
                    _items.Add(new ReadModels.ClassifiedAdDetails
                    {
                        ClassifiedAdId = e.Id,
                        SellerId = e.OwnerId
                    });
                    break;
                case Events.ClassifiedAdTitleChanged e:
                    UpdateItem(e.Id, ad => ad.Title = e.Title);
                    break;
                case Events.ClassifiedAdTextUpdated e:
                    UpdateItem(e.Id, ad => ad.Description = e.AdText);
                    break;
                case Events.ClassifiedAdPriceUpdated e:
                    UpdateItem(e.Id, ad =>
                    {
                        ad.Price = e.Price;
                        ad.CurrencyCode = e.CurrencyCode;
                    });
                    break;
                case Domain.UserProfile.Events.UserDisplayNameUpdated e:
                    UpdateMultipleItems(x => x.SellerId == e.UserId,
                        x => x.SellersDisplayName = e.DisplayName);
                    break;
            }
            
            return Task.CompletedTask;
        }
        
        private void UpdateItem(Guid id, Action<ReadModels.ClassifiedAdDetails> update)
        {
            var item = _items.FirstOrDefault(x => x.ClassifiedAdId == id);
            if (item == null)
                return;

            update(item);
        }

        private void UpdateMultipleItems(
            Func<ReadModels.ClassifiedAdDetails, bool> query, 
            Action<ReadModels.ClassifiedAdDetails> update)
        {
            foreach (var item in _items.Where(query))
                update(item);
        }
    }
}