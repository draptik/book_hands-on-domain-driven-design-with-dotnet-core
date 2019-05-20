using Marketplace.Framework;

namespace Marketplace.Domain
{
    public class ClassifiedAd : Entity
    {
        public ClassifiedAdId Id { get; private set; }

        public ClassifiedAd(ClassifiedAdId id, UserId ownerId)
        {
            Id = id;
            OwnerId = ownerId;
            State = ClassifiedAdState.Inactive;
            Apply(new Events.ClassifiedAdCreated
            {
                Id = id,
                OwnerId = ownerId
            });
        }
        
        public UserId OwnerId { get; private set; }
        public ClassifiedAdTitle Title { get; private set; }
        public ClassifiedAdText Text { get; private set; }
        public Price Price { get; private set; }
        public ClassifiedAdState State { get; private set; }
        
        public UserId ApprovedBy { get; private set; }

        public void SetTitle(ClassifiedAdTitle title)
        {
            Title = title;
            EnsureValidState();
            Apply(new Events.ClassifiedAdTitleChanged
            {
                Id = Id,
                Title = title
            });
        }

        public void UpdateText(ClassifiedAdText text)
        {
            Text = text;
            EnsureValidState();
            Apply(new Events.ClassifiedAdTextUpdated
            {
                Id = Id,
                Text = text
            });
        }

        public void UpdatePrice(Price price)
        {
            Price = price;
            EnsureValidState();
            Apply(new Events.ClassifiedAdPriceUpdated
            {
                Id = Id,
                Price = Price.Amount,
                CurrencyCode = price.Currency.CurrencyCode
            });
        }

        public void RequestToPublish()
        {
            State = ClassifiedAdState.PendingReview;
            EnsureValidState();
            Apply(new Events.ClassifiedAdSentForReview {Id = Id});
        }

        protected override void When(object @event)
        {
            switch (@event)
            {
                case Events.ClassifiedAdCreated e:
                    Id = new ClassifiedAdId(e.Id);
                    OwnerId = new UserId(e.OwnerId);
                    State = ClassifiedAdState.Inactive;
                    break;
                case Events.ClassifiedAdTitleChanged e:
                    Title = new ClassifiedAdTitle(e.Title);
                    break;
                case Events.ClassifiedAdTextUpdated e:
                    Text = new ClassifiedAdText(e.Text);
                    break;
                case Events.ClassifiedAdPriceUpdated e:
                    Price = new Price(e.Price, e.CurrencyCode);
                    break;
                case Events.ClassifiedAdSentForReview e:
                    State = ClassifiedAdState.PendingReview;
                    break;
            }
        }

        protected override void EnsureValidState()
        {
            bool stateValid;
            switch (State)
            {
                case ClassifiedAdState.PendingReview:
                    stateValid = Title != null
                        && Text != null
                        && Price?.Amount > 0;
                    break;
                case ClassifiedAdState.Active:
                    stateValid = Title != null
                                 && Text != null
                                 && Price?.Amount > 0
                                 && ApprovedBy != null;
                    break;
                default:
                    stateValid = true;
                    break;
            }

            
            var valid =
                Id != null &&
                OwnerId != null &&
                stateValid;

            if (!valid)
            {
                throw new InvalidEntityStateException(
                    this, $"Post-checks failed in state {State}");
            }
        }
        
        public enum ClassifiedAdState
        {
            PendingReview,
            Active,
            Inactive,
            MarkedAsSoled
        }
    }
}