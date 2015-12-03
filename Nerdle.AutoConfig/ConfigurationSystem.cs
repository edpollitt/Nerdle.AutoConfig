using System.Configuration;

namespace Nerdle.AutoConfig
{
    interface IConfigurationSystem
    {
        Section GetSection(string sectionName);
    }

    class ConfigurationSystem : IConfigurationSystem
    {
        public Section GetSection(string sectionName)
        {
            return ConfigurationManager.GetSection(sectionName) as Section;
        }
    }
}