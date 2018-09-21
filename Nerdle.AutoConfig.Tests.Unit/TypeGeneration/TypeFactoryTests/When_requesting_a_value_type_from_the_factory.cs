using System;
using System.Drawing;
using FluentAssertions;
using Moq;
using Nerdle.AutoConfig.Exceptions;
using Nerdle.AutoConfig.TypeGeneration;
using NUnit.Framework;

namespace Nerdle.AutoConfig.Tests.Unit.TypeGeneration.TypeFactoryTests
{
    [TestFixture]
    public class When_requesting_a_value_type_from_the_factory
    {
        TypeFactory _sut;
        Mock<ITypeEmitter> _typeEmitter;

        [SetUp]
        public void BeforeEach()
        {
            _typeEmitter = new Mock<ITypeEmitter>();
            _sut = new TypeFactory(_typeEmitter.Object);
        }

        [TestCase(typeof(int))]
        [TestCase(typeof(DateTime))]
        [TestCase(typeof(Point))]
        [TestCase(typeof(GCCollectionMode))]
        public void Value_types_are_not_yet_supported_because_we_would_need_to_deal_with_immutability_and_nullability(Type valueType)
        {
            Action creating = () => _sut.InstanceOf(valueType);
       
            creating.Should().ThrowExactly<AutoConfigTypeGenerationException>()
                .WithMessage(string.Format("Type '{0}' is a struct and is not supported. The requested configuration type should be an interface or class.", valueType));
        }
    }
}
