using System;
using FluentAssertions;
using Moq;
using Nerdle.AutoConfig.Exceptions;
using Nerdle.AutoConfig.Sections;
using NUnit.Framework;

namespace Nerdle.AutoConfig.Tests.Unit.Sections.SectionProviderTests
{
    class When_locating_a_section_by_explicit_name
    {
        SectionProvider _sut;
        Mock<ISectionNameConvention> _nameConvention;
        Mock<IConfigurationSystem> _configurationSystem;

        [SetUp]
        public void BeforeEach()
        {
            _configurationSystem = new Mock<IConfigurationSystem>();
            _nameConvention = new Mock<ISectionNameConvention>();
            _sut = new SectionProvider(_nameConvention.Object, _configurationSystem.Object);
        }
        [Test]
        public void The_section_is_returned_if_found()
        {
            var section = new Section();
            _configurationSystem.Setup(cs => cs.GetSection("blah")).Returns(section);
            var result = _sut.GetSection<IFoo>("blah");
            result.Should().Be(section);
        }

        [Test]
        public void An_exception_is_thrown_if_the_section_is_not_found()
        {
            Action locating = () => _sut.GetSection<IFoo>("blah");
            locating.ShouldThrowExactly<AutoConfigMappingException>()
                .Where(ex => ex.Message.Contains("looked for a config section named 'blah' but didn't find one"));
        }

        interface IFoo { }
    }
}