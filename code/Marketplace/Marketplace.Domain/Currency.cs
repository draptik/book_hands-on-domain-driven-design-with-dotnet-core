using Marketplace.Framework;

namespace Marketplace.Domain
{
    public class Currency : Value<Currency>
    {
        // TODO why do we have public setters in Value Object?
        
        public string CurrencyCode { get; set; }
        
        public bool InUse { get; set; }
        
        public int DecimalPlaces { get; set; }

        public static Currency None = new Currency {InUse = false};
    }
}