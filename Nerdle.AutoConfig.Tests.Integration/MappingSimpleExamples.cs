using System;
using FluentAssertions;
using NUnit.Framework;

namespace Nerdle.AutoConfig.Tests.Integration
{
    [TestFixture]
    class MappingSimpleExamples : EndToEndTest
    {
        [Test]
        public void Mapping_to_an_interface()
        {
            var config = AutoConfig.Map<ISimpleConfiguration>();
            config.Should().NotBeNull();
            config.MyString.Should().Be("hello");
            config.MyInt.Should().Be(42);
            config.MyDate.Should().Be(new DateTime(1969, 07, 21));
            config.MyBool.Should().BeTrue();
            config.MyNullable.Should().Be(23);
            config.MyEmptyNullable.Should().NotHaveValue();
        }

        [Test]
        public void Mapping_to_a_concrete_class()
        {
            var config = AutoConfig.Map<SimpleConfiguration>();
            config.Should().NotBeNull();
            config.MyString.Should().Be("hello");
            config.MyInt.Should().Be(42);
            config.MyDate.Should().Be(new DateTime(1969, 07, 21));
            config.MyBool.Should().BeTrue();
            config.MyNullable.Should().Be(23);
            config.MyEmptyNullable.Should().NotHaveValue();
        }
    }

    public interface ISimpleConfiguration
    {
        string MyString { get; }
        int MyInt { get; }
        DateTime MyDate { get; }
        bool MyBool { get; }
        int? MyNullable { get; }
        int? MyEmptyNullable { get; }
    }

    public class SimpleConfiguration
    {
        public string MyString { get; set; }
        public int MyInt { get; set; }
        public DateTime MyDate { get; set; }
        public bool MyBool { get; set; }
        public int? MyNullable { get; set; }
        public int? MyEmptyNullable { get; set; }
    }
}
