using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour {

	public Text dialogueBoxText;
	public Text characterNameBoxText;

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
		if (scene == -1 && dialogue == -1)
		{
			SceneManager.LoadScene(sceneName: "EndScene");
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
	}

	public void clickDialogue()
	{
		VNDialogue vnDialogue = PersistentManagerScript.Instance.vnScenes[currentScene - 1].Dialogues[currentDialogue - 1];
		if (!vnDialogue.isQuestion)
		{
			if (vnDialogue.hasRedirect)
			{
				loadDialogue(vnDialogue.Redirect.Scene, vnDialogue.Redirect.Dialogue);
			}
			else
			{
				loadDialogue(currentScene, ++currentDialogue);
			}
		}
	}
}
