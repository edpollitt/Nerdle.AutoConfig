using System.Text.RegularExpressions;

namespace Nerdle.AutoConfig.CaseConverters
{
    class CamelCaseConverter : ICaseConverter
    {
        public string Convert(string s)
        {
            var match = Regex.Match(s, "^([A-Z]*)([A-Z][^A-Z]+.*)*$");

            if (!match.Success)
                return s;

            if (match.Groups[1].Value.Length == 0)
                return char.ToLowerInvariant(s[0]) + s.Substring(1);

            return match.Groups[1].Value.ToLowerInvariant() + match.Groups[2].Value;
        }
    }
}