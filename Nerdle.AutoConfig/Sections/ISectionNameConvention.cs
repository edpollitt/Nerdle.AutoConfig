using System.Collections.Generic;

namespace Nerdle.AutoConfig.Sections
{
    interface ISectionNameConvention
    {
        IEnumerable<string> GetAlternativeNames(string name);
    }
}