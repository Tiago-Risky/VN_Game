using System.Collections.Generic;

namespace VisualNovel {
    public class Chapter {
        private Dictionary<int, Dialogue> dialogues;
        public Dictionary<int, Dialogue> Dialogues {
            get { return dialogues ?? (dialogues = new Dictionary<int, Dialogue>()); }
        }

        public Chapter() {

        }
    }
}