using System.Collections.Generic;

namespace VisualNovel {
    public class Dialogue {
        public List<Character> Characters;
        public string Text;
        public Redirect Redirect;
        public Question Question;
        public string Background;

        public Dialogue(List<Character> characters, string text, Redirect redirect, Question question, string background) {
            Characters = characters;
            Text = text;
            Redirect = redirect;
            Question = question;
            Background = background;
        }

        public bool IsQuestion() {
            return Question != null;
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
    }
}
