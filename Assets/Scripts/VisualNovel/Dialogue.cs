using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace VisualNovel {
    public class Dialogue {
        public int Number;
        public string Background;
        public string Text;
        public Redirect Redirect;
        public List<Character> Characters;
        public List<Option> Options;

        public Dialogue(int number, string text, string background, Redirect redirect, List<Character> characters, List<Option> options) {
            Number = number;
            Text = text;
            Background = background;
            Redirect = redirect;
            Characters = characters;
            Options = options;
        }

        public Dialogue(XElement xElement) {
            Number = int.Parse(xElement.Attribute("Number").Value);
            Text = xElement.Element("Text").Value;
            Background = (xElement.Attribute("Background") != null) ? xElement.Attribute("Background").Value : "";
            Redirect = (xElement.Element("Redirect") != null) ? new Redirect(xElement.Element("Redirect")) : null;
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

        public bool HasRedirect() {
            return Redirect != null;
        }

        public bool HasCharacters() {
            return (Characters!=null && Characters.Count > 0);
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

            if (HasRedirect()) {
                xElement.Add(Redirect.ExportXML());
            }

            if (HasCharacters()) {
                XElement charactersElement = new XElement("Characters");
                foreach (Character character in Characters) {
                    charactersElement.Add(character.ExportXML());
                }
                xElement.Add(charactersElement);
            }

            if (IsQuestion()) {
                XElement optionsElement = new XElement("Options");
                foreach (Option option in Options) {
                    optionsElement.Add(option.ExportXML());
                }
                xElement.Add(optionsElement);
            }

            return xElement;
        }
    }
}
