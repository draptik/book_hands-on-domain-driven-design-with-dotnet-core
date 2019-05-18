using System;

namespace Marketplace.Domain
{
    public class ClassifiedAd
    {
        private UserId _ownerId;
        private string _title;
        private string _text;
        private decimal _price;
        
        public ClassifiedAdId Id { get; private set; }

        public ClassifiedAd(ClassifiedAdId id, UserId ownerId)
        {
            Id = id;
            _ownerId = ownerId;
        }

        public void SetTitle(string title) => _title = title;

        public void UpdateText(string text) => _text = text;

        public void UpdatePrice(decimal price) => _price = price;

        public enum ClassifiedAdState
        {
            PendingReview,
            Active,
            Inactive,
            MarkedAsSoled
        }
    }
}