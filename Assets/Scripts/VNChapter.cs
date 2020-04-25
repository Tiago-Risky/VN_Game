using System.Collections.Generic;

public class VNChapter {
    private Dictionary<int, VNDialogue> dialogues;
    public Dictionary<int, VNDialogue> Dialogues {
        get { return dialogues ?? (dialogues = new Dictionary<int, VNDialogue>()); }
    }

    public VNChapter() {

    }
}
