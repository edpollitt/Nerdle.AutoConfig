using System.Configuration;

namespace Nerdle.AutoConfig
{
    interface IConfigurationSystem
    {
        Section GetSection(string sectionName, string configFilePath = null);
    }

    class ConfigurationSystem : IConfigurationSystem
    {
        public Section GetSection(string sectionName, string configFilePath = null)
        {
            if (configFilePath == null)
                return ConfigurationManager.GetSection(sectionName) as Section;

            var fileMap = new ConfigurationFileMap(configFilePath);
            var configuration = ConfigurationManager.OpenMappedMachineConfiguration(fileMap);
            return configuration.GetSection(sectionName) as Section;
        }
    }
}