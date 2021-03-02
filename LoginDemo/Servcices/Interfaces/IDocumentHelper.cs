using LoginDemo.Domain.Model;
using System.Collections.Generic;

namespace LoginDemo.Servcices.Interfaces
{
    public interface IDocumentHelper
    {
        Dictionary<string, string> GetParams(string pageContent, ActionParams actionParams, Credential credential);
    }
}
