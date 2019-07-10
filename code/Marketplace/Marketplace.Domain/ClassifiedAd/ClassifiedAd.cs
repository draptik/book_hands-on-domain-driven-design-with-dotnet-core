using System;
using System.Collections.Generic;
using System.Linq;
using Marketplace.Domain.Shared;
using Marketplace.Framework;

namespace Marketplace.Domain.ClassifiedAd
{
    public class ClassifiedAd : AggregateRoot<ClassifiedAdId>
    {
        
        // Properties to handle the persistence (EF)
        public Guid ClassifiedAdId { get; private set; }
        
        

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
                AdText = text
            });

        public void UpdatePrice(Price price) =>
            Apply(new Events.ClassifiedAdPriceUpdated
            {
                Id = Id,
                Price = price.Amount,
                CurrencyCode = price.Currency.CurrencyCode,
                InUse = price.Currency.InUse,
                DecimalPlaces = price.Currency.DecimalPlaces
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

        public void Publish(UserId userId) =>
            Apply(new Events.ClassifiedAdPublished
            {
                Id = Id, 
                ApprovedBy = userId,
                OwnerId = OwnerId
            });

        protected override void When(object @event)
        {
            switch (@event)
            {
                case Events.ClassifiedAdCreated e:
                    Id = new ClassifiedAdId(e.Id);
                    OwnerId = new UserId(e.OwnerId);
                    State = ClassifiedAdState.Inactive;
                    Pictures = new List<Picture>();
                    break;
                case Events.ClassifiedAdTitleChanged e:
                    Title = new ClassifiedAdTitle(e.Title);
                    break;
                case Events.ClassifiedAdTextUpdated e:
                    Text = new ClassifiedAdText(e.AdText);
                    break;
                case Events.ClassifiedAdPriceUpdated e:
                    Price = new Price(e.Price, e.CurrencyCode, e.InUse, e.DecimalPlaces);
                    break;
                case Events.ClassifiedAdSentForReview _:
                    State = ClassifiedAdState.PendingReview;
                    break;
                case Events.ClassifiedAdPublished e:
                    ApprovedBy = new UserId(e.ApprovedBy);
                    State = ClassifiedAdState.Active;
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
                        Title != (string)null
                        && Text != (string)null
                        && Price?.Amount > 0,
                    ClassifiedAdState.Active =>
                        Title != (string)null
                        && Text != (string)null
                        && Price?.Amount > 0
                        && ApprovedBy != null,
                    _ => true
                });

            if (!valid)
                throw new DomainExceptions.InvalidEntityStateException(
                    this, $"Post-checks failed in state {State}");
        }
        
        public enum ClassifiedAdState
        {
            PendingReview = 1, // add 1-based index for EF-Core / Postgres
            Active = 2,        // add index for EF-Core / Postgres
            Inactive = 3,      // add index for EF-Core / Postgres
            MarkedAsSold = 4   // add index for EF-Core / Postgres
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