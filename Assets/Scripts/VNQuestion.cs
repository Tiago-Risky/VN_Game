using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class VNQuestion
{
    private List<VNOption> vnOptions;
    public List<VNOption> Options
    {
        get {return vnOptions ?? (vnOptions = new List<VNOption>()); }
    }

    public VNQuestion()
    {

    }

    
}

public class VNOption
{
    public VNRedirect Redirect;
    public string Text;

    public VNOption(string txt, VNRedirect redir) {
        Text = txt;
        Redirect = redir;
    }
}