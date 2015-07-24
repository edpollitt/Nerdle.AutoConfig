namespace Nerdle.AutoConfig.Mapping
{
    interface ITypeMapping
    {
        void Include(IPropertyMapping propertyMapping);
        void Apply(object instance);
    }
}