using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Nerdle.AutoConfig.Mappers
{
    class DictionaryMapper : CollectionMapper
    {
        public override object Map(XElement element, Type type)
        {
            if (!CanMap(type))
                throw new InvalidOperationException(
                    string.Format("Type '{0}' cannot be mapped by {1} because it cannot be assigned from a Dictionary<,>.", type, GetType()));
           
            var kvps = base.Map(element, type) as IEnumerable;

            var dictionaryType = DictionaryTypeFor(type);
            var dictionary = Activator.CreateInstance(dictionaryType);
            var genericArgs = dictionaryType.GetGenericArguments();
            // Use the explicit ICollection.Add implementation as it allows us to add the KeyValuePair 
            // as a single object param instead of needing to extract the Key and Value separately.
            var kvpType = typeof(KeyValuePair<,>).MakeGenericType(genericArgs[0], genericArgs[1]);
            var collectionType = typeof(ICollection<>).MakeGenericType(kvpType);
            var collectionAddMethod = collectionType.GetMethod("Add");

            foreach (var kvp in kvps)
            {
                collectionAddMethod.Invoke(dictionary, new[] { kvp });
            }

            return dictionary;
        }

        public override bool CanMap(Type type)
        {
            var dictionaryType = DictionaryTypeFor(type);

            if (dictionaryType == null)
                return false;

            return type.IsAssignableFrom(dictionaryType);
        }

        static Type DictionaryTypeFor(Type type)
        {
            Type dictionaryType;
            
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IDictionary<,>))
                dictionaryType = type;

            else dictionaryType = type.GetInterfaces()
                .Where(i => i.IsGenericType)
                .SingleOrDefault(i => i.GetGenericTypeDefinition() == typeof(IDictionary<,>));

            if (dictionaryType == null)
                return null;

            var genericArgs = dictionaryType.GetGenericArguments();
            return typeof(Dictionary<,>).MakeGenericType(genericArgs[0], genericArgs[1]);
        }
    }
}
