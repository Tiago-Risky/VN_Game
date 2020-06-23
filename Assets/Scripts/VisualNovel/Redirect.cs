using System.Xml.Linq;

namespace VisualNovel {
    public class Redirect {
        public int Chapter;
        public int Dialogue;

        public Redirect(int chapter, int dialogue) {
            Chapter = chapter;
            Dialogue = dialogue;
        }

        public Redirect(XElement xElement) {
            Chapter = int.Parse(xElement.Attribute("Chapter").Value);
            Dialogue = int.Parse(xElement.Attribute("Dialogue").Value);
        }

        // For the dialogue editor
        public XElement ExportXML() {
            return new XElement("Redirect", new XAttribute("Chapter", Chapter), new XAttribute("Dialogue", Dialogue));
        }
    }
}