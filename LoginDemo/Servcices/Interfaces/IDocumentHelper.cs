using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoginDemo.Servcices.Interfaces
{
    public interface IDocumentHelper
    {
        Tuple<string[], string[]> GetParams(string pageContent, FacebookWebRequest.ActionParams actionParams);
    }
}
