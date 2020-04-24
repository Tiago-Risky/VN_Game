using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

public class VNScene
{
    public int Number;
    private List<VNDialogue> dialogues;
    public List<VNDialogue> Dialogues
    {
        get{return dialogues ?? (dialogues = new List<VNDialogue>());}
    }

    public VNScene(int number)
    {
        Number = number;
    }

    
}
