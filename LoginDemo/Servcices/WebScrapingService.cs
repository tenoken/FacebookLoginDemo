using HtmlAgilityPack;
using LoginDemo.Servcices.Interfaces;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LoginDemo.Servcices
{
    public class WebScrapingService : IWebScrapingService
    {
        private IRequest _request;

        public WebScrapingService()
        {
            _request = new FacebookWebRequest();
        }

        public void Navigate(string user, string password)
        {
           _request.GetHomePage(user, password).GetAwaiter().GetResult();
        }
    }
}
