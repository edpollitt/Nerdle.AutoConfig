using System;
using System.Collections.Generic;
using System.Xml.Linq;
using FluentAssertions;
using Nerdle.AutoConfig.Mappers;
using NUnit.Framework;

namespace Nerdle.AutoConfig.Tests.Unit.Mappers.KeyValuePairMapperTests
{
    [TestFixture]
    public class When_mapping_a_key_value_pair
    {
        readonly IMapper _sut = new KeyValuePairMapper();

        [Test]
        public void The_key_value_pair_is_created()
        {
            var xElement = XElement.Parse("<myKvp key=\"1\" value=\"true\" />");
            var result = _sut.Map(xElement, typeof(KeyValuePair<int, bool>));
            result.Should().NotBeNull();
        }

        [Test]
        public void The_key_value_pair_is_the_correct_type()
        {
            var xElement = XElement.Parse("<myKvp key=\"1.0\" value=\"dog\" />");
            var result = _sut.Map(xElement, typeof(KeyValuePair<float, string>));
            result.Should().BeOfType<KeyValuePair<float, string>>();
        }

        [Test]
        public void The_key_and_value_can_be_set_using_attributes()
        {
            var xElement = XElement.Parse("<myKvp key=\"Christmas\" value=\"25 Dec 2000\" />");
            var result = (KeyValuePair<string, DateTime>)_sut.Map(xElement, typeof(KeyValuePair<string, DateTime>));
            result.Key.Should().Be("Christmas");
            result.Value.Should().Be(new DateTime(2000, 12, 25));
        }

        [Test]
        public void The_key_and_value_can_be_set_using_elements()
        {
            var xElement = XElement.Parse("<myKvp><key>1</key><value>1.0</value></myKvp>");
            var result = (KeyValuePair<int, decimal>)_sut.Map(xElement, typeof(KeyValuePair<int, decimal>));
            result.Key.Should().Be(1);
            result.Value.Should().Be(1.0m);
        }
    }
}
