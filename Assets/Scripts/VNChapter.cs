using System.Collections.Generic;

namespace VisualNovel {
    public class VNChapter {
        private Dictionary<int, VNDialogue> dialogues;
        public Dictionary<int, VNDialogue> Dialogues {
            get { return dialogues ?? (dialogues = new Dictionary<int, VNDialogue>()); }
        }

        public VNChapter() {

        }
    }
}