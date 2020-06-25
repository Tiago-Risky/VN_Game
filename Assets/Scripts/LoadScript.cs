using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VisualNovel;

public class LoadScript : MonoBehaviour {

    public TextAsset PointsXML;
    public TextAsset DialogueXML;
    public Text StatusTextbox;
    private bool Ready = false;
    public float TimeToNextScene = 3f;

    void Start() {
        StatusTextbox.text = "Loading game files";

        Debug.Log("Loading PointsXML File");
        XElement pointsFile = XElement.Parse(PointsXML.text);
        PersistentManagerScript.Instance.PointsList = LoadPoints(pointsFile);
        Debug.Log("Loading PointsXML File Done");

        Debug.Log("Loading DialogueXML File");
        XElement dialogueFile = XElement.Parse(DialogueXML.text);
        PersistentManagerScript.Instance.ChapterList = LoadDialogue(dialogueFile);
        Debug.Log("Loading DialogueXML File Done");

        StatusTextbox.text = "Game files loaded";
        Ready = true; // This will signal to start counting the TimeToNextScene down.
    }

    public Dictionary<string, Point> LoadPoints(XElement file) {
        Dictionary<string, Point> LoadedPoints = new Dictionary<string, Point>();
        List<XElement> Points = file.Elements("Point").ToList();

        /* I'm repeating the Name as key to the Dictionary to make it
         * easier to search through than a List
         */
        foreach (XElement point in Points) {
            LoadedPoints.Add(point.Attribute("Name").Value, new Point(point));
        }

        return LoadedPoints;
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
