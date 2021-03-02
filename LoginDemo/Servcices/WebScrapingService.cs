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
using System.Windows;

namespace LoginDemo.Servcices
{
    public class WebScrapingService : IWebScrapingService
    {
        private IRequest _request;

        public WebScrapingService(IRequest request)
        {
            _request = request;
        }

        public async Task<string> GetHomePageDocument(string user, string password)
        {
            var pageHTMLDocument = "";
            try
            {
                pageHTMLDocument = await _request.GetHomePage(user, password);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show("It was not possible to login into facebook. Retype user and password and try again.", "Invalid User", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return pageHTMLDocument;
        }
    }
}
