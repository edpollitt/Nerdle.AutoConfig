using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Nerdle.AutoConfig.Exceptions;
using Nerdle.AutoConfig.TypeGeneration;
using NUnit.Framework;

namespace Nerdle.AutoConfig.Tests.Unit.TypeGenerationTests
{
    [TestFixture]
    public class When_requesting_an_interface_from_the_factory
    {
        [Test]
        public void A_concrete_implementation_is_generated()
        {
            var instance = TypeFactory.Create<IHasProperties>();
            instance.GetType().Assembly.GetName().Name.Should().Be(TypeEmitter.AssemblyName);
        }

        [Test]
        public void An_instance_is_returned()
        {
            var instance = TypeFactory.Create<IHasProperties>();
            instance.Should().NotBeNull();
        }

        [Test]
        public void Interface_methods_are_not_supported()
        {
            Action creating = () => TypeFactory.Create<IHasMethods>();

            var expectedMessage =
                string.Format(
                    "Cannot generate an implementation of interface '{0}' because it contains method definitions.",
                    typeof(IHasMethods));

            creating.ShouldThrowExactly<AutoConfigTypeGenerationException>()
                .WithMessage(expectedMessage);
        }

        [Test]
        public void The_interface_must_be_externally_accessible()
        {
            Action creating = () => TypeFactory.Create<IAmInternal>();

            var expectedMessage =
                string.Format(
                    "Cannot generate an implementation of interface '{0}' because it is not externally accessible.",
                    typeof(IAmInternal));

            creating.ShouldThrowExactly<AutoConfigTypeGenerationException>()
                .WithMessage(expectedMessage);
        }

        [Test]
        public void Concurrent_requests_do_not_generate_multiple_implementations()
        {
            var typeStack = new ConcurrentStack<Type>();

            Parallel.For(1, 1000, i =>
            {
                var instance = TypeFactory.Create<IHasProperties>();
                typeStack.Push(instance.GetType());
            });

            typeStack.Distinct().Should().HaveCount(1);
        }

        [TestFixture]
        public class When_the_interface_inherits_other_interfaces
        {
            [Test]
            public void A_concrete_implementation_is_generated()
            {
                var instance = TypeFactory.Create<IInheritsProperties>();
                instance.GetType().Assembly.GetName().Name.Should().Be(TypeEmitter.AssemblyName);
            }

            [Test]
            public void An_instance_is_returned()
            {
                var instance = TypeFactory.Create<IInheritsProperties>();
                instance.Should().NotBeNull();
            }

            [Test]
            public void Interface_methods_are_not_supported()
            {
                Action creating = () => TypeFactory.Create<IInheritsMethods>();

                var expectedMessage =
                    string.Format(
                        "Cannot generate an implementation of interface '{0}' because it contains method definitions.",
                        typeof(IInheritsMethods));

                creating.ShouldThrowExactly<AutoConfigTypeGenerationException>()
                    .WithMessage(expectedMessage);
            }
        }
    }

    public interface IHasProperties
    {
        int Foo { get; }
        string Bar { set; }
        object[] Baz { get; set; }
    }

    public interface IInheritsProperties : IHasProperties
    {
        int Qux { get; set; }
    }

    public interface IHasMethods
    {
        void Foo();
    }

    public interface IInheritsMethods : IHasMethods
    {
        int Bar { get; set; }
    }

    interface IAmInternal
    { }
}