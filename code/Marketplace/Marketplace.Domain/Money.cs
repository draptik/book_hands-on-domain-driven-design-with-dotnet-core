using Marketplace.Framework;

namespace Marketplace.Domain
{
    public class Money : Value<Money>
    {
        public decimal Amount { get; }

        public Money(decimal amount) => Amount = amount;
        
        public Money Add(Money summand) => new Money(Amount + summand.Amount);
        
        public Money Subtract(Money subtrahend) => new Money(Amount - subtrahend.Amount);

        public static Money operator +(Money summand1, Money summand2) => summand1.Add(summand2);
        
        public static Money operator -(Money minuend, Money subtrahend) => minuend.Subtract(subtrahend);
    }
}