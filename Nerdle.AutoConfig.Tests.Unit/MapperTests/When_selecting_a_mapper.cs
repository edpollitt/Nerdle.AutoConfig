using System;
using System.Collections.Generic;
using FluentAssertions;
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
        public void Simple_types_can_be_mapped_by_the_ValueMapper(Type type)
        {
            Mapper.For(type).Should().BeOfType<ValueMapper>();
        }

        [TestCase(typeof(List<int>))]
        [TestCase(typeof(IList<bool>))]
        [TestCase(typeof(ICollection<object>))]
        [TestCase(typeof(IEnumerable<DayOfWeek>))]
        [TestCase(typeof(IReadOnlyList<string>))]
        [TestCase(typeof(IReadOnlyCollection<DateTime>))]
        [TestCase(typeof(IList<IList<Exception>>))]
        public void Generic_collection_types_which_are_implemented_by_List_can_be_mapped_by_the_CollectionMapper(Type type)
        {
            Mapper.For(type).Should().BeOfType<CollectionMapper>();
        }

        [TestCase(typeof(int[]))]
        [TestCase(typeof(bool[]))]
        [TestCase(typeof(List<Exception>[]))]
        public void Single_dimensional_array_types_can_be_mapped_by_the_ArrayMapper(Type type)
        {
            Mapper.For(type).Should().BeOfType<ArrayMapper>();
        }

        [TestCase(typeof(int[,]))]
        [TestCase(typeof(IDictionary<int, string>))]
        [TestCase(typeof(Exception))]
        public void Types_which_cannot_be_mapped_by_any_other_mapper_should_attempt_to_use_the_ComplexMapper(Type type)
        {
            Mapper.For(type).Should().BeOfType<ComplexMapper>();
        }
    }
}
