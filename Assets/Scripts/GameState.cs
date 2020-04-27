using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VisualNovel;

public class GameState : MonoBehaviour {

    private PersistentManagerScript persistent = PersistentManagerScript.Instance;

    public Text dialogueBoxText;
    public Text characterNameBoxText;
    public GameObject TwoOptionsBox;
    public GameObject ThreeOptionsBox;
    public GameObject FourOptionsBox;

    private int CurrentChapterNumber = 1;
    private int CurrentDialogueNumber = 1;
    private Chapter CurrentChapter;
    private Dialogue CurrentDialogue;
    private Dictionary<int, GameObject> optionsBoxes;
    private Dictionary<int, GameObject> OptionsBoxes {
        get {
            return optionsBoxes ?? (optionsBoxes = new Dictionary<int, GameObject> {
                    { 2, TwoOptionsBox },
                    { 3, ThreeOptionsBox },
                    { 4, FourOptionsBox }
                });
        }
    }

    // Use this for initialization
    void Start() {
        loadDialogue(1, 1);
    }

    // Update is called once per frame
    void Update() {

    }

    void loadDialogue(int ChapterNumber, int DialogueNumber) {
        Debug.Log("loadDialogue called with " + ChapterNumber + " and " + DialogueNumber);

        // Calling -1 for chapter and dialogue moves to EndScene
        if (ChapterNumber == -1 && DialogueNumber == -1) {
            SceneManager.LoadScene(sceneName: "EndScene");
            return;
        }

        // If this was the last dialogue in the chapter, move to next chapter
        if (persistent.ChapterList[ChapterNumber].Dialogues.Count < DialogueNumber) {
            ChapterNumber++;
            DialogueNumber = 1;
        }

        // If this was the last chapter, move to EndScene
        if (persistent.ChapterList.Count < ChapterNumber) {
            SceneManager.LoadScene(sceneName: "EndScene");
            return;
        }

        CurrentChapterNumber = ChapterNumber;
        CurrentDialogueNumber = DialogueNumber;
        CurrentChapter = persistent.ChapterList[ChapterNumber];
        CurrentDialogue = CurrentChapter.Dialogues[DialogueNumber];

        dialogueBoxText.text = CurrentDialogue.Text;
        characterNameBoxText.text = CurrentDialogue.Character;

        if (CurrentDialogue.IsQuestion()) {
            int NumberOfOptions = CurrentDialogue.Question.Options.Count;
            setActiveOptionsBoxes(NumberOfOptions);
            for (int x = 0; x < CurrentDialogue.Question.Options.Count; x++) {
                GameObject OptionButton = OptionsBoxes[NumberOfOptions].transform.GetChild(x).gameObject;
                Text TextField = OptionButton.transform.GetChild(0).GetComponent<Text>();
                TextField.text = CurrentDialogue.Question.Options[x].Text;
            }
        }
        else {
            setActiveOptionsBoxes();
        }

    }

    public void clickDialogue() {
        if (!CurrentDialogue.IsQuestion()) {
            if (CurrentDialogue.HasRedirect()) {
                loadDialogue(CurrentDialogue.Redirect.Chapter, CurrentDialogue.Redirect.Dialogue);
            }
            else {
                loadDialogue(CurrentChapterNumber, ++CurrentDialogueNumber);
            }
        }
    }

    // Each button should have an option number assigned, starting from 0
    public void clickOption(int number) {
        loadDialogue(CurrentDialogue.Question.Options[number].Redirect.Chapter,
                     CurrentDialogue.Question.Options[number].Redirect.Dialogue);
    }

    private void setActiveOptionsBoxes(int NumberOfOptions = 0) {
        foreach (KeyValuePair<int, GameObject> optionsBox in OptionsBoxes) {
            optionsBox.Value.SetActive(false);
        }

        if (!OptionsBoxes.ContainsKey(NumberOfOptions)) {
            return;
        }
        else {
            OptionsBoxes[NumberOfOptions].SetActive(true);
        }
    }
}
