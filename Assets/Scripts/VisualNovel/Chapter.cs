using System.Collections.Generic;
using System.Xml.Linq;

namespace VisualNovel {
    public class Chapter {
        public int Number;
        public string Background;
        private List<Dialogue> dialogues;
        public List<Dialogue> Dialogues {
            get { return dialogues ?? (dialogues = new List<Dialogue>()); }
        }

        public Chapter(int number, string background) {
            Number = number;
            Background = background;
        }
        public bool HasBackground() {
            return Background.Length > 0;
        }

        // For the dialogue editor
        public XElement ExportXML() {
            XElement xElement = new XElement("Chapter", new XAttribute("Number", Number));

            if (HasBackground()) {
                xElement.Add(new XAttribute("Background", Background));
            }

            foreach (Dialogue dialogue in Dialogues) {
                xElement.Add(dialogue.ExportXML());
            }

            return xElement;
        }
    }
}