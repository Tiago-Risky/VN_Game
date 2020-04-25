using System.Collections.Generic;
using UnityEngine;
using VisualNovel;

public class PersistentManagerScript : MonoBehaviour {

    public static PersistentManagerScript Instance {
        get;
        private set;
    }

    public Dictionary<int, Chapter> ChapterList {
        get { return chapterList ?? (chapterList = new Dictionary<int, Chapter>()); }
    }
    private Dictionary<int, Chapter> chapterList;

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
