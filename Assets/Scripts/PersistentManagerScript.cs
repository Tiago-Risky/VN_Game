using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentManagerScript : MonoBehaviour {

    public static PersistentManagerScript Instance {
        get;
        private set;
    }

    public int Value = 0;

    public List<VNScene> vnScenes;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            //Initializing here so we don't have to do it later
            vnScenes = new List<VNScene>();


            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }
    }
}
