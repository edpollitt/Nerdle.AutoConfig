using FluentAssertions;
using Nerdle.AutoConfig.Casing;
using NUnit.Framework;

namespace Nerdle.AutoConfig.Tests.Unit.CasingTests
{
    [TestFixture]
    class When_converting_to_matching_case
    {
        readonly MatchingCase _sut = new MatchingCase();

        [TestCase("hello")]
        [TestCase("hELlo")]
        [TestCase("HELLO!!!")]
        [TestCase("_H3770 :) x")]
        public void Return_the_original_string_unmodified(string inputText)
        {
            _sut.Convert(inputText).ShouldBeEquivalentTo(inputText);
        }
    }
}
