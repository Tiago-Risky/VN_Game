using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour {

	public Text dialogueBoxText;
	public Text characterNameBoxText;
	public Text optionAText;
	public Text optionBText;
	public GameObject QuestionBox;

	private int currentScene = 1;
	private int currentDialogue = 1;

	// Use this for initialization
	void Start () {
		loadDialogue(1,1);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void loadDialogue(int scene, int dialogue) {
		Debug.Log("loadDialogue called with "+scene+" and "+dialogue);
		if (scene == -1 && dialogue == -1)
		{
			SceneManager.LoadScene(sceneName: "EndScene");
			return;
		}

		/* These two calls require the XML file to be strictly following the patterns.
		 * We can either enforce the patterns (and check them) or we can remove this,
		 * enforcing new patterns (every scene jump has to has a Redirection, and every
		 * ending has to call scene -1 and dialogue -1).
		 */

		if (PersistentManagerScript.Instance.SceneList[scene].Dialogues.Count < dialogue) {
			scene++;
			currentScene = scene;
			dialogue = 1;
			currentDialogue = dialogue;
		}

		if (PersistentManagerScript.Instance.SceneList.Count < scene) {
			SceneManager.LoadScene(sceneName: "EndScene");
			return;
		}

		VNScene vnScene = PersistentManagerScript.Instance.SceneList[scene];
		VNDialogue vnDialogue = vnScene.Dialogues[dialogue];

		dialogueBoxText.text = vnDialogue.Text;
		characterNameBoxText.text = vnDialogue.Character;
		if (vnDialogue.IsQuestion())
		{
			QuestionBox.SetActive(true);
			optionAText.text = vnDialogue.Question.Options[0].Text;
			optionBText.text = vnDialogue.Question.Options[1].Text;
		}
		else
		{
			QuestionBox.SetActive(false);
		}
		currentDialogue = dialogue;
		currentScene = scene;
	}

	public void clickDialogue()
	{
		VNDialogue vnDialogue = PersistentManagerScript.Instance.SceneList[currentScene].Dialogues[currentDialogue];
		if (!vnDialogue.IsQuestion())
		{
			if (vnDialogue.HasRedirect())
			{
				loadDialogue(vnDialogue.Redirect.Scene, vnDialogue.Redirect.Dialogue);
			}
			else
			{
				loadDialogue(currentScene, ++currentDialogue);
			}
		}
	}

	public void clickOptionA()
	{
		VNDialogue vnDialogue = PersistentManagerScript.Instance.SceneList[currentScene].Dialogues[currentDialogue];
		loadDialogue(vnDialogue.Question.Options[0].Redirect.Scene, vnDialogue.Question.Options[0].Redirect.Dialogue);
	}

	public void clickOptionB()
	{
		VNDialogue vnDialogue = PersistentManagerScript.Instance.SceneList[currentScene].Dialogues[currentDialogue];
		loadDialogue(vnDialogue.Question.Options[1].Redirect.Scene, vnDialogue.Question.Options[1].Redirect.Dialogue);
	}
}
