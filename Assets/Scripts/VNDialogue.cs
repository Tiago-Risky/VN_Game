using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

public class VNDialogue
{
    public string Character = "";
    public string Text = "";
    public VNRedirect Redirect;
    public VNQuestion Question;

    /* This one has no redirection set, if no redirection is set it will
       skip to the next available dialogue/scene.
       It also has no VNQuestion */

    public VNDialogue(string character, string text, VNRedirect redirect, VNQuestion question) {
        Character = character;
        Text = text;
        Redirect = redirect;
        Question = question;
    }

    public bool IsQuestion() { 
        return Question != null;
    }

    public bool HasRedirect() {
        return Redirect != null;
    }
}
