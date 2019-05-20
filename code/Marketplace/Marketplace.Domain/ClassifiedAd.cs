using System.IO.MemoryMappedFiles;

namespace Marketplace.Domain
{
    public class ClassifiedAd
    {
        public ClassifiedAdId Id { get; private set; }

        public ClassifiedAd(ClassifiedAdId id, UserId ownerId)
        {
            Id = id;
            OwnerId = ownerId;
            State = ClassifiedAdState.Inactive;
        }
        
        public UserId OwnerId { get; }
        public ClassifiedAdTitle Title { get; private set; }
        public ClassifiedAdText Text { get; private set; }
        public Price Price { get; private set; }
        public ClassifiedAdState State { get; private set; }
        
        public UserId ApprovedBy { get; private set; }

        public void SetTitle(ClassifiedAdTitle title) => Title = title;

        public void UpdateText(ClassifiedAdText text) => Text = text;

        public void UpdatePrice(Price price) => Price = price;

        public void RequestToPublish()
        {
            if (Title == null)
                throw new InvalidEntityStateException(this, "title cannot be empty");
            if (Text == null)
                throw new InvalidEntityStateException(this, "text cannot be empty");
            if (Price?.Amount == 0)
                throw new InvalidEntityStateException(this, "price cannot be zero");

            State = ClassifiedAdState.PendingReview;
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