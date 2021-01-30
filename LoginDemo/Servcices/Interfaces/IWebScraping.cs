using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LoginDemo.Servcices
{
    public interface IWebScrapingService
    {
        Task<string> GetHomePageDocument(string user, string password);
    }
}
