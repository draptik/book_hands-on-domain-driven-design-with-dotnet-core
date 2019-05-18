using Marketplace.Domain;
using Xunit;

namespace Marketplace.Tests
{
    public class MoneyTest
    {
        [Fact]
        public void Money_objects_with_same_amount_should_be_equal()
        {
            var firstAmount = Money.FromDecimal(5);
            var secondAmount = Money.FromDecimal(5);
            Assert.Equal(firstAmount, secondAmount);
        }

        [Fact]
        public void Sum_of_money_gives_full_amount()
        {
            var coin1 = Money.FromDecimal(1);
            var coin2 = Money.FromDecimal(2);
            var coin3 = Money.FromDecimal(2);
            var banknote = Money.FromDecimal(5);
            
            Assert.Equal(banknote, coin1 + coin2 + coin3);
        }
    }
}