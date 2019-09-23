using System;
using System.ComponentModel;
using FluentAssertions;
using Nerdle.AutoConfig.Extensions;
using NUnit.Framework;

namespace Nerdle.AutoConfig.Tests.Unit.Extensions
{
    [Flags] enum SByteEnum :   sbyte { Small = 1, Medium = 2, Large = 4 }
    [Flags] enum ByteEnum :     byte { Small = 1, Medium = 2, Large = 4 }
    [Flags] enum Int16Enum :   short { Small = 1, Medium = 2, Large = 4 }
    [Flags] enum UInt16Enum : ushort { Small = 1, Medium = 2, Large = 4 }
    [Flags] enum Int32Enum :     int { Small = 1, Medium = 2, Large = 4 }
    [Flags] enum UInt32Enum :   uint { Small = 1, Medium = 2, Large = 4 }
    [Flags] enum Int64Enum :    long { Small = 1, Medium = 2, Large = 4 }
    [Flags] enum UInt64Enum :  ulong { Small = 1, Medium = 2, Large = 4 }

    [TestFixture]
    class When_testing_flag
    {
        [TestCase(typeof(SByteEnum))]
        [TestCase(typeof(ByteEnum))]
        [TestCase(typeof(Int16Enum))]
        [TestCase(typeof(UInt16Enum))]
        [TestCase(typeof(Int32Enum))]
        [TestCase(typeof(UInt32Enum))]
        [TestCase(typeof(Int64Enum))]
        [TestCase(typeof(UInt64Enum))]
        public void IsFlagDefined_is_correct_for_all_possible_enum_types(Type enumType)
        {
            var converter = TypeDescriptor.GetConverter(enumType);
            enumType.IsFlagDefined(converter.ConvertFromInvariantString("Medium")).Should().BeTrue();
            enumType.IsFlagDefined(converter.ConvertFromInvariantString("2")).Should().BeTrue();
            enumType.IsFlagDefined(converter.ConvertFromInvariantString("Small,Medium,Large")).Should().BeTrue();
            enumType.IsFlagDefined(converter.ConvertFromInvariantString("7")).Should().BeTrue();
            enumType.IsFlagDefined(converter.ConvertFromInvariantString("8")).Should().BeFalse();
        }

        [TestCase]
        public void IsFlagDefined_returns_true_when_zero_exists_in_enum()
        {
            var enumType = typeof(System.Net.AuthenticationSchemes);
            var converter = TypeDescriptor.GetConverter(enumType);
            enumType.IsFlagDefined(converter.ConvertFromInvariantString("0")).Should().BeTrue();
        }

        [TestCase]
        public void IsFlagDefined_returns_false_when_zero_does_not_exist_in_enum()
        {
            var enumType = typeof(System.Reflection.CallingConventions);
            var converter = TypeDescriptor.GetConverter(enumType);
            enumType.IsFlagDefined(converter.ConvertFromInvariantString("0")).Should().BeFalse();
        }
    }
}