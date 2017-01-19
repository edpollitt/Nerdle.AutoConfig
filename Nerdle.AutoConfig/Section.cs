using System.Configuration;
using System.Xml;
using System.Xml.Linq;
using Nerdle.AutoConfig.Exceptions;

namespace Nerdle.AutoConfig
{
    public class Section : ConfigurationSection
    {
        protected XElement XElement;

        protected override void DeserializeSection(XmlReader reader)
        {
            XElement = XElement.Load(reader);
        }

        public static explicit operator XElement(Section section)
        {
            if (section.XElement == null)
                throw new AutoConfigMappingException($"No XML element located for config section '{section.SectionInformation.Name}'.");

            return section.XElement;
        }
    }
}