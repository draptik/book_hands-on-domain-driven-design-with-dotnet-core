using System;

namespace Marketplace.Domain
{
    public class Price : Money
    {
        public Price(decimal amount, string currencyCode) 
            : base(amount, new Currency{CurrencyCode = currencyCode})
        {
            if (amount < 0)
            {
                throw new ArgumentException(
                    "Price cannot be negative", nameof(amount));
            }
        }
        
        public new static Price FromDecimal(decimal amount, string currency, ICurrencyLookup currencyLookup) 
            => new Price(amount, currency);

    }
}