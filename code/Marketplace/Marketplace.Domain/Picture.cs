using System;
using Marketplace.Framework;

namespace Marketplace.Domain
{
    public class Picture : Entity<PictureId>
    {
        public PictureSize Size { get; set; }
        public Uri Location { get; set; }
        public int Order { get; set; }
        
        protected override void When(object @event)
        {
            switch (@event)
            {
                case Events.PictureAddedToClassifiedAd e:
                    Id = new PictureId(e.PictureId);
                    Location = new Uri(e.Url);
                    Size = new PictureSize {Height = e.Height, Width = e.Width};
                    Order = e.Order;
                    break;
            }
        }

        public Picture(Action<object> applier) : base(applier) { }
    }
}