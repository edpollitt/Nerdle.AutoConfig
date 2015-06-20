using System.Configuration;
using System.Xml;
using System.Xml.Linq;

namespace Nerdle.AutoConfig
{
    public class Section : ConfigurationSection
    {
        XElement _xElement;

        protected override void DeserializeSection(XmlReader reader)
        {
            _xElement = XElement.Load(reader);
        }

        public static implicit operator XElement(Section section)
        {
            return section._xElement;
        }
    }
}