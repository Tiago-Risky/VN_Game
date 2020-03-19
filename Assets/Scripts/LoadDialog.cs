using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using System.Xml.Linq;
using System.IO;
using UnityEngine;

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

				VNRedirect diaRedir;
				if(dialogue.Element("redirect") != null)
				{
					diaRedir = new VNRedirect(int.Parse(dialogue.Element("redirect").Attribute("scene").Value),
											int.Parse(dialogue.Element("redirect").Attribute("dialogue").Value));
				}

				VNQuestion diaQuestion;
				if(dialogue.Element("options") !=null)
				{
					diaQuestion = new VNQuestion(); // TODO
				}
			}

			PersistentManagerScript.Instance.vnScenes.Add(vnScene);
		}

		Debug.Log("Loading XML File Done");
	}
	
	void Update () {
		
	}
}
