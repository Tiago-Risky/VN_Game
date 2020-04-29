using System.Collections.Generic;

namespace VisualNovel {
    public class Chapter {
        public string Background;
        private Dictionary<int, Dialogue> dialogues;
        public Dictionary<int, Dialogue> Dialogues {
            get { return dialogues ?? (dialogues = new Dictionary<int, Dialogue>()); }
        }

        public Chapter(string background) {
            Background = background;
        }
        public bool HasBackground() {
            return Background.Length > 0;
        }
    }
}