using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using System.Xml.Linq;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadDialog : MonoBehaviour {

	void Start () {
		Debug.Log("Loading XML File");
		
		XElement file = XElement.Load("Assets/Content/GameDialog1.xml", LoadOptions.None);
        List<XElement> scenes = file.Elements("Scene").ToList();

		foreach (XElement scene in scenes)
		{
			VNScene vnScene = new VNScene(int.Parse(scene.Attribute("number").Value));
			foreach (XElement dialogue in scene.Elements("Dialogue").ToList())
			{
				int diaNr = int.Parse(dialogue.Attribute("number").Value);
				string diaChar = dialogue.Element("character").Value;
				string diaText = dialogue.Element("text").Value;

				VNRedirect diaRedir = null; //it should never be null, but won't compile unless set null
				if (dialogue.Element("redirect") != null)
				{
					diaRedir = new VNRedirect(int.Parse(dialogue.Element("redirect").Attribute("scene").Value),
											int.Parse(dialogue.Element("redirect").Attribute("dialogue").Value));
				}

				VNQuestion diaQuestion = null; //it should never be null, but won't compile unless set null
				if (dialogue.Element("options") !=null)
				{
					diaQuestion = new VNQuestion();
					foreach (XElement option in dialogue.Element("options").Elements("option").ToList())
					{

						VNRedirect optionRedir = new VNRedirect(int.Parse(option.Element("redirect").Attribute("scene").Value),
											int.Parse(option.Element("redirect").Attribute("dialogue").Value));
						diaQuestion.vnOptions.Add(new VNOption(option.Element("option_text").Value,optionRedir));
					}
				}

				VNDialogue vnDialogue = null; //it should never be null, but won't compile unless set null
				if (diaRedir == null && diaQuestion == null)
				{
					vnDialogue = new VNDialogue(diaNr, diaChar, diaText);
				}
				else {
					if (diaRedir != null)
					{
						vnDialogue = new VNDialogue(diaNr, diaChar, diaText, diaRedir);
					}
					if (diaQuestion != null)
					{
						vnDialogue = new VNDialogue(diaNr, diaChar, diaText, diaQuestion);
					}
				}

				vnScene.Dialogues.Add(vnDialogue);
			}

			PersistentManagerScript.Instance.vnScenes.Add(vnScene);
		}

		Debug.Log("Loading XML File Done");
		SceneManager.LoadScene(sceneName:"GameScene"); //Changing to the GameScene once the loading is done

	}
	
	void Update () {
		
	}
}
