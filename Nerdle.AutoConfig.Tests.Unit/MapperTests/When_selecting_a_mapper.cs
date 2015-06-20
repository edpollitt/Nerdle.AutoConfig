using System;
using System.Collections.Generic;
using FluentAssertions;
using Nerdle.AutoConfig.Exceptions;
using Nerdle.AutoConfig.Mappers;
using NUnit.Framework;

namespace Nerdle.AutoConfig.Tests.Unit.MapperTests
{
    [TestFixture]
    public class When_selecting_a_mapper
    {
        [TestCase(typeof(int))]
        [TestCase(typeof(uint))]
        [TestCase(typeof(byte))]
        [TestCase(typeof(sbyte))]
        [TestCase(typeof(long))]
        [TestCase(typeof(ulong))]
        [TestCase(typeof(float))]
        [TestCase(typeof(double))]
        [TestCase(typeof(decimal))]
        [TestCase(typeof(bool))]
        [TestCase(typeof(DayOfWeek))]
        [TestCase(typeof(DateTime))]
        [TestCase(typeof(DateTimeOffset))]
        [TestCase(typeof(TimeSpan))]
        [TestCase(typeof(string))]
        [TestCase(typeof(char))]
        [TestCase(typeof(int?))]
        [TestCase(typeof(byte?))]
        [TestCase(typeof(double?))]
        [TestCase(typeof(decimal?))]
        [TestCase(typeof(bool?))]
        [TestCase(typeof(ConsoleColor?))]
        [TestCase(typeof(DateTime?))]
        [TestCase(typeof(TimeSpan?))]
        public void Simple_types_can_be_mapped_by_a_SimpleMapper(Type type)
        {
            Mapper.For(type).Should().BeOfType<SimpletMapper>();
        }

        [TestCase(typeof(List<int>))]
        [TestCase(typeof(IList<bool>))]
        [TestCase(typeof(ICollection<object>))]
        [TestCase(typeof(IEnumerable<DayOfWeek>))]
        [TestCase(typeof(IReadOnlyList<string>))]
        [TestCase(typeof(IReadOnlyCollection<DateTime>))]
        [TestCase(typeof(IList<IList<Exception>>))]
        public void Geeric_collection_types_which_are_implemented_by_List_can_be_mapped_by_a_CollectionMapper(Type type)
        {
            Mapper.For(type).Should().BeOfType<CollectionMapper>();
        }

        [TestCase(typeof(int[]))]
        [TestCase(typeof(bool[]))]
        [TestCase(typeof(IDictionary<int, string>))]
        [TestCase(typeof(StringComparer))]
        [TestCase(typeof(IDisposable))]
        [TestCase(typeof(object))]
        public void An_exception_is_thrown_if_no_mapper_is_selected(Type type)
        {
            Action selecting = () => Mapper.For(type);
            selecting.ShouldThrowExactly<AutoConfigMappingException>()
                .WithMessage(string.Format("No IMapper found to handle type '{0}'.", type));

        }
    }
}
