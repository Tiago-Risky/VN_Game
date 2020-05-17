using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VisualNovel;

public class GameState : MonoBehaviour {
    //Game State Objects and Variables
    private PersistentManagerScript persistent = PersistentManagerScript.Instance;
    private int CurrentChapterNumber = 1;
    private int CurrentDialogueNumber = 1;
    private Chapter CurrentChapter;
    private Dialogue CurrentDialogue;

    //UI Objects
    private GameObject ScreenCanvas;
    private Image BackgroundImage;
    private Text DialogueText;
    private Dictionary<int, GameObject> OptionBoxes;
    private GameObject DialogueLoadingIndicator;
    private GameObject DialogueSkipIndicator;
    private List<GameObject> CharacterCanvas;
    private List<Text> CharacterNames;
    private List<Image> CharacterImages;

    // Initialization
    void Start() {
        ScreenCanvas = GameObject.Find("ScreenCanvas").gameObject;
        BackgroundImage = ScreenCanvas.GetComponent<Image>();
        CharacterCanvas = new List<GameObject> {
            ScreenCanvas.transform.Find("Character0").gameObject,
            ScreenCanvas.transform.Find("Character1").gameObject,
            ScreenCanvas.transform.Find("Character2").gameObject
        };
        CharacterNames = new List<Text> {
            ScreenCanvas.transform.Find("Character0/Name/Text").GetComponent<Text>(),
            ScreenCanvas.transform.Find("Character1/Name/Text").GetComponent<Text>(),
            ScreenCanvas.transform.Find("Character2/Name/Text").GetComponent<Text>()
        };
        CharacterImages = new List<Image> {
            ScreenCanvas.transform.Find("Character0/Image").GetComponent<Image>(),
            ScreenCanvas.transform.Find("Character1/Image").GetComponent<Image>(),
            ScreenCanvas.transform.Find("Character2/Image").GetComponent<Image>()
        };
        DialogueText = ScreenCanvas.transform.Find("DialogueBox/Text").GetComponent<Text>();
        OptionBoxes = new Dictionary<int, GameObject> {
                    { 2, ScreenCanvas.transform.Find("2OptionsBox").gameObject },
                    { 3, ScreenCanvas.transform.Find("3OptionsBox").gameObject },
                    { 4, ScreenCanvas.transform.Find("4OptionsBox").gameObject }
                };
        DialogueLoadingIndicator = ScreenCanvas.transform.Find("DialogueBox/Indicator_Loading").gameObject;
        DialogueSkipIndicator = ScreenCanvas.transform.Find("DialogueBox/Indicator_Skip").gameObject;

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

        if (CurrentChapter.HasBackground()) {
            loadBackground(CurrentChapter.Background);
        }
        if (CurrentDialogue.HasBackground()) {
            loadBackground(CurrentDialogue.Background);
        }
        if (!CurrentDialogue.HasBackground() && !CurrentChapter.HasBackground()) {
            loadBackground();
        }

        loadCharacters(CurrentDialogue.Characters);

        WriteText(CurrentDialogue.Text);

    }

    public void loadBackground(string background="") {
        if (background != "") {
            BackgroundImage.sprite = Resources.Load<Sprite>("Backgrounds/" + background);
            BackgroundImage.color = Color.white;
        }
        else {
            BackgroundImage.color = Color.black;
        }
        
    }



    public void loadCharacters(List<Character> characters) {
        foreach(GameObject characterCanvas in CharacterCanvas) {
            characterCanvas.SetActive(false);
        }
        foreach (Character character in characters) {
            CharacterCanvas[character.Side].SetActive(true);
            if (character.Name.Length > 0) {
                CharacterNames[character.Side].gameObject.SetActive(true);
                CharacterNames[character.Side].text = character.Name;
            }
            else {
                CharacterNames[character.Side].gameObject.SetActive(false);
            }

            if (character.HasPicture()) {
                CharacterImages[character.Side].gameObject.SetActive(true);
                CharacterImages[character.Side].sprite = Resources.Load<Sprite>("Characters/" + character.Picture);
            }
            else {
                CharacterImages[character.Side].gameObject.SetActive(false);
            }
        }
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
            GameObject OptionButton = OptionBoxes[NumberOfOptions].transform.GetChild(x).gameObject;
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
        foreach (KeyValuePair<int, GameObject> optionsBox in OptionBoxes) {
            optionsBox.Value.SetActive(false);
        }

        if (!OptionBoxes.ContainsKey(NumberOfOptions)) {
            return;
        }
        else {
            OptionBoxes[NumberOfOptions].SetActive(true);
        }
    }

    // Text writing stuff
    private bool Writing = false;
    private string TextToWrite = "";
    private int TextToWriteIndex = 0;
    private float WriteTimer = 0f;
    public float TimePerCharacter = .04f;


    private void WriteText(string textToWrite) {
        WriteTimer = 0f;
        TextToWrite = textToWrite;
        TextToWriteIndex = 0;
        Writing = true;
    }

    private void WriteAll() {
        Writing = false;
        DialogueText.text = TextToWrite;
    }

    private void WriteUpdate() {
        if (Writing) {
            SetWriteIndicator(1);
            WriteTimer += Time.deltaTime;
            if (WriteTimer > TimePerCharacter) {
                int charactersToWrite = (int)(WriteTimer / TimePerCharacter);
                WriteTimer -= (charactersToWrite * TimePerCharacter);
                TextToWriteIndex += charactersToWrite;

                if (TextToWriteIndex >= TextToWrite.Length) {
                    Writing = false;
                    DialogueText.text = TextToWrite;
                    WriteUpdate(); // Call itself to call SetWriteIndicator appropriately
                }
                else {
                    string formattedText = TextToWrite.Substring(0, TextToWriteIndex);
                    formattedText += "<color=#00000000>" + TextToWrite.Substring(TextToWriteIndex) + "</color>";
                    DialogueText.text = formattedText;
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
