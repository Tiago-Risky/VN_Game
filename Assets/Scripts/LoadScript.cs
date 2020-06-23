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
        List<Chapter> loadedChapters = new List<Chapter>();
        List<XElement> chapters = file.Elements("Chapter").ToList();

        foreach (XElement chapter in chapters) {
            //Deprecated for now
            int ChapterNumber = int.Parse(chapter.Attribute("Number").Value);
            string ChapterBackground = (chapter.Attribute("Background") != null) ? chapter.Attribute("Background").Value : "";
            Chapter Chapter = new Chapter(ChapterNumber, ChapterBackground);

            foreach (XElement dialogue in chapter.Elements("Dialogue").ToList()) {
                int DialogueNumber = int.Parse(dialogue.Attribute("Number").Value);
                string dialogueText = dialogue.Element("Text").Value;
                string DialogueBackground = (dialogue.Attribute("Background") != null) ? dialogue.Attribute("Background").Value : "";

                Redirect dialogueRedirect = null;
                if (dialogue.Element("Redirect") != null) {
                    dialogueRedirect = new Redirect(int.Parse(dialogue.Element("Redirect").Attribute("Chapter").Value),
                                            int.Parse(dialogue.Element("Redirect").Attribute("Dialogue").Value));
                }

                List<Option> DialogueOptions = null;
                if (dialogue.Element("Options") != null) {
                    DialogueOptions = new List<Option>();
                    foreach (XElement option in dialogue.Element("Options").Elements("Option").ToList()) {

                        Redirect optionRedir = new Redirect(int.Parse(option.Element("Redirect").Attribute("Chapter").Value),
                                            int.Parse(option.Element("Redirect").Attribute("Dialogue").Value));
                        DialogueOptions.Add(new Option(option.Attribute("Text").Value, optionRedir));
                    }
                }
                List<Character> DialogueCharacters = new List<Character>();
                if (dialogue.Element("Characters") != null) {
                    foreach (XElement character in dialogue.Element("Characters").Elements("Character").ToList()) {
                        // Defaults: Name = "" ; Picture = "" ; Side = 0 (Left); Selected = true; Hidden = false
                        string CharacterName = (character.Attribute("Name") != null) ? character.Attribute("Name").Value : "";
                        string CharacterPicture = (character.Attribute("picture") != null) ? character.Attribute("Picture").Value : "";
                        int CharacterSide = (character.Attribute("Side") != null) ? int.Parse(character.Attribute("Side").Value) : 0;
                        bool CharacterSelected = (character.Attribute("Selected") != null) ? bool.Parse(character.Attribute("Selected").Value) : true;
                        bool CharacterHidden = (character.Attribute("Hidden") != null) ? bool.Parse(character.Attribute("Hidden").Value) : false;
                        DialogueCharacters.Add(new Character(CharacterName, CharacterPicture, CharacterSide, CharacterSelected, CharacterHidden));
                    }
                }

                Dialogue Dialogue = new Dialogue(DialogueNumber, DialogueCharacters, dialogueText, dialogueRedirect, DialogueOptions, DialogueBackground);

                Chapter.Dialogues.Add(Dialogue);
            }

            loadedChapters.Add(Chapter);
        }
        return loadedChapters;
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
