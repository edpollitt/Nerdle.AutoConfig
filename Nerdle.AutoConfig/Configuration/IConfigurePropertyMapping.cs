using Nerdle.AutoConfig.Mappers;

namespace Nerdle.AutoConfig.Configuration
{
    public interface IConfigurePropertyMapping<in T>
    {
        IConfigurePropertyMapping<T> From(string elementOrAttributeName);
        IConfigurePropertyMapping<T> Optional();
        IConfigurePropertyMapping<T> OptionalWithDefault(T t);
        IConfigurePropertyMapping<T> Using<TMapper>() where TMapper : IMapper, new();
        IConfigurePropertyMapping<T> Using(IMapper mapper);
    }
}