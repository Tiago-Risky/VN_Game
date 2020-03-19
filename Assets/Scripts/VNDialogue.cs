using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

public class VNDialogue
{
    public int Number;
    public string Character = "";
    public string Text = "";
    public bool hasRedirect = false;
    public VNRedirect Redirect;
    public bool isQuestion = false;
    public VNQuestion Question;

    /* This one has no redirection set, if no redirection is set it will
       skip to the next available dialogue/scene.
       It also has no VNQuestion */
    public VNDialogue(int num, string chr, string txt)
    {
        Number = num;
        Character = chr;
        Text = txt;
    }

    // This one calls the main constructor and adds the Redirection

    public VNDialogue(int num, string chr, string txt, VNRedirect redir): this(num, chr, txt)
    {
        Redirect = redir;
        hasRedirect = true;
    }

    // This one calls the main constructor and adds the VNQuestion

    public VNDialogue(int num, string chr, string txt, VNQuestion qst): this(num, chr, txt)
    {
        Question = qst;
        isQuestion = true;
    }
}
