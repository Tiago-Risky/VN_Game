using System.Xml.Linq;

namespace VisualNovel {
    public class Redirect {
        public int Chapter;
        public int Dialogue;

        public Redirect(int chapter, int dialogue) {
            Chapter = chapter;
            Dialogue = dialogue;
        }

        // For the dialogue editor
        public XElement ExportXML() {
            return new XElement("Redirect", new XAttribute("Chapter", Chapter), new XAttribute("Dialogue", Dialogue));
        }
    }
}