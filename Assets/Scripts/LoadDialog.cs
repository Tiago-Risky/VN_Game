using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadDialog : MonoBehaviour {

    void Start() {
        Debug.Log("Loading XML File");

        XElement file = XElement.Load("Assets/Content/GameDialog1.xml", LoadOptions.None);
        List<XElement> chapters = file.Elements("Chapter").ToList();

        foreach (XElement chapter in chapters) {
            VNChapter vnChapter = new VNChapter();
            int vnChapterNumber = int.Parse(chapter.Attribute("number").Value);
            foreach (XElement dialogue in chapter.Elements("Dialogue").ToList()) {
                int dialogueNumber = int.Parse(dialogue.Attribute("number").Value);
                string dialogueCharacter = dialogue.Element("character").Value;
                string dialogueText = dialogue.Element("text").Value;

                VNRedirect dialogueRedirect = null;
                if (dialogue.Element("redirect") != null) {
                    dialogueRedirect = new VNRedirect(int.Parse(dialogue.Element("redirect").Attribute("chapter").Value),
                                            int.Parse(dialogue.Element("redirect").Attribute("dialogue").Value));
                }

                VNQuestion dialogueQuestion = null;
                if (dialogue.Element("options") != null) {
                    dialogueQuestion = new VNQuestion();
                    foreach (XElement option in dialogue.Element("options").Elements("option").ToList()) {

                        VNRedirect optionRedir = new VNRedirect(int.Parse(option.Element("redirect").Attribute("chapter").Value),
                                            int.Parse(option.Element("redirect").Attribute("dialogue").Value));
                        dialogueQuestion.Options.Add(new VNOption(option.Element("option_text").Value, optionRedir));
                    }
                }

                VNDialogue vnDialogue = new VNDialogue(dialogueCharacter, dialogueText, dialogueRedirect, dialogueQuestion);

                vnChapter.Dialogues.Add(dialogueNumber, vnDialogue);
            }

            PersistentManagerScript.Instance.ChapterList.Add(vnChapterNumber, vnChapter);
        }

        Debug.Log("Loading XML File Done");
        SceneManager.LoadScene(sceneName: "GameScene"); //Changing to the GameChapter once the loading is done

    }

    void Update() {

    }
}
