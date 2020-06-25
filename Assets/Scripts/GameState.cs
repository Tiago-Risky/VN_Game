using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VisualNovel;

public class GameState : MonoBehaviour {
    //Game State Objects and Variables
    private PersistentManagerScript persistent = PersistentManagerScript.Instance;
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
    private List<Text> CharacterNameText;
    private List<Image> CharacterNameBackground;
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
        CharacterNameText = new List<Text> {
            CharacterCanvas[0].transform.Find("Name/Text").GetComponent<Text>(),
            CharacterCanvas[1].transform.Find("Name/Text").GetComponent<Text>(),
            CharacterCanvas[2].transform.Find("Name/Text").GetComponent<Text>()
        };
        CharacterNameBackground = new List<Image> {
            CharacterCanvas[0].transform.Find("Name").GetComponent<Image>(),
            CharacterCanvas[1].transform.Find("Name").GetComponent<Image>(),
            CharacterCanvas[2].transform.Find("Name").GetComponent<Image>()
        };
        CharacterImages = new List<Image> {
            CharacterCanvas[0].transform.Find("Image").GetComponent<Image>(),
            CharacterCanvas[1].transform.Find("Image").GetComponent<Image>(),
            CharacterCanvas[2].transform.Find("Image").GetComponent<Image>()
        };
        DialogueText = ScreenCanvas.transform.Find("DialogueBox/Text").GetComponent<Text>();
        OptionBoxes = new Dictionary<int, GameObject> {
                    { 2, ScreenCanvas.transform.Find("2OptionsBox").gameObject },
                    { 3, ScreenCanvas.transform.Find("3OptionsBox").gameObject },
                    { 4, ScreenCanvas.transform.Find("4OptionsBox").gameObject }
                };
        DialogueLoadingIndicator = ScreenCanvas.transform.Find("DialogueBox/Indicator_Loading").gameObject;
        DialogueSkipIndicator = ScreenCanvas.transform.Find("DialogueBox/Indicator_Skip").gameObject;

        LoadDialogue(1, 1);
    }

    // Update is called once per frame
    void Update() {
        WriteUpdate();
    }

    private void LoadDialogue(int ChapterNumber, int DialogueNumber) {
        Debug.Log("LoadDialogue(" + ChapterNumber + ", " + DialogueNumber + ")");

        // Calling -1 for chapter and dialogue moves to EndScene
        if (ChapterNumber == -1 && DialogueNumber == -1) {
            SceneManager.LoadScene(sceneName: "EndScene");
            return;
        }

        // Making sure the OptionsBoxes are all disabled at the beginning of a new dialogue
        SetActiveOptionsBoxes();

        CurrentChapter = persistent.ChapterList[ChapterNumber - 1];
        CurrentDialogue = CurrentChapter.Dialogues[DialogueNumber - 1];

        SetBackground();

        LoadCharacters(CurrentDialogue.Characters);

        WriteText(CurrentDialogue.Text);

    }

    public void SetBackground() {
        LoadBackground();
        if (CurrentChapter.HasBackground()) {
            LoadBackground(CurrentChapter.Background);
        }
        if (CurrentDialogue.HasBackground()) {
            LoadBackground(CurrentDialogue.Background);
        }
    }

    public void LoadBackground(string background = "") {
        if (background != "") {
            BackgroundImage.sprite = Resources.Load<Sprite>("Backgrounds/" + background);
            BackgroundImage.color = Color.white;
        }
        else {
            BackgroundImage.color = Color.black;
        }

    }

    public void LoadCharacters(List<Character> characters) {
        foreach (GameObject characterCanvas in CharacterCanvas) {
            characterCanvas.SetActive(false);
        }
        foreach (Character character in characters) {
            int Side = character.Side;
            CharacterCanvas[character.Side].SetActive(true);
            if (character.Name.Length > 0) {
                CharacterNameText[Side].gameObject.SetActive(true);
                CharacterNameText[Side].text = character.Name;
            }
            else {
                CharacterNameText[Side].gameObject.SetActive(false);
            }

            if (character.HasPicture()) {
                CharacterImages[Side].gameObject.SetActive(true);
                CharacterImages[Side].sprite = Resources.Load<Sprite>("Characters/" + character.Picture);
            }
            else {
                CharacterImages[Side].gameObject.SetActive(false);
            }
            switch (character.GetVisibility()) {
                case 0:
                    CharacterImages[Side].color = new Color32(255, 255, 255, 200);
                    CharacterNameBackground[Side].color = new Color32(154, 154, 154, 140);
                    break;
                case 1:
                    CharacterImages[Side].color = new Color32(255, 255, 255, 255);
                    CharacterNameBackground[Side].color = new Color32(240, 240, 240, 200);
                    break;
                case 10:
                    CharacterImages[Side].color = new Color32(0, 0, 0, 200);
                    CharacterNameBackground[Side].color = new Color32(154, 154, 154, 140);
                    break;
                case 11:
                    CharacterImages[Side].color = new Color32(0, 0, 0, 255);
                    CharacterNameBackground[Side].color = new Color32(240, 240, 240, 200);
                    break;
            }
        }
    }

    public void ClickDialogue() {
        if (!Writing) {
            if (!CurrentDialogue.IsQuestion()) {
                LoadDialogue(CurrentDialogue.Redirect.Chapter, CurrentDialogue.Redirect.Dialogue);
            }
        }
        else {
            WriteAll();
        }
    }

    public void SetOptions() {
        int NumberOfOptions = CurrentDialogue.Options.Count;
        for (int x = 0; x < CurrentDialogue.Options.Count; x++) {
            GameObject OptionButton = OptionBoxes[NumberOfOptions].transform.GetChild(x).gameObject;
            Text TextField = OptionButton.transform.GetChild(0).GetComponent<Text>();
            TextField.text = CurrentDialogue.Options[x].Text;
        }
        SetActiveOptionsBoxes(NumberOfOptions);
    }

    // Each button should have an option number assigned, starting from 0
    public void ClickOption(int number) {
        LoadDialogue(CurrentDialogue.Options[number].Redirect.Chapter,
                     CurrentDialogue.Options[number].Redirect.Dialogue);
    }

    private void SetActiveOptionsBoxes(int NumberOfOptions = 0) {
        foreach (KeyValuePair<int, GameObject> optionsBox in OptionBoxes) {
            optionsBox.Value.SetActive(false);
        }

        if (OptionBoxes.ContainsKey(NumberOfOptions)) {
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
