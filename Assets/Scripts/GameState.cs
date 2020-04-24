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

		if (PersistentManagerScript.Instance.vnScenes[scene - 1].Dialogues.Count < dialogue) {
			scene++;
			currentScene = scene;
			dialogue = 1;
			currentDialogue = dialogue;
		}

		if (PersistentManagerScript.Instance.vnScenes.Count < scene) {
			SceneManager.LoadScene(sceneName: "EndScene");
			return;
		}

		VNScene vnScene = PersistentManagerScript.Instance.vnScenes[scene - 1];
		VNDialogue vnDialogue = vnScene.Dialogues[dialogue - 1];

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
		VNDialogue vnDialogue = PersistentManagerScript.Instance.vnScenes[currentScene - 1].Dialogues[currentDialogue - 1];
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
		VNDialogue vnDialogue = PersistentManagerScript.Instance.vnScenes[currentScene - 1].Dialogues[currentDialogue - 1];
		loadDialogue(vnDialogue.Question.Options[0].Redirect.Scene, vnDialogue.Question.Options[0].Redirect.Dialogue);
	}

	public void clickOptionB()
	{
		VNDialogue vnDialogue = PersistentManagerScript.Instance.vnScenes[currentScene - 1].Dialogues[currentDialogue - 1];
		loadDialogue(vnDialogue.Question.Options[1].Redirect.Scene, vnDialogue.Question.Options[1].Redirect.Dialogue);
	}
}
