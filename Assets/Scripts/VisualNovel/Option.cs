using System.Xml.Linq;

namespace VisualNovel {
    public class Option {
        public Redirect Redirect;
        public string Text;

        public Option(string text, Redirect redirect) {
            Text = text;
            Redirect = redirect;
        }

        // For the dialogue editor
        public XElement ExportXML() {
            return new XElement("Option", new XAttribute("Text", Text), Redirect.ExportXML());
        }
    }
}