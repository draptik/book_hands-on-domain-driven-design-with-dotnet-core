namespace Marketplace.Domain
{
    public interface ICurrencyLookup
    {
        Currency FindCurrency(string currencyCode);
    }
}