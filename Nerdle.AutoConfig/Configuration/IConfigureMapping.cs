using System;
using System.Collections.Concurrent;
using Nerdle.AutoConfig.Casing;

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
        ICase _case;

        public MappingConfigBuilder()
        {
            UseCamelCase();
        }

        public void UseCamelCase()
        {
            _case = new CamelCase();
        }

        public void UseMatchingCase()
        {
            _case = new MatchingCase();
        }

        public MappingConfig Build()
        {
            return new MappingConfig(_case);
        }
    }

    internal class MappingConfig
    {
        public ICase Case { get; private set; }

        public MappingConfig(ICase @case)
        {
            Case = @case;
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

        public static MappingConfig GetFor<T>()
        {
            return GetFor(typeof(T));
        }

        public static MappingConfig GetFor(Type type)
        {
            return ConfigDictionary.GetOrAdd(type, DefaultConfig());
        }

        static MappingConfig DefaultConfig()
        {
            return new MappingConfigBuilder().Build();
        }
    }
}
