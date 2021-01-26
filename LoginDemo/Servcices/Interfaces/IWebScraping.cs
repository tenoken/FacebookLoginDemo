using System;
using System.Collections.Generic;
using System.Text;

namespace LoginDemo.Servcices
{
    public interface IWebScrapingService
    {
        void Navigate(string user, string password);
    }
}
