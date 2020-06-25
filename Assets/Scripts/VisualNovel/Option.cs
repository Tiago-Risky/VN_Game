using System.Xml.Linq;

namespace VisualNovel {
    public class Option {
        public string Text;
        public Redirect Redirect;
        public PointGate PointGate;

        public Option(string text, Redirect redirect, PointGate pointGate) {
            Text = text;
            Redirect = redirect;
            PointGate = pointGate;
        }

        public Option(XElement xElement) {
            Text = xElement.Attribute("Text").Value;
            Redirect = new Redirect(xElement.Element("Redirect"));
            PointGate = (xElement.Element("PointGate") != null) ? new PointGate(xElement.Element("PointGate")) : null;
        }

        public bool HasPointGate() {
            return PointGate != null;
        }

        // For the dialogue editor
        public XElement ExportXML() {
            XElement xElement = new XElement("Option", new XAttribute("Text", Text));

            if (HasPointGate()) {
                xElement.Add(PointGate.ExportXML());
            }
            else {
                xElement.Add(Redirect.ExportXML());
            }

            return xElement;
        }
    }
}