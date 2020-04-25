namespace VisualNovel {
    public class Dialogue {
        public string Character = "";
        public string Text = "";
        public Redirect Redirect;
        public Question Question;

        public Dialogue(string character, string text, Redirect redirect, Question question) {
            Character = character;
            Text = text;
            Redirect = redirect;
            Question = question;
        }

        public bool IsQuestion() {
            return Question != null;
        }

        public bool HasRedirect() {
            return Redirect != null;
        }
    }
}
