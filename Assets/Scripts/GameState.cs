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
    public GameObject DialogueLoadingIndicator;
    public GameObject DialogueSkipIndicator;

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
        WriteUpdate();
    }

    private void loadDialogue(int ChapterNumber, int DialogueNumber) {
        Debug.Log("loadDialogue called with " + ChapterNumber + " and " + DialogueNumber);

        // Making sure the OptionsBoxes are all disabled at the beginning of a new dialogue
        setActiveOptionsBoxes();

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

        characterNameBoxText.text = CurrentDialogue.Character;
        WriteText(CurrentDialogue.Text);

    }

    public void clickDialogue() {
        if (!Writing) {
            if (!CurrentDialogue.IsQuestion()) {
                if (CurrentDialogue.HasRedirect()) {
                    loadDialogue(CurrentDialogue.Redirect.Chapter, CurrentDialogue.Redirect.Dialogue);
                }
                else {
                    loadDialogue(CurrentChapterNumber, ++CurrentDialogueNumber);
                }
            }
        }
        else {
            WriteAll();
        }
    }

    public void SetOptions() {
        int NumberOfOptions = CurrentDialogue.Question.Options.Count;
        for (int x = 0; x < CurrentDialogue.Question.Options.Count; x++) {
            GameObject OptionButton = OptionsBoxes[NumberOfOptions].transform.GetChild(x).gameObject;
            Text TextField = OptionButton.transform.GetChild(0).GetComponent<Text>();
            TextField.text = CurrentDialogue.Question.Options[x].Text;
        }
        setActiveOptionsBoxes(NumberOfOptions);
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

    // Text writing stuff
    private bool Writing = false;
    private string TextToWrite = "";
    private int TextToWriteIndex = 0;
    private float WriteTimer = 0f;
    public float TimePerCharacter = .1f;


    private void WriteText(string textToWrite) {
        WriteTimer = 0f;
        TextToWrite = textToWrite;
        TextToWriteIndex = 0;
        Writing = true;
    }

    private void WriteAll() {
        Writing = false;
        dialogueBoxText.text = TextToWrite;
    }

    private void WriteUpdate() {
        if (Writing) {
            SetWriteIndicator(1);
            WriteTimer += Time.deltaTime;
            if (WriteTimer > TimePerCharacter) {
                int charactersToWrite = (int)(WriteTimer / TimePerCharacter);
                WriteTimer -= (charactersToWrite*TimePerCharacter);
                TextToWriteIndex += charactersToWrite;

                if (TextToWriteIndex >= TextToWrite.Length) {
                    Writing = false;
                    dialogueBoxText.text = TextToWrite;
                    WriteUpdate(); // Call itself to call SetWriteIndicator appropriately
                }
                else {
                    string formattedText = TextToWrite.Substring(0, TextToWriteIndex);
                    formattedText += "<color=#00000000>" + TextToWrite.Substring(TextToWriteIndex) + "</color>";
                    dialogueBoxText.text = formattedText;
                }
            }
        }
        else {
            if (!CurrentDialogue.IsQuestion()) {
                SetWriteIndicator(0);
            }
            else {
                SetWriteIndicator();
                SetOptions();
            }
        }
    }

    private void SetWriteIndicator(int status = -1) {
        // Anything: All off; 0: Skip indicator; 1: Writing indicator
        switch (status) {
            case 0:
                DialogueSkipIndicator.SetActive(true);
                DialogueLoadingIndicator.SetActive(false);
                break;
            case 1:
                DialogueSkipIndicator.SetActive(false);
                DialogueLoadingIndicator.SetActive(true);
                break;
            default:
                DialogueSkipIndicator.SetActive(false);
                DialogueLoadingIndicator.SetActive(false);
                break;
        }
    }
}
