using FluentAssertions;
using Marketplace.Domain.ClassifiedAd;
using Xunit;

namespace Marketplace.Tests
{
    public class ValueObjectTests
    {
        [Fact]
        public void AdText_missing_can_be_checked()
        {
            var sut = ClassifiedAdText.NoText;
            sut.Value.Should().BeNull();
            Assert.True(sut == (string)null);
        }

        [Fact]
        public void AdText_present_can_be_checked()
        {
            var sut = ClassifiedAdText.FromString("foo");
            Assert.True(sut != null);
        }
    }
}