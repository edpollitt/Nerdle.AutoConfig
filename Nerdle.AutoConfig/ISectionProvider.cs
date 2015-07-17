namespace Nerdle.AutoConfig
{
    interface ISectionProvider
    {
        Section GetSection(string sectionName);
    }
}