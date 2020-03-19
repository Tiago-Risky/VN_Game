using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class VNQuestion
{
    public List<VNOption> vnOptions;

    public VNQuestion()
    {
        vnOptions = new List<VNOption>();
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