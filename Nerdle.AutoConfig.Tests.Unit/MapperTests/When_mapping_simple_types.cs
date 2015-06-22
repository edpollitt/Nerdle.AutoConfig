using System;
using System.Xml.Linq;
using FluentAssertions;
using Nerdle.AutoConfig.Mappers;
using NUnit.Framework;

namespace Nerdle.AutoConfig.Tests.Unit.MapperTests
{
    [TestFixture]
    public class When_mapping_simple_types
    {
        readonly IMapper _mapper = new ValueMapper();

        [Test]
        [TestCaseSource("Numbers")]
        [TestCaseSource("Bools")]
        [TestCaseSource("Strings")]
        [TestCaseSource("Enums")]
        [TestCaseSource("DateTimes")]
        public void The_correct_type_and_value_is_returned(string value, Type type, object expectedResult)
        {
            var xElement = XElement.Parse(string.Format("<test>{0}</test>", value));
            var result = _mapper.Map(xElement, type);
            result.Should().BeOfType(type);
            result.Should().Be(expectedResult);
        }

        static readonly object[] Numbers =
        {
            new object[] { "1", typeof(int), 1 },
            new object[] { "0", typeof(int), 0 },
            new object[] { "-1", typeof(int), -1 },
            new object[] { "1.500", typeof(float), 1.5f },
            new object[] { "-99.9", typeof(decimal), -99.9M },
            new object[] { "9999999999", typeof(long), 9999999999L },
            new object[] { "-0", typeof(ulong), 0UL },
        };

        static readonly object[] Bools =
        {
            new object[] { "true", typeof(bool), true },
            new object[] { "True", typeof(bool), true },
            new object[] { "TRUE", typeof(bool), true },
            new object[] { "false", typeof(bool), false },
        };

        static readonly object[] Strings =
        {
            new object[] { "hello world", typeof(string), "hello world" },
            new object[] { "", typeof(string), "" },
            new object[] { "X", typeof(char), 'X' },
        };

        static readonly object[] Enums =
        {
            new object[] { "Monday", typeof(DayOfWeek), DayOfWeek.Monday },
            new object[] { "MONDAY", typeof(DayOfWeek), DayOfWeek.Monday },
            new object[] { "monday", typeof(DayOfWeek), DayOfWeek.Monday },
            new object[] { "1", typeof(DayOfWeek), DayOfWeek.Monday },
        };

        static readonly object[] DateTimes =
        {
            new object[] { "1 Jan 2001", typeof(DateTime), new DateTime(2001, 1, 1) },
            new object[] { "1:02:03:04", typeof(TimeSpan), new TimeSpan(1, 2, 3, 4),  },
        };
    }
}
