using System;
using System.Linq;
using FluentAssertions;
using Nerdle.AutoConfig.Strategy;
using NUnit.Framework;

namespace Nerdle.AutoConfig.Tests.Unit.Strategy.MappingStrategyProviderTests
{
    [TestFixture]
    class When_creating_a_strategy
    {
        StrategyManager _strategyManager;

        [SetUp]
        public void BeforeEach()
        {
            _strategyManager = new StrategyManager();
        }

        [Test]
        public void The_strategy_configuration_action_is_called()
        {
            var theActionWasCalled = false;
            _strategyManager.UpdateStrategy<ICloneable>(strategy => { theActionWasCalled = true; });
            theActionWasCalled.Should().BeTrue();
        }

        public void Strategy_updates_are_applied_progressively()
        {
            var strategies = new IConfigureMappingStrategy<ICloneable>[5];
         
            for (var i = 0; i < strategies.Length; i++)
            {
                _strategyManager.UpdateStrategy<ICloneable>(strategy => { strategies[i] = strategy; });
            }
            
            strategies.Distinct().Should().HaveCount(1);
        }
    }
}