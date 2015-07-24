using System.Linq;
using FluentAssertions;
using Nerdle.AutoConfig.Extensions;
using NUnit.Framework;

namespace Nerdle.AutoConfig.Tests.Unit.Extensions
{
    [TestFixture]
    class When_getting_public_setters_for_a_type
    {
        [Test]
        public void Public_setters_are_returned()
        {
            var setters = typeof(Foo).PublicSetters().ToList();
            setters.Should().HaveCount(2);
            setters.Select(s => s.Name).Should().BeEquivalentTo("Bar", "Baz");
        }

        class Foo
        {
            public int Bar { get; set; }
            public int Baz { internal get; set; }
            public int Internal { get; internal set; }
            protected int Protected { get; set; }
            public int Private { get; private set; }
        }
    }
}