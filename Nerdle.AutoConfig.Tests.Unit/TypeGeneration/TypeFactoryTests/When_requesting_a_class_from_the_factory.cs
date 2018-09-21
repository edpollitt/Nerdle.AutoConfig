using System;
using System.Security;
using System.Text;
using FluentAssertions;
using Moq;
using Nerdle.AutoConfig.Exceptions;
using Nerdle.AutoConfig.TypeGeneration;
using NUnit.Framework;

namespace Nerdle.AutoConfig.Tests.Unit.TypeGeneration.TypeFactoryTests
{
    [TestFixture]
    public class When_requesting_a_class_from_the_factory
    {
        TypeFactory _sut;
        Mock<ITypeEmitter> _typeEmitter;

        [SetUp]
        public void BeforeEach()
        {
            _typeEmitter = new Mock<ITypeEmitter>();
            _sut = new TypeFactory(_typeEmitter.Object);
        }

        [TestCase(typeof(object))]
        [TestCase(typeof(Exception))]
        [TestCase(typeof(SecureString))]
        public void An_instance_is_returned(Type type)
        {
            var instance = _sut.InstanceOf(type);
            instance.Should().NotBeNull();
        }

        [TestCase(typeof(object))]
        [TestCase(typeof(StringBuilder))]
        [TestCase(typeof(TypeUnloadedException))]
        public void A_new_type_is_not_generated(Type type)
        {
            _sut.InstanceOf(type);
            _typeEmitter.Verify(te => te.GenerateInterfaceImplementation(It.IsAny<Type>()), Times.Never);
        }

        [TestCase(typeof(StringComparer))]
        [TestCase(typeof(Attribute))]
        [TestCase(typeof(Enum))]
        public void An_exception_is_thrown_if_the_class_is_abstract(Type abstractType)
        {
            Action instantiating = () => _sut.InstanceOf(abstractType);
            instantiating.Should().ThrowExactly<AutoConfigTypeGenerationException>()
                .WithMessage(string.Format("Cannot instantiate abstract class '{0}'.", abstractType));
        }

        [TestCase(typeof(string))]
        [TestCase(typeof(Action))]
        public void An_exception_is_thrown_if_the_class_has_no_parameterless_constructor(Type type)
        {
            Action instantiating = () => _sut.InstanceOf(type);
            instantiating.Should().ThrowExactly<AutoConfigTypeGenerationException>()
               .WithMessage(string.Format("Cannot instantiate type '{0}' because no parameterless constructor was found.", type));
        }
    }
}
