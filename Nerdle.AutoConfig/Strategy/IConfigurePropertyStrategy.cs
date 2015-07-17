using Nerdle.AutoConfig.Mappers;

namespace Nerdle.AutoConfig.Strategy
{
    public interface IConfigurePropertyStrategy<in T>
    {
        IConfigurePropertyStrategy<T> From(string elementOrAttributeName);
        IConfigurePropertyStrategy<T> Optional();
        IConfigurePropertyStrategy<T> OptionalWithDefault(T t);
        IConfigurePropertyStrategy<T> Using<TMapper>() where TMapper : IMapper, new();
        IConfigurePropertyStrategy<T> Using(IMapper mapper);
    }
}