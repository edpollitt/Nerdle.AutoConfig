using System;
using System.ComponentModel;
using FluentAssertions;
using NUnit.Framework;

namespace Nerdle.AutoConfig.Tests.Integration
{
    public enum Size {
        Small,
        Medium,
        Large
    }
    
    [TestFixture]
    public class ConfiguringTheMappingStrategyWithAttributes : EndToEndTest
    {
        [Test]
        public void Mapping_to_an_interface()
        {
            var config = AutoConfig.Map<ISimpleConfiguration>(configFilePath: ConfigFilePath);
            config.Should().BeAssignableTo<ISimpleConfiguration>();
            config.Should().NotBeNull();
            config.MyString.Should().Be("hello");
            config.MyInt.Should().Be(42);
            config.MyDate1.Should().Be(new DateTime(1969, 07, 21));
            config.MyDate2.Should().Be(new DateTime(1969, 07, 22));
            config.MyBool.Should().BeTrue();
            config.MyNullable.Should().Be(23);
            config.MyEmptyNullable.Should().NotHaveValue();
            config.MyTimeSpan1.Should().Be(TimeSpan.FromMinutes(5));
            config.MyTimeSpan2.Should().Be(TimeSpan.FromMinutes(6));
            config.MySize.Should().Be(Size.Medium);
        }

        [Test]
        public void Mapping_to_a_concrete_class()
        {
            var config = AutoConfig.Map<SimpleConfiguration>(configFilePath: ConfigFilePath);
            config.Should().BeOfType<SimpleConfiguration>();
            config.Should().NotBeNull();
            config.MyString.Should().Be("hello");
            config.MyInt.Should().Be(42);
            config.MyDate1.Should().Be(new DateTime(1969, 07, 21));
            config.MyDate2.Should().Be(new DateTime(1969, 07, 22));
            config.MyBool.Should().BeTrue();
            config.MyNullable.Should().Be(23);
            config.MyEmptyNullable.Should().NotHaveValue();
            config.MyTimeSpan1.Should().Be(TimeSpan.FromMinutes(5));
            config.MyTimeSpan2.Should().Be(TimeSpan.FromMinutes(6));
            config.MySize.Should().Be(Size.Medium);
        }

        public interface ISimpleConfiguration
        {
            [DefaultValue("hello")]
            string MyString { get; }
            [DefaultValue(42)]
            int MyInt { get; }
            [DefaultValue("1969-07-21")]
            DateTime MyDate1 { get; }
            [DefaultValue(typeof(DateTime), "1969-07-22")]
            DateTime MyDate2 { get; }
            [DefaultValue(true)]
            bool MyBool { get; }
            [DefaultValue(23)]
            int? MyNullable { get; }
            [DefaultValue(null)]
            int? MyEmptyNullable { get; }
            [DefaultValue("00:05:00")]
            TimeSpan MyTimeSpan1 { get; }
            [DefaultValue(typeof(TimeSpan), "00:06:00")]
            TimeSpan MyTimeSpan2 { get; }
            [DefaultValue(Size.Medium)]
            Size MySize { get; }
        }

        public class SimpleConfiguration
        {
            [DefaultValue("hello")]
            public string MyString { get; set; }
            [DefaultValue(42)]
            public int MyInt { get; set; }
            [DefaultValue("1969-07-21")]
            public DateTime MyDate1 { get; set; }
            [DefaultValue(typeof(DateTime), "1969-07-22")]
            public DateTime MyDate2 { get; set; }
            [DefaultValue(true)]
            public bool MyBool { get; set; }
            [DefaultValue(23)]
            public int? MyNullable { get; set; }
            [DefaultValue(null)]
            public int? MyEmptyNullable { get; set; }
            [DefaultValue("00:05:00")]
            public TimeSpan MyTimeSpan1 { get; set; }
            [DefaultValue(typeof(TimeSpan), "00:06:00")]
            public TimeSpan MyTimeSpan2 { get; set; }
            [DefaultValue(Size.Medium)]
            public Size MySize { get; set; }
        }
    }
}
