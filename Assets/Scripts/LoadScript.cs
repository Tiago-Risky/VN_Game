using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VisualNovel;

public class LoadScript : MonoBehaviour {

    public TextAsset XMLFile;
    public Text StatusTextbox;
    private bool Ready = false;
    public float TimeToNextScene = 3f;

    void Start() {
        Debug.Log("Loading XML File");
        StatusTextbox.text = "Loading game files";

        XElement file = XElement.Parse(XMLFile.text);
        PersistentManagerScript.Instance.ChapterList = LoadDialogue(file);

        Debug.Log("Loading XML File Done");
        StatusTextbox.text = "Game files loaded";
        Ready = true; // This will signal to start counting the TimeToNextScene down.
    }

    public List<Chapter> LoadDialogue(XElement file) {
        List<Chapter> LoadedChapters = new List<Chapter>();
        List<XElement> Chapters = file.Elements("Chapter").ToList();

        foreach (XElement chapter in Chapters) {
            LoadedChapters.Add(new Chapter(chapter));
        }
        return LoadedChapters;
    }

    void Update() {
        if (Ready) {
            TimeToNextScene -= Time.deltaTime;
            if (TimeToNextScene <= 0) {
                Ready = false;
                SceneManager.LoadScene(sceneName: "GameScene"); //Changing to the GameScene once the timer is done
            }
        }

    }
}
