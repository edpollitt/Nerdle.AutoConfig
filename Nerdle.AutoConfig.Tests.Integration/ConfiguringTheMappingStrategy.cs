using System;
using System.Xml.Linq;
using FluentAssertions;
using Nerdle.AutoConfig.Mappers;
using NUnit.Framework;

namespace Nerdle.AutoConfig.Tests.Integration
{
    [TestFixture]
    class ConfiguringTheMappingStrategy : EndToEndTest
    {
        [Test]
        public void Configuring_the_mapping()
        {
            AutoConfig.WhenMapping<ICustomConfiguration>(
                mapper =>
                {
                    mapper.UseMatchingCase();
                    mapper.Map(x => x.Foo).From("foofoo");
                    mapper.Map(x => x.Bar).Optional();
                    mapper.Map(x => x.Baz).OptionalWithDefault("baz");
                    mapper.Map(x => x.Qux).Using<CustomMapper>();
                });

            var config = AutoConfig.Map<ICustomConfiguration>();

            config.Foo.Should().Be("fooooo");
            config.Bar.Should().BeNull();
            config.Baz.Should().Be("baz");
            config.Qux.Should().Be("From custom mapper");
        }

        [Test]
        public void Progressively_configuring_the_mapping()
        {
            AutoConfig.WhenMapping<ICustomConfiguration>(
                mapper =>
                {
                    mapper.UseMatchingCase();
                });

            AutoConfig.WhenMapping<ICustomConfiguration>(
               mapper =>
               {
                   mapper.Map(x => x.Bar).From("barbar");
                   mapper.UseCamelCase();
               });

            AutoConfig.WhenMapping<ICustomConfiguration>(
               mapper =>
               {
                   mapper.Map(x => x.Bar).Optional();
                   mapper.UseCamelCase();
                   mapper.Map(x => x.Foo).From("foofoo");
               });

            AutoConfig.WhenMapping<ICustomConfiguration>(
               mapper =>
               {
                   mapper.Map(x => x.Foo).From("foofoo");
                   mapper.Map(x => x.Baz).OptionalWithDefault("baz");
                   mapper.Map(x => x.Qux).Using<CustomMapper>();
                   mapper.UseMatchingCase();
               });

            var config = AutoConfig.Map<ICustomConfiguration>();

            config.Foo.Should().Be("fooooo");
            config.Bar.Should().BeNull();
            config.Baz.Should().Be("baz");
            config.Qux.Should().Be("From custom mapper");
        }
    }

    public interface ICustomConfiguration
    {
        string Foo { get; }
        string Bar { get; }
        string Baz { get; }
        string Qux { get; }
    }

    class CustomMapper : IMapper
    {
        public object Map(XElement element, Type type)
        {
            return "From custom mapper";
        }
    }
}