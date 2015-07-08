using FluentAssertions;
using Nerdle.AutoConfig.CaseConverters;
using NUnit.Framework;

namespace Nerdle.AutoConfig.Tests.Unit.CaseConverterTests
{
    [TestFixture]
    class When_converting_to_matching_case
    {
        readonly MatchingCaseConverter _sut = new MatchingCaseConverter();

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
