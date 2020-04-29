namespace VisualNovel {
    public class Dialogue {
        public string Character = "";
        public string Text = "";
        public Redirect Redirect;
        public Question Question;
        public string Background;

        public Dialogue(string character, string text, Redirect redirect, Question question, string background) {
            Character = character;
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
    }
}
