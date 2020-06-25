using System.Collections.Generic;
using UnityEngine;
using VisualNovel;

public class PersistentManagerScript : MonoBehaviour {

    public static PersistentManagerScript Instance {
        get;
        private set;
    }

    public Dictionary<string, Point> PointsList;
    public List<Chapter> ChapterList;

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
