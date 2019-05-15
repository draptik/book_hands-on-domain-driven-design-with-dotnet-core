using System;
using Marketplace.Framework;

namespace Marketplace.Domain
{
    public class ClassifiedAdId : Value<ClassifiedAdId>
    {
        private readonly Guid _value;

        public ClassifiedAdId(Guid value) => _value = value;
    }
}