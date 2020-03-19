using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

public class VNScene
{
    public int Number;
    public List<VNDialogue> Dialogues;

    public VNScene(int num)
    {
        Number = num;
        Dialogues = new List<VNDialogue>();
    }
}
