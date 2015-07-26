using Nerdle.AutoConfig.Mappers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Nerdle.AutoConfig.Tests.Integration
{
    [TestFixture]
    class UsingCustomMappers : EndToEndTest
    {
        [Test]
        public void Using_a_custom_mapper()
        {
            AutoConfig.WhenMapping<ICustomMapperExample>(
                mapper =>
                {
                    mapper.Map(x => x.Foo).Using<CommaSeparatedListMapper>();
                    mapper.Map(x => x.Bar).Using<CommaSeparatedListMapper>();
                    mapper.Map(x => x.Baz).Using<CommaSeparatedListMapper>();
                });

            var config = AutoConfig.Map<ICustomMapperExample>();

            config.Foo.Should().Equal(1, 2, 3, 4, 5);
            config.Bar.Should().Equal("one", "two", "three");
            config.Baz.Should().Equal(DayOfWeek.Monday, DayOfWeek.Tuesday);
        }
    }

    public interface ICustomMapperExample
    {
        IEnumerable<int> Foo { get; }
        IEnumerable<string> Bar { get; }
        IEnumerable<DayOfWeek> Baz { get; }
    }

    class CommaSeparatedListMapper : IMapper
    {
        public object Map(XElement element, Type type)
        {
            var isEnumerable = type.IsGenericType &&
                    type.GetGenericTypeDefinition() == typeof(IEnumerable<>);

            if (!isEnumerable)
                throw new InvalidOperationException("Expected an IEnumerable<> but got " + type);

            var enumerableType = type.GetGenericArguments().First();

            var converter = TypeDescriptor.GetConverter(enumerableType);

            if (converter.CanConvertFrom(typeof(string)))
            {
                var values = element.Value.Split(new[] { ',' },
                            StringSplitOptions.RemoveEmptyEntries)
                    .Select(value => converter.ConvertFromString(value))
                    .ToList();

                var list = Activator.CreateInstance(typeof(List<>).MakeGenericType(enumerableType));
                var listAdd = list.GetType().GetMethod("Add");

                values.ForEach(value => listAdd.Invoke(list, new[] { value }));

                return list;
            }

            throw new InvalidOperationException("Cannot convert string to type " + enumerableType);
        }
    }
}
