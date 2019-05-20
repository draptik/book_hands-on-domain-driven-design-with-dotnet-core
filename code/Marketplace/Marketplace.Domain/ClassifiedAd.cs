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
            EnsureValidState();
            Raise(new Events.ClassifiedAdCreated
            {
                Id = id,
                OwnerId = ownerId
            });
        }
        
        public UserId OwnerId { get; }
        public ClassifiedAdTitle Title { get; private set; }
        public ClassifiedAdText Text { get; private set; }
        public Price Price { get; private set; }
        public ClassifiedAdState State { get; private set; }
        
        public UserId ApprovedBy { get; private set; }

        public void SetTitle(ClassifiedAdTitle title)
        {
            Title = title;
            EnsureValidState();
            Raise(new Events.ClassifiedAdTitleChanged
            {
                Id = Id,
                Title = title
            });
        }

        public void UpdateText(ClassifiedAdText text)
        {
            Text = text;
            EnsureValidState();
            Raise(new Events.ClassifiedAdTextUpdated
            {
                Id = Id,
                Text = text
            });
        }

        public void UpdatePrice(Price price)
        {
            Price = price;
            EnsureValidState();
            Raise(new Events.ClassifiedAdPriceUpdated
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
            Raise(new Events.ClassifiedAdSentForReview {Id = Id});
        }

        public void EnsureValidState()
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