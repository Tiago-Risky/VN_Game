using System.Collections.Generic;

namespace VisualNovel {
    public class Question {
        private List<Option> vnOptions;
        public List<Option> Options {
            get { return vnOptions ?? (vnOptions = new List<Option>()); }
        }

        public Question() {

        }
    }

    public class Option {
        public Redirect Redirect;
        public string Text;

        public Option(string text, Redirect redirect) {
            Text = text;
            Redirect = redirect;
        }
    }
}