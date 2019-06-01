﻿using System;
using Marketplace.Framework;

namespace Marketplace.Domain.Shared
{
    public class UserId : Value<UserId>
    {
        public Guid Value { get; private set; }

        public UserId(Guid value)
        {
            if (value == default)
                throw new ArgumentNullException(nameof(value), "User id cannot be empty");
            
            Value = value;
        }
        
        public static implicit operator Guid(UserId self) => self.Value;
        
        // required for ef-core persistence
        protected UserId() {}
        
        public static UserId NoUser =>
            new UserId();
    }
}
