using NUnit.Framework;

namespace Nerdle.AutoConfig.Tests.Integration
{
    [TestFixture]
    public abstract class EndToEndTest
    {
        public EndToEndTest()
        {
            // See https://github.com/nunit/nunit/issues/1072#issuecomment-218725978
            System.Environment.CurrentDirectory = TestContext.CurrentContext.TestDirectory;
        }
        
        public string ConfigFilePath => GetType().Name + ".xml";
    }
}