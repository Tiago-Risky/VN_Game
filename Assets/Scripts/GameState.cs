using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VisualNovel;

public class GameState : MonoBehaviour {

    private PersistentManagerScript persistent = PersistentManagerScript.Instance;

    public Text dialogueBoxText;
    public Text characterNameBoxText;
    public Text optionAText;
    public Text optionBText;
    public GameObject QuestionBox;

    private int currentChapter = 1;
    private int currentDialogue = 1;

    // Use this for initialization
    void Start() {
        loadDialogue(1, 1);
    }

    // Update is called once per frame
    void Update() {

    }

    void loadDialogue(int chapter, int dialogue) {
        Debug.Log("loadDialogue called with " + chapter + " and " + dialogue);
        if (chapter == -1 && dialogue == -1) {
            SceneManager.LoadScene(sceneName: "EndScene");
            return;
        }

        /* These two calls require the XML file to be strictly following the patterns.
		 * We can either enforce the patterns (and check them) or we can remove this,
		 * enforcing new patterns (every chapter jump has to has a Redirection, and every
		 * ending has to call chapter -1 and dialogue -1).
		 */

        if (persistent.ChapterList[chapter].Dialogues.Count < dialogue) {
            chapter++;
            currentChapter = chapter;
            dialogue = 1;
            currentDialogue = dialogue;
        }

        if (persistent.ChapterList.Count < chapter) {
            SceneManager.LoadScene(sceneName: "EndScene");
            return;
        }

        Chapter Chapter = persistent.ChapterList[chapter];
        Dialogue Dialogue = Chapter.Dialogues[dialogue];

        dialogueBoxText.text = Dialogue.Text;
        characterNameBoxText.text = Dialogue.Character;
        if (Dialogue.IsQuestion()) {
            QuestionBox.SetActive(true);
            optionAText.text = Dialogue.Question.Options[0].Text;
            optionBText.text = Dialogue.Question.Options[1].Text;
        }
        else {
            QuestionBox.SetActive(false);
        }
        currentDialogue = dialogue;
        currentChapter = chapter;
    }

    public void clickDialogue() {
        Dialogue Dialogue = persistent.ChapterList[currentChapter].Dialogues[currentDialogue];
        if (!Dialogue.IsQuestion()) {
            if (Dialogue.HasRedirect()) {
                loadDialogue(Dialogue.Redirect.Chapter, Dialogue.Redirect.Dialogue);
            }
            else {
                loadDialogue(currentChapter, ++currentDialogue);
            }
        }
    }

    public void clickOptionA() {
        Dialogue Dialogue = persistent.ChapterList[currentChapter].Dialogues[currentDialogue];
        loadDialogue(Dialogue.Question.Options[0].Redirect.Chapter, Dialogue.Question.Options[0].Redirect.Dialogue);
    }

    public void clickOptionB() {
        Dialogue Dialogue = persistent.ChapterList[currentChapter].Dialogues[currentDialogue];
        loadDialogue(Dialogue.Question.Options[1].Redirect.Chapter, Dialogue.Question.Options[1].Redirect.Dialogue);
    }
}
