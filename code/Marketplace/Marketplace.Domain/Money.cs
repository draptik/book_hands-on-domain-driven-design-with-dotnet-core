using System;
using Marketplace.Framework;

namespace Marketplace.Domain
{
    public class Money : Value<Money>
    {
        private const string DefaultCurrency = "EUR";

        protected Money(decimal amount, string currency = "EUR")
        {
            if (decimal.Round(amount, 2) != amount)
                throw new ArgumentOutOfRangeException(nameof(amount),
                    "Amount cannot have more than two decimals");

            Amount = amount;
            CurrencyCode = currency;
        }

        public decimal Amount { get; }
        public string CurrencyCode { get; }

        public static Money FromDecimal(decimal amount, string currency = DefaultCurrency) 
            => new Money(amount, currency);

        public static Money FromString(string amount, string currency = DefaultCurrency) 
            => new Money(decimal.Parse(amount), currency);

        public Money Add(Money summand)
        {
            if (CurrencyCode != summand.CurrencyCode)
                throw new CurrencyMismatchException("Cannot sum amounts with different currencies");

            return new Money(Amount + summand.Amount, CurrencyCode);
        }

        public Money Subtract(Money subtrahend)
        {
            if (CurrencyCode != subtrahend.CurrencyCode)
                throw new CurrencyMismatchException("Cannot subtract amounts with different currencies");

            return new Money(Amount - subtrahend.Amount, CurrencyCode);
        }

        public static Money operator +(Money summand1, Money summand2) => summand1.Add(summand2);

        public static Money operator -(Money minuend, Money subtrahend) => minuend.Subtract(subtrahend);
    }

    public class CurrencyMismatchException : Exception
    {
        public CurrencyMismatchException(string message) : base(message)
        {
        }
    }
}