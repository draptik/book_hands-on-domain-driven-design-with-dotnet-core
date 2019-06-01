using System;
using Marketplace.Framework;

namespace Marketplace.Domain.ClassifiedAd
{
    public class PictureId : Value<PictureId>
    {
        public Guid Value { get; }

        public PictureId(Guid value) => Value = value;
        
        // required for ef-core persistence
        protected PictureId() {}
    }
}