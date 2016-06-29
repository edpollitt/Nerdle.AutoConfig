using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Nerdle.AutoConfig.Strategy;
using NUnit.Framework;

namespace Nerdle.AutoConfig.Tests.Unit.Strategy.StrategyManagerTests
{
    [TestFixture]
    class When_creating_a_strategy
    {
        StrategyManager _sut;

        [SetUp]
        public void BeforeEach()
        {
            _sut = new StrategyManager();
        }

        [Test]
        public void The_strategy_configuration_action_is_called()
        {
            var theActionWasCalled = false;
            _sut.UpdateStrategy<ICloneable>(strategy => { theActionWasCalled = true; });
            theActionWasCalled.Should().BeTrue();
        }

        public void Strategy_updates_are_applied_progressively()
        {
            var strategies = new List<IConfigureMappingStrategy<ICloneable>>();
         
            for (var i = 0; i < 10; i++)
            {
                _sut.UpdateStrategy<ICloneable>(strategy => { strategies.Add(strategy); });
            }

            // 10 configurations were invoked
            strategies.Count.Should().Be(10);
            // all the configurations were invoked on the same strategy object
            strategies.Distinct().Should().HaveCount(1);
        }
    }
}