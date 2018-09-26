using NUnit.Framework;

namespace Nerdle.AutoConfig.Tests.Integration
{
    [TestFixture]
    public abstract class EndToEndTest
    {
        public string ConfigFilePath => GetType().Name + ".xml";
    }
}