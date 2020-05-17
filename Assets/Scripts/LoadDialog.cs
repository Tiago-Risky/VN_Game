using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VisualNovel;

public class LoadDialog : MonoBehaviour {

    public TextAsset XMLFile;
    public Text StatusTextbox;
    private bool Ready = false;
    public float TimeToNextScene = 3f;

    void Start() {
        Debug.Log("Loading XML File");
        StatusTextbox.text = "Loading game files";

        XElement file = XElement.Parse(XMLFile.text);
        List<XElement> chapters = file.Elements("Chapter").ToList();

        foreach (XElement chapter in chapters) {
            int ChapterNumber = int.Parse(chapter.Attribute("number").Value);
            string ChapterBackground = (chapter.Attribute("background") != null) ? chapter.Attribute("background").Value : "";
            Chapter Chapter = new Chapter(ChapterBackground);

            foreach (XElement dialogue in chapter.Elements("Dialogue").ToList()) {
                int dialogueNumber = int.Parse(dialogue.Attribute("number").Value);
                string dialogueText = dialogue.Element("text").Value;
                string DialogueBackground = (dialogue.Attribute("background") != null) ? dialogue.Attribute("background").Value : "";

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
                List<Character> DialogueCharacters = new List<Character>();
                if (dialogue.Element("characters") != null) {
                    foreach (XElement character in dialogue.Element("characters").Elements("character").ToList()) {
                        // Defaults: Name = "" ; Picture = "" ; Side = 0 (Left); Selected = true; Hidden = false
                        string CharacterName = (character.Attribute("name") != null) ? character.Attribute("name").Value : "";
                        string CharacterPicture = (character.Attribute("picture") != null) ? character.Attribute("picture").Value : "";
                        int CharacterSide = (character.Attribute("side") != null) ? int.Parse(character.Attribute("side").Value) : 0;
                        bool CharacterSelected = (character.Attribute("selected") != null) ? bool.Parse(character.Attribute("selected").Value) : true;
                        bool CharacterHidden = (character.Attribute("hidden") != null) ? bool.Parse(character.Attribute("hidden").Value) : false;
                        DialogueCharacters.Add(new Character(CharacterName,CharacterPicture,CharacterSide,CharacterSelected,CharacterHidden));
                    }
                }

                Dialogue Dialogue = new Dialogue(DialogueCharacters, dialogueText, dialogueRedirect, dialogueQuestion, DialogueBackground);

                Chapter.Dialogues.Add(dialogueNumber, Dialogue);
            }

            PersistentManagerScript.Instance.ChapterList.Add(ChapterNumber, Chapter);
        }

        Debug.Log("Loading XML File Done");
        StatusTextbox.text = "Game files loaded";
        Ready = true; // This will signal to start counting the TimeToNextScene down.
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
