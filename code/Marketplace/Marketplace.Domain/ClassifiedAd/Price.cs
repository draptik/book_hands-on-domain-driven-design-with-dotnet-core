using System;
using Marketplace.Domain.Shared;

namespace Marketplace.Domain.ClassifiedAd
{
    public class Price : Money
    {
        private Price(decimal amount, string currencyCode, ICurrencyLookup currencyLookup)
            : base(amount, currencyCode, currencyLookup)
        {
            if (amount < 0)
                throw new ArgumentException(
                    "Price cannot be negative",
                    nameof(amount));
        }

        internal Price(decimal amount, string currencyCode)
            : base(amount, new Currency{CurrencyCode = currencyCode})
        {
        }

        internal Price(decimal amount, string currencyCode, bool inUse, int decimalPlaces)
            : base(amount, new Currency{CurrencyCode = currencyCode, InUse = inUse, DecimalPlaces = decimalPlaces})
        {
        }

        public new static Price FromDecimal(decimal amount, string currency,
            ICurrencyLookup currencyLookup) =>
            new Price(amount, currency, currencyLookup);
        
        // Satisfy the serialization requirements 
        protected Price() { }
        
        public static Price NoPrice =>
            new Price
            {
                Amount = -1, 
                Currency = Currency.None
            };
    }
}
