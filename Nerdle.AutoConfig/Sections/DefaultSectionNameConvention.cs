using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Nerdle.AutoConfig.Sections
{
    class DefaultSectionNameConvention : ISectionNameConvention
    {
        public IEnumerable<string> GetAlternativeNames(string name)
        {
            var alternativeName = Regex.Replace(name, "config(uration)?$", string.Empty, RegexOptions.IgnoreCase);

            if (!string.IsNullOrWhiteSpace(alternativeName) && alternativeName != name)
                yield return alternativeName;
        }
    }
}