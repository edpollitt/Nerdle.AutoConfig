using System;
using System.Collections.Concurrent;
using Nerdle.AutoConfig.CaseConverters;

namespace Nerdle.AutoConfig.Configuration
{
    public interface IConfigureMapping
    {
        /// <summary>
        /// Configures the mapping to try to match property names exactly.
        /// </summary>
        void UseMatchingCase();

        /// <summary>
        /// Configures the mapping to try to match property names to camelCased elements/attributes.
        /// </summary>
        void UseCamelCase();
    }

    internal class MappingConfigBuilder : IConfigureMapping
    {
        ICaseConverter _caseConverter;

        public MappingConfigBuilder()
        {
            UseCamelCase();
        }

        public void UseCamelCase()
        {
            _caseConverter = new CamelCaseConverter();
        }

        public void UseMatchingCase()
        {
            _caseConverter = new MatchingCaseConverter();
        }

        public MappingConfig Build()
        {
            return new MappingConfig(_caseConverter);
        }
    }

    internal class MappingConfig
    {
        readonly ICaseConverter _caseConverter;

        public MappingConfig(ICaseConverter caseConverter)
        {
            _caseConverter = caseConverter;
        }

        public string ConvertCase(string s)
        {
            return _caseConverter.Convert(s);
        }
    }

    internal static class MappingConfigs
    {
        static readonly ConcurrentDictionary<Type, MappingConfig> ConfigDictionary = new ConcurrentDictionary<Type, MappingConfig>();
       
        public static void AddConfiguration<T>(Action<IConfigureMapping> configureMapping)
        {
            var builder = new MappingConfigBuilder();
            configureMapping(builder);
            var configuration = builder.Build();

            ConfigDictionary.TryAdd(typeof(T), configuration);

            //Configurations.AddOrUpdate(typeof(T), 
            //    type => CreateMapping(strategyUpdate),
            //    (type, strategy) => UpdateStrategy(strategyUpdate, strategy));
        }

        public static MappingConfig Get<T>()
        {
            return Get(typeof(T));
        }

        public static MappingConfig Get(Type type)
        {
            return ConfigDictionary.GetOrAdd(type, DefaultConfig());
        }

        static MappingConfig DefaultConfig()
        {
            return new MappingConfigBuilder().Build();
        }
    }
}
