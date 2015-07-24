using System.Configuration;
using System.Xml;
using System.Xml.Linq;

namespace Nerdle.AutoConfig
{
    public class Section : ConfigurationSection
    {
        protected XElement XElement;

        protected override void DeserializeSection(XmlReader reader)
        {
            XElement = XElement.Load(reader);
        }

        public static implicit operator XElement(Section section)
        {
            return section.XElement;
        }
    }

    interface ISectionProvider
    {
        Section GetSection(string sectionName);
    }

    class SectionProvider : ISectionProvider
    {
        public Section GetSection(string sectionName)
        {
            return ConfigurationManager.GetSection(sectionName) as Section;
        }
    }
}