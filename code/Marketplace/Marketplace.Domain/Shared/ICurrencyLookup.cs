namespace Marketplace.Domain.Shared
{
    public interface ICurrencyLookup
    {
        Currency FindCurrency(string currencyCode);
    }
}