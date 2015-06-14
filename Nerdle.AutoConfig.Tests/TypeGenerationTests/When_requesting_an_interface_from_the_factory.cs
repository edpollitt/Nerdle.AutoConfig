using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Nerdle.AutoConfig.Exceptions;
using Nerdle.AutoConfig.TypeGeneration;
using NUnit.Framework;

namespace Nerdle.AutoConfig.Tests.TypeGenerationTests
{
    [TestFixture]
    public class When_requesting_an_interface_from_the_factory
    {
        [Test]
        public void A_concrete_implementation_is_generated()
        {
            var instance = TypeFactory.Create<IHaveProperties>();
            instance.GetType().Assembly.GetName().Name.Should().Be(TypeEmitter.AssemblyName);
        }

        [Test]
        public void An_instance_is_returned()
        {
            var instance = TypeFactory.Create<IHaveProperties>();
            instance.Should().NotBeNull();
        }

        [Test]
        public void Interface_methods_are_not_supported()
        {
            Action creating = () => TypeFactory.Create<IHaveMethods>();

            var expectedMessage =
                string.Format(
                    "Cannot generate an implementation of interface {0} because it contains method definitions.",
                    typeof(IHaveMethods));

            creating.ShouldThrowExactly<AutoConfigTypeGenerationException>()
                .WithMessage(expectedMessage);
        }

        [Test]
        public void The_interface_must_be_externally_accessible()
        {
            Action creating = () => TypeFactory.Create<IAmInternal>();

            var expectedMessage =
                string.Format(
                    "Cannot generate an implementation of interface {0} because it is not externally accessible.",
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
                var instance = TypeFactory.Create<IHaveProperties>();
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
                var instance = TypeFactory.Create<IInheritProperties>();
                instance.GetType().Assembly.GetName().Name.Should().Be(TypeEmitter.AssemblyName);
            }

            [Test]
            public void An_instance_is_returned()
            {
                var instance = TypeFactory.Create<IInheritProperties>();
                instance.Should().NotBeNull();
            }

            [Test]
            public void Interface_methods_are_not_supported()
            {
                Action creating = () => TypeFactory.Create<IInheritMethods>();

                var expectedMessage =
                    string.Format(
                        "Cannot generate an implementation of interface {0} because it contains method definitions.",
                        typeof(IInheritMethods));

                creating.ShouldThrowExactly<AutoConfigTypeGenerationException>()
                    .WithMessage(expectedMessage);
            }
        }
    }

    public interface IHaveProperties
    {
        int Foo { get; }
        string Bar { set; }
        object[] Baz { get; set; }
    }

    public interface IInheritProperties : IHaveProperties
    {
        int Qux { get; set; }
    }

    public interface IHaveMethods
    {
        void Foo();
    }

    public interface IInheritMethods : IHaveMethods
    {
        int Bar { get; set; }
    }

    interface IAmInternal
    { }
}