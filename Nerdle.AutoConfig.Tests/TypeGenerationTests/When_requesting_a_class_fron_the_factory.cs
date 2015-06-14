using System;
using System.IO;
using FluentAssertions;
using Nerdle.AutoConfig.Exceptions;
using Nerdle.AutoConfig.TypeGeneration;
using NUnit.Framework;

namespace Nerdle.AutoConfig.Tests.TypeGenerationTests
{
    [TestFixture]
    public class When_requesting_a_class_fron_the_factory
    {
        public void An_instance_is_returned()
        {
            var instance = TypeFactory.Create<object>();
            instance.Should().NotBeNull();
        }

        [Test]
        public void An_exception_is_thrown_if_the_class_is_abstract()
        {
            Action creating = () => TypeFactory.Create<Stream>();

            var expectedMessage =
                string.Format(
                    "Cannot instantiate abstract class {0}.",
                    typeof(Stream));

            creating.ShouldThrowExactly<AutoConfigTypeGenerationException>()
                .WithMessage(expectedMessage);
        }

        [Test]
        public void An_exception_is_thrown_if_the_class_has_no_parameterless_constructor()
        {
            Action creating = () => TypeFactory.Create<string>();

            var expectedMessage =
                string.Format(
                    "Cannot instantiate type {0} because no parameterless constructor was found.",
                    typeof(string));

            creating.ShouldThrowExactly<AutoConfigTypeGenerationException>()
               .WithMessage(expectedMessage);
        }
    }
}
