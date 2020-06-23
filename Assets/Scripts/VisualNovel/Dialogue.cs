using System.Collections.Generic;
using System.Xml.Linq;

namespace VisualNovel {
    public class Dialogue {
        public int Number;
        public List<Character> Characters;
        public string Text;
        public Redirect Redirect;
        public List<Option> Options;
        public string Background;

        public Dialogue(int number, List<Character> characters, string text, Redirect redirect, List<Option> options, string background) {
            Number = number;
            Characters = characters;
            Text = text;
            Redirect = redirect;
            Options = options;
            Background = background;
        }

        public bool IsQuestion() {
            return Options != null;
        }

        public bool HasRedirect() {
            return Redirect != null;
        }

        public bool HasBackground() {
            return Background.Length > 0;
        }

        public bool HasCharacters() {
            return Characters.Count > 0;
        }

        // For the dialogue editor
        public XElement ExportXML() {
            XElement xElement = new XElement("Dialogue", new XAttribute("Number", Number), new XElement("Text", Text));

            if (HasRedirect()) {
                xElement.Add(Redirect.ExportXML());
            }

            if (HasBackground()) {
                xElement.Add(new XAttribute("Background", Background));
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
