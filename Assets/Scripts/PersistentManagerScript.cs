using System.Collections.Generic;
using UnityEngine;
using VisualNovel;

public class PersistentManagerScript : MonoBehaviour {

    public static PersistentManagerScript Instance {
        get;
        private set;
    }

    public Dictionary<int, VNChapter> ChapterList {
        get { return chapterList ?? (chapterList = new Dictionary<int, VNChapter>()); }
    }
    private Dictionary<int, VNChapter> chapterList;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else {
            Destroy(gameObject);
        }
    }
}
