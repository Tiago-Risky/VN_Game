using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VisualNovel;

public class LoadDialog : MonoBehaviour {

    public Text statusText;
    private bool Ready = false;
    private float timeToNextScene = 3f;

    void Start() {
        Debug.Log("Loading XML File");
        statusText.text = "Loading game files";

        XElement file = XElement.Load("Assets/Content/GameDialog1.xml", LoadOptions.None);
        List<XElement> chapters = file.Elements("Chapter").ToList();

        foreach (XElement chapter in chapters) {
            Chapter Chapter = new Chapter();
            int ChapterNumber = int.Parse(chapter.Attribute("number").Value);
            foreach (XElement dialogue in chapter.Elements("Dialogue").ToList()) {
                int dialogueNumber = int.Parse(dialogue.Attribute("number").Value);
                string dialogueCharacter = dialogue.Element("character").Value;
                string dialogueText = dialogue.Element("text").Value;

                Redirect dialogueRedirect = null;
                if (dialogue.Element("redirect") != null) {
                    dialogueRedirect = new Redirect(int.Parse(dialogue.Element("redirect").Attribute("chapter").Value),
                                            int.Parse(dialogue.Element("redirect").Attribute("dialogue").Value));
                }

                Question dialogueQuestion = null;
                if (dialogue.Element("options") != null) {
                    dialogueQuestion = new Question();
                    foreach (XElement option in dialogue.Element("options").Elements("option").ToList()) {

                        Redirect optionRedir = new Redirect(int.Parse(option.Element("redirect").Attribute("chapter").Value),
                                            int.Parse(option.Element("redirect").Attribute("dialogue").Value));
                        dialogueQuestion.Options.Add(new Option(option.Element("option_text").Value, optionRedir));
                    }
                }

                Dialogue Dialogue = new Dialogue(dialogueCharacter, dialogueText, dialogueRedirect, dialogueQuestion);

                Chapter.Dialogues.Add(dialogueNumber, Dialogue);
            }

            PersistentManagerScript.Instance.ChapterList.Add(ChapterNumber, Chapter);
        }

        Debug.Log("Loading XML File Done");
        statusText.text = "Game files loaded";
        Ready = true; // This will signal to start counting the timeToNextScene down.
    }

    void Update() {
        if (Ready) {
            timeToNextScene -= Time.deltaTime;
            if (timeToNextScene <= 0) {
                Ready = false;
                SceneManager.LoadScene(sceneName: "GameScene"); //Changing to the GameScene once the timer is done
            }
        }

    }
}
