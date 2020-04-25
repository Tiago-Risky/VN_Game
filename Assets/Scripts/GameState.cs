using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

        VNChapter vnChapter = persistent.ChapterList[chapter];
        VNDialogue vnDialogue = vnChapter.Dialogues[dialogue];

        dialogueBoxText.text = vnDialogue.Text;
        characterNameBoxText.text = vnDialogue.Character;
        if (vnDialogue.IsQuestion()) {
            QuestionBox.SetActive(true);
            optionAText.text = vnDialogue.Question.Options[0].Text;
            optionBText.text = vnDialogue.Question.Options[1].Text;
        }
        else {
            QuestionBox.SetActive(false);
        }
        currentDialogue = dialogue;
        currentChapter = chapter;
    }

    public void clickDialogue() {
        VNDialogue vnDialogue = persistent.ChapterList[currentChapter].Dialogues[currentDialogue];
        if (!vnDialogue.IsQuestion()) {
            if (vnDialogue.HasRedirect()) {
                loadDialogue(vnDialogue.Redirect.Chapter, vnDialogue.Redirect.Dialogue);
            }
            else {
                loadDialogue(currentChapter, ++currentDialogue);
            }
        }
    }

    public void clickOptionA() {
        VNDialogue vnDialogue = persistent.ChapterList[currentChapter].Dialogues[currentDialogue];
        loadDialogue(vnDialogue.Question.Options[0].Redirect.Chapter, vnDialogue.Question.Options[0].Redirect.Dialogue);
    }

    public void clickOptionB() {
        VNDialogue vnDialogue = persistent.ChapterList[currentChapter].Dialogues[currentDialogue];
        loadDialogue(vnDialogue.Question.Options[1].Redirect.Chapter, vnDialogue.Question.Options[1].Redirect.Dialogue);
    }
}
