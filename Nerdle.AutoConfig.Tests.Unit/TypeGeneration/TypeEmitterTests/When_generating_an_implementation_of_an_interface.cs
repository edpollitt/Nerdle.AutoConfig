using System;
using FluentAssertions;
using Nerdle.AutoConfig.Exceptions;
using Nerdle.AutoConfig.TypeGeneration;
using NUnit.Framework;

namespace Nerdle.AutoConfig.Tests.Unit.TypeGeneration.TypeEmitterTests
{
    [TestFixture]
    class When_generating_an_implementation_of_an_interface
    {
        readonly TypeEmitter _sut = new TypeEmitter();

        [Test]
        public void A_concrete_type_is_generated()
        {
            var type = _sut.GenerateInterfaceImplementation(typeof(IHaveProperties));
            type.IsClass.Should().BeTrue();
            type.IsAbstract.Should().BeFalse();
        }

        [Test]
        public void The_generated_type_implements_the_interface()
        {
            var type = _sut.GenerateInterfaceImplementation(typeof(IHaveProperties));
            type.GetInterfaces().Should().Equal(typeof(IHaveProperties));
        }

        [Test]
        public void The_implementation_is_emitted_into_the_assembly()
        {
            var type = _sut.GenerateInterfaceImplementation(typeof(IHaveProperties));   
            type.Assembly.GetName().Name.Should().Be(TypeEmitter.AssemblyName);
        }

        [Test]
        public void Interface_methods_are_not_supported()
        {
            Action generating = () => _sut.GenerateInterfaceImplementation(typeof(IHaveMethods));

            var expectedMessage =
                string.Format(
                    "Cannot generate an implementation of interface '{0}' because it contains method definitions.",
                    typeof(IHaveMethods));

            generating.Should().ThrowExactly<AutoConfigTypeGenerationException>()
                .WithMessage(expectedMessage);
        }

        [Test]
        public void The_interface_must_be_externally_accessible()
        {
            Action generating = () => _sut.GenerateInterfaceImplementation(typeof(IAmInternal));

            generating.Should().ThrowExactly<AutoConfigTypeGenerationException>()
                .Where(ex => ex.Message.Contains(string.Format("Cannot generate an implementation of interface '{0}' because it is not externally accessible.", typeof(IAmInternal))));
        }

        [TestFixture]
        public class When_the_interface_inherits_other_interfaces
        {
            readonly TypeEmitter _sut = new TypeEmitter();

            [Test]
            public void A_concrete_type_is_generated()
            {
                var type = _sut.GenerateInterfaceImplementation(typeof(IInheritProperties));
                type.IsClass.Should().BeTrue();
                type.IsAbstract.Should().BeFalse();
            }

            [Test]
            public void The_generated_type_implements_the_interfaces()
            {
                var type = _sut.GenerateInterfaceImplementation(typeof(IInheritProperties));
                type.GetInterfaces().Should().BeEquivalentTo(typeof(IInheritProperties), typeof(IHaveProperties));
            }

            [Test]
            public void The_implementation_is_emitted_into_the_assembly()
            {
                var instance = _sut.GenerateInterfaceImplementation(typeof(IInheritProperties));
                instance.Assembly.GetName().Name.Should().Be(TypeEmitter.AssemblyName);
            }

            [Test]
            public void Interface_methods_are_not_supported()
            {
                Action creating = () => _sut.GenerateInterfaceImplementation(typeof(IInheritMethods));

                var expectedMessage =
                    string.Format(
                        "Cannot generate an implementation of interface '{0}' because it contains method definitions.",
                        typeof(IInheritMethods));

                creating.Should().ThrowExactly<AutoConfigTypeGenerationException>()
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
