using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

public class VNScene
{
    private Dictionary<int, VNDialogue> dialogues;
    public Dictionary<int, VNDialogue> Dialogues
    {
        get{return dialogues ?? (dialogues = new Dictionary<int, VNDialogue>());}
    }

    public VNScene()
    {

    }

    
}
