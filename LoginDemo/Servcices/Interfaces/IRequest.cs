using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LoginDemo.Servcices.Interfaces
{
    public interface IRequest
    {
        Task<string> GetHomePage(string user, string password);
    }
}
