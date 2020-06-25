using System.Xml.Linq;

namespace VisualNovel {
    public class Option {
        public string Text;
        public Redirect Redirect;
        public PointGate PointGate;
        public PointOperations PointOperations;

        public Option(string text, Redirect redirect, PointGate pointGate, PointOperations pointOperations) {
            Text = text;
            Redirect = redirect;
            PointGate = pointGate;
            PointOperations = pointOperations;
        }

        public Option(XElement xElement) {
            Text = xElement.Attribute("Text").Value;
            Redirect = new Redirect(xElement.Element("Redirect"));
            PointGate = (xElement.Element("PointGate") != null) ? new PointGate(xElement.Element("PointGate")) : null;
            PointOperations = (xElement.Element("PointOperations") != null) ? new PointOperations(xElement.Element("PointOperations")) : null;
        }

        public bool HasPointGate() {
            return PointGate != null;
        }

        public bool HasPointOperations() {
            return PointOperations != null;
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

            if (HasPointOperations()) {
                xElement.Add(PointOperations.ExportXML());
            }

            return xElement;
        }
    }
}