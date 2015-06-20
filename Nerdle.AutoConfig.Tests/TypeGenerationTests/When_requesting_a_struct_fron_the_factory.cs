using System;
using FluentAssertions;
using Nerdle.AutoConfig.Exceptions;
using Nerdle.AutoConfig.TypeGeneration;
using NUnit.Framework;

namespace Nerdle.AutoConfig.Tests.TypeGenerationTests
{
    [TestFixture]
    public class When_requesting_a_struct_fron_the_factory
    {
        [Test]
        public void Structs_are_not_yet_supported_because_we_will_need_to_deal_with_immutability_and_nullability()
        {
            Action creating = () => TypeFactory.Create<int>();
       
            creating.ShouldThrowExactly<AutoConfigTypeGenerationException>()
                .WithMessage("Type System.Int32 is a struct and is not supported. The requested configuration type should be an interface or class.");
        }
    }
}
