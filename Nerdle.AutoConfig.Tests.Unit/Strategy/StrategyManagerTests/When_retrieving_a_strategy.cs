using System;
using FluentAssertions;
using Nerdle.AutoConfig.Strategy;
using NUnit.Framework;

namespace Nerdle.AutoConfig.Tests.Unit.Strategy.StrategyManagerTests
{
    [TestFixture]
    class When_retrieving_a_strategy
    {
        StrategyManager _strategyManager;

        [SetUp]
        public void BeforeEach()
        {
            _strategyManager = new StrategyManager();
        }

        [Test]
        public void The_strategy_set_for_the_type_is_returned()
        {
            IMappingStrategy theStrategy = null;
            _strategyManager.UpdateStrategy<IConvertible>(strategy => { theStrategy = (IMappingStrategy)strategy; });
            _strategyManager.GetStrategyFor<IConvertible>().Should().Be(theStrategy);
        }

        [Test]
        public void If_no_strategy_is_set_then_the_default_strategy_is_returned()
        {
            _strategyManager.GetStrategyFor<IFormattable>().Should().Be(StrategyManager.DefaultStrategy);
        }
    }
}