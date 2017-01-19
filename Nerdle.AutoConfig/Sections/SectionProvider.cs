using System.Linq;
using Nerdle.AutoConfig.Exceptions;
using Nerdle.AutoConfig.Strategy;

namespace Nerdle.AutoConfig.Sections
{
    interface ISectionProvider
    {
        Section GetSection<T>(IMappingStrategy mappingStrategy, string configFilePath = null);
        Section GetSection<T>(string sectionName, string configFilePath = null);
    }

    class SectionProvider : ISectionProvider
    {
        readonly ISectionNameConvention _nameConvention;
        readonly IConfigurationSystem _configurationSystem;

        public SectionProvider(ISectionNameConvention nameConvention, IConfigurationSystem configurationSystem)
        {
            _nameConvention = nameConvention;
            _configurationSystem = configurationSystem;
        }

        public Section GetSection<T>(string sectionName, string configFilePath = null)
        {
            var section = _configurationSystem.GetSection(sectionName, configFilePath);

            if (section != null)
                return section;

            throw new AutoConfigMappingException(
                string.Format("Could not map type '{0}', looked for a config section named '{1}' but didn't find one. Make sure the section exists and is correctly cased.",
                    typeof(T), sectionName));
        }

        public Section GetSection<T>(IMappingStrategy mappingStrategy, string configFilePath = null)
        {
            var sectionName = mappingStrategy.SectionNameFor<T>();

            var section = _configurationSystem.GetSection(sectionName, configFilePath);

            if (section != null)
                return section;

            var alternativeNames = _nameConvention.GetAlternativeNames(sectionName).ToList();

            if (!alternativeNames.Any())
                throw new AutoConfigMappingException(
                    string.Format("Could not map type '{0}', looked for a config section named '{1}' but didn't find one. Make sure the section exists and is correctly cased.",
                        typeof(T), sectionName));

            foreach (var alternative in alternativeNames)
            {
                section = _configurationSystem.GetSection(alternative, configFilePath);

                if (section != null)
                    return section;
            }

            throw new AutoConfigMappingException(
                string.Format("Could not map type '{0}', looked for a config section named '{1}'{2} but didn't find one. Make sure the section exists and is correctly cased.",
                    typeof(T), sectionName, alternativeNames.Aggregate(string.Empty, (acc, name) => acc + " or '" + name + "'")));
        }
    }
}