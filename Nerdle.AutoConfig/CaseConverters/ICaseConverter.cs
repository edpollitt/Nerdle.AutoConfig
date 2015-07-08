namespace Nerdle.AutoConfig.CaseConverters
{
    interface ICaseConverter
    {
        string Convert(string s);
    }

    class CamelCaseConverter : ICaseConverter
    {
        public string Convert(string s)
        {
            throw new System.NotImplementedException();
        }
    }

    class MatchingCaseConverter : ICaseConverter
    {
        public string Convert(string s)
        {
            throw new System.NotImplementedException();
        }
    }
}
