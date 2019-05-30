using System;
using System.Collections.Generic;
using System.Linq;
using Marketplace.Framework;

namespace Marketplace.Domain
{
    public class ClassifiedAd : AggregateRoot<ClassifiedAdId>
    {
        // Properties to handle the persistence
        private string DbId
        {
            get => $"ClassifiedAd/{Id.Value}";
            set {}
        }

        public ClassifiedAd(ClassifiedAdId id, UserId ownerId)
        {
            Pictures = new List<Picture>();
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

        public List<Picture> Pictures { get; private set; }
        
        
        public void SetTitle(ClassifiedAdTitle title) =>
            Apply(new Events.ClassifiedAdTitleChanged
            {
                Id = Id,
                Title = title
            });

        public void UpdateText(ClassifiedAdText text) =>
            Apply(new Events.ClassifiedAdTextUpdated
            {
                Id = Id,
                Text = text
            });

        public void UpdatePrice(Price price) =>
            Apply(new Events.ClassifiedAdPriceUpdated
            {
                Id = Id,
                Price = price.Amount,
                CurrencyCode = price.Currency.CurrencyCode
            });

        public void AddPicture(Uri pictureUrl, PictureSize size)
            => Apply(new Events.PictureAddedToClassifiedAd
            {
                PictureId = new Guid(),
                ClassifiedAdId = Id,
                Url = pictureUrl.ToString(),
                Height = size.Height,
                Width = size.Width,
                Order = NewPictureOrder()
            });

        public void ResizePicture(PictureId pictureId, PictureSize newSize)
        {
            var picture = FindPicture(pictureId);
            if (picture == null)
                throw new InvalidOperationException(
                    "Cannot resize a picture that I don't have");
            picture.Resize(newSize);
        }
        
        public void RequestToPublish() => 
            Apply(new Events.ClassifiedAdSentForReview {Id = Id});

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
                case Events.PictureAddedToClassifiedAd e:
                    var picture = new Picture(Apply);
                    ApplyToEntity(picture, e);
                    Pictures.Add(picture);
                    break;
                case Events.ClassifiedAdPictureResized e:
                    picture = FindPicture(new PictureId(e.PictureId));
                    ApplyToEntity(picture, @event);
                    break;
            }
        }

        protected override void EnsureValidState()
        {
            var valid =
                Id != null &&
                OwnerId != null &&
                (State switch
                {
                    ClassifiedAdState.PendingReview =>
                        Title != null
                        && Text != null
                        && Price?.Amount > 0
                        && FirstPicture.HasCorrectSize(),
                    ClassifiedAdState.Active =>
                        Title != null
                        && Text != null
                        && Price?.Amount > 0
                        && FirstPicture.HasCorrectSize()
                        && ApprovedBy != null,
                    _ => true
                });

            if (!valid)
                throw new InvalidEntityStateException(
                    this, $"Post-checks failed in state {State}");
        }
        
        public enum ClassifiedAdState
        {
            PendingReview,
            Active,
            Inactive,
            MarkedAsSoled
        }
        
        private Picture FirstPicture => Pictures.OrderBy(x => x.Order).FirstOrDefault();

        private Picture FindPicture(PictureId id)
            => Pictures.FirstOrDefault(x => x.Id == id);
        
        private int NewPictureOrder()
            => Pictures.Any()
                ? Pictures.Max(x => x.Order) + 1
                : 0;
        
        // For persistence
        protected ClassifiedAd() { }
    }
}