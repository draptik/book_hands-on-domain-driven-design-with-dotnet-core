using System;
using Marketplace.Framework;

namespace Marketplace.Domain
{
    public class ClassifiedAdId : Value<ClassifiedAdId>
    {
        public Guid Value { get; }

        public ClassifiedAdId(Guid value)
        {
            if (value == default)
                throw new ArgumentNullException(nameof(value), "Classified Ad id cannot be empty");

            Value = value;
        }

        public static implicit operator Guid(ClassifiedAdId self) => self.Value;
        
        public override string ToString() => Value.ToString();
        
        public static implicit operator ClassifiedAdId(string value) 
            => new ClassifiedAdId(Guid.Parse(value));
        
        // Serialization requirement
        protected ClassifiedAdId() { }
    }
}