using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Nerdle.AutoConfig.TypeGeneration;
using NUnit.Framework;

namespace Nerdle.AutoConfig.Tests.Unit.TypeGeneration.TypeFactoryTests
{
    [TestFixture]
    public class When_requesting_an_interface_from_the_factory
    {
        TypeFactory _sut;
        Mock<ITypeEmitter> _typeEmitter;

        [SetUp]
        public void BeforeEach()
        {
            _typeEmitter = new Mock<ITypeEmitter>();
            _typeEmitter.Setup(te => te.GenerateInterfaceImplementation(typeof(IMyInterface))).Returns(typeof(MyImplementation));
            _sut = new TypeFactory(_typeEmitter.Object);
        }

        [Test]
        public void An_instance_is_returned()
        {
            var instance = _sut.InstanceOf<IMyInterface>();
            instance.Should().NotBeNull();
            instance.Should().BeOfType<MyImplementation>();
        }
        
        [Test]
        public void A_concrete_implementation_is_generated()
        {
            _sut.InstanceOf<IMyInterface>();
            _typeEmitter.Verify(te => te.GenerateInterfaceImplementation(typeof(IMyInterface)), Times.Once);
        }

        [Test]
        public void Concurrent_requests_do_not_generate_multiple_implementations()
        {
            Parallel.For(1, 1000, i =>
            {
                _sut.InstanceOf<IMyInterface>();
            });
            _typeEmitter.Verify(te => te.GenerateInterfaceImplementation(typeof(IMyInterface)), Times.Once);
        }
    }

    public interface IMyInterface
    {}

    public class MyImplementation : IMyInterface
    {}
}