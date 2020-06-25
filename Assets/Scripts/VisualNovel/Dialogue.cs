using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace VisualNovel {
    public class Dialogue {
        public int Number;
        public string Background;
        public string Text;
        public Redirect Redirect;
        public PointGate PointGate;
        public PointOperations PointOperations;
        public List<Option> Options;
        public List<Character> Characters;

        public Dialogue(int number, string background, string text, Redirect redirect, PointGate pointGate, PointOperations pointOperation, List<Option> options, List<Character> characters) {
            Number = number;
            Background = background;
            Text = text;
            Redirect = redirect;
            PointGate = pointGate;
            PointOperations = pointOperation;
            Options = options;
            Characters = characters;
        }

        public Dialogue(XElement xElement) {
            Number = int.Parse(xElement.Attribute("Number").Value);
            Background = (xElement.Attribute("Background") != null) ? xElement.Attribute("Background").Value : "";
            Text = xElement.Element("Text").Value;
            Redirect = (xElement.Element("Redirect") != null) ? new Redirect(xElement.Element("Redirect")) : null;
            PointGate = (xElement.Element("PointGate") != null) ? new PointGate(xElement.Element("PointGate")) : null;
            PointOperations = (xElement.Element("PointOperations") != null) ? new PointOperations(xElement.Element("PointOperations")) : null;

            Characters = new List<Character>();
            if (xElement.Element("Characters") != null) {
                foreach (XElement character in xElement.Element("Characters").Elements("Character").ToList()) {
                    Characters.Add(new Character(character));
                }
            }

            if (xElement.Element("Options") != null) {
                Options = new List<Option>();
                foreach (XElement option in xElement.Element("Options").Elements("Option").ToList()) {
                    Options.Add(new Option(option));
                }
            }
        }

        public bool HasBackground() {
            return Background.Length > 0;
        }

        public bool HasPointGate() {
            return PointGate != null;
        }

        public bool HasPointOperations() {
            return PointOperations != null;
        }

        public bool HasCharacters() {
            return Characters != null && Characters.Count > 0;
        }

        public bool IsQuestion() {
            return Options != null;
        }

        // For the dialogue editor
        public XElement ExportXML() {
            XElement xElement = new XElement("Dialogue", new XAttribute("Number", Number), new XElement("Text", Text));

            if (HasBackground()) {
                xElement.Add(new XAttribute("Background", Background));
            }

            if (IsQuestion()) {
                XElement optionsElement = new XElement("Options");
                foreach (Option option in Options) {
                    optionsElement.Add(option.ExportXML());
                }
                xElement.Add(optionsElement);
            }
            else {
                if (HasPointGate()) {
                    xElement.Add(PointGate.ExportXML());
                }
                else {
                    xElement.Add(Redirect.ExportXML());
                }
            }

            if (HasPointOperations()) {
                xElement.Add(PointOperations.ExportXML());
            }

            if (HasCharacters()) {
                XElement charactersElement = new XElement("Characters");
                foreach (Character character in Characters) {
                    charactersElement.Add(character.ExportXML());
                }
                xElement.Add(charactersElement);
            }

            return xElement;
        }
    }
}
