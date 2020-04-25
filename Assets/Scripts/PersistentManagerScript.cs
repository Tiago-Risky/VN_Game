using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentManagerScript : MonoBehaviour {

    public static PersistentManagerScript Instance {
        get;
        private set;
    }

    public Dictionary<int, VNScene> SceneList
    {
        get {return sceneList ?? (sceneList = new Dictionary<int, VNScene>());}
    }
    private Dictionary<int, VNScene> sceneList;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }
    }
}
