using System;
using System.Configuration;
using System.Xml.Linq;
using NUnit.Framework;

namespace Nerdle.AutoConfig.Tests.Integration
{
    [TestFixture]
    abstract class EndToEndTest
    {
        [TestFixtureSetUp]
        protected void InjectConfig()
        {
            var xml = ConfigExamples.ResourceManager.GetString(GetType().Name);

            if (xml == null)
                throw new InvalidOperationException(
                    string.Format("Could not find replacement config resource 'ConfigsExamples.{0}'.", GetType().Name));

            var config = XDocument.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

            var replacementConfig = XElement.Parse(xml);
            config.ReplaceNodes(replacementConfig);

            config.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

            foreach (var section in replacementConfig.Elements())
            {
                ConfigurationManager.RefreshSection(section.Name.LocalName);
            }
        }
    }
}