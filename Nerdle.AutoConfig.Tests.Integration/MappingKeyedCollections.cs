using FluentAssertions;
using NUnit.Framework;
using System.Collections.ObjectModel;

namespace Nerdle.AutoConfig.Tests.Integration
{
    [TestFixture]
    class MappingKeyedCollections : EndToEndTest
    {
        [Test]
        public void Mapping_keyed_collections()
        {
            var config = AutoConfig.Map<IThemeConfig>();
            config.Should().NotBeNull();
            config.Themes.Should().NotBeNull();
            config.Themes.Should().HaveCount(3);
            config.Themes["Blue"].Should().Match<ITheme>(theme => theme.Name == "Blue");
            config.Themes["Red"].Should().Match<ITheme>(theme => theme.Name == "Red");
            config.Themes["Black"].Should().Match<ITheme>(theme => theme.Name == "Black");
            config.Default.Should().Be("Red");
        }

        public interface IThemeConfig
        {
            string Default { get; }
            ThemeCollection Themes { get; }
        }

        public interface ITheme
        {
            string Name { get; }
        }

        public class ThemeCollection : KeyedCollection<string, ITheme>
        {
            protected override string GetKeyForItem(ITheme item)
            {
                return item.Name;
            }
        }
    }
}
