using System;
using System.ComponentModel;
using System.Xml.Linq;
using FluentAssertions;
using Nerdle.AutoConfig.Mapping.Mappers;
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

            var config = AutoConfig.Map<ICustomConfiguration>(configFilePath: ConfigFilePath);

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

            var config = AutoConfig.Map<ICustomConfiguration>(configFilePath: ConfigFilePath);

            config.Foo.Should().Be("fooooo");
            config.Bar.Should().BeNull();
            config.Baz.Should().Be("baz");
            config.Qux.Should().Be("From custom mapper");
        }
        
        [Test]
        public void Configuring_the_mapping_with_both_fluent_api_and_default_value_attribute()
        {
            AutoConfig.WhenMapping<IDefaultValueConfiguration>(mapper => mapper.Map(x => x.Foo).OptionalWithDefault("default_from_fluent_api"));
            var config = AutoConfig.Map<IDefaultValueConfiguration>(configFilePath: ConfigFilePath);
            config.Foo.Should().Be("default_from_fluent_api");
        }
    }

    public interface ICustomConfiguration
    {
        string Foo { get; }
        string Bar { get; }
        string Baz { get; }
        string Qux { get; }
    }

    public interface IDefaultValueConfiguration
    {
        [DefaultValue("default_from_attribute")]
        string Foo { get; }
    }
    
    class CustomMapper : IMapper
    {
        public object Map(XElement element, Type type)
        {
            return "From custom mapper";
        }
    }
}