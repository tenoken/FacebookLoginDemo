using LoginDemo.Domain.Model;
using LoginDemo.Domain.Models.Interfaces;
using LoginDemo.Servcices.Interfaces;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoginDemo.Servcices
{
    public abstract class WebRequest : IRequest
    {
        protected IList<RestResponseCookie> _cookieContainer;
        protected IDocumentHelper _documentHelper;
        protected Credential _credential;
        private const string USERAGENT = "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:46.0) Gecko/20100101 Firefox/46.0";

        public abstract Task<string> GetHomePage(string user, string password);

        public WebRequest(IDocumentHelper documentHelper, IList<RestResponseCookie> responseCookies, Credential credential)
        {
            _documentHelper = documentHelper;
            _cookieContainer = responseCookies;
            _credential = credential;
        }

        protected virtual async Task<string> Get(string uri, string pageContent = null)
        {
            var header = SetGetRequestHeader(uri);
            IRestResponse response = header.Item1.Execute(header.Item2);
            SetHeadersCookies(response.Cookies);

            return response.Content;
        }
        //TODO: Create a header object(Header)
        private Tuple<RestClient, RestRequest> SetGetRequestHeader(string uri)
        {
            var client = SetClient(uri);
            var request = new RestRequest(Method.GET);
            request.AddHeader("Pragma", "no-cache");
            AddCookie(request);

            var header = new Tuple<RestClient, RestRequest>(client, request);
            return header;
        }

        private void AddCookie(RestRequest request)
        {
            foreach (var item in _cookieContainer)
            {
                request.AddCookie(item.Name, item.Value);
            }
        }

        protected virtual async Task<string> Post(string uri, string pageContent = null, ActionParams action = ActionParams.Default)
        {
            var header = SetPostRequestHeader(uri);

            //TODO: transform in a keyvaluepair method
            if (!string.IsNullOrEmpty(pageContent))
            {
                var param = _documentHelper.GetParams(pageContent, action, _credential);

                for (int i = 0; i < param.Keys.Count; i++)
                {
                    header.Item2.AddParameter(param.Keys.ElementAt(i), param.Values.ElementAt(i));
                }

            }

            IRestResponse response = header.Item1.Execute(header.Item2);

            SetHeadersCookies(response.Cookies);

            return response.Content;
        }

        //TODO: Create a header object(Header)
        protected virtual Tuple<RestClient, RestRequest> SetPostRequestHeader(string uri)
        {
            RestClient client = SetClient(uri);
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            AddCookie(request);

            var header = new Tuple<RestClient, RestRequest>(client, request);
            return header;
        }

        protected virtual RestClient SetClient(string uri)
        {
            var client = new RestClient(uri);
            client.Timeout = -1;
            client.FollowRedirects = false;
            client.UserAgent = USERAGENT;
            return client;
        }

        private void SetHeadersCookies(IList<RestResponseCookie> cookies)
        {
            foreach (var cookie in cookies)
                _cookieContainer.Add(cookie);
        }

    }
}