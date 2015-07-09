using FluentAssertions;
using Nerdle.AutoConfig.Casing;
using NUnit.Framework;

namespace Nerdle.AutoConfig.Tests.Unit.CasingTests
{
    [TestFixture]
    class When_converting_to_camel_case
    {
        readonly CamelCase _sut = new CamelCase();

        [TestCase("lower", "lower")]
        [TestCase("helloWorld", "helloWorld")]
        [TestCase("bOOO", "bOOO")]
        [TestCase("systemIO", "systemIO")]
        [TestCase("_X", "_X")]
        [TestCase("{a*&%£2", "{a*&%£2")]
        public void Return_the_original_string_if_first_letter_is_lower_case(string input, string expectedResult)
        {
            _sut.Convert(input).Should().Be(expectedResult);
        }

        [TestCase("Upper", "upper")]
        [TestCase("HelloWorld", "helloWorld")]
        [TestCase("B00", "b00")]
        [TestCase("SystemIO", "systemIO")]
        public void Lower_case_first_letter_if_first_letter_is_upper_case_and_not_part_of_an_acronym(string input, string expectedResult)
        {
            _sut.Convert(input).Should().Be(expectedResult);
        }

        [TestCase("IOException", "ioException")]
        [TestCase("CIBuildServer", "ciBuildServer")]
        [TestCase("RSPV", "rspv")]
        public void Lower_case_first_letters_if_string_starts_with_an_acronym(string input, string expectedResult)
        {
            _sut.Convert(input).Should().Be(expectedResult);
        }
    }
}